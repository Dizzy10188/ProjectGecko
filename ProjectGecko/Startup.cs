using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ProjectGecko
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(s => s.EnableEndpointRouting = false);
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            DatabaseConnection.DatabaseConnect();

            app.UseStaticFiles();

            app.UseMvc
            (
                m =>
                {
                    m.MapRoute("Search", "search", new { controller = "Home", action = "Search" });
                    m.MapRoute("Comment", "comment", new { controller = "User", action = "Comment" });
                    m.MapRoute("UserFeed", "{userid:long}", new { controller = "User", action = "ShowFeed" });
                    m.MapRoute("PostLike", "{PostId:long}/AddLike", new { controller = "post", action = "AddLike" });
                    m.MapRoute("RemoveLike", "{PostId:long}/RemoveLike", new { controller = "post", action = "RemoveLike" });
                    m.MapRoute("UserComms", "{userid:long}/commissions", new { controller = "User", action = "UserCommissions" });
                    m.MapRoute("RequestComms", "request", new { controller = "User", action = "RequestCommission" });
                    m.MapRoute("UserAccount", "{userid:long}/account", new { controller = "User", action = "ShowAccount" });
                    m.MapRoute("EditAccount", "{userid:long}/edit", new { controller = "User", action = "EditAccount" });
                    m.MapRoute("PostArt", "{userid:long}/NewPost", new { controller = "Post", action = "CreatePost" });
                    m.MapRoute("ShowPost", "{postid:long}/ShowPost", new { controller = "Post", action = "ShowPost" });
                    m.MapRoute("LogInSignUp", "LogInSignUp", new { controller = "Home", action = "LogInSignUp" });
                    m.MapRoute("LogIn", "LogIn", new { controller = "User", action = "LogIn" });
                    m.MapRoute("CreateAccount", "CreateAccount", new { controller = "User", action = "CreateAccount" });
                    m.MapRoute("Home", "/", new { controller = "Home", action = "Index"});
                    m.MapRoute("Catchall", "{**a}", new { controller = "Home", action = "Index" });
                }
            );
        }
    }
}
