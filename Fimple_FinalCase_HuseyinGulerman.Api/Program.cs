
using Fimple_FinalCase_HuseyinGulerman.Core.Entities;
using Fimple_FinalCase_HuseyinGulerman.Repository;
using Fimple_FinalCase_HuseyinGulerman.Service.Mapping;
using Microsoft.AspNetCore.Identity;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Autofac.Extensions.DependencyInjection;
using System.Reflection;
using Fimple_FinalCase_HuseyinGulerman.Api.Modules;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Hangfire;
using Hangfire.PostgreSql;

namespace Fimple_FinalCase_HuseyinGulerman.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddIdentity<AppUser, IdentityRole>()
           .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            builder.Services.AddAutoMapper(typeof(MapProfile));
            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Services.AddDbContext<AppDbContext>(x =>
            {
                x.UseNpgsql(builder.Configuration.GetConnectionString("Postgresql"), options =>
                {
                    options.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
                });
            });
    //         builder.Services.AddHangfire(configuration => configuration
    //.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    //.UseSimpleAssemblyNameTypeSerializer()
    //.UseRecommendedSerializerSettings()
    //.UsePostgreSqlStorage("Postgresql"));

    //        builder.Services.AddHangfireServer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Final Case", Version = "v1" });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Sim Management for IT Company",
                    Description = "Enter JWT Bearer token **only**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
 {
     {securityScheme, new string[] { }}
});
            });
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                    //ValidateLifetime = true,
                    //ClockSkew = TimeSpan.FromMinutes(2)
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}