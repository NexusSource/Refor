using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Refor.Services;
using System.Security.Cryptography;

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
            services.AddSingleton<IRandomStringService, RandomStringService>();
            services.AddSingleton<RandomNumberGenerator, RNGCryptoServiceProvider>();
            services.AddScoped<ITextStoreService, TextStoreService>();

            services.AddDbContext<ReforContext>(options =>
            {
                // We load our configuration from ConnectionStrings:Default (appsettings.json or environment variable).
                // We also register NodaTime into the DbContext. Without it, EF cannot serialize/deserialize NodaTime types like Instant.
                options.UseNpgsql(Configuration.GetConnectionString("Default"), options => options.UseNodaTime());

                // We use the snake case naming convention in the database since that's the standard for Npgsql.
                options.UseSnakeCaseNamingConvention();
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Refor", Version = "v1" });
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
