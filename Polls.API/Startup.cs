using System;
using System.IO;
using System.Net;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Polls API",
                    Description = "An API for a minimalistic polls/survey service.",
                    License = new OpenApiLicense
                    {
                        Name = "GNU General Public License v3.0",
                        Url = new Uri("https://www.gnu.org/licenses/gpl-3.0.en.html"),
                    }
                });
            });
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

            app.UseSwagger();
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Polls API");
                
                c.RoutePrefix = string.Empty;
            });
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}