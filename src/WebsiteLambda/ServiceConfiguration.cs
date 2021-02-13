using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Website.Data.Interface;
using Website.DataLayer;
using Website.Logic;
using Website.Logic.Interface;
using Microsoft.Extensions.Hosting;
using Amazon.S3;

namespace WebsiteLambda
{
    public static class ServiceConfiguration
    {
        public static void ConfigureLogicLayer(this IServiceCollection services)
        {
            services.AddScoped<IProjectManager, ProjectManager>();
        }

        public static void ConfigureDataLayer(this IServiceCollection services, IWebHostEnvironment env)
        {
            services.AddScoped<IProjectAccessor, ProjectAccessor>();
            services.AddScoped<IDynamoDBContext>(x =>
            {
                var client = new AmazonDynamoDBClient();

                if (env.IsDevelopment())
                {

                }

                return new DynamoDBContext(client);
            });
            services.AddScoped<IAmazonS3>(x =>
            {
                var clientConfig = new AmazonS3Config()
                {

                };

                return new AmazonS3Client();
            });
        }
    }
}
