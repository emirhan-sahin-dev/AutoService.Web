using AutoService.Business.Abstract;
using AutoService.Business.Concrete;
using AutoService.Business.Mappings;
using AutoService.Business.Services.Abstract;
using AutoService.Business.Services.Concrete;
using AutoService.Business.Validators.CustomerValidators;
using AutoService.DataAccess.Contexts;
using AutoService.DataAccess.Repositories;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.DataAccess.Repositories.Concrete;
using AutoService.DataAccess.Repositories.Interfaces;
using AutoService.Web.Middlewares;
using AutoService.Web.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using System.Globalization;

QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AutoServiceContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerManager>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserManager>();

builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IDashboardService, DashboardManager>();

builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleService, VehicleManager>();

builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IBrandService, BrandManager>();

builder.Services.AddScoped<IModelRepository, ModelRepository>();
builder.Services.AddScoped<IModelService, ModelManager>();

builder.Services.AddScoped<IMechanicRepository, MechanicRepository>();
builder.Services.AddScoped<IMechanicService, MechanicManager>();

builder.Services.AddScoped<IServiceRecordRepository, ServiceRecordRepository>();
builder.Services.AddScoped<IServiceRecordService, ServiceRecordManager>();

builder.Services.AddScoped<IServiceDetailRepository, ServiceDetailRepository>();
builder.Services.AddScoped<IServiceDetailService, ServiceDetailManager>();

builder.Services.AddScoped<ISparePartRepository, SparePartRepository>();
builder.Services.AddScoped<ISparePartService, SparePartManager>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerValidator>();

builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IReportExportService, ReportExportService>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleManager>();

builder.Services.AddScoped<ISystemSettingRepository, SystemSettingRepository>();
builder.Services.AddScoped<ISystemSettingService, SystemSettingManager>();

builder.Services.AddScoped<IServiceOperationRepository, ServiceOperationRepository>();
builder.Services.AddScoped<IServiceOperationService, ServiceOperationManager>();

builder.Services.AddScoped<IServiceOperationTypeRepository, ServiceOperationTypeRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IPaymentRepository,PaymentRepository>();

builder.Services.AddScoped<IPaymentService, PaymentManager>();

builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

builder.Services.AddScoped<IAppointmentService,  AppointmentManager>();

builder.Services.AddScoped<ISystemSettingRepository, SystemSettingRepository>();

builder.Services.AddScoped<IProfileService, ProfileManager>();

builder.Services.AddAutoMapper(typeof(GeneralMapping));

builder.Services.AddScoped<
    IServiceOperationPartRepository,
    ServiceOperationPartRepository>();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.AccessDeniedPath = "/Login/AccessDenied";
        options.Cookie.Name = "AutoServiceCookie";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });
builder.Services.AddScoped<
    IServiceOperationTypeSparePartRepository,
    ServiceOperationTypeSparePartRepository>();

builder.Services.AddControllersWithViews(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

var app = builder.Build();

var culture = new CultureInfo("tr-TR");

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(culture),
    SupportedCultures = new[] { culture },
    SupportedUICultures = new[] { culture }
};

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();