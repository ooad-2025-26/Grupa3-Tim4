using Aplikacija.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<Korisnik>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Rezervacija> Rezervacija { get; set; }
    public DbSet<Narudzba> Narudzba { get; set; }
    public DbSet<Takmicenje> Takmicenje { get; set; }
    public DbSet<SistemZaTakmicenje> SistemZaTakmicenje { get; set; }
    public DbSet<Sesija> Sesija { get; set; }
    public DbSet<Kvar> Kvar { get; set; }

    public DbSet<Uredjaj> Uredjaj { get; set; }
    public DbSet<PC> PC { get; set; }
    public DbSet<PlayStation> PlayStation { get; set; }
    public DbSet<XBox> XBox { get; set; }

    public DbSet<LoyaltyProgram> LoyaltyProgram { get; set; }
    public DbSet<Placanje> Placanje { get; set; }

    public DbSet<DeviceService> DeviceService { get; set; } = default!;
    public DbSet<GotovinskoPlacanje> GotovinskoPlacanje { get; set; } = default!;
    public DbSet<KarticnoPlacanje> KarticnoPlacanje { get; set; } = default!;
    public DbSet<LoyaltyPlacanje> LoyaltyPlacanje { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Korisnik se NE mapira na tabelu "Korisnik"
        // jer Identity koristi sistemsku tabelu "AspNetUsers"

        modelBuilder.Entity<Rezervacija>().ToTable("Rezervacija");
        modelBuilder.Entity<Narudzba>().ToTable("Narudzba");
        modelBuilder.Entity<Takmicenje>().ToTable("Takmicenje");
        modelBuilder.Entity<SistemZaTakmicenje>().ToTable("SistemZaTakmicenje");
        modelBuilder.Entity<Sesija>().ToTable("Sesija");
        modelBuilder.Entity<Kvar>().ToTable("Kvar");

        modelBuilder.Entity<Uredjaj>().ToTable("Uredjaj");
        modelBuilder.Entity<PC>().ToTable("PC");
        modelBuilder.Entity<PlayStation>().ToTable("PlayStation");
        modelBuilder.Entity<XBox>().ToTable("XBox");

        modelBuilder.Entity<LoyaltyProgram>().ToTable("LoyaltyProgram");
        modelBuilder.Entity<Placanje>().ToTable("Placanje");

        modelBuilder.Entity<DeviceService>().ToTable("DeviceService");
        modelBuilder.Entity<GotovinskoPlacanje>().ToTable("GotovinskoPlacanje");
        modelBuilder.Entity<KarticnoPlacanje>().ToTable("KarticnoPlacanje");
        modelBuilder.Entity<LoyaltyPlacanje>().ToTable("LoyaltyPlacanje");

        modelBuilder.Entity<Rezervacija>()
            .Property(r => r.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Sesija>()
            .Property(s => s.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Kvar>()
            .Property(k => k.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Uredjaj>()
            .Property(u => u.Status)
            .HasConversion<string>();
    }
}