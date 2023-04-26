using System.Text.Json;
using Cocona;
using Cocona.Builder;
using Flowctl.Api;
using Spectre.Console;

namespace Flowctl.Commands;

public static class ClusterSubCommand
{
    public static void AddClusterSubCommand(this ICoconaAppBuilder builder)
    {
        builder.AddSubCommand("cluster", configure =>
        {
            configure.AddCommand("inspect", async ([FromService] IClusterApi api) =>
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
    }
}