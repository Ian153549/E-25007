# Copilot Instructions

## Project Guidelines
- 在效能問題上，先量測 UI thread 阻塞與各段耗時（如 Grab、比對、PLC 移動、回圈週期）再決定優化策略，避免盲目修改。
  - 回覆時請詳細標示修改地方跟修改的程式碼
  - 使用者希望提供可直接編譯的修改，新增變數時要一併定義欄位與初始化。