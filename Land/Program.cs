var builder = WebApplication.CreateBuilder(args);
// THIS IS SCHEDULER 
// in case we need this just add Quartz and Quartz.Extensions.Hosting NUGETS
//builder.Services.AddQuartz(q =>
//{
//    q.UseMicrosoftDependencyInjectionJobFactory();
//    var jobKey = new JobKey("DemoJob");
//    q.AddJob<DeleteAdTask>(opts => opts.WithIdentity(jobKey));

//    q.AddTrigger(opts => opts
//        .ForJob(jobKey)
//        .WithIdentity("DemoJob-trigger")
//        .WithCronSchedule("0 */5 * ? * *"));

//});
//builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// TODO add async await
// TODO https
builder.Services.AddDbContext<LandDbContext>(options =>
{
    options.UseSqlServer(connectionString);
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
//builder.Services.AddScoped<IAuthService, AuthService>();
var app = builder.Build();

 // Serve the React app from wwwroot
//app.UseDefaultFiles(); // This middleware will look for default files like index.html
//app.UseStaticFiles(); // This middleware will serve the static files in the wwwroot folder
//app.UseRouting();
app.UseHttpsRedirection();
//app.MapGet("/security/getMessage", () => "Hello World!").RequireAuthorization();
app.UseCors("landCorsPolicy");
// Configure the HTTP request pipeline.
//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.Run();
