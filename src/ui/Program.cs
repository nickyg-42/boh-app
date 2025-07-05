using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ui;
using ui.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://192.168.0.182:5099/") });
builder.Services.AddScoped<VerbService>();
builder.Services.AddScoped<VerbOfTheDayStateService>();

await builder.Build().RunAsync();
