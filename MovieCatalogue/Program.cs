using Microsoft.EntityFrameworkCore;
using MovieCatalog.Data;
using MovieCatalog.Data.Entities;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Ensure database is created
    context.Database.EnsureCreated();

    // Seed Directors if empty
    if (!context.Directors.Any())
    {
        var directorsJson = File.ReadAllText("directors.json");
        var directors = JsonSerializer.Deserialize<List<Director>>(directorsJson);

        if (directors != null)
        {
            context.Directors.AddRange(directors);
            context.SaveChanges();
        }
    }

    // Seed Movies if empty
    if (!context.Movies.Any())
    {
        var moviesJson = File.ReadAllText("movies.json");
        var movies = JsonSerializer.Deserialize<List<Movie>>(moviesJson);

        if (movies != null)
        {
            context.Movies.AddRange(movies);
            context.SaveChanges();
        }
    }
    if (!context.Users.Any())
    {
        var usersJson = File.ReadAllText("users.json");
        var users = JsonSerializer.Deserialize<List<User>>(usersJson);

        if (users != null)
        {
            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
    if (!context.UserReviews.Any())
    {
        var reviewsJson = File.ReadAllText("reviews.json");
        var reviews = JsonSerializer.Deserialize<List<UserReview>>(reviewsJson);

        if (reviews != null)
        {
            context.UserReviews.AddRange(reviews);
            context.SaveChanges();
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
