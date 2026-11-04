# E25007 2026-10-30 dated ImageEnhance batch: offline threshold comparison only.
# Does not touch production output, matcher, recipe, or camera.
from pathlib import Path
from collections import defaultdict
import csv,cv2,numpy as np
from PIL import Image, ImageDraw, ImageFont
ROOT=Path(r'\\E-25007\Project\E-25007\NSAA-16Axis\bin\x64\DebugVIII\imageEnhance')
ORIG=Path(r'\\Mr-ftp\檔案暫存區\00-Kuo暫存區\pattern\pattern')
DEST=Path(r'C:\Project\E-25007\pattern_filter_preview\threshold_20261030')
DEST.mkdir(exist_ok=True,parents=True)
SESS={'Median':'20261030_170856','Bilateral':'20261030_170921','TotalVariation':'20261030_170950','Horizontal':'20261030_171201','Vertical':'20261030_171220','FftBandStop':'20261030_171243','UnsharpMask':'20261030_171318'}
INDEX=defaultdict(dict)
for method,sess in SESS.items():
 for txt in (ROOT/sess/method).glob('*.txt'):
  try:
   name=txt.read_text(encoding='utf-8-sig').splitlines()[0].rsplit('\\',1)[-1]
   INDEX[name][method]=txt.with_suffix('.png')
  except Exception:pass
NAMES=[
 '14_19_48_LearnPatternCaptureL.jpg',
 '14_19_48_LearnPatternCaptureR.jpg',
 '14_19_53_LearnPatternCaptureL.jpg',
 '14_19_53_LearnPatternCaptureR.jpg',
 '14_20_00_LearnPatternCaptureL.jpg',
]
METHODS=['Original','Median','Bilateral','TotalVariation','Horizontal','Vertical','FftBandStop','UnsharpMask']
FONT=ImageFont.truetype('C:/Windows/Fonts/arial.ttf',19)
SMFONT=ImageFont.truetype('C:/Windows/Fonts/arial.ttf',15)
def decode(p):
 b=np.fromfile(str(p),dtype=np.uint8)
 a=cv2.imdecode(b,cv2.IMREAD_GRAYSCALE)
 if a is None:raise ValueError(str(p))
 return a
def modes(a,roi):
 sub=a[roi[0]:roi[1],roi[2]:roi[3]]
 otsu_f,bin_otsuf=cv2.threshold(a,0,255,cv2.THRESH_BINARY+cv2.THRESH_OTSU)
 otsu_r,bin_otsur=cv2.threshold(sub,0,255,cv2.THRESH_BINARY+cv2.THRESH_OTSU)
 adaptive=cv2.adaptiveThreshold(sub,255,cv2.ADAPTIVE_THRESH_GAUSSIAN_C,cv2.THRESH_BINARY,91,7)
 morph=cv2.morphologyEx(bin_otsur,cv2.MORPH_OPEN,cv2.getStructuringElement(cv2.MORPH_ELLIPSE,(3,3)))
 return [( 'Gray',sub),('T=110',cv2.threshold(sub,110,255,cv2.THRESH_BINARY)[1]),('T=145',cv2.threshold(sub,145,255,cv2.THRESH_BINARY)[1]),('Otsu full '+str(int(otsu_f)),bin_otsuf[roi[0]:roi[1],roi[2]:roi[3]]),('Otsu ROI '+str(int(otsu_r)),bin_otsur),('Adaptive G91 C7',adaptive),('Otsu ROI Open3',morph)],otsu_f,otsu_r
def card(im,i,x,y,label,W=278,H=281):
 d=ImageDraw.Draw(im)
 d.text((x+8,y+6),label,fill=(20,28,45),font=FONT)
 res=cv2.resize(i,(W-14,H-37),interpolation=cv2.INTER_AREA)
 im.paste(Image.fromarray(res).convert('RGB'),(x+7,y+32))
for name in NAMES:
 if any(m not in INDEX[name] for m in METHODS if m!='Original'):
  print('SKIP_INCOMPLETE',name)
  continue
 ori=decode(ORIG/name)
 # Equal physical crop coordinates for all methods, not a localization process.
 ROI=(1100,1880,2070,2800) if name.endswith('L.jpg') else (1100,1880,1050,1780)
 if name.endswith('R.jpg'): 
  # Right images use analogous scene coordinates; adjust visually if needed.
  ROI=(1100,1880,2070,2800)
 tiles=[]
 for m in METHODS:
  a=ori if m=='Original' else decode(INDEX[name][m])
  if a.shape!=ori.shape:raise ValueError('SIZE_MISMATCH '+m+' '+name)
  tiles.append((m,*modes(a,ROI)))
 ident=Path(name).stem
 for batchidx in (0,1):
  chosen=tiles[batchidx*4:(batchidx+1)*4]
  WIDTH=278; HEIGHT=281
  sheet=Image.new('RGB',(WIDTH*7,HEIGHT*4),(242,245,248))
  for row,(m,arr,otsuf,otsur) in enumerate(chosen):
   for col,(lab,mi) in enumerate(arr):
    card(sheet,mi,col*WIDTH,row*HEIGHT,('%s  %s'%(m,lab) if col==0 else lab),WIDTH,HEIGHT)
  dest=DEST/('%s_threshold_sheet_%02d.png'%(ident,batchidx+1))
  sheet.save(dest,optimize=True)
  print('SHEET',dest)
 for m,arr,otsuf,otsur in tiles:
  stats=[]
  for label,data in arr[1:]:
   stats.append(f'{label}:{(data!=0).mean()*100:.1f}%')
  print('METRIC',name,m,'otsu_full',round(otsuf,1),'otsu_roi',round(otsur,1),';'.join(stats))
print('RESULTS',DEST)
