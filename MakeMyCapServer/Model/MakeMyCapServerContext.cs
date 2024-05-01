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

    public virtual DbSet<InHouseInventory> InHouseInventories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderLineItem> OrderLineItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<PurchaseDistributorOrder> PurchaseOrders { get; set; }

    public virtual DbSet<ServiceLog> ServiceLogs { get; set; }

    public virtual DbSet<Setting> Settings { get; set; }

    public virtual DbSet<Shipping> Shippings { get; set; }

    public virtual DbSet<User> Users { get; set; }
    
    public virtual DbSet<UserToken> UserTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

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
        
        modelBuilder.Entity<InHouseInventory>(entity =>
        {
            entity.HasKey(e => e.Sku).HasName("PK_InHouseInventory");
            
            entity.ToTable("InHouseInventory");

            entity.Property(e => e.Sku)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK_Order_1");

            entity.ToTable("Order");

            entity.HasIndex(e => e.OrderNumber, "IX_Order");

            entity.Property(e => e.OrderId).ValueGeneratedNever();
            entity.Property(e => e.CheckoutToken)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OrderNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DeliverToName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DeliverToAddress1)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DeliverToAddress2)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DeliverToCity)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DeliverToStateProv)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DeliverToZipPC)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DeliverToCountry)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.OrderNotes)
                .HasMaxLength(4096)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OrderLineItem>(entity =>
        {
            entity.HasKey(e => e.LineItemId);

            entity.ToTable("OrderLineItem");

            entity.Property(e => e.LineItemId).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Ponumber)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("PONumber");
            entity.Property(e => e.Correlation)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Correlation");
            entity.Property(e => e.ImageOrText)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ImageOrText");
            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Position");
            entity.Property(e => e.SpecialInstructions)
                .HasMaxLength(4000)
                .IsUnicode(false)
                .HasColumnName("SpecialInstructions");
            entity.Property(e => e.Sku)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShopifyName)
                .HasMaxLength(4000)
                .IsUnicode(false)
                .HasColumnName("ShopifyName");
            
            entity.HasOne(d => d.Order).WithMany(p => p.OrderLineItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderLineItem_Order");
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

        modelBuilder.Entity<PurchaseDistributorOrder>(entity =>
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
            entity.Property(e => e.Brand)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Name");
            entity.Property(e => e.Correlation)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Correlation");
            entity.Property(e => e.ImageOrText)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ImageOrText");
            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Position");
            entity.Property(e => e.SpecialInstructions)
                .HasMaxLength(4000)
                .IsUnicode(false)
                .HasColumnName("SpecialInstructions");
            entity.Property(e => e.ShopifyName)
                .HasMaxLength(1024)
                .IsUnicode(false)
                .HasColumnName("ShopifyName");
            entity.Property(e => e.Supplier)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Supplier");
            entity.Property(e => e.SupplierPoNumber)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("SupplierPoNumber");
            entity.Property(e => e.SupplierPoNumber2)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("SupplierPoNumber2");
            entity.Property(e => e.SupplierPoNumber3)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("SupplierPoNumber3");
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
            entity.Property(e => e.Id).ValueGeneratedNever();
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
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.HasIndex(e => e.Username, "IX_User").IsUnique();
            
            entity.HasIndex(e => e.Email, "IX_User_1").IsUnique();
            
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.CreateDate).HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.ResetExpirationDatetime).HasColumnType("datetime");
            entity.Property(e => e.ResetKey)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.ToTable("UserToken");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");

            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.UserTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserToken_User");
        });
        
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
