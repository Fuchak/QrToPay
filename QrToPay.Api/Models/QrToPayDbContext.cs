using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QrToPay.Api.Models;

public partial class QrToPayDbContext : DbContext
{
    public QrToPayDbContext()
    {
    }

    public QrToPayDbContext(DbContextOptions<QrToPayDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FunFair> FunFairs { get; set; }

    public virtual DbSet<FunFairAttraction> FunFairAttractions { get; set; }

    public virtual DbSet<FunFairPrice> FunFairPrices { get; set; }

    public virtual DbSet<HelpForm> HelpForms { get; set; }

    public virtual DbSet<ServiceCategory> ServiceCategories { get; set; }

    public virtual DbSet<SkiLift> SkiLifts { get; set; }

    public virtual DbSet<SkiResort> SkiResorts { get; set; }

    public virtual DbSet<SkiResortPrice> SkiResortPrices { get; set; }

    public virtual DbSet<TicketHistory> TicketHistories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserTicket> UserTickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FunFair>(entity =>
        {
            entity.HasKey(e => e.FunFairId).HasName("PK__FunFairs__7636299B984842DA");

            entity.Property(e => e.FunFairId).HasColumnName("FunFairID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Service).WithMany(p => p.FunFairs)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FunFairs__Servic__09746778");
        });

        modelBuilder.Entity<FunFairAttraction>(entity =>
        {
            entity.HasKey(e => e.AttractionId).HasName("PK__FunFairA__DAE24DBA12FBC3C4");

            entity.Property(e => e.AttractionId).HasColumnName("AttractionID");
            entity.Property(e => e.AttractionName).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FunFairId).HasColumnName("FunFairID");
            entity.Property(e => e.Qrcode)
                .HasMaxLength(11)
                .HasColumnName("QRCode");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.FunFair).WithMany(p => p.FunFairAttractions)
                .HasForeignKey(d => d.FunFairId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FunFairAt__FunFa__0F2D40CE");
        });

        modelBuilder.Entity<FunFairPrice>(entity =>
        {
            entity.HasKey(e => e.FunFairPriceId).HasName("PK__FunFairP__83D52B9DD365475C");

            entity.Property(e => e.FunFairPriceId).HasColumnName("FunFairPriceID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FunFairId).HasColumnName("FunFairID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.FunFair).WithMany(p => p.FunFairPrices)
                .HasForeignKey(d => d.FunFairId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FunFairPr__FunFa__14E61A24");
        });

        modelBuilder.Entity<HelpForm>(entity =>
        {
            entity.HasKey(e => e.HelpFormId).HasName("PK__HelpForm__12B59BD92927CB9A");

            entity.ToTable("HelpForm");

            entity.Property(e => e.HelpFormId).HasColumnName("HelpFormID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Subject).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserEmail).HasMaxLength(255);
            entity.Property(e => e.UserName).HasMaxLength(255);
        });

        modelBuilder.Entity<ServiceCategory>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__ServiceC__C51BB0EAC5F8504C");

            entity.Property(e => e.ServiceId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ServiceID");
            entity.Property(e => e.CityName).HasMaxLength(20);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ServiceName).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<SkiLift>(entity =>
        {
            entity.HasKey(e => e.SkiLiftId).HasName("PK__SkiLifts__59FEC4339C561E4A");

            entity.Property(e => e.SkiLiftId).HasColumnName("SkiLiftID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LiftName).HasMaxLength(100);
            entity.Property(e => e.Qrcode)
                .HasMaxLength(11)
                .HasColumnName("QRCode");
            entity.Property(e => e.SkiResortId).HasColumnName("SkiResortID");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.SkiResort).WithMany(p => p.SkiLifts)
                .HasForeignKey(d => d.SkiResortId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SkiLifts__SkiRes__2057CCD0");
        });

        modelBuilder.Entity<SkiResort>(entity =>
        {
            entity.HasKey(e => e.SkiResortId).HasName("PK__SkiResor__3CEDE73BC545CB83");

            entity.Property(e => e.SkiResortId).HasColumnName("SkiResortID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Service).WithMany(p => p.SkiResorts)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SkiResort__Servi__1A9EF37A");
        });

        modelBuilder.Entity<SkiResortPrice>(entity =>
        {
            entity.HasKey(e => e.SkiResortPriceId).HasName("PK__SkiResor__F9B03331147959C8");

            entity.Property(e => e.SkiResortPriceId).HasColumnName("SkiResortPriceID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SkiResortId).HasColumnName("SkiResortID");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.SkiResort).WithMany(p => p.SkiResortPrices)
                .HasForeignKey(d => d.SkiResortId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SkiResort__SkiRe__2610A626");
        });

        modelBuilder.Entity<TicketHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__TicketHi__4D7B4ADD9AA9347E");

            entity.ToTable("TicketHistory");

            entity.Property(e => e.HistoryId).HasColumnName("HistoryID");
            entity.Property(e => e.PurchaseDate).HasColumnType("datetime");
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Service).WithMany(p => p.TicketHistories)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TicketHis__Servi__32767D0B");

            entity.HasOne(d => d.User).WithMany(p => p.TicketHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TicketHis__UserI__318258D2");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC0E4EE006");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.AccountBalance).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.VerificationCode).HasMaxLength(6);
        });

        modelBuilder.Entity<UserTicket>(entity =>
        {
            entity.HasKey(e => e.UserTicketId).HasName("PK__UserTick__96EB945BD7870245");

            entity.Property(e => e.UserTicketId).HasColumnName("UserTicketID");
            entity.Property(e => e.PurchaseDate).HasColumnType("datetime");
            entity.Property(e => e.QrCodeGeneratedAt).HasColumnType("datetime");
            entity.Property(e => e.QrCodeIsActive).HasDefaultValue(false);
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Service).WithMany(p => p.UserTickets)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserTicke__Servi__2EA5EC27");

            entity.HasOne(d => d.User).WithMany(p => p.UserTickets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserTicke__UserI__2DB1C7EE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
