using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;

namespace UrlShortener.Services;

public class ApplicationDbContext : DbContext
{
    public DbSet<ShortUrl> ShortUrls { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
}