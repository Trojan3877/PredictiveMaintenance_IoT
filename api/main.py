from fastapi import FastAPI
from pydantic import BaseModel
import joblib
import numpy as np

app = FastAPI(title="Predictive Maintenance IoT API")

model = joblib.load("models/model.pkl")

class SensorInput(BaseModel):
    temperature: float
    vibration: float
    pressure: float

@app.post("/predict")
def predict(data: SensorInput):
    features = np.array([[data.temperature, data.vibration, data.pressure]])
    prediction = model.predict(features)
    return {"failure_risk": int(prediction[0])}
