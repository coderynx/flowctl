using System.Text.Json;
using Cocona;
using Cocona.Builder;
using Flowctl.Api;
using Spectre.Console;

namespace Flowctl.Commands;

public static class ResourcesSubCommand
{
    public static void AddResourcesSubCommand(this ICoconaAppBuilder builder)
    {
        builder.AddSubCommand("resources", configure =>
        {
            configure.AddCommand("inspect",
                async ([Argument] string tenant, [Argument] string resource, [FromService] IResourcesApi api) =>
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


            configure.AddCommand("ls", async ([Argument] string tenant, [FromService] IResourcesApi api) =>
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
    }
}