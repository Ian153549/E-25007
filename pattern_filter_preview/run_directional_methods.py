import cv2,numpy as np
from pathlib import Path
from PIL import Image,ImageDraw,ImageFont
from skimage.restoration import denoise_tv_chambolle
src=Path(r'\\Mr-ftp\檔案暫存區\00-Kuo暫存區\pattern\pattern\15_55_58_M-EXP-Z221A3-ZDL.jpg')
out=Path(r'C:\Project\E-25007\pattern_filter_preview')
im=cv2.imdecode(np.fromfile(str(src),dtype=np.uint8),cv2.IMREAD_GRAYSCALE)
a=im[1275:1490,1790:2050]
h=cv2.GaussianBlur(a,(41,1),10)
v=cv2.GaussianBlur(a,(1,41),10)
lower=np.minimum(h,v)
avg=np.uint8((h.astype('float32')+v.astype('float32'))/2)
tv=lambda a,w: np.uint8(np.clip(denoise_tv_chambolle(a.astype('float32')/255,weight=w,channel_axis=None,max_num_iter=120)*255,0,255))
b=cv2.bilateralFilter(a,15,35,15)
methods=[('Original',a),('Gaussian H',h),('Gaussian V',v),('H/V minimum',lower),('H/V average',avg),('H/V max',np.maximum(h,v)),('Min + TV0.08',tv(lower,.08)),('Bilateral + TV0.12',tv(b,.12))]
font=ImageFont.truetype('C:/Windows/Fonts/arial.ttf',20)
W,H=305,280
canvas=Image.new('RGB',(W*4,H*2),(241,245,249));d=ImageDraw.Draw(canvas)
for j,(name,image) in enumerate(methods):
    x=(j%4)*W+10;y=(j//4)*H+10
    d.text((x,y),name,font=font,fill=(22,31,44))
    canvas.paste(Image.fromarray(image).resize((280,232)),(x,y+35))
    cv2.imwrite(str(out/('direction_'+name.replace(' ','_').replace('/','_').replace('+','_')+'.png')),image)
canvas.save(out/'E25007_directional_comparison.png')
print('saved directional comparison and 8 versions')
