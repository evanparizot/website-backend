using Amazon.XRay.Recorder.Handlers.AwsSdk;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WebsiteLambda.Middleware;
using WebsiteLambda.Models.Configuration;

namespace WebsiteLambda
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            AWSSDKHandler.RegisterXRayForAllServices();
            
            services.ConfigureAutoMapaper();
            services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.IgnoreNullValues = true;
                });
            services.ConfigureBusinessLayer();
            services.ConfigureDataLayer();
            services.Configure<AwsResourceConfig>(Configuration.GetSection(AwsResourceConfig.ConfigKey));

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Website API",
                    Description = "",
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (!env.IsProduction())
            {
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "/website/swagger/{documentName}/swagger.json";
                });
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/website/swagger/v1/swagger.json", "V1");
                    c.RoutePrefix = "website/swagger";
                });
            }

            app.UseXRay("WebsiteLambda");
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors(x => 
            {
                x.AllowAnyOrigin();
                x.AllowAnyMethod();
                x.AllowAnyHeader();
            });

            //app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseEndpoints(e =>
            {
                e.MapControllers();
            });
        }
    }
}
