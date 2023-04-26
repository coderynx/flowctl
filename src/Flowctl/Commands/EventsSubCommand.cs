using System.Text.Json;
using Cocona;
using Cocona.Builder;
using Flowctl.Api;
using Spectre.Console;

namespace Flowctl.Commands;

public static class EventsSubCommand
{
    public static void AddEventsSubCommand(this ICoconaCommandsBuilder builder)
    {
        builder.AddSubCommand("events", configure =>
        {
            configure.AddCommand("ls", async ([Argument] string tenant, [FromService] IEventsApi api) =>
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
    }
}