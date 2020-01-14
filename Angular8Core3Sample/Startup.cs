
using System;
using System.Text;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

using Angular8Core3Sample.Data;
using Angular8Core3Sample.Services;
using Angular8Core3Sample.Models.Identity;
using Angular8Core3Sample.Policies;
using Angular8Core3Sample.MIddleware;

namespace Angular8Core3Sample
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultSQLConnection")));

            // Add ASP.NET Identity support
            services.AddIdentity<ApplicationUser, IdentityRole>(
            opts =>
            {
                opts.Password.RequireDigit = true;
                opts.Password.RequireLowercase = true;
                opts.Password.RequireUppercase = true;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequiredLength = 7;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // Add Authentication with JWT Tokens
            services.AddAuthentication(opts =>
            {
                opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Configuration["Auth:Jwt:Issuer"],
                    ValidAudience = Configuration["Auth:Jwt:Audience"],
                    RoleClaimType = "roles",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Auth:Jwt:Key"])),
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true
                };
            });

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 2000;
            });

            services.AddRazorPages();

            services.AddMvc().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.MetadataPropertyHandling = Newtonsoft.Json.MetadataPropertyHandling.ReadAhead;
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
                options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            });

            services.AddHttpContextAccessor();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator",
                                  policy =>
                                  policy.Requirements.Add(new RoleRequirement("Administrator")));

                options.AddPolicy("UserAccount",
                                    policy =>
                                    policy.Requirements.Add(new RoleRequirement("RegisteredUser")));
            });

            services.AddControllers();

            services.AddScoped<IAuthorizationHandler, AdministratorAuthorizationHandler>();
            services.AddScoped<MyListsService>();
            services.AddScoped<RegistrationService>();
            
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.Configure<RazorViewEngineOptions>(options => options.ViewLocationExpanders.Add(new RazorViewsLocationExpander()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext applicationDbContext)
        {

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

                DbInitializer.InitializeCatalog(applicationDbContext, roleManager, userManager);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = (context) =>
                {
                    // Disable caching for all static files. 
                    context.Context.Response.Headers["Cache-Control"] =
                        Configuration["StaticFiles:Headers:Cache-Control"];
                    context.Context.Response.Headers["Pragma"] =
                        Configuration["StaticFiles:Headers:Pragma"];
                    context.Context.Response.Headers["Expires"] =
                        Configuration["StaticFiles:Headers:Expires"];
                }
            });
            
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseContentSecurityPolicy( cspBuilder => 
            {
                cspBuilder.DefaultSrcsDirective
                            .AllowSelf();

                cspBuilder.ScriptSrcElemsDirective
                            .Allow("https://code.jquery.com/jquery-3.4.1.slim.min.js")
                            .Allow("https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js")
                            .Allow("https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js")
                            .Allow("https://connect.facebook.net/en_US/sdk.js");

                cspBuilder.FrameSrcDirective
                            .AllowSelf()
                            .Allow("https://localhost:8080/");

                cspBuilder.ConnectSrcsDirective
                            .Allow("https://localhost:44349")
                            .Allow("https://localhost:8080")
                            .Allow("wss://localhost:8080");

                cspBuilder.StyleSrcElemsDirective
                            .Allow("https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css")
                            .Allow("https://netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap-glyphicons.css")
                            .Allow("https://use.fontawesome.com/releases/v5.7.1/css/all.css")
                            .Allow("https://localhost:44349/app.css")
                            .AllowUnsafeInline(); // To be fixed

                cspBuilder.ScriptSrcsDirective
                            .AllowSelf();

                cspBuilder.StyleSrcsDirective
                            .AllowSelf()
                            .AllowUnsafeInline(); // To be fixed

                cspBuilder.FontSrcsDirective
                            .Allow("http://netdna.bootstrapcdn.com")
                            .Allow("https://use.fontawesome.com")
                            .Allow("https://localhost:44349");

            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller}/{action=Index}/{id?}");
            });

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                await next();
            });

            app.UseSpa(spa =>
            {

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("https://localhost:8080");
                }
            });
        }
    }
}
