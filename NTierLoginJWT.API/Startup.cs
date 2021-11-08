using NTierLoginJWT.BLL.Services;
using NTierLoginJWT.DAL.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WebApi.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace NTierLoginJWT.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // add services to the DI container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            services.AddDbContext<CRUDContext>(Options =>
            {
                Options.UseSqlServer(Configuration.GetConnectionString("DbUsersss"));
            });
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<CRUDContext>();
            // configure strongly typed settings object
            //services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddScoped<IUserService, UserService>();
            services.AddMvc();


        }

        // configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());


            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(x => x.MapControllers());

        }
    }
}
