using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MakeMyCapServer.Model;

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

    public virtual DbSet<DistributorSkuMap> DistributorSkuMaps { get; set; }

    public virtual DbSet<EmailQueue> EmailQueues { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<ServiceLog> ServiceLogs { get; set; }

    public virtual DbSet<Setting> Settings { get; set; }

    public virtual DbSet<Shipping> Shippings { get; set; }

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

            entity.HasIndex(e => e.LookupCode, "IX_Distributor_1").IsUnique();

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.LookupCode)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DistributorSkuMap>(entity =>
        {
            entity.HasKey(e => e.Sku);

            entity.ToTable("DistributorSkuMap");

            entity.HasIndex(e => e.DistributorSku, "IX_DistributorSkuMap");

            entity.HasIndex(e => e.StyleCode, "IX_DistributorSkuMap_1");

            entity.Property(e => e.Sku)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Brand)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Color)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ColorCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DistributorCode)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.DistributorSku)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.PartId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SizeCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.StyleCode)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EmailQueue>(entity =>
        {
            entity.ToTable("EmailQueue");

            entity.HasIndex(e => e.SentDateTime, "IX_EmailQueue");

            entity.Property(e => e.Body).IsUnicode(false);
            entity.Property(e => e.Recipient)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.Recipient2)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.Recipient3)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.Recipient4)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.Sender)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.Subject)
                .HasMaxLength(250)
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

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.ToTable("PurchaseOrder");

            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ponumber)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("PONumber");
            entity.Property(e => e.Size)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Sku)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Style)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Distributor).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.DistributorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseOrder_Distributor");
        });

        modelBuilder.Entity<ServiceLog>(entity =>
        {
            entity.ToTable("ServiceLog");

            entity.Property(e => e.ServiceName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Setting>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.CriticalEmailRecipient1)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.CriticalEmailRecipient2)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.CriticalEmailRecipient3)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.FulfillmentCheckHours).HasDefaultValueSql("((2))");
            entity.Property(e => e.InventoryCheckHours).HasDefaultValueSql("((8))");
            entity.Property(e => e.NextPosequence).HasColumnName("NextPOSequence");
            entity.Property(e => e.StatusEmailRecipient1)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.StatusEmailRecipient2)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.StatusEmailRecipient3)
                .HasMaxLength(120)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Shipping>(entity =>
        {
            entity.ToTable("Shipping");

            entity.Property(e => e.Attention)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.ShipAddress)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.ShipCity)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ShipEmail)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ShipMethod)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.ShipState)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.ShipTo)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ShipZip)
                .HasMaxLength(10)
                .IsUnicode(false);
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
