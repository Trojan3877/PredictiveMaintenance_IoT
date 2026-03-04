import pandas as pd
from sklearn.ensemble import RandomForestClassifier
import joblib

df = pd.read_csv("data/sample_sensor_data.csv")

X = df[['temperature','vibration','pressure']]
y = df['failure']

model = RandomForestClassifier(n_estimators=100)
model.fit(X, y)

joblib.dump(model, "models/model.pkl")
