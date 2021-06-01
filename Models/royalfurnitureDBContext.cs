using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1.Models
{
    public partial class royalfurnitureDBContext : DbContext
    {
        public royalfurnitureDBContext()
        {
        }

        public royalfurnitureDBContext(DbContextOptions<royalfurnitureDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Brand> Brand { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<CurrentStock> CurrentStock { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<DispatchReady> DispatchReady { get; set; }
        public virtual DbSet<Items> Items { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderItem> OrderItem { get; set; }
        //public virtual DbSet<PendingItems> PendingItems { get; set; }
        public virtual DbSet<Purchase> Purchase { get; set; }
        public virtual DbSet<PurchaseItem> PurchaseItem { get; set; }
        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<Vendor> Vendor { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=royalfurniture.database.windows.net;Database=royalfurnitureDB;User ID=royalfurniturecentre;password=Arpit_jain@1912;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.Property(e => e.BrandId)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.BrandName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CurrentStock>(entity =>
            {
                entity.HasKey(e => new { e.ItemName, e.StoreId })
                    .HasName("PK__tmp_ms_x__4DFB5CE69BCB3EAA");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdate).HasColumnType("date");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId).ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DispatchReady>(entity =>
            {
                entity.HasKey(e => new { e.ItemName, e.OrderId });

                entity.Property(e => e.ItemName)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.OrderId)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryDate).HasColumnType("date");
            });

            modelBuilder.Entity<Items>(entity =>
            {
                entity.HasKey(e => e.ItemName)
                    .HasName("PK__tmp_ms_x__4E4373F6BC1589B8");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.AliasCode)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.BrandId)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryId)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Checker).HasColumnName("checker");

                entity.Property(e => e.Description)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.DiscountPercent).HasDefaultValueSql("((0))");

                entity.Property(e => e.Gstpercent)
                    .HasColumnName("GSTPercent")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Hsncode)
                    .HasColumnName("HSNCode")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                //entity.Property(e => e.Quantity).HasDefaultValueSql("((0))");

                entity.Property(e => e.WarehouseId)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.HasOne(d => d.Brand)
                    .WithMany(/*p => p.Items*/)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK__Items__BrandId__7B264821");

                entity.HasOne(d => d.Category)
                    .WithMany(/*p => p.Items*/)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Items__CategoryI__7A3223E8");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).ValueGeneratedNever();

                entity.Property(e => e.BookingDate).HasColumnType("date");

                entity.Property(e => e.DeliveryDate).HasColumnType("date");

                entity.Property(e => e.Gstpercent).HasColumnName("GSTPercent");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.Customer)
                    .WithMany(/*p => p.Order*/)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Order__CustomerI__5E8A0973");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => new { e.ItemName, e.OrderId, e.StoreId })
                    .HasName("PK__OrderIte__A241F4BB6387AC42");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.HasOne(d => d.ItemNameNavigation)
                    .WithMany(/*p => p.OrderItem*/)
                    .HasForeignKey(d => d.ItemName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderItem__ItemN__7C1A6C5A");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItem)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderItem__Order__690797E6");

                entity.HasOne(d => d.Store)
                    .WithMany(/*p => p.OrderItem*/)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderItem__Store__69FBBC1F");

                entity.HasOne(d => d.CurrentStock)
                    .WithMany(/*p => p.OrderItem*/)
                    .HasForeignKey(d => new { d.ItemName, d.StoreId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderItem__6FB49575");
            });

            /*modelBuilder.Entity<PendingItems>(entity =>
            {
                entity.HasKey(e => new { e.ItemName, e.OrderId });

                entity.Property(e => e.ItemName)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.OrderId)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryDate).HasColumnType("date");
            });*/

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.Property(e => e.PurchaseId).ValueGeneratedNever();

                entity.Property(e => e.BookingDate).HasColumnType("date");

                entity.Property(e => e.DeliveryDate).HasColumnType("date");

                entity.Property(e => e.Gstpercent).HasColumnName("GSTPercent");

                entity.HasOne(d => d.Vendor)
                    .WithMany(/*p => p.Purchase*/)
                    .HasForeignKey(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Purchase__Vendor__6EC0713C");
            });

            modelBuilder.Entity<PurchaseItem>(entity =>
            {
                entity.HasKey(e => new { e.PurchaseId, e.StoreId, e.ItemName })
                    .HasName("PK__Purchase__3FC1640D976C6C52");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.HasOne(d => d.ItemNameNavigation)
                    .WithMany(/*p => p.PurchaseItem*/)
                    .HasForeignKey(d => d.ItemName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PurchaseI__ItemN__793DFFAF");

                entity.HasOne(d => d.Purchase)
                    .WithMany(p => p.PurchaseItem)
                    .HasForeignKey(d => d.PurchaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PurchaseI__Purch__719CDDE7");

                entity.HasOne(d => d.Store)
                    .WithMany(/*p => p.PurchaseItem*/)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PurchaseI__Store__72910220");

                entity.HasOne(d => d.CurrentStock)
                    .WithMany(/*p => p.PurchaseItem*/)
                    .HasForeignKey(d => new { d.ItemName, d.StoreId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PurchaseItem__73852659");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.Property(e => e.StoreId)
                    .HasColumnName("Store_Id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(128)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.Property(e => e.VendorId).ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Gstnumber)
                    .IsRequired()
                    .HasColumnName("GSTNumber")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.VendorName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
