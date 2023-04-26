using System.Text.Json.Nodes;
using Refit;

namespace Flowctl.Api;

public interface IResourcesApi
{
    [Get("/api/tenants/{tenant}/resources/{resource}")]
    Task<IApiResponse<JsonNode>> GetResourceAsync(string tenant, string resource);

    [Get("/api/tenants/{tenant}/resources")]
    Task<ApiResponse<JsonNode>> GetResourcesAsync(string tenant);
}