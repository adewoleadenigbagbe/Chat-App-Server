using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalR_Chat_Server.Concrete;
using SignalR_Chat_Server.Model;

namespace SignalR_Chat_Server
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DataBaseSettings>(Configuration.GetSection("DatabaseSettings"));
            services.AddOptions();
            services.AddCors(o => o.AddPolicy("EnableCORS", builder =>
            {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithOrigins("*")
                .AllowCredentials();
            }));
            services.AddSignalR();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseCors("CorsPolicy");
             
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chathub");
            });
            app.UseMvc();
            app.UseCors("EnableCORS");

        }
    }
}
