from pathlib import Path
import cv2,numpy as np
from PIL import Image,ImageDraw,ImageFont
root=Path(r'\\E-25007\Project\E-25007\NSAA-16Axis\bin\x64\DebugVIII\imageEnhance')
out=Path(r'C:\Project\E-25007\pattern_filter_preview\threshold_20261030')
target='14_19_48_LearnPatternCaptureL.jpg'
dirs={'Bilateral':root/'20261030_170921'/'Bilateral','TotalVariation':root/'20261030_170950'/'TotalVariation'}
def imread(p):return cv2.imdecode(np.fromfile(str(p),dtype=np.uint8),cv2.IMREAD_GRAYSCALE)
FONT=ImageFont.truetype('C:/Windows/Fonts/arial.ttf',20)
W,H=350,380
for method,dirp in dirs.items():
 p=None
 for txt in dirp.glob('*.txt'):
  if target in txt.read_text(encoding='utf-8-sig'):
   p=txt.with_suffix('.png');break
 assert p is not None,method
 img=imread(p)
 crop=img[1100:1880,2070:2800]
 t,b=cv2.threshold(crop,0,255,cv2.THRESH_BINARY+cv2.THRESH_OTSU)
 _,bf=cv2.threshold(crop,110,255,cv2.THRESH_BINARY)
 arrs=[('Gray',crop),('Otsu ROI %d'%t,b),('Fixed T110',bf)]
 for k in (5,9,15,21):
  kernel=cv2.getStructuringElement(cv2.MORPH_ELLIPSE,(k,k))
  arrs.append(('Otsu Close '+str(k),cv2.morphologyEx(b,cv2.MORPH_CLOSE,kernel)))
  arrs.append(('T110 Close '+str(k),cv2.morphologyEx(bf,cv2.MORPH_CLOSE,kernel)))
 cols=4;rows=(len(arrs)+cols-1)//cols
 fig=Image.new('RGB',(W*cols,H*rows),(243,245,247))
 d=ImageDraw.Draw(fig)
 for i,(lbl,a) in enumerate(arrs):
  x=(i%cols)*W+8;y=(i//cols)*H+7
  d.text((x,y),method+' '+lbl,fill=(19,29,44),font=FONT)
  scaled=cv2.resize(a,(325,345),interpolation=cv2.INTER_AREA)
  fig.paste(Image.fromarray(scaled).convert('RGB'),(x,y+29))
 outname=out/('Closing_after_threshold_'+method+'.png')
 fig.save(outname)
 print(method,'OtsuROI',t,'saved',outname)
