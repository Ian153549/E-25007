# E25007 pattern ripple removal — offline analysis only; does not modify source images
import os
from pathlib import Path
import cv2
import numpy as np
from PIL import Image, ImageDraw, ImageFont
from scipy.ndimage import gaussian_filter1d

ROOT=Path(r'\\Mr-ftp\檔案暫存區\00-Kuo暫存區\pattern\pattern')
OUT=Path(r'C:\Project\E-25007\pattern_filter_preview')
OUT.mkdir(exist_ok=True,parents=True)

def load(name):
    p=ROOT/name
    a=cv2.imdecode(np.fromfile(str(p),dtype=np.uint8),cv2.IMREAD_GRAYSCALE)
    if a is None: raise ValueError(f'Cannot decode: {p}')
    return a

def save(name,img):
    ok=cv2.imwrite(str(OUT/name),img)
    if not ok: raise ValueError(f'Cannot save: {name}')

def fft_bandstop(a, bands=((.062,.018,.87),)):
    pad=70
    x=cv2.copyMakeBorder(a.astype('float32'),pad,pad,pad,pad,cv2.BORDER_REFLECT_101)
    h,w=x.shape
    fy=np.fft.fftshift(np.fft.fftfreq(h))[:,None]
    fx=np.fft.fftshift(np.fft.fftfreq(w))[None,:]
    R=np.sqrt(fx*fx+fy*fy)
    M=np.ones_like(R)
    for f,s,depth in bands:
        M*=(1-depth*np.exp(-0.5*((R-f)/s)**2))
    f=np.fft.fftshift(np.fft.fft2(x))
    y=np.fft.ifft2(np.fft.ifftshift(f*M)).real
    return np.uint8(np.clip(y[pad:pad+a.shape[0],pad:pad+a.shape[1]],0,255))

def radial_remove(a,cx,cy,alpha=1.4,maxrad=84,smooth_radius=18):
    x=a.astype('float32')
    yy,xx=np.indices(x.shape)
    radius=np.hypot(xx-cx,yy-cy)
    b=np.floor(radius*2).astype('int32')
    valid=(radius<=maxrad)&(x>82)
    size=int(b.max())+1
    s=np.bincount(b[valid],weights=x[valid],minlength=size)
    counts=np.bincount(b[valid],minlength=size)
    prof=np.zeros(size,dtype=np.float64)
    ok=counts>0
    prof[ok]=s[ok]/counts[ok]
    prof=np.interp(np.arange(size),np.flatnonzero(ok),prof[ok])
    prof=gaussian_filter1d(prof,2.)
    baseline=gaussian_filter1d(prof,smooth_radius*2)
    wave=prof-baseline
    t=np.clip((maxrad-radius)/10.,0,1)
    corrected=x - alpha*wave[b]*t*np.float32(x>82)
    return np.uint8(np.clip(corrected,0,255))

def close(a,n):
    return cv2.morphologyEx(a,cv2.MORPH_CLOSE,
        cv2.getStructuringElement(cv2.MORPH_ELLIPSE,(n,n)))

source='15_55_58_M-EXP-Z221A3-ZDL.jpg'
I=load(source)
roi=I[1210:1530,1760:2100]
SAVE_ROI=roi.copy()
save('01_original_center.png',roi)

methods=[
 ('01 Original',roi),
 ('02 Gaussian sigma=5',cv2.GaussianBlur(roi,(0,0),5)),
 ('03 Median k=15',cv2.medianBlur(roi,15)),
 ('04 Bilateral d=21',cv2.bilateralFilter(roi,21,42,16)),
 ('05 FFT radial bandstop',fft_bandstop(roi)),
 ('06 Close ellipse=19',close(roi,19)),
 ('07 Radial ripple subtract',radial_remove(roi,160,157,1.5,87,18)),
 ('08 Close19 + median5',cv2.medianBlur(close(roi,19),5))
]
font=ImageFont.truetype('C:/Windows/Fonts/arial.ttf',24)
CW,CH=390,386
mont=Image.new('RGB',(CW*4,CH*2),(239,242,247))
d=ImageDraw.Draw(mont)
for j,(name,arr) in enumerate(methods):
    filename=f'{j+1:02d}_'+name.lower().replace(' ','_').replace('=','')+'.png'
    save(filename,arr)
    r,c=divmod(j,4)
    x0,y0=c*CW+15,r*CH+16
    d.text((x0,y0),name,fill=(22,31,44),font=font)
    mont.paste(Image.fromarray(arr).resize((350,330)),(x0,y0+45))
mont.save(OUT/'E25007_filtered_center_comparison.png')
# Full-frame examples, unmodified source directory
save('E25007_full_median15.png',cv2.medianBlur(I,15))
save('E25007_full_close19.png',close(I,19))
print('source:',ROOT/source,'shape:',I.shape)
print('saved_to:',OUT)
print('method_images:',len(methods),'+ 2 full-frame filters')
