using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Definitions.Database.Contexts;

public partial class UsersDbContext : DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<RoleInfo> RoleInfos { get; set; } = null!;
    public virtual DbSet<UserInfo> UserInfos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoleInfo>(entity =>
        {
            entity.HasKey(e => e.RoleId)
                .HasName("RoleInfo_pk");

            entity.ToTable("RoleInfo");

            entity.Property(e => e.RoleId).ValueGeneratedNever();

            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.UserId)
                .HasName("UserInfo_pk");

            entity.ToTable("UserInfo");

            entity.HasOne(d => d.Role)
                .WithMany(p => p.UserInfos)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("UserInfo_RoleInfo_null_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}