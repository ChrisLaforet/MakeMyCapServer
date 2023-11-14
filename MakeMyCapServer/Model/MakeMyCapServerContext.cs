using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MakeMyCap.Model;

public partial class MakeMyCapServerContext : DbContext
{
    public MakeMyCapServerContext()
    {
    }

    public MakeMyCapServerContext(DbContextOptions<MakeMyCapServerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Distributor> Distributors { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<SanMarSkuMap> SanMarSkuMaps { get; set; }

    public virtual DbSet<ServiceLog> ServiceLogs { get; set; }

    public virtual DbSet<Setting> Settings { get; set; }

    public virtual DbSet<SkuDistributor> SkuDistributors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=MakeMyCapServer;Trusted_Connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Distributor>(entity =>
        {
            entity.ToTable("Distributor");

            entity.HasIndex(e => e.Name, "IX_Distributor").IsUnique();

            entity.Property(e => e.CredentialKey)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.HasIndex(e => e.VariantId, "IX_Product").IsUnique();

            entity.HasIndex(e => e.Sku, "IX_Product_1").IsUnique();

            entity.Property(e => e.Sku)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Vendor)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SanMarSkuMap>(entity =>
        {
            entity.HasKey(e => e.Sku);

            entity.ToTable("SanMarSkuMap");

            entity.Property(e => e.Sku)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Size)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Style)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ServiceLog>(entity =>
        {
            entity.ToTable("ServiceLog");

            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StartTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<Setting>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.FulfillmentCheckHours).HasDefaultValueSql("((2))");
            entity.Property(e => e.InventoryCheckHours).HasDefaultValueSql("((8))");
        });

        modelBuilder.Entity<SkuDistributor>(entity =>
        {
            entity.ToTable("SkuDistributor");

            entity.HasIndex(e => e.Sku, "IX_SkuDistributor").IsUnique();

            entity.Property(e => e.Sku)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Distributor).WithMany(p => p.SkuDistributors)
                .HasForeignKey(d => d.DistributorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SkuDistributor_Distributor");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
