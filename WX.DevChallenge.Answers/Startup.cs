using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WX.DevChallenge.Answers.Models;
using WX.DevChallenge.Answers.Services;

namespace WX.DevChallenge.Answers
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var challengeConfig = Configuration
                .GetSection("ChallengeConfig")
                .Get<ChallengeConfig>();
            services.AddSingleton(challengeConfig);
            services.AddSingleton(typeof(HelperResourceService));

            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // we return Bad Request to non https requests so no redirection needed
            //app.UseHttpsRedirection();

            // return HTTP 500 with a message for AppExceptions. Any other exception let it run the normal course 
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exception = context
                    .Features
                    .Get<IExceptionHandlerPathFeature>()
                    .Error;

                if (!(exception is AppException))
                    return;

                context.Response.ContentType = "application/text";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                
                await context.Response.WriteAsync(exception.Message);
            }));

            app.UseMvc();
        }
    }
}
