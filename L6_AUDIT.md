# PredictiveMaintenance IoT — L6 Engineering Audit

## Executive assessment

The repository has useful portfolio depth: C#/ML.NET predictive-maintenance components, a Python FastAPI diagnostic gateway, local `llama-cpp-python` inference, deterministic safety post-processing, readiness semantics, containerization, and a broad CI/security surface. The strongest engineering choice is the explicit fail-closed behavior when the model artifact is unavailable outside test mode.

The largest risk is evidence drift. Historical README text presented precise latency, throughput, F1, TTFT, memory, format-compliance, and production-readiness claims that are not reproduced by the committed CI benchmark. The current performance test explicitly disables the real LLM and enables mock diagnostics, so its measurements must be described as mock-path/API regression evidence only.

## Verified Python serving path

1. `POST /diagnose` validates `SensorData`.
2. Non-anomalous requests bypass generation.
3. An anomalous request uses the mounted Phi-3 GGUF if available.
4. If the model is missing, the service returns 503 unless `ALLOW_MOCK_DIAGNOSTICS` was explicitly enabled.
5. Real model output is passed through the deterministic safety agent before it is returned.
6. `/health` reports process state; `/ready` fails when the required diagnostic backend is unavailable.

The C# ML.NET modules exist in the same repository but are not called by this Python request path. Treat them as adjacent components until an integration boundary is implemented and tested.

## Release/package root cause

The previous GHCR workflow built `IMAGE_NAME` from `${{ github.repository }}`. The repository name `PredictiveMaintenance_IoT` contains uppercase characters. Docker/GHCR image repository names must be lowercase, so that path is not a safe package identifier.

The release baseline fixes the package target to:

`ghcr.io/coreyleath-code/predictivemaintenance-iot`

The workflow also supports an explicit semantic `release_tag` for manual recovery and checks out that tag before publishing.

## P0 gaps

### Real model evaluation

Add a dedicated evaluation job that actually loads a pinned GGUF model. Record model revision, artifact SHA-256, llama.cpp version, hardware, threads, decoding configuration, prompts, warm-up, sample count, and raw outputs. Do not mix those results with the mock-path CI benchmark.

### Detection-quality evidence

No field-representative, versioned dataset and split protocol is established by the serving tests. Before publishing F1/precision/recall as model evidence, commit or reference an immutable dataset manifest, define leakage controls, and record exact training/evaluation commands.

### Safety case

The deterministic safety agent is a useful defense layer but is not safety certification. Build a versioned scenario corpus with expected overrides, negative cases, false-negative analysis, and domain-expert review before claiming safe industrial guidance.

## P1 gaps

- Pin runtime dependencies for release reproducibility.
- Separate runtime/test/LLM dependency manifests.
- Add request-size limits, authentication, authorization, rate limits, and abuse controls before external exposure.
- Add structured audit logging with privacy/redaction rules.
- Define the contract between C# prediction and Python diagnostics if both are meant to operate in one system.
- Add real HTTP load tests with controlled concurrency and tail-latency/error-rate reporting.
- Add timeout/cancellation/resource-budget controls around local LLM inference.

## P2 supply-chain improvements

- Record GHCR digest in the GitHub Release.
- Attach SBOM and provenance/attestation artifacts.
- Verify the GGUF artifact by SHA-256 before startup.
- Pin high-impact GitHub Actions by immutable commit SHA.
- Consider a model-artifact manifest that binds release version, model digest, inference engine version, and expected resource envelope.

## Container decision

The v1.0.3 baseline removes the mutable multi-GB model download from `docker build`. The image now packages the application and inference runtime while the reviewed model artifact is mounted separately. This makes package publication cheaper, more reproducible, and easier to audit.

## Release acceptance criteria

- CI/security checks pass on the release PR.
- Docker image builds successfully without downloading model weights.
- GHCR target is lowercase and package-write permission is present.
- README contains no unsupported real-model or production-capacity measurements.
- Mermaid architecture/system-design diagrams render on GitHub.
- Release tag is immutable and the package is built from that exact tag.
- GHCR publishes both the semantic version and `latest` tags.

## Recommended next milestone

A strong next milestone would add a small, legally distributable evaluation fixture plus a hardware-bound real-Phi-3 benchmark. Keep three evidence classes separate: predictive model quality, LLM diagnostic performance, and API/container regression performance.