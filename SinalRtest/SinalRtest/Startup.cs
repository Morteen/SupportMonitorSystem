using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SinalRtest.Hubs;
using SinalRtest.Models;

namespace SinalRtest
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
            services.AddControllers();
            services.AddSignalR();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAny", x =>
                {
                    x.AllowAnyOrigin()
                  .WithOrigins("http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowCredentials()
                    
                    .AllowAnyHeader();
                }
                
                );

            });
          
            string ConnectionString = @"Server=(localdb)\MSSQLLocalDB;Database=TMS;Trusted_Connection= true;";  //ConnectionRetryCount=1
            services.AddDbContext<TMSDbContext>(options =>
         options.UseSqlServer(ConnectionString)

          ); ;

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseCors("AllowAny");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
         
           
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MessageHub>("/signalRmessage");
            });
          
        }
    }
}
