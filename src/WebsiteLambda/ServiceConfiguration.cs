using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.DependencyInjection;
using WebsiteLambda.Data.Interface;
using Amazon.S3;
using WebsiteLambda.Business.Interface;
using WebsiteLambda.Business;
using WebsiteLambda.Data;
using WebsiteLambda.Mapper;
using WebsiteLambda.Data.Mapper;
using WebsiteLambda.Business.Helpers;

namespace WebsiteLambda
{
    public static class ServiceConfiguration
    {
        public static void ConfigureBusinessLayer(this IServiceCollection services)
        {
            services.AddScoped<IProjectManager, ProjectManager>();
            services.AddScoped<IProjectUpdateHelper, ProjectUpdateHelper>();
        }

        public static void ConfigureDataLayer(this IServiceCollection services)
        {
            services.AddScoped<IProjectAccessor, ProjectAccessor>();
            services.AddScoped<IAmazonDynamoDB, AmazonDynamoDBClient>();

            services.AddScoped<IDynamoDBContext>(x =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var client = serviceProvider.GetService<IAmazonDynamoDB>();

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

        public static void ConfigureAutoMapaper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile), typeof(DataAutoMapperProfile));
        }
    }
}
