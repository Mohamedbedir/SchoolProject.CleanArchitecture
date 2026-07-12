using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SchoolProject.Core.Behaviors;
using SchoolProject.Core.Features.Accounts.Commands.Handlars;
using SchoolProject.Core.Features.Accounts.Commands.Validators;
using SchoolProject.Core.Features.ApplicationRoles.Commands.Handlers;
using SchoolProject.Core.Features.ApplicationRoles.Commands.Validators;
using SchoolProject.Core.Features.ApplicationRoles.Queries.Handlers;
using SchoolProject.Core.Features.ApplicationUser.Commands.Handlers;
using SchoolProject.Core.Features.ApplicationUser.Commands.Validators;
using SchoolProject.Core.Features.ApplicationUser.Queries.Handlers;
using SchoolProject.Core.Features.Departments.Queries.Handlers;
using SchoolProject.Core.Features.Students.Commands.Handlers;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.Features.Students.Commands.Validators;
using SchoolProject.Core.Features.Students.Queries.Handlers;
using SchoolProject.Core.Features.Subjects.Command.Handlers;
using SchoolProject.Core.Features.Subjects.Command.Validators;
using SchoolProject.Core.Features.Subjects.Query.Handlers;
using SchoolProject.Core.Mapping.ApplicationUser;
using SchoolProject.Core.Mapping.AppRoles;
using SchoolProject.Core.Mapping.Departments;
using SchoolProject.Core.Mapping.Students;
using SchoolProject.Core.Mapping.Subjects;
using SchoolProject.Core.Middlewares;
using SchoolProject.Data.Entities;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Infrastructure.Data;
using SchoolProject.Infrastructure.Repos;
using SchoolProject.Infrastructure.Repos.Contract;
using SchoolProject.Infrastructure.Seeder;
using SchoolProject.Service.Services;
using SchoolProject.Service.Services.Contract;
using System.Globalization;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "School Project API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT Token.\nExample: Bearer eyJhbGciOiJIUzI1NiIs..."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


//Connection Sql
builder.Services.AddDbContext<SchoolDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); 
});

#region AddIdentity

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;

}).AddEntityFrameworkStores<SchoolDbContext>();
//.AddDefaultTokenProviders();

#endregion
#region Add Authentication JWT Breare

var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.Secret)),

        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddSingleton(sp =>
{
    return sp.GetRequiredService<IOptions<JwtSettings>>().Value;
});

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("CreateStudent", option =>
    {
        option.RequireClaim("Create Student", "True");
    });
});

#endregion
#region DI
//builder.Services.AddScoped(typeof(IGenericRepos<>), typeof(GenericRepos<>));
//builder.Services.AddScoped(typeof(IGenericRepos<Subject>), typeof(GenericRepos<Subject>));
builder.Services.AddScoped<IStudentRepo, StudentRepo>();
builder.Services.AddScoped<ISubjectRepo, SubjectRepo>();
builder.Services.AddScoped<IDepartmentRepo, DepartmentRepo>();
builder.Services.AddScoped<IInstructorRepo, InstructorRepo>();
builder.Services.AddScoped<IUserRefreshTokenRepo, UserRefreshTokenRepo>();

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAutoMapper(m => m.AddProfile(new StudentProfile()));
builder.Services.AddAutoMapper(m => m.AddProfile(new SubjectProfile()));
builder.Services.AddAutoMapper(m => m.AddProfile(new DepartmentProfile()));
builder.Services.AddAutoMapper(m => m.AddProfile(new AppUserProfile()));
builder.Services.AddAutoMapper(m => m.AddProfile(new RoleProfile()));
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        typeof(StudentsQueryHandler).Assembly); 
    cfg.RegisterServicesFromAssembly(
        typeof(StudentsCommandHandler).Assembly); 

    cfg.RegisterServicesFromAssembly(
        typeof(SubjectsQueryHandler).Assembly);
    cfg.RegisterServicesFromAssembly(
        typeof(SubjectsCommandHandler).Assembly);

    cfg.RegisterServicesFromAssembly(
        typeof(DepartmentQueryHandler).Assembly);

    cfg.RegisterServicesFromAssembly(
        typeof(AppUsersCommandHandler).Assembly);
    cfg.RegisterServicesFromAssembly(
        typeof(AppUsersQueryHandler).Assembly);

    cfg.RegisterServicesFromAssembly(
        typeof(AccountCommandHandler).Assembly);

    cfg.RegisterServicesFromAssembly(
        typeof(RolesCommandHandler).Assembly);
    cfg.RegisterServicesFromAssembly(
        typeof(RolesQueryHandler).Assembly);
});
//builder.Services.AddMediatR(c=>c.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
// DI For Validations
//builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
//builder.Services.AddScoped<IValidator<AddStudentCommand>, AddStudentValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AddStudentValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EditStudentValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AddSubjectValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateSubjectValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AddAppUserValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EditAppUserValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ChangeUserPasswordValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<SignInValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AddRoleValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EditRoleValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

#endregion

#region Localization
builder.Services.AddLocalization(opt =>
{
    opt.ResourcesPath = "";
});
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    List<CultureInfo> SupportedCulture = new List<CultureInfo>()
    {
        new CultureInfo("en-US"),
        new CultureInfo("de-DE"),
        new CultureInfo("fr-FR"),
        new CultureInfo("ar-EG")
    };
    options.DefaultRequestCulture=new RequestCulture("en-US");
    options.SupportedCultures=SupportedCulture;
    options.SupportedUICultures=SupportedCulture;
});
#endregion

#region Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("Policy1",
          policy =>
          {
              policy.WithOrigins("http://example.com",
                                  "http://www.contoso.com")
                                  .AllowAnyHeader()
                                  .AllowAnyMethod();
          });

});

#endregion



var app = builder.Build();

using var scope=app.Services.CreateScope();
var service= scope.ServiceProvider;
var dbContext=service.GetRequiredService<SchoolDbContext>();
var usermanger=service.GetRequiredService<UserManager<AppUser>>();
var rolemanger=service.GetRequiredService<RoleManager<IdentityRole>>();
var LoggerFactory = service.GetRequiredService<ILoggerFactory>();

try
{
    await dbContext.Database.MigrateAsync();
    await DataSeeder.SeedRoles(rolemanger);
    await DataSeeder.SeedUsers(usermanger);
}
catch(Exception ex)
{
    var logger = LoggerFactory.CreateLogger<Program>();
    logger.LogError(ex, "An Error Occurred During Applying Migrations Database");
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
#region Localization Middleware
var options=app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(options.Value);

#endregion
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseCors("Policy1");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
