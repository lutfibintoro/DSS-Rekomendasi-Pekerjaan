# aplikasi dss rekomendasi pekerjaan berdasarkan preferensi pengguna
alur aplikasi: pengguna akan login lalu pengguna melakukan input kirteria berdasarkan preferensi mereka sendiri, setelah data disimpan, list loker di dashboard akan muncul dari yang paling cocok dengan preferensi pengguna.

## list kriteria yang digunakan dan skala pengukurannya
| Kode | Nama Kriteria | Jenis Atribut | Satuan | Deskripsi | Skala |
|------|---------------|---------------|--------|-----------|-------|
| C1 | Gaji Pokok & Tunjangan | Benefit | Rupiah per bulan | Total pendapatan bulanan. | Skala 1: < Rp 4.000.000<br>Skala 2: Rp 4.000.000 - Rp 7.000.000<br>Skala 3: > Rp 7.000.000 - Rp 12.000.000<br>Skala 4: > Rp 12.000.000 - Rp 20.000.000<br>Skala 5: > Rp 20.000.000 |
| C2 | Jarak & Waktu Tempuh | Cost | Kilometer | Jarak dari rumah ke kantor atau waktu tempuh. | Skala 1: < 5 Km (Waktu tempuh < 15 menit)<br>Skala 2: 5 - 15 Km (Waktu tempuh 15 - 30 menit)<br>Skala 3: > 15 - 30 Km (Waktu tempuh 30 - 60 menit)<br>Skala 4: > 30 - 50 Km (Waktu tempuh 1 - 2 jam)<br>Skala 5: > 50 Km (Waktu tempuh > 2 jam / Luar Kota) |
| C3 | Fleksibilitas Kerja | Benefit | Skala kerja | Sistem kerja. | Skala 1: Full WFO (5 hari di kantor) + Absensi ketat<br>Skala 2: Full WFO (Jam kerja fleksibel)<br>Skala 3: Hybrid (3 hari WFO, 2 hari WFH)<br>Skala 4: Hybrid (1-2 hari WFO, sisanya WFH)<br>Skala 5: Full Remote / Work From Anywhere (WFA) |
| C4 | Jenjang Karir | Benefit | Skala tingkat kejelasan | Kejelasan jalur promosi dan kenaikan jabatan. | Skala 1: Sangat Buruk / Tidak Ada<br>Skala 2: Buruk / Kurang Jelas<br>Skala 3: Cukup / Standar<br>Skala 4: Baik / Jelas<br>Skala 5: Sangat Baik / Sangat Jelas |
| C5 | Jam Kerja & Lemburan | Cost | Frekuensi lembur per bulan | Frekuensi dan durasi lembur dalam sebulan. | Skala 1: Tepat waktu (9-to-5), hampir tidak pernah lembur<br>Skala 2: Lembur jarang (1-2 kali sebulan)<br>Skala 3: Lembur terjadwal (Hanya saat closing bulanan / release)<br>Skala 4: Sering lembur (2-3 kali seminggu)<br>Skala 5: Sangat sering lembur (Tiap hari + akhir pekan tetap dihubungi) |
| C6 | Reputasi Perusahaan | Benefit | Skala reputasi | Stabilitas keuangan dan nama baik perusahaan di industri. | Skala 1: Sangat Buruk / Tidak Ada<br>Skala 2: Buruk / Kurang Jelas<br>Skala 3: Cukup / Standar<br>Skala 4: Baik / Jelas<br>Skala 5: Sangat Baik / Sangat Jelas |
| C7 | Budaya Kerja (Culture) | Benefit | Skala budaya kerja | Lingkungan kerja yang suportif dan minim politik kantor. | Skala 1: Sangat Buruk / Tidak Ada<br>Skala 2: Buruk / Kurang Jelas<br>Skala 3: Cukup / Standar<br>Skala 4: Baik / Jelas<br>Skala 5: Sangat Baik / Sangat Jelas |
| C8 | Fasilitas & Benefit Non-Tunai | Benefit | Skala fasilitas | Adanya asuransi swasta, laptop kantor, gym membership, makan siang, dll. | Skala 1: Tidak ada fasilitas tambahan<br>Skala 2: Hanya fasilitas dasar (BPJS Kesehatan)<br>Skala 3: BPJS + Asuransi Swasta + Laptop Kantor<br>Skala 4: BPJS + Asuransi Swasta (bisa klaim keluarga) + Gadget + Subsidi Gym/Makan<br>Skala 5: Fasilitas lengkap di atas + Saham Perusahaan (ESOP) / Bonus Tahunan Besar |
| C9 | Biaya Transportasi & Parkir | Cost | Rupiah per bulan | Estimasi pengeluaran bulanan untuk bensin, tol, atau parkir jika WFO. | Skala 1: < Rp 200.000 (Atau ditanggung penuh/disediakan jemputan)<br>Skala 2: Rp 200.000 - Rp 500.000<br>Skala 3: > Rp 500.000 - Rp 1.000.000<br>Skala 4: > Rp 1.000.000 - Rp 1.500.000<br>Skala 5: > Rp 1.500.000 (Banyak lewat tol/tarif parkir mahal) |
| C10 | Pelatihan & Pengembangan | Benefit | Skala program/pelatihan | Anggaran atau program kursus/sertifikasi gratis yang disediakan perusahaan. | Skala 1: Sangat Buruk / Tidak Ada<br>Skala 2: Buruk / Kurang Jelas<br>Skala 3: Cukup / Standar<br>Skala 4: Baik / Jelas<br>Skala 5: Sangat Baik / Sangat Jelas |

## contoh cara user melakukan input preferensi
| Kode | Kriteria | Bentuk Kolom Input yang Diisi User | Contoh Isian User | Konversi Sistem (Backend) |
|------|----------|-----------------------------------|-------------------|---------------------------|
| C1 | Gaji Pokok | Angka/Rupiah | 8.500.000 | 3 (karena rentang 7-12jt) |
| C2 | Jarak | Angka (Km) | 12 | 2 (karena rentang 5-15 Km) |
| C3 | Fleksibilitas | Dropdown Pilihan | Hybrid (3 hari kantor, 2 hari rumah) | 3 |
| C4 | Jenjang Karir | Dropdown Pilihan | Cukup / Standar | 3 |
| C5 | Jam Kerja | Dropdown Pilihan | Tepat waktu (9-to-5), jarang lembur | 1 |
| C6 | Reputasi | Dropdown Pilihan | Multinational / BUMN Besar | 5 |
| C7 | Budaya Kerja | Dropdown Pilihan | Baik / Jelas | 4 |
| C8 | Fasilitas | Dropdown Pilihan | BPJS + Asuransi Swasta + Laptop | 3 |
| C9 | Biaya Transport | Angka/Rupiah (Bulanan) | 400.000 | 2 (karena rentang 200-500rb) |
| C10 | Pelatihan | Dropdown Pilihan | Tidak Ada / Minim | 1 |

## desain rdbms yang akan digunakan
1. Tabel Pengguna (User)

| Kolom | Tipe Data | Constraint | Keterangan |
|------|-----------|------------|------------|
| user_id | int | pk, increment | ID unik pengguna. |
| username | varchar(50) | not null | Nama pengguna. |
| role | varchar(100) | enum('admin','pelamar') | role user. |
| email | varchar(100) | unique, not null | Email pengguna. |
| password_hash | varchar(255) | not null | Hash password pengguna. |

2. Tabel Kriteria (C1 - C10)

| Kolom | Tipe Data | Constraint | Keterangan |
|------|-----------|------------|------------|
| kriteria_id | varchar(5) | pk | ID kriteria (misalnya C1, C2, dst.). |
| nama_kriteria | varchar(100) | not null | Nama kriteria. |
| jenis_atribut | enum('benefit','cost') | not null | Jenis atribut kriteria. |

3. Tabel Bobot Preferensi User

| Kolom | Tipe Data | Constraint | Keterangan |
|------|-----------|------------|------------|
| preference_id | int | pk, increment | ID unik preferensi. |
| user_id | int |  | Relasi ke tabel users. |
| kriteria_id | varchar(5) |  | Relasi ke tabel kriteria. |
| nilai_bobot | int | note: 'Nilai antara 1 dan 5' | Bobot preferensi user. |
| nilai_asli | varchar(255) || nilai asli yang diinputkan user. |

4. Tabel Perusahaan (Diisi Admin/Scraping)

| Kolom | Tipe Data | Constraint | Keterangan |
|------|-----------|------------|------------|
| perusahaan_id | int | pk, increment | ID unik perusahaan. |
| nama_perusahaan | varchar(100) | not null | Nama perusahaan. |
| posisi_tersedia | varchar(100) |  | Posisi yang tersedia. |
| lokasi | varchar(100) |  | Lokasi perusahaan. |

5. Tabel Nilai Perusahaan

| Kolom | Tipe Data | Constraint | Keterangan |
|------|-----------|------------|------------|
| nilai_id | int | pk, increment | ID unik nilai alternatif. |
| perusahaan_id | int |  | Relasi ke tabel perusahaan. |
| kriteria_id | varchar(5) |  | Relasi ke tabel kriteria. |
| nilai_riil | varchar(255) | not null | Nilai asli dari perusahaan untuk kriteria tertentu. |
| nilai_skala | int | note: 'Nilai antara 1 dan 5' | Nilai skala hasil konversi sistem. |

## arsitektur aplikasi / tech stact
aplikasi dibagun menggunakan arsitektur client-server dengan request response berbentuk json, autentikasi menggunakan basic auth, menggunakan database postgresql, backend/server menggunakan asp.net core 10.0 dengan minimal api, frontend menggunakan reactjs, semuanya di build di atas docker dan selalu menggunakan docker compose untuk menjalankan atau mematikan container.

## halaman/page pada aplikasi web
1. halaman login dan registrasi.<br>
berisi input email dan password
2. halaman dashboard bagi role pelamar.<br>
berisi list daftar loker yang berurutan dari paling cocok dengan preferensi pelamar.<br>
dihalaman dashboard ada sidebar(preferensi dan home) preferensi untuk menavigasikan ke halaman input preferensi, home untuk navigasi kembali ke halaman awal dashboard.
3. halaman preferensi.<br>
berisi form input dengan struktur pada bagian "contoh cara user melakukan input preferensi"
4. halaman dashboard bagi role admin(hanya role admin yang bisa akses).<br>
berisi list daftar loker dengan tombol hapus untuk menghapus loker yang sudah tidak relevan atau tidak ingin menambah karyawan.<br>
dihalaman dashboard ada sidebar(loker-baru dan home) loker-baru untuk menavigasikan ke halaman input perusahaan baru yang sedang open loker, home untuk navigasi kembali ke halaman awal dashboard.

## api spec
bisa dibaca di API.md dan api-spec.yaml