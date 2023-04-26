using System.Text.Json.Nodes;
using Refit;

namespace Flowctl.Api;

public interface IEventsApi
{
    [Get("/api/tenants/{tenant}/events")]
    Task<IApiResponse<JsonNode>> GetEventsAsync(string tenant);
}