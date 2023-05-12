

using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// TODO add async await
// TODO https
builder.Services.AddDbContext<LandDbContext>(options =>
{
  options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddDbContext<ApplicationUserIdentityDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationUserIdentityDbContext>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, ApplicationUserIdentityDbContext>();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt()
    .AddGoogle(googleOptions =>
    {
      googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
      googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    });


builder.Services.AddCors(options =>
{
  options.AddPolicy("landCorsPolicy", builder =>
  {
    builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
  });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}
else
{
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseCors("landCorsPolicy");

app.MapControllers();

app.Run();
