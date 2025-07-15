using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateEmptyApplicationBuilder(settings: null);

builder.Services.AddMcpServer()
  .WithStdioServerTransport()
  .WithToolsFromAssembly();

builder.Services.AddSingleton(_ =>
    {
    var client = new HttpClient() { BaseAddress = new Uri("https://potterapi-fedeperin.vercel.app") };
    return client;
});

var app = builder.Build();

await app.RunAsync();
