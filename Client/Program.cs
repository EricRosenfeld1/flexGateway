using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Radzen;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace flexGateway.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<NotificationService>();

            var unhandledExceptionSender = new UnhandledExceptionSender();
            var unhandledExceptionProvider = new UnhandledExceptionProvider(unhandledExceptionSender);
            builder.Logging.AddProvider(unhandledExceptionProvider);
            builder.Services.AddSingleton<IUnhandledExceptionSender>(unhandledExceptionSender);

            await builder.Build().RunAsync();
        }
    }
}
