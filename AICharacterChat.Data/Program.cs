using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AICharacterChat.Data;

var builder = Host.CreateApplicationBuilder(args);

// Add DbContext
builder.Services.AddDbContext<ChatDbContext>(options =>
    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AICharacterChatDb;Trusted_Connection=true;MultipleActiveResultSets=true"));

var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
    context.Database.EnsureCreated();
}

app.Run();

