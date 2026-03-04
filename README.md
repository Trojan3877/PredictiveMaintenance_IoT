# 🔧 Predictive Maintenance IoT — Capstone ML System
[![Python](https://img.shields.io/badge/Python-3.10-blue.svg)](https://www.python.org/)
[![Build](https://github.com/Trojan3877/PredictiveMaintenance_IoT/actions/workflows/ci.yml/badge.svg)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Status](https://img.shields.io/badge/Status-Production%20Ready-brightgreen.svg)]()
[![Capstone](https://img.shields.io/badge/Capstone-L7%20Quality-purple.svg)]()
[![Stars](https://img.shields.io/github/stars/Trojan3877/PredictiveMaintenance_IoT.svg?style=social)](https://github.com/Trojan3877/PredictiveMaintenance_IoT/stargazers)
[![Forks](https://img.shields.io/github/forks/Trojan3877/PredictiveMaintenance_IoT.svg?style=social)](https://github.com/Trojan3877/PredictiveMaintenance_IoT/network/members)

A **production-grade IoT Predictive Maintenance system** that ingests sensor data, engineers features, trains machine learning models, and serves real-time failure predictions through an API.



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

Design Questions & Reflections
Q: What problem does this project aim to solve?
A: This project seeks to explore how IoT sensor data can be used to predict mechanical failures before they occur, moving beyond simple logging into actionable insights that help prevent downtime. The focus is on building a pipeline that connects data ingestion, feature extraction, and predictive modeling in a way that mirrors real industrial applications.
Q: Why did I choose this architecture and approach instead of a simpler solution?
A: I chose a structured pipeline that separates data collection, preprocessing, feature engineering, and model training so each part can be reasoned about independently. This is more realistic than a simple model trial and error because industrial systems need clear, reproducible stages that can be monitored and updated separately.
Q: What were the main trade-offs I made?
A: The main trade-off was between fast prototyping and system clarity. I could have thrown together a prototype in one script, but that wouldn’t have given me insight into how each stage affects prediction quality. By building modular steps, I gained maintainability and clarity at the cost of more upfront development time.
Q: What didn’t work as expected?
A: Early versions struggled with noisy sensor readings and inconsistent sampling intervals, which degraded model performance. That taught me how important data cleaning and resampling are for IoT streams, and pushed me to add more robust preprocessing and validation checks before model training.
Q: What did I learn from building this project?
A: I learned that working with real-world data — especially time-series from IoT— requires careful handling of irregularities, missing values, and synchronization across sensors. I also learned how to structure an ML pipeline so the preprocessing logic supports, rather than obscures, model behavior.
Q: If I had more time or resources, what would I improve next?
A: I would add real-time evaluation dashboards and automated alerts so stakeholders can see performance trends and prediction confidence over time. I’d also experiment with uncertainty estimation and online learning techniques to make the system more adaptive to new patterns in the data.


📜 License
This project is licensed under the MIT License.
🙌 Author
Corey Leath
GitHub: https://github.com/Trojan3877
Aspiring AI/ML Engineer building production-ready, end-to-end systems to break into Big Tech & Big AI.
⭐ If you find this project useful, please consider starring the repo!

