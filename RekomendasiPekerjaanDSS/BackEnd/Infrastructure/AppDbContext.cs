using BackEnd.Domain;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Kriteria> Kriteria => Set<Kriteria>();
    public DbSet<PreferensiUser> PreferensiUser => Set<PreferensiUser>();
    public DbSet<Perusahaan> Perusahaan => Set<Perusahaan>();
    public DbSet<NilaiPerusahaan> NilaiPerusahaan => Set<NilaiPerusahaan>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        // ---------------- Users ----------------
        b.Entity<User>(e =>
        {
            e.ToTable("users");
            e.HasKey(x => x.UserId);
            e.Property(x => x.UserId).HasColumnName("user_id").UseIdentityAlwaysColumn();
            e.Property(x => x.Username).HasColumnName("username").HasMaxLength(50).IsRequired();
            e.Property(x => x.Email).HasColumnName("email").HasMaxLength(100).IsRequired();
            e.Property(x => x.PasswordHash).HasColumnName("password_hash").HasMaxLength(255).IsRequired();
            e.Property(x => x.Role).HasColumnName("role").HasMaxLength(20).IsRequired();
            e.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
            e.HasIndex(x => x.Email).IsUnique();
        });

        // ---------------- Kriteria ----------------
        b.Entity<Kriteria>(e =>
        {
            e.ToTable("kriteria");
            e.HasKey(x => x.KriteriaId);
            e.Property(x => x.KriteriaId).HasColumnName("kriteria_id").HasMaxLength(5);
            e.Property(x => x.NamaKriteria).HasColumnName("nama_kriteria").HasMaxLength(100).IsRequired();
            e.Property(x => x.JenisAtribut).HasColumnName("jenis_atribut").HasMaxLength(10).IsRequired();
            e.Property(x => x.Satuan).HasColumnName("satuan").HasMaxLength(50);
            e.Property(x => x.Deskripsi).HasColumnName("deskripsi");
            e.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
        });

        // ---------------- PreferensiUser ----------------
        b.Entity<PreferensiUser>(e =>
        {
            e.ToTable("preferensi_user");
            e.HasKey(x => x.PreferenceId);
            e.Property(x => x.PreferenceId).HasColumnName("preference_id").UseIdentityAlwaysColumn();
            e.Property(x => x.UserId).HasColumnName("user_id");
            e.Property(x => x.KriteriaId).HasColumnName("kriteria_id").HasMaxLength(5);
            e.Property(x => x.NilaiAsli).HasColumnName("nilai_asli").HasMaxLength(255).IsRequired();
            e.Property(x => x.NilaiBobot).HasColumnName("nilai_bobot");
            e.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("NOW()");
            e.HasIndex(x => new { x.UserId, x.KriteriaId }).IsUnique();
            e.HasOne(x => x.User).WithMany(u => u.Preferensi).HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Kriteria).WithMany().HasForeignKey(x => x.KriteriaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ---------------- Perusahaan ----------------
        b.Entity<Perusahaan>(e =>
        {
            e.ToTable("perusahaan");
            e.HasKey(x => x.PerusahaanId);
            e.Property(x => x.PerusahaanId).HasColumnName("perusahaan_id").UseIdentityAlwaysColumn();
            e.Property(x => x.NamaPerusahaan).HasColumnName("nama_perusahaan").HasMaxLength(100).IsRequired();
            e.Property(x => x.PosisiTersedia).HasColumnName("posisi_tersedia").HasMaxLength(100);
            e.Property(x => x.Lokasi).HasColumnName("lokasi").HasMaxLength(100);
            e.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
        });

        // ---------------- NilaiPerusahaan ----------------
        b.Entity<NilaiPerusahaan>(e =>
        {
            e.ToTable("nilai_perusahaan");
            e.HasKey(x => x.NilaiId);
            e.Property(x => x.NilaiId).HasColumnName("nilai_id").UseIdentityAlwaysColumn();
            e.Property(x => x.PerusahaanId).HasColumnName("perusahaan_id");
            e.Property(x => x.KriteriaId).HasColumnName("kriteria_id").HasMaxLength(5);
            e.Property(x => x.NilaiRiil).HasColumnName("nilai_riil").HasMaxLength(255).IsRequired();
            e.Property(x => x.NilaiSkala).HasColumnName("nilai_skala");
            e.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("NOW()");
            e.HasIndex(x => new { x.PerusahaanId, x.KriteriaId }).IsUnique();
            e.HasOne(x => x.Perusahaan).WithMany(p => p.Nilai).HasForeignKey(x => x.PerusahaanId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Kriteria).WithMany().HasForeignKey(x => x.KriteriaId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}