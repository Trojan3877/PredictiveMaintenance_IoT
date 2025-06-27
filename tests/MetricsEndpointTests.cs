// tests/MetricsEndpointTests.cs
//────────────────────────────────────────────────────────────────────────────
// Spins up the PredictorService in-memory using ASP.NET Core's WebApplicationFactory
// and asserts that the /metrics endpoint returns Prometheus plaintext containing
// at least one of our custom metrics (predict_requests_total).
//────────────────────────────────────────────────────────────────────────────

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using PredictiveMaintenance_IoT;
using Xunit;

namespace PredMaint.Tests
{
    public class MetricsEndpointTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public MetricsEndpointTests(WebApplicationFactory<Program> factory)
        {
            // The factory boots Program.cs in memory
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new System.Uri("http://localhost")
            });
        }

        [Fact]
        public async Task Metrics_Endpoint_Returns_Prometheus_Text()
        {
            var response = await _client.GetAsync("/metrics");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            string body = await response.Content.ReadAsStringAsync();
            Assert.Contains("predict_requests_total", body);
            Assert.StartsWith("# HELP", body.TrimStart().Substring(0, 6));
        }
    }
}
