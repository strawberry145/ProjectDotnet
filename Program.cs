using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProjectIntelligence.Data;
using ProjectIntelligence.Services;
using ProjectIntelligence.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddControllers();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency injection for services
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

var app = builder.Build();

// Create database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.EnsureCreatedAsync();   // recreate full schema
    
    // Seed only if --seed is passed in terminal
    if (args.Contains("--seed"))
    {
        await DbSeeder.SeedAsync(db);
        Console.WriteLine("Database seeded successfully!");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();

app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();