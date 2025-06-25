using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartHome.Domain.Entities;
using SmartHome.Domain.Entities.Identity;

namespace SmartHome.Persistence.Contexts
{
    public class SmartHomeContext:IdentityDbContext<AppUser,IdentityRole,string>
    {
        public SmartHomeContext(DbContextOptions<SmartHomeContext> options):base(options) { }

        public DbSet<Device>? Devices { get; set; }
        public DbSet<Category>? Categories { get; set; }

        public DbSet<DeviceUser>? DeviceUsers { get; set; }
        public DbSet<Location>? Locations { get; set; }
        public DbSet<SensorData>? SensorDatas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Device>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.HasOne(d => d.Category)
                      .WithMany(c => c.Devices)
                      .HasForeignKey(d => d.CategoryId)
                      .IsRequired();

                entity.HasOne(d => d.Location)
                      .WithMany(l => l.Devices)
                      .HasForeignKey(d => d.LocationId)
                      .IsRequired();

                entity.Property(d => d.Name)
                      .IsRequired();

                entity.Property(d => d.CreatedDate)
                      .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
                      .ValueGeneratedOnAdd();

                entity.Property(d => d.UpdatedDate)
                      .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
                      .ValueGeneratedOnAddOrUpdate();
                entity.Property(d => d.CpuUsage)
                      .HasPrecision(5, 2); 

                entity.Property(d => d.RamUsage)
                      .HasPrecision(5, 2);

                entity.Property(d => d.Temperature)
                      .HasPrecision(5, 2);

                entity.Property(d => d.EnergyConsumption)
                      .HasPrecision(10, 2);

                entity.Property(d => d.HealthStatus)
                      .HasMaxLength(20); 
            });

            modelBuilder.Entity<DeviceUser>(entity =>
            {
                entity.HasKey(du => new { du.UserId, du.DeviceId });

                entity.HasOne(du => du.User)
                      .WithMany(u => u.DeviceUsers)
                      .HasForeignKey(du => du.UserId)
                      .IsRequired();

                entity.HasOne(du => du.Device)
                      .WithMany(d => d.DeviceUsers)
                      .HasForeignKey(du => du.DeviceId)
                      .IsRequired();

                entity.Property<DateTime>("CreatedDate")
                      .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
                      .ValueGeneratedOnAdd();

                entity.Property<DateTime>("UpdatedDate")
                      .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
                      .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name)
                      .IsRequired();

                entity.Property(c => c.CreatedDate)
                      .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
                      .ValueGeneratedOnAdd();

                entity.Property(c => c.UpdatedDate)
                      .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
                      .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(l => l.Id);

                entity.Property(l => l.Name)
                      .IsRequired();

                entity.Property(l => l.CreatedDate)
                      .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
                      .ValueGeneratedOnAdd();

                entity.Property(l => l.UpdatedDate)
                      .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
                      .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<SensorData>(entity =>
            {
                entity.HasKey(sd => sd.Id);

                entity.HasOne(sd => sd.Device)
                      .WithMany(d => d.SensorDatas)
                      .HasForeignKey(sd => sd.DeviceId)
                      .IsRequired();

                entity.Property(sd => sd.ReadingType)
                      .IsRequired();

                entity.Property(sd => sd.Unit)
                      .IsRequired();

                entity.Property(sd => sd.Timestamp)
                      .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

                entity.Property(sd => sd.CreatedDate)
                      .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
                      .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.Property(u => u.FirstName)
                      .IsRequired();
                
                entity.Property(u => u.LastName)
                      .IsRequired();

                entity.Property<DateTime>("CreatedDate")
                      .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
                      .ValueGeneratedOnAdd();

                entity.Property<DateTime>("UpdatedDate")
                      .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
                      .ValueGeneratedOnAddOrUpdate();
                entity.HasOne(u => u.Host)
                      .WithMany(h => h.Members)
                      .HasForeignKey(u => u.HostId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
