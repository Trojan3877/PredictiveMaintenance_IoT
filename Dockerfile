###############################################################################
# PredictiveMaintenance IoT — Dockerfile
# ---------------------------------------------------------------------------
# Stage 1 : build   →   dotnet publish -c Release
# Stage 2 : runtime →   copy self-contained, trimmed binary
###############################################################################

########################  Stage 1 : build / publish  ##########################
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY *.sln ./
COPY src/ src/
COPY tests/ tests/

# Restore dependencies
RUN dotnet restore

# Publish self-contained (linux-x64) binary, trimmed
RUN dotnet publish src/PredictiveMaintenance_IoT.csproj \
    -c Release \
    -o /publish \
    -r linux-x64 \
    --self-contained true \
    -p:PublishReadyToRun=true \
    -p:PublishTrimmed=true

########################  Stage 2 : runtime  ##################################
FROM mcr.microsoft.com/dotnet/runtime-deps:8.0 AS runtime
WORKDIR /app

# Copy published bits
COPY --from=build /publish .

# Minimal runtime ENV
ENV ASPNETCORE_URLS=http://0.0.0.0:8080 \
    DOTNET_EnableDiagnostics=0 \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

EXPOSE 8080
ENTRYPOINT ["./PredictiveMaintenance_IoT"]
