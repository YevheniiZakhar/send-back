using Land.Schedule;
using Quartz;

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
    options.UseMySql("server=localhost;database=land;user=root;password=1234;port=3306;", ServerVersion.AutoDetect("server=localhost;database=land;user=root;password=1234;port=3306;"));
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

var app = builder.Build();

app.UseCors("landCorsPolicy");
// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
