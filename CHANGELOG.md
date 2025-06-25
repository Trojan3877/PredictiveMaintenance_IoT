# Changelog
All notable changes to **PredictiveMaintenance IoT** will be documented in this file.

This project follows **Semantic Versioning 2.0.0** and the  
[Keep a Changelog](https://keepachangelog.com/en/1.1.0/) specification.

---

## [Unreleased]
### Added
- Modular C# pipeline: `DataIngest`, `FeatureEngineer`, `ModelTrainer`, `Predictor`.
- Prometheus + OTEL instrumentation; `/metrics` endpoint.
- xUnit unit tests & integration tests (Feature + Trainer).
- Multi-stage Dockerfile (self-contained .NET 8 build).
- Helm chart (`values.yaml`, deployment, service) + chart metadata.
- GitHub Actions: build + coverage, container-security scan (Trivy).
- Flow-chart architecture diagram and comprehensive README.
- Governance files: LICENSE (MIT) & CONTRIBUTING guide.
- Kafka interface abstraction for testability.

### Changed
- README upgraded with badges, KPIs, and quick-start instructions.

---

## [0.1.0] – 2025-07-02
### Added
- Initial proof-of-concept: console app reading CSV, training ML.NET model, printing predictions.

[Unreleased]: https://github.com/Trojan3877/PredictiveMaintenance_IoT/compare/v0.1.0...HEAD
[0.1.0]: https://github.com/Trojan3877/PredictiveMaintenance_IoT/releases/tag/v0.1.0
