using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SM.DAL.Dao_interfaces;
using SM.DAL.Dao;
using SM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SM.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SM.Bll;
using SM.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SM.SmartContractInteraction;

namespace SM.WebAPI
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

			services.AddScoped<IDaoUser, DaoUser>();
			services.AddScoped<IDaoRealEstate, DaoRealEstate>();
			services.AddScoped<IDaoRent, DaoRent>();

			services.AddScoped<IBllRealEstate, BllRealEstate>();
			services.AddScoped<IBllUser, BllUser>();
			services.AddScoped<IBllRent, BllRent>();

		

			services.AddAutoMapper(typeof(MappingProfile));
			services.AddHostedService<BackgroundRentTasks>();
			services.AddHostedService<EventListenerWeb3>();


			
			var key = Encoding.ASCII.GetBytes(Configuration.GetSection("Jwt")["SecretKey"]);

			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(x =>
			{
				x.RequireHttpsMetadata = false;
				x.SaveToken = true;
				x.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = true,
					ValidIssuer= Configuration.GetSection("Jwt")["TokenIssuer"],
					ValidateAudience = true,
					ValidAudience= Configuration.GetSection("Jwt")["TokenAudience"],
					ValidateLifetime=true,
					RequireExpirationTime = true,
					ClockSkew = TimeSpan.Zero
				};
			});


			services.AddCors(o => o.AddPolicy("AllowOrigin", builder =>
			{
				builder.WithOrigins(new[] { "https://localhost:3000", "http://localhost:3000", "http://localhost:44339", "http://localhost:44344", "http://127.0.0.1:3000" })
					   .AllowAnyMethod()
					   .AllowAnyHeader()
					   .AllowCredentials()
					   .WithExposedHeaders("Authorization");
			}));
			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "SM.WebAPI", Version = "v1" });
				c.AddSecurityDefinition("Bearer", //Name the security scheme
					new OpenApiSecurityScheme
					{
						Description = "JWT Authorization header using the Bearer scheme.",
						Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
						Scheme = "bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
					});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement{
					{
						new OpenApiSecurityScheme{
							Reference = new OpenApiReference{
								Id = "Bearer", //The name of the previously defined security scheme.
								Type = ReferenceType.SecurityScheme
							}
						},new List<string>()
					}
				});
			});

			services.AddDbContext<SmartPropDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
			services.AddHostedService<BackgroundRentTasks>();

		}



		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseRouting();
			app.UseCors("AllowOrigin");
			app.UseAuthentication();
			app.UseMiddleware<JwtTokenSlidingExpirationMiddleware>();
			app.UseAuthorization();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			
				
			}
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "SM.WebAPI v1");
			});

			app.UseHttpsRedirection();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
