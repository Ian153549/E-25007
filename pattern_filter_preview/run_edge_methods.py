# Second pass: edge-preserving smoothing on actual E25007 sample
from pathlib import Path
import cv2,numpy as np
from PIL import Image,ImageDraw,ImageFont
from skimage.restoration import denoise_tv_chambolle
p=Path(r'\\Mr-ftp\檔案暫存區\00-Kuo暫存區\pattern\pattern\15_55_58_M-EXP-Z221A3-ZDL.jpg')
o=Path(r'C:\Project\E-25007\pattern_filter_preview')
s=cv2.imdecode(np.fromfile(str(p),dtype=np.uint8),cv2.IMREAD_GRAYSCALE)
a=s[1275:1490,1790:2050]
def guided(p,sigma=8,r=14,eps=200.):
    x=p.astype('float32')
    I=cv2.GaussianBlur(x,(0,0),sigma)
    def m(v): return cv2.boxFilter(v,-1,(2*r+1,2*r+1),normalize=True)
    mI=m(I);mp=m(x);vI=m(I*I)-mI*mI;cov=m(I*x)-mI*mp
    A=cov/(vI+eps);B=mp-A*mI
    return np.uint8(np.clip(m(A)*I + m(B),0,255))
def tv(a,weight):
    return np.uint8(np.clip(denoise_tv_chambolle(a.astype('float32')/255,weight=weight,channel_axis=None,max_num_iter=150)*255,0,255))
methods=[
    ('Original',a),
    ('Median 11',cv2.medianBlur(a,11)),
    ('Bilateral 15',cv2.bilateralFilter(a,15,35,15)),
    ('Guided s6 r12',guided(a,6,12,200)),
    ('Guided s10 r18',guided(a,10,18,250)),
    ('Guided s14 r23',guided(a,14,23,400)),
    ('TV 0.08',tv(a,.08)),
    ('TV 0.15',tv(a,.15)),
    ('TV 0.25',tv(a,.25)),
    ('NLMeans h=15',cv2.fastNlMeansDenoising(a,None,15,7,21)),
    ('Median15 + TV.06',tv(cv2.medianBlur(a,15),.06)),
    ('Median9 + Guided',guided(cv2.medianBlur(a,9),9,16,275))
]
font=ImageFont.truetype('C:/Windows/Fonts/arial.ttf',19)
W,H=305,286
out=Image.new('RGB',(W*4,H*3),(242,245,249));d=ImageDraw.Draw(out)
for j,(name,img) in enumerate(methods):
    x=(j%4)*W+11;y=(j//4)*H+10
    d.text((x,y),name,fill=(24,32,42),font=font)
    out.paste(Image.fromarray(img).resize((280,232)),(x,y+35))
    cv2.imwrite(str(o/('edge_preserve_'+name.replace(' ','_').replace('=','')+'.png')),img)
out.save(o/'E25007_edge_preservation_comparison.png')
print('Saved 12 methods in',o)
