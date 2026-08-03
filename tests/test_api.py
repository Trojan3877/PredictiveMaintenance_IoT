"""Unit tests for the FastAPI prediction and diagnostic endpoints."""

from fastapi.testclient import TestClient

# Import the FastAPI app instance from your source code
# Note: Adjust the import path 'src.api.main' if your app is located in a different directory
from src.api import main as api_main

client = TestClient(api_main.app)

def test_health_check():
    """Verify the API boots correctly. Essential for Tier 6 container validation."""
    response = client.get("/health")
    assert response.status_code == 200
    # Update the expected JSON below if your health endpoint returns something different
    assert response.json()["status"] == "healthy"

def test_diagnostic_endpoint_healthy_sensor():
    """Verify the diagnostic agent bypasses the LLM when no anomaly is detected."""
    payload = {
        "sensor_id": 101,
        "temperature": 45.2,
        "vibration": 0.12,
        "anomaly_detected": False
    }
    
    response = client.post("/diagnose", json=payload)
    assert response.status_code == 200
    
    data = response.json()
    assert data["status"] == "Healthy"
    assert "None required" in data["mitigation"]

def test_diagnostic_endpoint_critical_anomaly(monkeypatch):
    """Verify explicit CI mode can exercise the diagnostic fallback."""
    monkeypatch.setattr(api_main, "llm", None)
    monkeypatch.setattr(api_main, "ALLOW_MOCK_DIAGNOSTICS", True)
    payload = {
        "sensor_id": 102,
        "temperature": 95.5,
        "vibration": 5.1,
        "anomaly_detected": True
    }

    response = client.post("/diagnose", json=payload)
    assert response.status_code == 200
    
    data = response.json()
    assert data["sensor_id"] == 102
    assert "mitigation_plan" in data
    assert isinstance(data["mitigation_plan"], str)
    
    # Ensure the LLM actually generated text and didn't return an empty string
    assert len(data["mitigation_plan"]) > 0


def test_missing_model_fails_closed_without_explicit_mock_mode(monkeypatch):
    """A model-less deployment must not return a realistic-looking mock mitigation."""
    monkeypatch.setattr(api_main, "llm", None)
    monkeypatch.setattr(api_main, "ALLOW_MOCK_DIAGNOSTICS", False)

    response = client.post(
        "/diagnose",
        json={
            "sensor_id": 103,
            "temperature": 95.5,
            "vibration": 5.1,
            "anomaly_detected": True,
        },
    )

    assert response.status_code == 503
    assert response.json()["detail"] == "Diagnostic model is unavailable"


def test_readiness_requires_model_outside_explicit_mock_mode(monkeypatch):
    monkeypatch.setattr(api_main, "llm", None)
    monkeypatch.setattr(api_main, "ALLOW_MOCK_DIAGNOSTICS", False)

    response = client.get("/ready")

    assert response.status_code == 503
