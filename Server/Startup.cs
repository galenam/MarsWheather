using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GraphQL.Server;
using System;

namespace Mars
{
    public class Startup
    {
        const string NasaSectionName = "NasaData";
        IConfiguration configuration;
        public Startup(IConfiguration _config)
        {
            configuration = _config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions<AppSettings>().
                Bind(configuration.GetSection(NasaSectionName));
            services.AddHttpClient<INasaStream, NasaStream>();
            services.AddSingleton<INasaProvider, NasaProvider>();
            services.AddSingleton<INasaStream, NasaStream>();

            services.AddSingleton<ISchema, SolSchema>();
            services.AddSingleton<SolDataQuery>();
            services.AddSingleton<SolDataMutation>();
            services.AddSingleton<DataDescriptionType>();
            services.AddSingleton<SeasonEnum>();
            services.AddSingleton<MarsWheatherType>();

            services.Configure<KestrelServerOptions>(options =>
   {
       options.AllowSynchronousIO = true;
   });

            services.AddGraphQL(options =>
            {
                options.EnableMetrics = true;
            })
            .AddSystemTextJson(deserializerSettings => { }, serializerSettings => { })
            //.AddUserContextBuilder(httpContext => new GraphQLUserContext { User = httpContext.User })
            .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true);
          
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });

            app.UseGraphQL<ISchema>();
            app.UseGraphQLPlayground();
        }
    }
}
