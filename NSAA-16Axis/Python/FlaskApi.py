from flask import Flask, request, jsonify
from ultralytics import YOLO
import tempfile
import cv2
import json
import os

app = Flask(__name__)
# 讀取配置文件
config_path = os.path.join(os.path.dirname(__file__), "AIConfig.json")
with open(config_path, 'r', encoding='utf-8') as f:
    config = json.load(f)
model_name = config["loadRecipe"]
model = YOLO(f"Models/{model_name}")  # 僅加載一次
if hasattr(model, "model") and hasattr(model.model, "fuse"):
    model.model.fuse = lambda verbose=True: model.model
names = model.model.names

@app.route("/infer", methods=["POST"])
def infer():
    file = request.files['image']    # 和 C# 端的欄位一致
    temp_img = tempfile.NamedTemporaryFile(suffix=".png", delete=False)
    file.save(temp_img.name)
    temp_img.close()

    # 推論
    results = model(temp_img.name)
    output = []
    for box in results[0].boxes:
        x1, y1, x2, y2 = box.xyxy[0].tolist()
        conf = float(box.conf[0])
        cls = int(box.cls[0])
        output.append({
            "x1": x1, "y1": y1, "x2": x2, "y2": y2,
            "conf": conf, "class": cls,
            "name": names[int(cls)] # 類別名稱
        })
    return jsonify(output)

@app.route("/class_names", methods=["GET"])
def class_names():
    print("收到 class_names 查詢！")
    return jsonify(names)
