using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using dMyBlogAPI.Models.EMail;
using dMyBlogAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace dMyBlogAPI
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_AllowOrigins";
        public static IHostingEnvironment _env { get; set; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(c =>
               c.AddPolicy(MyAllowSpecificOrigins, b => { b.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4201", "https://www.dmyblog.co"); })
            );

            services.AddScoped<PostService>();
            services.AddScoped<InteractionService>();
            services.AddScoped<CommentService>();
            services.AddScoped<SharesSevice>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            services.AddSingleton<IEmailConfiguration>(Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
            services.AddTransient<IEmailService, EmailService>();

            services.AddResponseCompression(options =>
            {
                IEnumerable<string> MimeTypes = new[]
                      {                      
                             "text/plain",
                             "text/css",
                             "font/woff2",
                             "application/javascript",
                             "image/x-icon",
                             "image/png","image/jpeg"
                         };
                options.EnableForHttps = true;
                options.MimeTypes = MimeTypes;
                options.Providers.Add<GzipCompressionProvider>();
                options.Providers.Add<BrotliCompressionProvider>();
            });
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.Use(async (context, nextMiddleware) =>
            {
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers.Add("can't-be-evil", "true");
                    return Task.FromResult(0);
                });
                await nextMiddleware();

            });
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();


            app.UseResponseCompression();

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //    name: "default",
            //    template: "{controller=Home}/{action=Index}/{id?}");


            //});

            app.UseMvcWithDefaultRoute().UseRewriter(new RewriteOptions().AddRedirectToWwwPermanent().AddRedirectToHttpsPermanent());
        }
    }
}
