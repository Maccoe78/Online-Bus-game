using OnlineBussen.Data.Interfaces;
using OnlineBussen.Data.Repositories;
using OnlineBussen.Logic.Interfaces;
using OnlineBussen.Logic.Controllers;
using OnlineBussen.Logic.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<UserController>();
builder.Services.AddScoped<ILobbyRepository, LobbyRepository>();
builder.Services.AddScoped<ILobbyService, LobbyService>();
builder.Services.AddScoped<LobbyController>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "OnlineBussen.Session";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Session middleware moet voor endpoints komen
app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});


app.MapRazorPages();

await app.RunAsync();
