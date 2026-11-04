E25007 2026-10-30 影像處理後二值化比較（獨立離線測試）
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
