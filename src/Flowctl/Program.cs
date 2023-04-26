using Cocona;
using Flowctl;
using Flowctl.Api;
using Flowctl.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

var appBuilder = CoconaApp.CreateBuilder();

var cluster = appBuilder.Configuration.GetSection("Cluster").Get<ClusterConfiguration>();

if (cluster.Address is null) return;

appBuilder.Services.AddRefitClient<IResourcesApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(cluster.Address));

appBuilder.Services.AddRefitClient<IClusterApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(cluster.Address));

appBuilder.Services.AddRefitClient<IEventsApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(cluster.Address));


var app = appBuilder.Build();

app.AddClusterSubCommand();
app.AddResourcesSubCommand();
app.AddEventsSubCommand();

app.Run();