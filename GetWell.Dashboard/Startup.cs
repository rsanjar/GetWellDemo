using System.Collections.Generic;
using System.Globalization;
using GetWell.Core;
using GetWell.Core.Mailer;
using GetWell.Dashboard.Cache;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GetWell.Dashboard.UserServices;
using GetWell.Data;
using GetWell.Service.Interface;
using GetWell.Service.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

namespace GetWell.Dashboard
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
            services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));
			services.AddControllersWithViews();
			services.AddHttpContextAccessor();
            services.AddSingleton<IEmailSender, EmailService>();

			services.AddDbContext<Data.Model.GetWellContext>(options => 
				options.UseSqlServer(Configuration.GetConnectionString("GetWellDatabase")));

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.LoginPath = "/Account/Login";
					options.LogoutPath = "/Account/SignOut";
					options.SlidingExpiration = true;
					options.Cookie = new CookieBuilder()
					{
						Name = "GetWell.Auth",
						SameSite = SameSiteMode.Strict
					};
				});
			
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddAuthorization(options =>
            {
                options.AddPolicy(UserPolicies.EditClinic, policy => policy.RequireRole(UserRoles.Admin, UserRoles.Clinic));
            });

			services.AddScoped<UserServiceFactory>();
			services.AddScoped<FormCacheHelper>();
			//services.AddSingleton<ICustomMiddleware, CustomMiddleware>();

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
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
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
            if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
		
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ru-RU"),
                // Formatting numbers, dates, etc.
                SupportedCultures = new List<CultureInfo>()
                {
					new ("ru-RU")
                },
                // UI strings that we have localized.
                SupportedUICultures = new List<CultureInfo>()
                {
					new ("ru-RU")
                }
            });
         

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			
			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			//app.UseMiddleware<ICustomMiddleware>();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
