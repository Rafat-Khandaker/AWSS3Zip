using AWSS3Zip.Commands;
using AWSS3Zip.Commands.Contracts;
using AWSS3Zip.Start;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class Program
{
    private static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        var service = host.Services.GetRequiredService<CommandLineRunner>();
        service.Run(args);
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ExtractJob, ExtractJob>();
                services.AddSingleton<IProcessFactory<IProcessJob>, JobFactory>();

                services.AddTransient<CommandLineRunner, CommandLineRunner>();
            });


}