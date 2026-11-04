from pathlib import Path
import cv2,numpy as np
from PIL import Image,ImageDraw,ImageFont
src=Path(r'\\Mr-ftp\檔案暫存區\00-Kuo暫存區\pattern\pattern\14_19_48_LearnPatternCaptureL.jpg')
out=Path(r'C:\Project\E-25007\pattern_filter_preview')
im=cv2.imdecode(np.fromfile(str(src),dtype=np.uint8),cv2.IMREAD_GRAYSCALE)
roi=im[1000:1880,1930:2780]
def usm(a,k=1.2,s=1.0):
 b=cv2.GaussianBlur(a,(0,0),s)
 return cv2.addWeighted(a,1+k,b,-k,0)
v=[
('Original',roi),
('Median 3',cv2.medianBlur(roi,3)),
('Median 5',cv2.medianBlur(roi,5)),
('Median 7',cv2.medianBlur(roi,7)),
('Bilateral 5 15 4',cv2.bilateralFilter(roi,5,15,4)),
('Bilateral 7 22 5',cv2.bilateralFilter(roi,7,22,5)),
('Bilateral 9 25 7',cv2.bilateralFilter(roi,9,25,7)),
('Gaussian 9 s2',cv2.GaussianBlur(roi,(9,1),2)),
('Bilateral+USM',usm(cv2.bilateralFilter(roi,7,22,5),.65,1.2)),
('Median5+USM',usm(cv2.medianBlur(roi,5),.55,1.0)),
('USM only',usm(roi,.55,1.0)),
('CLAHE 2 tile8',cv2.createCLAHE(clipLimit=2,tileGridSize=(8,8)).apply(roi)),
]
font=ImageFont.truetype('C:/Windows/Fonts/arial.ttf',20)
W,H=330,335
canvas=Image.new('RGB',(W*4,H*3),(243,245,248))
d=ImageDraw.Draw(canvas)
for j,(name,img) in enumerate(v):
 x=(j%4)*W+10;y=(j//4)*H+10
 d.text((x,y),name,fill=(20,30,44),font=font)
 mini=cv2.resize(img,(310,290),interpolation=cv2.INTER_AREA)
 canvas.paste(Image.fromarray(mini).convert('RGB'),(x,y+30))
canvas.save(out/'actual_weak_parameter_comparison.png')
print('saved',out/'actual_weak_parameter_comparison.png',im.shape)
