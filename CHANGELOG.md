# Changelog

All notable changes to **PredictiveMaintenance IoT** are documented here.

The project follows Semantic Versioning and the Keep a Changelog format.

## [Unreleased]

## [1.0.3] - 2026-08-22

### Fixed

- Corrected GHCR publication to use the lowercase package name `ghcr.io/coreyleath-code/predictivemaintenance-iot`.
- Added explicit semantic-tag input for manual package-publish recovery.
- Ensured the container package is built from the selected immutable release tag.

### Changed

- Externalized the Phi-3 GGUF artifact from the Docker build instead of downloading mutable multi-GB model weights from an upstream `main` URL during image publication.
- Rebuilt the README with an evidence-first badge block, architecture and system-design Mermaid diagrams, clean-clone Quickstart, reproducibility contract, benchmark boundaries, release/package instructions, and reviewer Q&A.
- Removed unsupported presentation of mock-path benchmark results as real Phi-3 latency, TTFT, generation-speed, memory, field-quality, or production-capacity evidence.
- Added an L6 engineering audit covering model provenance, evaluation, safety, integration, load testing, and supply-chain gaps.

## [0.2.0] - 2025-07-03

### Added

- Modular C# pipeline (`DataIngest`, `FeatureEngineer`, `ModelTrainer`, `Predictor`).
- Prometheus/OTEL instrumentation and metrics support.
- Unit/integration tests and synthetic-data tooling.
- Container, Helm, CI/security, governance, and documentation foundations.

## [0.1.0] - 2025-07-02

### Added

- Initial predictive-maintenance proof of concept.

[Unreleased]: https://github.com/CoreyLeath-code/PredictiveMaintenance_IoT/compare/v1.0.3...HEAD
[1.0.3]: https://github.com/CoreyLeath-code/PredictiveMaintenance_IoT/compare/v1.0.2...v1.0.3
[0.2.0]: https://github.com/CoreyLeath-code/PredictiveMaintenance_IoT/compare/v0.1.0...v0.2.0
[0.1.0]: https://github.com/CoreyLeath-code/PredictiveMaintenance_IoT/releases/tag/v0.1.0
