using System.Globalization;
using System.Text.RegularExpressions;

namespace BackEnd.Common;

/// <summary>
/// Konverter dari nilai_asli yang diinput user -> nilai_bobot skala 1-5.
/// Berdasarkan tabel skala di README.md dan api-spec.yaml.
/// Mendukung angka (untuk C1, C2, C9) atau label teks (untuk C3..C8, C10).
/// </summary>
public static class NilaiKonverter
{
    public static short Konversi(string kriteriaId, string nilaiAsli)
    {
        if (string.IsNullOrWhiteSpace(nilaiAsli))
            throw new InvalidOperationException($"Nilai asli untuk {kriteriaId} kosong");

        var text = nilaiAsli.Trim().ToLowerInvariant();

        return kriteriaId switch
        {
            "C1" => KonversiGaji(text),
            "C2" => KonversiJarak(text),
            "C3" => KonversiFleksibilitas(text),
            "C4" or "C6" or "C7" or "C10" => KonversiSkalaLima(text),
            "C5" => KonversiLembur(text),
            "C8" => KonversiFasilitas(text),
            "C9" => KonversiTransport(text),
            _ => throw new InvalidOperationException($"Kriteria {kriteriaId} tidak dikenal")
        };
    }

    private static long ParseNumber(string s) =>
        long.Parse(Regex.Replace(s, "[^0-9]", ""), CultureInfo.InvariantCulture);

    // C1: Gaji (benefit, semakin besar semakin tinggi skalanya)
    private static short KonversiGaji(string t)
    {
        var v = ParseNumber(t);
        if (v < 4_000_000) return 1;
        if (v <= 7_000_000) return 2;
        if (v <= 12_000_000) return 3;
        if (v <= 20_000_000) return 4;
        return 5;
    }

    // C2: Jarak (cost, semakin besar semakin rendah skalanya)
    private static short KonversiJarak(string t)
    {
        var v = ParseNumber(t);
        if (v < 5) return 1;
        if (v <= 15) return 2;
        if (v <= 30) return 3;
        if (v <= 50) return 4;
        return 5;
    }

    // C9: Biaya transport (cost, semakin besar semakin rendah)
    private static short KonversiTransport(string t)
    {
        var v = ParseNumber(t);
        if (v < 200_000) return 1;
        if (v <= 500_000) return 2;
        if (v <= 1_000_000) return 3;
        if (v <= 1_500_000) return 4;
        return 5;
    }

    // C3: Fleksibilitas (benefit, berdasarkan label)
    private static short KonversiFleksibilitas(string t)
    {
        if (t.Contains("full remote") || t.Contains("wfa") || t.Contains("work from anywhere")) return 5;
        if (t.Contains("hybrid") && (t.Contains("1-2") || t.Contains("1 hari") || t.Contains("2 hari") && t.Contains("hybrid"))) return 4;
        if (t.Contains("hybrid")) return 3;
        if (t.Contains("full wfo") && t.Contains("fleksibel")) return 2;
        if (t.Contains("full wfo")) return 1;
        return 3; // default fallback
    }

    // C5: Lembur (cost)
    private static short KonversiLembur(string t)
    {
        if (t.Contains("tepat waktu") || t.Contains("tidak pernah") || t.Contains("9-to-5")) return 1;
        if (t.Contains("jarang") || t.Contains("1-2 kali")) return 2;
        if (t.Contains("terjadwal") || t.Contains("closing") || t.Contains("release")) return 3;
        if (t.Contains("sering") && (t.Contains("2-3") || t.Contains("seminggu"))) return 4;
        if (t.Contains("tiap hari") || t.Contains("setiap hari")) return 5;
        return 3;
    }

    // C8: Fasilitas (benefit)
    private static short KonversiFasilitas(string t)
    {
        if (t.Contains("esop") || t.Contains("saham") || t.Contains("bonus tahunan")) return 5;
        if (t.Contains("gadget") || t.Contains("gym") || t.Contains("makan")) return 4;
        if (t.Contains("bpjs") && t.Contains("asuransi") && t.Contains("laptop")) return 3;
        if (t.Contains("bpjs")) return 2;
        return 1;
    }

    // Generic 1-5 label matcher untuk C4, C6, C7, C10
    private static short KonversiSkalaLima(string t)
    {
        if (t.Contains("tidak ada") || t.Contains("sangat buruk")) return 1;
        if (t.Contains("buruk") || t.Contains("kurang")) return 2;
        if (t.Contains("cukup") || t.Contains("standar")) return 3;
        if (t.Contains("baik") || t.Contains("jelas")) return 4;
        if (t.Contains("sangat baik") || t.Contains("sangat jelas") || t.Contains("multinational") || t.Contains("bumn")) return 5;
        return 3;
    }
}