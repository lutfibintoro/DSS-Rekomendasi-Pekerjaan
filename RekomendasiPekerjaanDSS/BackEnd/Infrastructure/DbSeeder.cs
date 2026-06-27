using BackEnd.Domain;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Infrastructure;

/// <summary>
/// Seeder idempotent untuk master data Kriteria (C1-C10).
/// Aman dijalankan berkali-kali; kalau seed sudah ada di DB akan di-skip.
/// Berguna sebagai fallback kalau volume DB kosong / init SQL tidak jalan.
/// </summary>
public static class DbSeeder
{
    private static readonly (string Id, string Nama, string Jenis, string? Satuan, string? Deskripsi)[] Kriterias =
    [
        ("C1",  "Gaji Pokok & Tunjangan",        "benefit", "Rupiah per bulan",          "Total pendapatan bulanan."),
        ("C2",  "Jarak & Waktu Tempuh",          "cost",    "Kilometer",                  "Jarak dari rumah ke kantor atau waktu tempuh."),
        ("C3",  "Fleksibilitas Kerja",           "benefit", "Skala kerja",                "Sistem kerja (WFO / Hybrid / Remote)."),
        ("C4",  "Jenjang Karir",                 "benefit", "Skala tingkat kejelasan",    "Kejelasan jalur promosi dan kenaikan jabatan."),
        ("C5",  "Jam Kerja & Lemburan",          "cost",    "Frekuensi lembur per bulan", "Frekuensi dan durasi lembur dalam sebulan."),
        ("C6",  "Reputasi Perusahaan",           "benefit", "Skala reputasi",             "Stabilitas keuangan dan nama baik perusahaan."),
        ("C7",  "Budaya Kerja (Culture)",        "benefit", "Skala budaya kerja",         "Lingkungan kerja suportif, minim politik kantor."),
        ("C8",  "Fasilitas & Benefit Non-Tunai", "benefit", "Skala fasilitas",            "Asuransi, laptop, gym, makan siang, dll."),
        ("C9",  "Biaya Transportasi & Parkir",   "cost",    "Rupiah per bulan",           "Bensin, tol, parkir jika WFO."),
        ("C10", "Pelatihan & Pengembangan",      "benefit", "Skala program/pelatihan",    "Kursus/sertifikasi gratis dari perusahaan."),
    ];

    public static void SeedKriteria(AppDbContext db)
    {
        foreach (var (id, nama, jenis, satuan, deskripsi) in Kriterias)
        {
            if (db.Kriteria.Any(k => k.KriteriaId == id)) continue;
            db.Kriteria.Add(new Kriteria
            {
                KriteriaId = id,
                NamaKriteria = nama,
                JenisAtribut = jenis,
                Satuan = satuan,
                Deskripsi = deskripsi
            });
        }
        db.SaveChanges();
    }
}