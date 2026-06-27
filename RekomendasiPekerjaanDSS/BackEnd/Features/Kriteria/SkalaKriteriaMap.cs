namespace BackEnd.Features.Kriteria;

/// <summary>
/// Master deskripsi skala 1-5 untuk setiap kriteria (C1-C10).
/// Sumber kebenaran tunggal, dipakai oleh GetAll dan GetById.
/// </summary>
public static class SkalaKriteriaMap
{
    public static readonly Dictionary<string, Dictionary<string, string>> Map = new()
    {
        ["C1"] = new() {
            ["1"] = "< Rp 4.000.000",
            ["2"] = "Rp 4.000.000 - Rp 7.000.000",
            ["3"] = "> Rp 7.000.000 - Rp 12.000.000",
            ["4"] = "> Rp 12.000.000 - Rp 20.000.000",
            ["5"] = "> Rp 20.000.000"
        },
        ["C2"] = new() {
            ["1"] = "< 5 Km (< 15 menit)",
            ["2"] = "5 - 15 Km (15 - 30 menit)",
            ["3"] = "> 15 - 30 Km (30 - 60 menit)",
            ["4"] = "> 30 - 50 Km (1 - 2 jam)",
            ["5"] = "> 50 Km (> 2 jam / Luar Kota)"
        },
        ["C3"] = new() {
            ["1"] = "Full WFO (5 hari) + Absensi ketat",
            ["2"] = "Full WFO (Jam kerja fleksibel)",
            ["3"] = "Hybrid (3 hari WFO, 2 hari WFH)",
            ["4"] = "Hybrid (1-2 hari WFO, sisanya WFH)",
            ["5"] = "Full Remote / Work From Anywhere"
        },
        ["C4"] = new() {
            ["1"] = "Sangat Buruk / Tidak Ada",
            ["2"] = "Buruk / Kurang Jelas",
            ["3"] = "Cukup / Standar",
            ["4"] = "Baik / Jelas",
            ["5"] = "Sangat Baik / Sangat Jelas"
        },
        ["C5"] = new() {
            ["1"] = "Tepat waktu (9-to-5), hampir tidak pernah lembur",
            ["2"] = "Lembur jarang (1-2 kali sebulan)",
            ["3"] = "Lembur terjadwal (closing/release)",
            ["4"] = "Sering lembur (2-3 kali seminggu)",
            ["5"] = "Sangat sering (tiap hari + akhir pekan)"
        },
        ["C6"] = new() {
            ["1"] = "Sangat Buruk / Tidak Ada",
            ["2"] = "Buruk / Kurang Jelas",
            ["3"] = "Cukup / Standar",
            ["4"] = "Baik / Jelas",
            ["5"] = "Sangat Baik / Sangat Jelas"
        },
        ["C7"] = new() {
            ["1"] = "Sangat Buruk / Tidak Ada",
            ["2"] = "Buruk / Kurang Jelas",
            ["3"] = "Cukup / Standar",
            ["4"] = "Baik / Jelas",
            ["5"] = "Sangat Baik / Sangat Jelas"
        },
        ["C8"] = new() {
            ["1"] = "Tidak ada fasilitas tambahan",
            ["2"] = "Hanya fasilitas dasar (BPJS Kesehatan)",
            ["3"] = "BPJS + Asuransi Swasta + Laptop Kantor",
            ["4"] = "BPJS + Asuransi (keluarga) + Gadget + Subsidi",
            ["5"] = "Lengkap + ESOP / Bonus Tahunan Besar"
        },
        ["C9"] = new() {
            ["1"] = "< Rp 200.000 (atau ditanggung penuh)",
            ["2"] = "Rp 200.000 - Rp 500.000",
            ["3"] = "> Rp 500.000 - Rp 1.000.000",
            ["4"] = "> Rp 1.000.000 - Rp 1.500.000",
            ["5"] = "> Rp 1.500.000"
        },
        ["C10"] = new() {
            ["1"] = "Sangat Buruk / Tidak Ada",
            ["2"] = "Buruk / Kurang Jelas",
            ["3"] = "Cukup / Standar",
            ["4"] = "Baik / Jelas",
            ["5"] = "Sangat Baik / Sangat Jelas"
        }
    };

    public static Dictionary<string, string> Get(string kriteriaId) =>
        Map.GetValueOrDefault(kriteriaId, new Dictionary<string, string>());
}