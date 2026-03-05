import streamlit as st
import requests

API_URL = "http://localhost:8000/predict"

st.set_page_config(page_title="Predictive Maintenance Dashboard", layout="wide")

st.title("🏭 Predictive Maintenance IoT Dashboard")

st.markdown("Enter sensor readings to predict equipment failure risk.")

# Sensor inputs
temperature = st.slider("Temperature (°C)", 0.0, 150.0, 75.0)
vibration = st.slider("Vibration (mm/s)", 0.0, 50.0, 10.0)
pressure = st.slider("Pressure (psi)", 0.0, 300.0, 120.0)

if st.button("Predict Failure Risk"):

    payload = {
        "temperature": temperature,
        "vibration": vibration,
        "pressure": pressure
    }

    try:
        response = requests.post(API_URL, json=payload)
        result = response.json()

        failure_risk = result["failure_risk"]

        if failure_risk == 1:
            st.error("⚠ High Risk of Equipment Failure")
        else:
            st.success("✅ Equipment Operating Normally")

    except Exception as e:
        st.error(f"API Error: {e}")
