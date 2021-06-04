using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GraphQL.Server;

namespace Mars
{
    public class Startup
    {
        const string NasaSectionName = "NasaData";
        static string CorsNamePolicy = "ClientPolicy";

        static string UseFakeData = "UseFakeData";
        static string FakeData = "FakeData";
        IConfiguration configuration;
        public Startup(IConfiguration _config)
        {
            configuration = _config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var path = configuration.GetValue<string>(CorsNamePolicy);
            services.AddCors(options =>
            {
                options.AddPolicy(name: CorsNamePolicy,
                    builder =>
                    {
                        builder.AllowAnyHeader()
                           .WithMethods("GET", "POST")
                           .WithOrigins(path);
                    });
            });

            services.AddOptions<AppSettings>().
                Bind(configuration.GetSection(NasaSectionName));
            services.AddOptions<FakeDataPath>().Bind(configuration.GetSection(FakeData));

            var useFakeData = configuration.GetValue<bool>(UseFakeData);
            if (useFakeData)
            {
                services.AddSingleton<INasaStream, FakeNasaStream>();
            }
            else
            {
                services.AddHttpClient<INasaStream, NasaStream>();
            }
            services.AddSingleton<INasaProvider, NasaProvider>();

            services.AddSingleton<SolDataQuery>();
            services.AddSingleton<SolDataMutation>();
            services.AddSingleton<DataDescriptionType>();
            services.AddSingleton<SeasonEnum>();
            services.AddSingleton<RoverInfoType>();
            services.AddSingleton<MarsWeatherType>();
            services.AddSingleton<ISchema, SolSchema>();

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
            app.UseCors(CorsNamePolicy);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseGraphQL<ISchema>();
            app.UseGraphQLPlayground();
        }
    }
}
