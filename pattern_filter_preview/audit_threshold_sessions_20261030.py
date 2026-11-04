# E25007: audit newest dated ImageEnhance sessions; read-only source and output inspections.
from pathlib import Path
from collections import defaultdict
import numpy as np, cv2
root=Path(r'\\E-25007\Project\E-25007\NSAA-16Axis\bin\x64\DebugVIII\imageEnhance')
source=Path(r'\\Mr-ftp\檔案暫存區\00-Kuo暫存區\pattern\pattern')
out=Path(r'C:\Project\E-25007\pattern_filter_preview\threshold_20261030')
out.mkdir(exist_ok=True,parents=True)
sessions={'Median':'20261030_170856','Bilateral':'20261030_170921',
 'TotalVariation':'20261030_170950','Horizontal':'20261030_171201',
 'Vertical':'20261030_171220','FftBandStop':'20261030_171243',
 'UnsharpMask':'20261030_171318'}
rows=defaultdict(dict)
for m,session in sessions.items():
    for p in (root/session/m).glob('*.txt'):
        try:
            lines=p.read_text(encoding='utf-8-sig').splitlines()
            name=lines[0].rsplit('\\',1)[-1]
            rows[name][m]=(p.with_suffix('.png'),lines[1:])
        except Exception as e: print('READ_SKIP',p,e)
print('SESSIONS:',sessions)
print('TOTAL_SOURCES:',len(rows))
print('TOTAL_IMAGES:',sum(len(row) for row in rows.values()))
print('COMMON:')
for name,data in sorted(rows.items(),key=lambda a:(-len(a[1]),a[0]))[:22]:
    print(len(data),name,','.join(data))
target='14_19_48_LearnPatternCaptureL.jpg'
im=cv2.imdecode(np.fromfile(str(source/target),dtype=np.uint8),cv2.IMREAD_GRAYSCALE)
print('SRC:',target,im.shape, np.percentile(im,[0,5,25,50,75,90,95,99,100]))
if im is not None:
    roi=im[870:2150,1450:2950]
    cv2.imwrite(str(out/'reference_large_roi.png'),roi)
    roi=im[1110:1880,2100:2820]
    cv2.imwrite(str(out/'reference_target_roi.png'),roi)
    print('REFERENCE_CROPS:',out)
