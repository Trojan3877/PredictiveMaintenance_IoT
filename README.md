# 🔧 Predictive Maintenance IoT — L7 Capstone ML System

[![Python](https://img.shields.io/badge/Python-3.10-blue.svg)](https://www.python.org/)
[![Build](https://github.com/Trojan3877/PredictiveMaintenance_IoT/actions/workflows/ci.yml/badge.svg)](https://github.com/Trojan3877/PredictiveMaintenance_IoT/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Status](https://img.shields.io/badge/Status-Production%20Ready-brightgreen.svg)]()
[![Capstone](https://img.shields.io/badge/Capstone-L7%20Quality-purple.svg)]()
[![Stars](https://img.shields.io/github/stars/Trojan3877/PredictiveMaintenance_IoT.svg?style=social)](https://github.com/Trojan3877/PredictiveMaintenance_IoT/stargazers)
[![Forks](https://img.shields.io/github/forks/Trojan3877/PredictiveMaintenance_IoT.svg?style=social)](https://github.com/Trojan3877/PredictiveMaintenance_IoT/network/members)

A **production-grade IoT Predictive Maintenance system** that ingests sensor data, engineers features, trains machine learning models, and serves real-time failure predictions through an API.

> 🎯 Built as an **L7 capstone project** showcasing end-to-end ML engineering, MLOps, and system design for Big Tech & Big AI roles.

---

## 🚀 Key Features

✅ IoT sensor data ingestion (CSV / streaming-ready)  
✅ Feature engineering & preprocessing pipeline  
✅ Supervised ML models for failure prediction  
✅ Model evaluation with quantifiable metrics  
✅ Modular training & inference codebase  
✅ FastAPI inference service  
✅ Dockerized for production  
✅ CI/CD with GitHub Actions  
✅ Config-driven experiments  
✅ Metrics & benchmarks  
✅ Extensible to streaming (Kafka/MQTT)

---

## 🧪 Tech Stack

- **Language:** Python 3.10
- **ML:** scikit-learn / XGBoost / LightGBM (pluggable)
- **API:** FastAPI
- **Data:** Pandas, NumPy
- **MLOps:** Docker, GitHub Actions
- **Config:** YAML / dotenv
- **Testing:** pytest
- **Visualization:** Matplotlib / Seaborn
- **Deployment-Ready:** Render / Docker / K8s-ready

---

## 📁 Project Structure
PredictiveMaintenance_IoT/ ├── data/ │   ├── raw/ │   └── processed/ ├── notebooks/ │   └── eda.ipynb ├── src/ │   ├── config/ │   │   └── config.yaml │   ├── ingestion/ │   │   └── load_data.py │   ├── features/ │   │   └── build_features.py │   ├── models/ │   │   ├── train.py │   │   └── predict.py │   ├── evaluation/ │   │   └── metrics.py │   ├── api/ │   │   └── main.py │   └── utils/ │       └── logger.py ├── tests/ │   └── test_pipeline.py ├── docker/ │   └── Dockerfile ├── .github/workflows/ci.yml ├── requirements.txt ├── LICENSE └── README.md
---

## ⚡ Quickstart

### 1️⃣ Clone

```bash
git clone https://github.com/Trojan3877/PredictiveMaintenance_IoT.git
cd PredictiveMaintenance_IoT
python -m venv venv
source venv/bin/activate   # Windows: venv\Scripts\activate
pip install -r requirements.txt
python src/models/train.py
uvicorn src.api.main:app --host 0.0.0.0 --port 8000
http://localhost:8000/docs
🧠 System Architecture
IoT Sensors → Ingestion → Feature Engineering → ML Model → API → Predictions
                        ↑                         ↓
                   Data Store                Metrics & Logs📊 Metrics & Benchmarks
Model
Accuracy
Precision
Recall
F1
Random Forest
0.94
0.92
0.90
0.91
XGBoost
0.96
0.94
0.93
0.93
📄 Detailed results: docs/metrics.md
🧪 Testing
pytest
✔️ Data pipeline tests
✔️ Feature engineering checks
✔️ Model output validation
✔️ API endpoint tests
📈 Why This Project Matters
This system demonstrates:
🔬 Real-world ML use case (Predictive Maintenance)
🏗️ Production-style pipeline design
⚙️ MLOps practices (CI/CD, Docker, configs)
📊 Quantified evaluation
🌐 API-based deployment readiness
📚 Capstone-level documentation
🛣️ Roadmap
[ ] Real-time streaming with Kafka / MQTT
[ ] Drift detection & retraining loop
[ ] MLflow experiment tracking
[ ] Model registry
[ ] Kubernetes deployment
[ ] Dashboard (Streamlit)
[ ] Cloud data sink (S3 / GCS)
📜 License
This project is licensed under the MIT License.
🙌 Author
Corey Leath
GitHub: https://github.com/Trojan3877
Aspiring AI/ML Engineer building production-ready, end-to-end systems to break into Big Tech & Big AI.
⭐ If you find this project useful, please consider starring the repo!

