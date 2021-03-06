using MicroElements.Swashbuckle.NodaTime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using Refor.Services;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Refor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<RandomNumberGenerator, RNGCryptoServiceProvider>();
            services.AddSingleton<IRandomStringService, RandomStringService>();
            services.AddScoped<ITextStoreService, TextStoreService>();
            services.AddSingleton<IClock>(SystemClock.Instance);

            services.AddDbContext<ReforContext>(options =>
            {
                // We load our configuration from ConnectionStrings:Default (appsettings.json or environment variable).
                // We also register NodaTime into the DbContext. Without it, EF cannot serialize/deserialize NodaTime types like Instant.
                options.UseNpgsql(Configuration.GetConnectionString("Default"), options => options.UseNodaTime());

                // We use the snake case naming convention in the database since that's the standard for Npgsql.
                options.UseSnakeCaseNamingConvention();
            });

            services.AddControllers().AddJsonOptions(c =>
            {
                c.JsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
                c.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Refor", Version = "v1" });

                JsonSerializerOptions jsonOptions = new();
                jsonOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
                jsonOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                c.ConfigureForNodaTimeWithSystemTextJson(jsonOptions);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ReforContext reforContext, ILogger<ReforContext> logger)
        {
            logger.LogInformation("Ensuring that the database is created and migrated.");
            reforContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Refor v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
