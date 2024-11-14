using IronBarCode;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
    optionsBuilder.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));

builder.Services.AddTransient<IShortUrlRepository, ShortUrlRepository>();
builder.Services.AddTransient<IShortUrlService, ShortUrlService>();
builder.Services.AddTransient<IQrCodeService, QrCodeService>();

var app = builder.Build();

License.LicenseKey = builder.Configuration.GetValue<string>("IronBarcodeLicenceKey");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();