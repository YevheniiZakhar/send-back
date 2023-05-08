using Land.Schedule;
using Land.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    var jobKey = new JobKey("DemoJob");
    q.AddJob<DeleteAdTask>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("DemoJob-trigger")
        .WithCronSchedule("0 */5 * ? * *"));

});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// TODO add async await
// TODO https
builder.Services.AddDbContext<LandDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("landCorsPolicy", builder => {
        //builder.WithOrigins("http://localhost:800").AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        //builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
        //builder.SetIsOriginAllowed(origin => true);
    });
});

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(o =>
//{
//    o.TokenValidationParameters = new TokenValidationParameters
//    {
//        //ValidIssuer = "senduasendua",
//        //ValidAudience = "senduasendua",
//        IssuerSigningKey = new SymmetricSecurityKey
//        (Encoding.UTF8.GetBytes("senduasenduasenduasenduasenduasenduasenduasenduasenduasenduasenduasenduasenduasenduasenduasendua")),
//        ValidateIssuer = false,
//        ValidateAudience = false,
//        ValidateLifetime = false,
//        ValidateIssuerSigningKey = true
//    };
//});
//builder.Services.AddAuthorization();
builder.Services.AddScoped<IAuthService, AuthService>();
var app = builder.Build();
app.UseHttpsRedirection();
//app.MapGet("/security/getMessage", () => "Hello World!").RequireAuthorization();
app.UseCors("landCorsPolicy");
// Configure the HTTP request pipeline.
//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.Run();
