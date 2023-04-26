using System.Text.Json.Nodes;
using Refit;

namespace Flowctl.Api;

public interface IClusterApi
{
    [Get("/api/cluster")]
    Task<IApiResponse<JsonNode>> GetManifestAsync();
}