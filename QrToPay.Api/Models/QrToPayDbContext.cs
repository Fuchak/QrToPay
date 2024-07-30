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

    public virtual DbSet<Entity> Entities { get; set; }

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
        modelBuilder.Entity<Entity>(entity =>
        {
            entity.HasKey(e => e.EntityId).HasName("PK__Entities__9C892FFDE83945CA");

            entity.Property(e => e.EntityId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("EntityID");
            entity.Property(e => e.CityName).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EntityName).HasMaxLength(255);
            entity.Property(e => e.EntityType).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<FunFair>(entity =>
        {
            entity.HasKey(e => e.FunFairId).HasName("PK__FunFairs__7636299B9FDB80EB");

            entity.Property(e => e.FunFairId).HasColumnName("FunFairID");
            entity.Property(e => e.CityName).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EntityId).HasColumnName("EntityID");
            entity.Property(e => e.ParkName).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Entity).WithMany(p => p.FunFairs)
                .HasForeignKey(d => d.EntityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FunFairs__Entity__00DF2177");
        });

        modelBuilder.Entity<FunFairAttraction>(entity =>
        {
            entity.HasKey(e => e.AttractionId).HasName("PK__FunFairA__DAE24DBA441604B2");

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
                .HasConstraintName("FK__FunFairAt__FunFa__0697FACD");
        });

        modelBuilder.Entity<FunFairPrice>(entity =>
        {
            entity.HasKey(e => e.FunFairPriceId).HasName("PK__FunFairP__83D52B9D52306CBB");

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
                .HasConstraintName("FK__FunFairPr__FunFa__0C50D423");
        });

        modelBuilder.Entity<HelpForm>(entity =>
        {
            entity.HasKey(e => e.HelpFormId).HasName("PK__HelpForm__12B59BD96E849BB9");

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
            entity.HasKey(e => e.SkiLiftId).HasName("PK__SkiLifts__59FEC4331CA64AE1");

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
                .HasConstraintName("FK__SkiLifts__SkiRes__17C286CF");
        });

        modelBuilder.Entity<SkiSlope>(entity =>
        {
            entity.HasKey(e => e.SkiResortId).HasName("PK__SkiSlope__3CEDE73B8B715A50");

            entity.Property(e => e.SkiResortId).HasColumnName("SkiResortID");
            entity.Property(e => e.CityName).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EntityId).HasColumnName("EntityID");
            entity.Property(e => e.ResortName).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Entity).WithMany(p => p.SkiSlopes)
                .HasForeignKey(d => d.EntityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SkiSlopes__Entit__1209AD79");
        });

        modelBuilder.Entity<SkiSlopePrice>(entity =>
        {
            entity.HasKey(e => e.SkiSlopePriceId).HasName("PK__SkiSlope__D30F044503B84B21");

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
                .HasConstraintName("FK__SkiSlopeP__SkiRe__1D7B6025");
        });

        modelBuilder.Entity<TicketHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__TicketHi__4D7B4ADDEEE56AB3");

            entity.ToTable("TicketHistory");

            entity.Property(e => e.HistoryId).HasColumnName("HistoryID");
            entity.Property(e => e.EntityId).HasColumnName("EntityID");
            entity.Property(e => e.PurchaseDate).HasColumnType("datetime");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Entity).WithMany(p => p.TicketHistories)
                .HasForeignKey(d => d.EntityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TicketHis__Entit__251C81ED");

            entity.HasOne(d => d.User).WithMany(p => p.TicketHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TicketHis__UserI__24285DB4");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACF69FDD65");

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
            entity.HasKey(e => e.UserTicketId).HasName("PK__UserTick__96EB945B68BF2A48");

            entity.Property(e => e.UserTicketId).HasColumnName("UserTicketID");
            entity.Property(e => e.EntityId).HasColumnName("EntityID");
            entity.Property(e => e.PurchaseDate).HasColumnType("datetime");
            entity.Property(e => e.Qrcode)
                .HasMaxLength(255)
                .HasColumnName("QRCode");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Entity).WithMany(p => p.UserTickets)
                .HasForeignKey(d => d.EntityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserTicke__Entit__214BF109");

            entity.HasOne(d => d.User).WithMany(p => p.UserTickets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserTicke__UserI__2057CCD0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
