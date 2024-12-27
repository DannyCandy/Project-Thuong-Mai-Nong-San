using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebApp.Models;
using WebApp.Areas.Poster.Models;
using WebApp.Areas.Admin.Models;
using WebApp.MobileApp.Models;

namespace WebApp.Data
{
    public partial class AgriculturalProductsContext : IdentityDbContext<ApplicationUser>
    {
        public AgriculturalProductsContext()
        {
        }

        public AgriculturalProductsContext(DbContextOptions<AgriculturalProductsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<ChungNhan> ChungNhans { get; set; } = null!;
        /*public virtual DbSet<NhaPhanPhoi> NhaPhanPhois { get; set; } = null!;*/
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<SanPham> SanPhams { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<ThongTinLienHe> ThongTinLienHes { get; set; } = null!;
        public virtual DbSet<DaiLyOnline> DaiLyOnlines { get; set; } = null!;
        public virtual DbSet<VNPaymentResponse> VNPaymentResponses { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public DbSet<VisitorStatistic> VisitorStatistics { get; set; }
        //Cho mobile app
        public virtual DbSet<TokenInfo> TokenInfos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DANNYCANDY;Database=AgriGreenDB;Trusted_Connection=True;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Idcategory);

                entity.ToTable("Category");

                entity.Property(e => e.Idcategory).HasColumnName("IDCategory");

                entity.Property(e => e.NameCategory).HasMaxLength(100);
            });

            modelBuilder.Entity<ChungNhan>(entity =>
            {
                entity.HasKey(e => e.IdchungNhan);

                entity.ToTable("ChungNhan");

                entity.Property(e => e.IdchungNhan).HasColumnName("IDChungNhan");
            });

            /*modelBuilder.Entity<NhaPhanPhoi>(entity =>
            {
                entity.HasKey(e => e.Idnpp);

                entity.ToTable("NhaPhanPhoi");

                entity.Property(e => e.Idnpp).HasColumnName("IDNPP");

                //Thêm mới
                entity.Property(e => e.HinhAnhNPP).HasColumnName("HinhAnhNPP");

                entity.Property(e => e.DiaChiNpp).HasColumnName("DiaChiNPP");

                entity.Property(e => e.GioDongCua).HasColumnType("time(0)");

                entity.Property(e => e.GioMoCua).HasColumnType("time(0)");

                entity.Property(e => e.PhoneNpp)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("PhoneNPP");

                entity.Property(e => e.TenNpp)
                    .HasMaxLength(100)
                    .HasColumnName("TenNPP");
            });*/
            //Thêm db cho Token JWT
            modelBuilder.Entity<TokenInfo>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("TokenInfo");

                entity.HasIndex(e => e.UserId, "IX_TokenInfo_UserId");

                entity.Property(e => e.Id)
                .HasMaxLength(36)
                .HasDefaultValueSql("(NEWID())");

                entity.Property(e => e.RefreshToken)
                .IsRequired(false);
            });
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");

                entity.Property(e => e.Idsp)
                    .HasMaxLength(450)
                    .HasColumnName("IDSp");

                entity.Property(e => e.OrderId)
                    .HasMaxLength(450)
                    .HasColumnName("OrderId");

                entity.HasOne(d => d.IdspNavigation)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.Idsp)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tb.OrderDetail_Tb.SanPham");

                entity.HasOne(d => d.OrderIdNavigation)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tb.OrderDetail_Tb.Order");
            });

            //Thêm model Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.OrderId).HasColumnName("OrderId");

                entity.Property(e => e.OrderTime).HasColumnType("datetime");

                entity.Property(e => e.DcGiaoHang)
                    .HasMaxLength(450)
                    .HasColumnName("DcGiaoHang");

                entity.Property(e => e.TotalPrice)
                    .HasColumnType("decimal(18,4)");

                entity.Property(e => e.UserId)
                    .HasMaxLength(450)
                    .HasColumnName("UserID");

                entity.Property(e => e.Message).HasColumnName("Message");
            });

            //Thêm bảng đại lý online
            modelBuilder.Entity<DaiLyOnline>(entity =>
            {
                entity.HasKey(e => e.IdDaiLy);

                entity.ToTable("DaiLyOnline");

                entity.Property(e => e.IdDaiLy).HasColumnName("IDDaiLy");

                entity.Property(e => e.IdUser).HasColumnName("IDUser");

                entity.Property(e => e.NameDaiLy).HasMaxLength(200).HasColumnName("NameDaiLy");
            });

            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.HasKey(e => e.Idsp);

                entity.ToTable("SanPham");

                entity.HasIndex(e => e.Idcategory, "IX_SanPham_IDCategory");

                entity.HasIndex(e => e.IdchungNhan, "IX_SanPham_IDChungNhan");

                /*entity.HasIndex(e => e.Idnpp, "IX_SanPham_IDNPP");*/

                entity.HasIndex(e => e.IdDaiLy, "IX_SanPham_IdDaiLy");

                entity.Property(e => e.Idsp).HasColumnName("IDSP");

                entity.Property(e => e.Hdsd).HasColumnName("HDSD");

                entity.Property(e => e.HinhAnhSp).HasColumnName("HinhAnhSP");

                entity.Property(e => e.Idcategory).HasColumnName("IDCategory");

                entity.Property(e => e.IdchungNhan).HasColumnName("IDChungNhan");

                /*entity.Property(e => e.Idnpp).HasColumnName("IDNPP");*/

                entity.Property(e => e.SoLuongSp).HasColumnName("SoLuongSP");

                entity.HasOne(d => d.IdcategoryNavigation)
                    .WithMany(p => p.SanPhams)
                    .HasForeignKey(d => d.Idcategory)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tb.SanPham_Tb.Category");

                entity.HasOne(d => d.IdchungNhanNavigation)
                    .WithMany(p => p.SanPhams)
                    .HasForeignKey(d => d.IdchungNhan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tb.SanPham_Tb.ChungNhan");

                /*entity.HasOne(d => d.IdnppNavigation)
                    .WithMany(p => p.SanPhams)
                    .HasForeignKey(d => d.Idnpp)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tb.SanPham_Tb.NhaPhanPhoi");*/

                //Thêm liên kết đại lý online
                entity.HasOne(d => d.IddaiLyNavigation)
                    .WithMany(p => p.SanPhams)
                    .HasForeignKey(d => d.IdDaiLy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tb.SanPham_Tb.DaiLyOnline");
            });

            modelBuilder.Entity<ThongTinLienHe>(entity =>
            {
                entity.HasKey(e => e.Idttlh);

                entity.ToTable("ThongTinLienHe");

                entity.Property(e => e.Idttlh).HasColumnName("IDTTLH");

                entity.Property(e => e.DcgiaoHang).HasColumnName("DCGiaoHang");

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasMaxLength(450);
            });

            modelBuilder.Entity<VNPaymentResponse>(entity =>
            {
                entity.HasKey(e => e.TransactionId);

                entity.ToTable("GiaoDichVnPay");

                entity.Property(e => e.Success).HasColumnName("Success").HasDefaultValue(false);

                entity.Property(e => e.PaymentMethod).HasColumnName("PaymentMethod");

                entity.Property(e => e.OrderDescription).HasColumnName("OrderDescription");

                entity.Property(e => e.OrderId).HasColumnName("OrderId");

                entity.Property(e => e.Token).HasColumnName("Token");

                entity.Property(e => e.VnPayResponseCode).HasColumnName("VnPayResponseCode");

                entity.HasOne(d => d.OrderIdNavigation)
                    .WithMany(p => p.VNPayments)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tb.GiaoDichVnPay_Tb.Order");
            });

            //Thêm trường post bài viết
            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.AuthorId).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()");

                entity.HasOne(d => d.Author)
                      .WithMany(p => p.Posts)
                      .HasForeignKey(d => d.AuthorId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_Post_Author");
            });            

            modelBuilder.Entity<VisitorStatistic>(entity =>
            {
                entity.HasKey(v => v.Id); // Khóa chính
                entity.Property(v => v.Date).IsRequired(); // Bắt buộc nhập Date
                entity.Property(v => v.VisitorCount).HasDefaultValue(0); // Giá trị mặc định cho VisitorCount
            });

            OnModelCreatingPartial(modelBuilder);
        }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
