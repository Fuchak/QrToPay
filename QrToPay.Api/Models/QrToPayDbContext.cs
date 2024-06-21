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

    public virtual DbSet<Attraction> Attractions { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<FunFair> FunFairs { get; set; }

    public virtual DbSet<FunFairPrice> FunFairPrices { get; set; }

    public virtual DbSet<HelpForm> HelpForms { get; set; }

    public virtual DbSet<SkiLift> SkiLifts { get; set; }

    public virtual DbSet<SkiSlope> SkiSlopes { get; set; }

    public virtual DbSet<SkiSlopePrice> SkiSlopePrices { get; set; }

    public virtual DbSet<TicketHistory> TicketHistories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserTicket> UserTickets { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attraction>(entity =>
        {
            entity.HasKey(e => e.AttractionId).HasName("PK__Attracti__DAE24DBAECBE7D74");

            entity.Property(e => e.AttractionId).HasColumnName("AttractionID");
            entity.Property(e => e.AttractionName).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FunFairId).HasColumnName("FunFairID");
            entity.Property(e => e.Qrcode)
                .HasMaxLength(255)
                .HasColumnName("QRCode");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.FunFair).WithMany(p => p.Attractions)
                .HasForeignKey(d => d.FunFairId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Attractio__FunFa__5C37ACAD");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__Cities__F2D21A96560CF642");

            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CityName).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<FunFair>(entity =>
        {
            entity.HasKey(e => e.FunFairId).HasName("PK__FunFairs__7636299B71D7C5D4");

            entity.Property(e => e.FunFairId).HasColumnName("FunFairID");
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ParkName).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.City).WithMany(p => p.FunFairs)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FunFairs__CityID__567ED357");
        });

        modelBuilder.Entity<FunFairPrice>(entity =>
        {
            entity.HasKey(e => e.FunFairPriceId).HasName("PK__FunFairP__83D52B9DDE322D61");

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
                .HasConstraintName("FK__FunFairPr__FunFa__61F08603");
        });

        modelBuilder.Entity<HelpForm>(entity =>
        {
            entity.HasKey(e => e.HelpFormId).HasName("PK__HelpForm__12B59BD9068EA848");

            entity.ToTable("HelpForm");

            entity.Property(e => e.HelpFormId).HasColumnName("HelpFormID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Subject).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserEmail).HasMaxLength(255);
            entity.Property(e => e.UserName).HasMaxLength(255);
        });

        modelBuilder.Entity<SkiLift>(entity =>
        {
            entity.HasKey(e => e.SkiLiftId).HasName("PK__SkiLifts__59FEC433434AB260");

            entity.Property(e => e.SkiLiftId).HasColumnName("SkiLiftID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LiftName).HasMaxLength(255);
            entity.Property(e => e.Qrcode)
                .HasMaxLength(255)
                .HasColumnName("QRCode");
            entity.Property(e => e.SkiResortId).HasColumnName("SkiResortID");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.SkiResort).WithMany(p => p.SkiLifts)
                .HasForeignKey(d => d.SkiResortId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SkiLifts__SkiRes__6D6238AF");
        });

        modelBuilder.Entity<SkiSlope>(entity =>
        {
            entity.HasKey(e => e.SkiResortId).HasName("PK__SkiSlope__3CEDE73BEBC0C3EA");

            entity.Property(e => e.SkiResortId).HasColumnName("SkiResortID");
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ResortName).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.City).WithMany(p => p.SkiSlopes)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SkiSlopes__CityI__67A95F59");
        });

        modelBuilder.Entity<SkiSlopePrice>(entity =>
        {
            entity.HasKey(e => e.SkiSlopePriceId).HasName("PK__SkiSlope__D30F0445E02A6DB9");

            entity.Property(e => e.SkiSlopePriceId).HasColumnName("SkiSlopePriceID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SkiResortId).HasColumnName("SkiResortID");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.SkiResort).WithMany(p => p.SkiSlopePrices)
                .HasForeignKey(d => d.SkiResortId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SkiSlopeP__SkiRe__731B1205");
        });

        modelBuilder.Entity<TicketHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__TicketHi__4D7B4ADD5C670479");

            entity.ToTable("TicketHistory");

            entity.Property(e => e.HistoryId).HasColumnName("HistoryID");
            entity.Property(e => e.FunFairId).HasColumnName("FunFairID");
            entity.Property(e => e.PurchaseDate).HasColumnType("datetime");
            entity.Property(e => e.SkiResortId).HasColumnName("SkiResortID");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.FunFair).WithMany(p => p.TicketHistories)
                .HasForeignKey(d => d.FunFairId)
                .HasConstraintName("FK__TicketHis__FunFa__04459E07");

            entity.HasOne(d => d.SkiResort).WithMany(p => p.TicketHistories)
                .HasForeignKey(d => d.SkiResortId)
                .HasConstraintName("FK__TicketHis__SkiRe__035179CE");

            entity.HasOne(d => d.User).WithMany(p => p.TicketHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TicketHis__UserI__025D5595");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC0F6A86CB");

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
            entity.HasKey(e => e.UserTicketId).HasName("PK__UserTick__96EB945B7281C0D7");

            entity.Property(e => e.UserTicketId).HasColumnName("UserTicketID");
            entity.Property(e => e.FunFairId).HasColumnName("FunFairID");
            entity.Property(e => e.Qrcode)
                .HasMaxLength(255)
                .HasColumnName("QRCode");
            entity.Property(e => e.SkiResortId).HasColumnName("SkiResortID");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.FunFair).WithMany(p => p.UserTickets)
                .HasForeignKey(d => d.FunFairId)
                .HasConstraintName("FK__UserTicke__FunFa__7F80E8EA");

            entity.HasOne(d => d.SkiResort).WithMany(p => p.UserTickets)
                .HasForeignKey(d => d.SkiResortId)
                .HasConstraintName("FK__UserTicke__SkiRe__7E8CC4B1");

            entity.HasOne(d => d.User).WithMany(p => p.UserTickets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserTicke__UserI__7D98A078");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
