from pathlib import Path
from collections import defaultdict
import csv, zipfile, cv2, numpy as np
ROOT=Path(r'\\E-25007\Project\E-25007\NSAA-16Axis\bin\x64\DebugVIII\imageEnhance')
SRC=Path(r'\\Mr-ftp\檔案暫存區\00-Kuo暫存區\pattern\pattern')
OUT=Path(r'C:\Project\E-25007\pattern_filter_preview\threshold_20261030')
SESS={'Median':'20261030_170856','Bilateral':'20261030_170921','TotalVariation':'20261030_170950','Horizontal':'20261030_171201','Vertical':'20261030_171220','FftBandStop':'20261030_171243','UnsharpMask':'20261030_171318'}
NAMES=['14_19_48_LearnPatternCaptureL.jpg','14_19_48_LearnPatternCaptureR.jpg','14_19_53_LearnPatternCaptureL.jpg','14_19_53_LearnPatternCaptureR.jpg','14_20_00_LearnPatternCaptureL.jpg']
index=defaultdict(dict)
for m,s in SESS.items():
 for t in (ROOT/s/m).glob('*.txt'):
  try:
   name=t.read_text(encoding='utf-8-sig').splitlines()[0].rsplit('\\',1)[-1]
   index[name][m]=t.with_suffix('.png')
  except Exception: pass
def read(p):
 i=cv2.imdecode(np.fromfile(str(p),dtype=np.uint8),cv2.IMREAD_GRAYSCALE)
 if i is None:raise ValueError(str(p))
 return i
fields=['source','method','full_otsu_threshold','roi_otsu_threshold','roi_white_pct_fixed110','roi_white_pct_fixed145','roi_white_pct_full_otsu','roi_white_pct_roi_otsu','roi_white_pct_adaptive_G91_C7']
outcsv=OUT/'threshold_metrics_20261030.csv'
with outcsv.open('w',encoding='utf-8-sig',newline='') as f:
 writer=csv.DictWriter(f,fieldnames=fields);writer.writeheader()
 for name in NAMES:
  rows=[('Original',SRC/name)]+[(m,index[name][m]) for m in SESS]
  for m,p in rows:
   a=read(p)
   aroi=a[1100:1880,2070:2800]
   gt,full=cv2.threshold(a,0,255,cv2.THRESH_BINARY+cv2.THRESH_OTSU)
   rt,broi=cv2.threshold(aroi,0,255,cv2.THRESH_BINARY+cv2.THRESH_OTSU)
   aa=cv2.adaptiveThreshold(aroi,255,cv2.ADAPTIVE_THRESH_GAUSSIAN_C,cv2.THRESH_BINARY,91,7)
   mean=lambda b:round(float((b>0).mean()*100),2)
   writer.writerow({'source':name,'method':m,'full_otsu_threshold':round(gt,1),'roi_otsu_threshold':round(rt,1),
    'roi_white_pct_fixed110':mean(aroi>110),'roi_white_pct_fixed145':mean(aroi>145),
    'roi_white_pct_full_otsu':mean(full[1100:1880,2070:2800]),'roi_white_pct_roi_otsu':mean(broi),'roi_white_pct_adaptive_G91_C7':mean(aa)})
readme='''E25007 2026-10-30 影像處理後二值化比較（獨立離線測試）
==================================================
輸入資料：
  20261030_170856 Median Kernel=5
  20261030_170921 Bilateral d=7 sigmaColor=22 sigmaSpace=5
  20261030_170950 TotalVariation weight=0.03 iterations=60
  20261030_171201 Horizontal Gaussian 9x1 sigma=2
  20261030_171220 Vertical Gaussian 1x9 sigma=2
  20261030_171243 FFT f0=0.062 sigma=0.004 strength=0.4
  20261030_171318 UnsharpMask k=0.6 sigma=1.2

比對來源：14_19_48_LearnPatternCaptureL/R.jpg、
         14_19_53_LearnPatternCaptureL/R.jpg、
         14_20_00_LearnPatternCaptureL.jpg
以上 5 張來源圖皆有 7 種方法輸出；相同來源才做橫向比較。
每張來源圖產生 threshold_sheet_01 / _02，共 10 張比較圖。

欄位：
  Gray：實際濾波後的灰階影像
  T=110 / T=145：固定二值化閾值
  Otsu full：全圖計算自動閾值，再裁切顯示
  Otsu ROI：比較用固定裁切視野內計算自動閾值
  Adaptive G91 C7：Gaussian adaptive threshold, blockSize=91, C=7
  Otsu ROI Open3：Otsu 後 3x3 橢圓 opening（觀察去細雜點）

另有 Closing_after_threshold_Bilateral/TotalVariation.png：
比較 Otsu / T110 二值化後再做 5/9/15/21 形態學閉合；
較大閉合雖能填充漣漪，卻破壞內部真實細節。

觀察：
1. 對 14_19_48 L/R，全圖 Otsu 閾值約 121/126；
   指定裁切視野 Otsu 約 135-139/145-146（不同濾波略有變化）。
2. 閾值低時，漣漪會被併為白色塊；閾值高時，較暗真實圖案會消失。
3. Adaptive G91 C7 對這兩張裁切視野白色像素比例約 82%-88%，
   相當多背景也變白；無法視為去除漣漪。
4. 比較結果不支持所有影像一律做二值化後直接送入 Matcher。
5. 建議灰階輸出保留，二值化僅作獨立可切換分支；
   若用於匹配需以同樣前處理重新驗證學習樣板和辨識品質。

注意：固定裁切視野是為了呈現影像處理差異，並未進行目標定位或追蹤。
本次沒有更動生產影像、原本七組處理輸出、設備 Recipe 或正式 C# 程式。
'''
(OUT/'README_threshold_experiment_zhTW.txt').write_text(readme,encoding='utf-8-sig')
archive=OUT/'E25007_20261030_Threshold_Comparison.zip'
files=sorted([x for x in OUT.glob('*.png') if 'threshold_sheet_' in x.name or x.name.startswith('Closing_after_threshold_')])
files += [outcsv,OUT/'README_threshold_experiment_zhTW.txt']
with zipfile.ZipFile(archive,'w',compression=zipfile.ZIP_DEFLATED,compresslevel=6) as z:
 for f in files:z.write(f,f.name)
print('CSV rows:',len(NAMES)*8)
print('ZIP:',archive,'size_MiB:',round(archive.stat().st_size/1048576,2),'count:',len(files))
