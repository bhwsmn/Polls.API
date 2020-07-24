using System;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polls.API.DbContexts;
using Polls.API.Services;

namespace Polls.API
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
            services.AddControllers(options =>
                {
                    // Return HTTP 406 Not Acceptable if Accept header is anything beside application/json or application/xml
                    options.ReturnHttpNotAcceptable = true;

                    // Fixes the routing issue for async controller methods, when using CreatedAtAction() 
                    options.SuppressAsyncSuffixInActionNames = false;
                }).AddXmlDataContractSerializerFormatters()
                .ConfigureApiBehaviorOptions(options => { options.SuppressMapClientErrors = true; });

            services.AddCors();
            services.AddScoped<IPollsRepository, PollsRepository>();
            services.AddDbContext<PollsContext>(options =>
            {
                options.UseSqlite("Data Source=/Database/Polls.API.db");
            });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            // Change this according to production deployment requirements
            app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}