using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.DependencyInjection;
using Website.Data.Interface;
using Website.DataLayer;
using Website.Logic;
using Website.Logic.Interface;
using Amazon.S3;
using Website.Data.Mapper;
using WebsiteLambda.Mapper;

namespace WebsiteLambda
{
    public static class ServiceConfiguration
    {
        public static void ConfigureLogicLayer(this IServiceCollection services)
        {
            services.AddScoped<IProjectManager, ProjectManager>();
        }

        public static void ConfigureDataLayer(this IServiceCollection services)
        {
            services.AddScoped<IProjectAccessor, ProjectAccessor>();
            services.AddScoped<IDynamoDBContext>(x =>
            {
                var client = new AmazonDynamoDBClient();

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
