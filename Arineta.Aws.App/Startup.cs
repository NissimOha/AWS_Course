using System.Reflection;
using System.Text.Json.Serialization;
using Arineta.Aws.Common.Entities;
using Arineta.Aws.Common.IFC;
using Arineta.Aws.DataAccess;
using Arineta.Aws.DataAccess.Repositories;
using Arineta.Aws.Logic;
using Arineta.Aws.Service.Controllers;
using Arineta.Aws.Service.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Arineta.Aws.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            services.AddControllers()
                .AddApplicationPart(Assembly.GetAssembly(typeof(UsersManagementController)))
                .AddControllersAsServices()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddScoped<IManagement<User>, Management<User>>();
            services.AddScoped<IRepository<User>, Repository<User>>();

            services.AddDbContextPool<ApiContext>(options =>
            {
                if (bool.Parse(Configuration.GetSection("IsUseInMemory").Value))
                {
                    options.UseInMemoryDatabase("InMemoryDb");
                }
                else
                {
                    options.UseSqlite(Configuration.GetConnectionString("AwsCourseConnectionString"),
                        m => m.MigrationsAssembly("Arineta.Aws.DataAccess"));
                }

                options.UseLazyLoadingProxies();
            });

            services.AddAutoMapper(Assembly.GetAssembly(typeof(MapperProfile)));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AwsCourse", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication2 v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            if (!bool.Parse(Configuration.GetSection("IsUseInMemory").Value))
            {
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<ApiContext>();
                    context.Database.Migrate();
                }
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
