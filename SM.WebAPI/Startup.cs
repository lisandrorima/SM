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
			services.AddTransient<IBllUser, BllUser>();
			services.AddTransient<IDaoUser, DaoUser>();

			services.AddAutoMapper(typeof(MappingProfile));


			services.AddCors(o => o.AddPolicy("AllowOrigin", builder =>
			{
				builder.WithOrigins(new[] { "https://localhost:3000", "http://localhost:3000", "http://localhost:44339" })
					   .AllowAnyMethod()
					   .AllowAnyHeader()
					   .AllowCredentials();
			}));
			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "SM.WebAPI", Version = "v1" });
			});

			services.AddDbContext<SmartPropDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{

			app.UseCors("AllowOrigin");


			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SM.WebAPI v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
