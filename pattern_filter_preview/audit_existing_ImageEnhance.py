from pathlib import Path
from collections import defaultdict
import cv2
import numpy as np
root = Path(r'\\E-25007\Project\E-25007\NSAA-16Axis\bin\x64\DebugVIII\imageEnhance')
sources = Path(r'\\Mr-ftp\檔案暫存區\00-Kuo暫存區\pattern\pattern')
out = Path(r'C:\Project\E-25007\pattern_filter_preview')
out.mkdir(exist_ok=True, parents=True)
d = defaultdict(dict)
methods = ('Median', 'Bilateral', 'TotalVariation', 'Horizontal', 'Vertical', 'FftBandStop')
for method in methods:
    for p in (root / method).glob('*.txt'):
        try:
            line = p.read_text(encoding='utf-8-sig').splitlines()[0]
            source = line.rsplit('\\', 1)[-1]
            d[source][method] = str(p.with_suffix('.png'))
        except Exception as ex:
            print('skipped', p.name, str(ex)[:100])
print('sources:', len(d))
print('method counts:', {m: sum(m in row for row in d.values()) for m in methods})
for source, found in sorted(d.items(), key=lambda kv: (-len(kv[1]), kv[0]))[:30]:
    print(len(found), source, ','.join(found))
for name in ('14_19_48_LearnPatternCaptureL.jpg', '14_43_20_LearnPatternCaptureR.bmp', '15_55_58_M-EXP-Z221A3-ZDL.jpg'):
    p = sources / name
    if p.exists():
        im = cv2.imdecode(np.fromfile(str(p), dtype=np.uint8), cv2.IMREAD_GRAYSCALE)
        if im is not None:
            h, w = im.shape
            thumbnail = cv2.resize(im, (964, 691), interpolation=cv2.INTER_AREA)
            save = out / ('inspect_source_' + Path(name).stem + '.png')
            cv2.imwrite(str(save), thumbnail)
            print('thumbnail:', save, 'original size:', (w, h))
