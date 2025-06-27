# Changelog
# Changelog
All notable changes to **PredictiveMaintenance IoT** will be documented here.

This project follows **Semantic Versioning 2.0.0** and the  
[Keep a Changelog](https://keepachangelog.com/en/1.1.0/) spec.

---

## [Unreleased]
### Added
- (next features go here)

---

## [0.2.0] – 2025-07-03
### Added
- Modular C# pipeline (`DataIngest`, `FeatureEngineer`, `ModelTrainer`, `Predictor`).
- Prometheus/OTEL instrumentation & `/metrics` endpoint.
- Unit + integration tests, synthetic-data generator.
- Multi-stage Dockerfile; GHCR publish & Cosign sign workflow.
- Helm chart: values, deployment, service, chart metadata.
- CI workflows: build-test-coverage, container scan, docker publish.
- Governance docs (LICENSE, CONTRIBUTING, CODE_OF_CONDUCT), CHANGELOG.
- Grafana dashboard JSON & architecture flow-chart.
- MkDocs site config.

### Changed
- README upgraded with badges, KPIs, and Quick Start.

---

## [0.1.0] – 2025-07-02
### Added
- Initial console-app proof of concept.

[Unreleased]: https://github.com/Trojan3877/PredictiveMaintenance_IoT/compare/v0.2.0...HEAD
[0.2.0]: https://github.com/Trojan3877/PredictiveMaintenance_IoT/compare/v0.1.0...v0.2.0
[0.1.0]: https://github.com/Trojan3877/PredictiveMaintenance_IoT/releases/tag/v0.1.0


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
