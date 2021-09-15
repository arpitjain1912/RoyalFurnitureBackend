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
        public virtual DbSet<ChildItem> ChildItem { get; set; }
        public virtual DbSet<CostPrice> CostPrice { get; set; }
        public virtual DbSet<CostPriceAssigned> CostPriceAssigned { get; set; }
        public virtual DbSet<CurrentStock> CurrentStock { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Items> Items { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderItem> OrderItem { get; set; }
        public virtual DbSet<Purchase> Purchase { get; set; }
        public virtual DbSet<PurchaseItem> PurchaseItem { get; set; }
        public virtual DbSet<SalesTransaction> SalesTransaction { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Vendor> Vendor { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=royalfurniture.database.windows.net;Database=royalfurnitureDB;User ID=royalfurniturecentre;password=Arpit_jain@1912");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.Property(e => e.BrandId)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.AddedAt).HasColumnType("datetime");

                entity.Property(e => e.BrandName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.AddedAt).HasColumnType("datetime");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<ChildItem>(entity =>
            {
                entity.HasKey(e => new { e.ItemName, e.ChildItemName })
                    .HasName("PK__ChildIte__ED8386ECE2CF6AE9");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.ChildItemName)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.AddedAt).HasColumnType("datetime");

                entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

                entity.Property(e => e.NumberOfCopy).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.ItemNameNavigation)
                    .WithMany(p => p.ChildItem)
                    .HasForeignKey(d => d.ItemName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChildItem__ItemN__3552E9B6");
            });

            modelBuilder.Entity<CostPrice>(entity =>
            {
                entity.HasKey(e => new { e.CostPriceId, e.ItemName, e.StoreId })
                    .HasName("PK__CostPric__0FC0FE80BFC346E4");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.AddedAt).HasColumnType("datetime");

                entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

                entity.HasOne(d => d.CurrentStock)
                    .WithMany(p => p.CostPrice)
                    .HasForeignKey(d => new { d.ItemName, d.StoreId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CostPrice__1209AD79");
            });

            modelBuilder.Entity<CostPriceAssigned>(entity =>
            {
                entity.HasKey(e => new { e.ItemName, e.OrderId, e.StoreId, e.CostPrice })
                    .HasName("PK__CostPric__CC6186A007F9442C");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.AddedAt).HasColumnType("datetime");

                entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

                entity.HasOne(d => d.OrderItem)
                    .WithMany(p => p.CostPriceAssigned)
                    .HasForeignKey(d => new { d.ItemName, d.OrderId, d.StoreId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CostPriceAssigne__2116E6DF");
            });

            modelBuilder.Entity<CurrentStock>(entity =>
            {
                entity.HasKey(e => new { e.ItemName, e.StoreId })
                    .HasName("PK__tmp_ms_x__4DFB5CE685F1FCB3");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.AddedAt).HasColumnType("datetime");

                entity.Property(e => e.CostPriceId).HasColumnName("CostPriceID");

                entity.Property(e => e.ModifiedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId).ValueGeneratedNever();

                entity.Property(e => e.AddedAt).HasColumnType("datetime");

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

                entity.Property(e => e.Gstno)
                    .HasColumnName("GSTNo")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Items>(entity =>
            {
                entity.HasKey(e => e.ItemName)
                    .HasName("PK__tmp_ms_x__4E4373F636B3C6F2");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.AddedAt).HasColumnType("datetime");

                entity.Property(e => e.AliasCode)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.BrandId)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryId)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(128)
                    .IsUnicode(false);

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

                entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

                entity.Property(e => e.ParentItemName)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.HasOne(d => d.Brand)
                    .WithMany(/*p => p.Items*/)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK__Items__BrandId__32767D0B");

                entity.HasOne(d => d.Category)
                    .WithMany(/*p => p.Items*/)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Items__CategoryI__318258D2");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).ValueGeneratedNever();

                entity.Property(e => e.AddedAt).HasColumnType("datetime");

                entity.Property(e => e.BookingDate).HasColumnType("date");

                entity.Property(e => e.DeliveryDate).HasColumnType("date");

                entity.Property(e => e.Gstpercent).HasColumnName("GSTPercent");

                entity.Property(e => e.IsGst).HasColumnName("IsGST");

                entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

                entity.Property(e => e.OrderInvoice)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.Customer)
                    .WithMany(/*p => p.Order*/)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Order__CustomerI__67DE6983");

                entity.HasOne(d => d.Staff)
                    .WithMany(/*p => p.Order*/)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Order__StaffId__66EA454A");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => new { e.ItemName, e.OrderId, e.StoreId })
                    .HasName("PK__tmp_ms_x__A241F4BB222172BE");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.AddedAt).HasColumnType("datetime");

                entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

                entity.HasOne(d => d.ItemNameNavigation)
                    .WithMany(/*p => p.OrderItem*/)
                    .HasForeignKey(d => d.ItemName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderItem__ItemN__3A179ED3");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItem)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderItem__Order__65F62111");

                entity.HasOne(d => d.Store)
                    .WithMany(/*p => p.OrderItem*/)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderItem__Store__019E3B86");

                entity.HasOne(d => d.CurrentStock)
                    .WithMany(/*p => p.OrderItem*/)
                    .HasForeignKey(d => new { d.ItemName, d.StoreId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderItem__382F5661");
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.Property(e => e.PurchaseId).ValueGeneratedNever();

                entity.Property(e => e.AddedAt).HasColumnType("datetime");

                entity.Property(e => e.IsGst).HasColumnName("IsGST");

                entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

                entity.Property(e => e.PurchaseDate).HasColumnType("date");

                entity.Property(e => e.PurchaseInvoice)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.HasOne(d => d.Vendor)
                    .WithMany(/*p => p.Purchase*/)
                    .HasForeignKey(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Purchase__Vendor__50FB042B");
            });

            modelBuilder.Entity<PurchaseItem>(entity =>
            {
                entity.HasKey(e => new { e.PurchaseId, e.StoreId, e.ItemName })
                    .HasName("PK__Purchase__3FC1640D976C6C52");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.AddedAt).HasColumnType("datetime");

                entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

                entity.HasOne(d => d.ItemNameNavigation)
                    .WithMany(/*p => p.PurchaseItem*/)
                    .HasForeignKey(d => d.ItemName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PurchaseI__ItemN__345EC57D");

                entity.HasOne(d => d.Purchase)
                    .WithMany(p => p.PurchaseItem)
                    .HasForeignKey(d => d.PurchaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PurchaseI__Purch__5006DFF2");

                entity.HasOne(d => d.Store)
                    .WithMany(/*p => p.PurchaseItem*/)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PurchaseI__Store__02925FBF");

                entity.HasOne(d => d.CurrentStock)
                    .WithMany(/*p => p.PurchaseItem*/)
                    .HasForeignKey(d => new { d.ItemName, d.StoreId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PurchaseItem__0E391C95");
            });

            modelBuilder.Entity<SalesTransaction>(entity =>
            {
                entity.HasKey(e => new { e.TransactionId, e.OrderId })
                    .HasName("PK__tmp_ms_x__B97A3FD78DB42AAF");

                entity.Property(e => e.AddedAt).HasColumnType("datetime");

                entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.SalesTransaction)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SalesTran__Order__68D28DBC");
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.Property(e => e.StaffId).ValueGeneratedNever();

                entity.Property(e => e.AddedAt).HasColumnType("datetime");

                entity.Property(e => e.Address)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

                entity.Property(e => e.OptionalData)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.StaffName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.Property(e => e.StoreId)
                    .HasColumnName("Store_Id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddedAt).HasColumnType("datetime");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

                entity.Property(e => e.Phone)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.StoreName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Temp_Store')");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.AccessLevel)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.AddedAt).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.Property(e => e.VendorId).ValueGeneratedNever();

                entity.Property(e => e.AddedAt).HasColumnType("datetime");

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

                entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

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
