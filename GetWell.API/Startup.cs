using GetWell.API.JwtManager;
using GetWell.API.UserServices;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using GetWell.API.Models;
using GetWell.Data;
using GetWell.Service.Services;
using Microsoft.AspNetCore.Localization;

namespace GetWell.API
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
            services.AddMemoryCache();

			services.AddControllers().AddNewtonsoftJson(options =>
				{
					options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
					options.SerializerSettings.MaxDepth = 10;
				}
			);

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Get Well API", Version = "v1" });

				var securityScheme = new OpenApiSecurityScheme
				{
					Name = "JWT Authentication",
					Description = "Enter JWT Bearer token **_only_**",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = "bearer", // must be lower case
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
					{ securityScheme, new string[] { }}
				});
			});

			services.AddCors(options =>
			{
				options.AddPolicy("AllowAll",
					builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
			});

			services.AddDbContext<Data.Model.GetWellContext>(options => 
				options.UseSqlServer(Configuration.GetConnectionString("GetWellDatabase")));
			
			services.AddSingleton<IJwtAuthManager, JwtAuthManager>();
			services.AddSingleton<ConfigHelper>();
			services.AddHostedService<JwtRefreshTokenCache>();
			services.AddScoped<UserServiceFactory>();
            services.AddScoped<PatientCacheHelper>();

			services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
			services.AddTransient(typeof(IUserService<>), typeof(UserService<>));
			services.AddTransient<IAdminAccountService, AdminAccountService>();
			services.AddTransient<IAppointmentProfileService, AppointmentProfileService>();
			services.AddTransient<IAppointmentService, AppointmentService>();
			services.AddTransient<ICityService, CityService>();
			services.AddTransient<IClinicAccountService, ClinicAccountService>();
			services.AddTransient<IClinicDoctorAccountService, ClinicDoctorAccountService>();
			services.AddTransient<IClinicDoctorService, ClinicDoctorService>();
			services.AddTransient<IClinicDoctorWorkDayService, ClinicDoctorWorkDayService>();
			services.AddTransient<IClinicGalleryService, ClinicGalleryService>();
			services.AddTransient<IClinicPhoneService, ClinicPhoneService>();
			services.AddTransient<IClinicReviewService, ClinicReviewService>();
			services.AddTransient<IClinicRoleService, ClinicRoleService>();
			services.AddTransient<IClinicService, ClinicService>();
			services.AddTransient<IClinicWorkDayService, ClinicWorkDayService>();
			services.AddTransient<ICountryService, CountryService>();
			services.AddTransient<IDoctorPhoneService, DoctorPhoneService>();
			services.AddTransient<IDoctorProfileService, DoctorProfileService>();
			services.AddTransient<IDoctorService, DoctorService>();
			services.AddTransient<IDoctorSpecialtyService, DoctorSpecialtyService>();
			services.AddTransient<INewsService, NewsService>();
			services.AddTransient<IPatientProfileService, PatientProfileService>();
			services.AddTransient<IPatientService, PatientService>();
			services.AddTransient<IPatientAccountService, PatientAccountService>();
			services.AddTransient<IRegionService, RegionService>();
			services.AddTransient<ICategoryService, CategoryService>();
			services.AddTransient<IServiceCategoryService, ServiceCategoryService>();
			services.AddTransient<IServiceService, ServiceService>();
			services.AddTransient<IServiceClinicService, ServiceClinicService>();
            services.AddTransient<IServiceClinicDoctorService, ServiceClinicDoctorService>();
			services.AddTransient<ISpecialtyService, SpecialtyService>();
			services.AddTransient<ITitleService, TitleService>();
			services.AddTransient<IWeekDayService, WeekDayService>();
			services.AddTransient<IZipCodeService, ZipCodeService>();
			services.AddTransient<IClinicDiscountService, ClinicDiscountService>();
			services.AddTransient<IPatientFavoriteClinicService, PatientFavoriteClinicService>();
			services.AddTransient<IPatientFavoriteDoctorService, PatientFavoriteDoctorService>();
			
            var paymentConfig = Configuration.GetSection(nameof(PaymentConfig)).Get<PaymentConfig>();
            services.AddSingleton(paymentConfig);

			var jwt = Configuration.GetSection(nameof(JwtTokenConfig)).Get<JwtTokenConfig>();
			services.AddSingleton(jwt);
			services.AddAuthentication(c =>
			{
				c.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				c.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(c =>
			{
				c.RequireHttpsMetadata = true;
				c.SaveToken = true;
				c.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidIssuer = jwt.Issuer,
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwt.Secret)),
					ValidAudience = jwt.Audience,
					ValidateAudience = true,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.FromMinutes(1)
				};
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

            var supportedCultures = new[]
            {
                new CultureInfo("ru-RU")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ru-RU"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

			app.UseHttpsRedirection();
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Get Well API v1");
				c.DocumentTitle = "Get Well API";
				c.RoutePrefix = string.Empty;
			});
			
			app.UseRouting();
			app.UseCors("AllowAll");
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapDefaultControllerRoute();
			});
		}
	}
}
