using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace WebsiteLambda
{
    public class LambdaEntryPoint : Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
    {

        protected override void Init(IWebHostBuilder builder)
        {
            builder
                .ConfigureAppConfiguration((context, config) =>
                {
                    var env = context.HostingEnvironment;

                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                    config.AddEnvironmentVariables();
                })
                .UseStartup<Startup>();
        }
    }
}
