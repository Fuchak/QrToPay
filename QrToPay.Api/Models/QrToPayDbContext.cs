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

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<FunFair> FunFairs { get; set; }

    public virtual DbSet<FunFairAttraction> FunFairAttractions { get; set; }

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
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__Cities__F2D21A96C94F1007");

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
            entity.HasKey(e => e.FunFairId).HasName("PK__FunFairs__7636299B8D5EA35F");

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
                .HasConstraintName("FK__FunFairs__CityID__1C873BEC");
        });

        modelBuilder.Entity<FunFairAttraction>(entity =>
        {
            entity.HasKey(e => e.AttractionId).HasName("PK__FunFairA__DAE24DBAC891628E");

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

            entity.HasOne(d => d.FunFair).WithMany(p => p.FunFairAttractions)
                .HasForeignKey(d => d.FunFairId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FunFairAt__FunFa__5006DFF2");
        });

        modelBuilder.Entity<FunFairPrice>(entity =>
        {
            entity.HasKey(e => e.FunFairPriceId).HasName("PK__FunFairP__83D52B9D41C39FDD");

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
                .HasConstraintName("FK__FunFairPr__FunFa__27F8EE98");
        });

        modelBuilder.Entity<HelpForm>(entity =>
        {
            entity.HasKey(e => e.HelpFormId).HasName("PK__HelpForm__12B59BD9EF20A80F");

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
            entity.HasKey(e => e.SkiLiftId).HasName("PK__SkiLifts__59FEC433F7C526BC");

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
                .HasConstraintName("FK__SkiLifts__SkiRes__336AA144");
        });

        modelBuilder.Entity<SkiSlope>(entity =>
        {
            entity.HasKey(e => e.SkiResortId).HasName("PK__SkiSlope__3CEDE73BB84BC04E");

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
                .HasConstraintName("FK__SkiSlopes__CityI__2DB1C7EE");
        });

        modelBuilder.Entity<SkiSlopePrice>(entity =>
        {
            entity.HasKey(e => e.SkiSlopePriceId).HasName("PK__SkiSlope__D30F0445B1A0D144");

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
                .HasConstraintName("FK__SkiSlopeP__SkiRe__39237A9A");
        });

        modelBuilder.Entity<TicketHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__TicketHi__4D7B4ADD34C364F6");

            entity.ToTable("TicketHistory");

            entity.Property(e => e.HistoryId).HasColumnName("HistoryID");
            entity.Property(e => e.FunFairId).HasColumnName("FunFairID");
            entity.Property(e => e.PurchaseDate).HasColumnType("datetime");
            entity.Property(e => e.SkiResortId).HasColumnName("SkiResortID");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.FunFair).WithMany(p => p.TicketHistories)
                .HasForeignKey(d => d.FunFairId)
                .HasConstraintName("FK__TicketHis__FunFa__7167D3BD");

            entity.HasOne(d => d.SkiResort).WithMany(p => p.TicketHistories)
                .HasForeignKey(d => d.SkiResortId)
                .HasConstraintName("FK__TicketHis__SkiRe__7073AF84");

            entity.HasOne(d => d.User).WithMany(p => p.TicketHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TicketHis__UserI__6F7F8B4B");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACB3133106");

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
            entity.HasKey(e => e.UserTicketId).HasName("PK__UserTick__96EB945B549F2BE2");

            entity.Property(e => e.UserTicketId).HasColumnName("UserTicketID");
            entity.Property(e => e.FunFairId).HasColumnName("FunFairID");
            entity.Property(e => e.PurchaseDate).HasColumnType("datetime");
            entity.Property(e => e.Qrcode)
                .HasMaxLength(255)
                .HasColumnName("QRCode");
            entity.Property(e => e.SkiResortId).HasColumnName("SkiResortID");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.FunFair).WithMany(p => p.UserTickets)
                .HasForeignKey(d => d.FunFairId)
                .HasConstraintName("FK__UserTicke__FunFa__6CA31EA0");

            entity.HasOne(d => d.SkiResort).WithMany(p => p.UserTickets)
                .HasForeignKey(d => d.SkiResortId)
                .HasConstraintName("FK__UserTicke__SkiRe__6BAEFA67");

            entity.HasOne(d => d.User).WithMany(p => p.UserTickets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserTicke__UserI__6ABAD62E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
