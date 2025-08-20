using BooksStoreApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure the database connection
var usePostgreSQL = builder.Configuration.GetValue<bool>("UsePostgreSQL", true);

if (usePostgreSQL)
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<BooksStoreContext>(options =>
        options.UseNpgsql(connectionString));
}
else
{
    var sqliteConnectionString = builder.Configuration.GetConnectionString("SqliteConnection");
    builder.Services.AddDbContext<BooksStoreContext>(options =>
        options.UseSqlite(sqliteConnectionString));
}

var app = builder.Build();

// Ensure database is created and migrations are applied
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BooksStoreContext>();
    try
    {
        context.Database.EnsureCreated();
        // Alternatively, you can use: context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while creating/migrating the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();