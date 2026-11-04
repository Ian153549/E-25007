from pathlib import Path
from collections import defaultdict
import cv2,numpy as np
from PIL import Image,ImageDraw,ImageFont
base=Path(r'\\E-25007\Project\E-25007\NSAA-16Axis\bin\x64\DebugVIII\imageEnhance')
src=Path(r'\\Mr-ftp\檔案暫存區\00-Kuo暫存區\pattern\pattern')
out=Path(r'C:\Project\E-25007\pattern_filter_preview')
methods=['Median','Bilateral','TotalVariation','Horizontal','Vertical','FftBandStop']
d=defaultdict(dict)
for m in methods:
 for p in (base/m).glob('*.txt'):
  line=p.read_text(encoding='utf-8-sig').splitlines()[0]
  d[line.rsplit('\\',1)[-1]][m]=p.with_suffix('.png')
names=[n for n,v in d.items() if len(v)==6][:3]
for idx,name in enumerate(names):
 im=cv2.imdecode(np.fromfile(str(src/name),dtype=np.uint8),cv2.IMREAD_GRAYSCALE)
 imgs=[('Original',im)]
 for m in methods:
  img=cv2.imdecode(np.fromfile(str(d[name][m]),dtype=np.uint8),cv2.IMREAD_GRAYSCALE)
  imgs.append((m,img))
 w,h=im.shape[1],im.shape[0]
 font=ImageFont.truetype('C:/Windows/Fonts/arial.ttf',21)
 thumb=(386,277)
 canvas=Image.new('RGB',(4*400,2*320),(244,246,248));draw=ImageDraw.Draw(canvas)
 for j,(lab,img) in enumerate(imgs):
  if img.shape!=im.shape:print('SHAPE MISMATCH',lab,img.shape,im.shape)
  small=cv2.resize(img,thumb,interpolation=cv2.INTER_AREA)
  x=(j%4)*400+7;y=(j//4)*320+7
  draw.text((x,y),lab,fill=(20,30,50),font=font)
  canvas.paste(Image.fromarray(small).convert('RGB'),(x,y+31))
 canvas.save(out/('actual_six_methods_'+str(idx)+'.png'))
 print('saved',idx,name)
