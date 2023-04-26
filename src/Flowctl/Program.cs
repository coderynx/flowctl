using System.Text.Json;
using Cocona;
using Flowctl;
using Flowctl.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Spectre.Console;

var appBuilder = CoconaApp.CreateBuilder();

var cluster = appBuilder.Configuration.GetSection("Cluster").Get<ClusterConfiguration>();

if (cluster.Address is null)
{
    return;
}

appBuilder.Services.AddRefitClient<IResourcesApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(cluster.Address));

appBuilder.Services.AddRefitClient<IClusterApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(cluster.Address));

appBuilder.Services.AddRefitClient<IEventsApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(cluster.Address));


var app = appBuilder.Build();

app.AddSubCommand("cluster", builder =>
{
    builder.AddCommand("inspect", async ([FromService] IClusterApi api) =>
    {
        AnsiConsole.WriteLine("Retrieving Flownodes cluster manifest");
        
        var result = await api.GetManifestAsync();
        if (!result.IsSuccessStatusCode)
        {
            AnsiConsole.MarkupLine("[red]Error[/] Could not retrieve cluster manifest");
            return;
        }

        var text = result.Content!.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
        AnsiConsole.WriteLine(text);
        
        AnsiConsole.WriteLine("Successfully Retrieved Flownodes cluster manifest");
    });
});

app.AddSubCommand("resources", builder =>
{
    builder.AddCommand("inspect", async ([Argument] string tenant, [Argument] string resource, [FromService] IResourcesApi api) =>
    {
        AnsiConsole.WriteLine("Retrieving resource from Flownodes cluster...");

        var result = await api.GetResourceAsync(tenant, resource);
        if (!result.IsSuccessStatusCode)
        {
            AnsiConsole.MarkupLine("[red]Error[/] Could not retrieve resources");
            return;
        }

        var text = result.Content!.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
        AnsiConsole.WriteLine(text);
    });
    
    
    builder.AddCommand("ls", async ([Argument] string tenant, [FromService] IResourcesApi api) =>
    {
        AnsiConsole.WriteLine("Retrieving resource from Flownodes cluster...");

        var result = await api.GetResourcesAsync(tenant);
        if (!result.IsSuccessStatusCode)
        {
            AnsiConsole.MarkupLine("[red]Error[/] Could not retrieve resources");
            return;
        }

        var text = result.Content!.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
        AnsiConsole.WriteLine(text);
    });
});

app.AddSubCommand("events", builder =>
{
    builder.AddCommand("ls", async ([Argument] string tenant, [FromService] IEventsApi api) =>
    {
        AnsiConsole.WriteLine("Retrieving events from Flownodes cluster...");

        var result = await api.GetEventsAsync(tenant);
        if (!result.IsSuccessStatusCode)
        {
            AnsiConsole.MarkupLine("[red]Error[/] Could not retrieve events");
            return;
        }

        var text = result.Content!.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
        AnsiConsole.WriteLine(text);
    });
});


app.Run();