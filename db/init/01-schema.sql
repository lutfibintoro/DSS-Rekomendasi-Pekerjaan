-- =============================================================
-- Schema DB untuk DSS Rekomendasi Pekerjaan
-- Otomatis dijalankan oleh PostgreSQL saat container pertama
-- kali start (folder ./db/init/ di-mount ke /docker-entrypoint-initdb.d/).
-- =============================================================

-- (Optional) ekstensi yang umum dipakai
-- CREATE EXTENSION IF NOT EXISTS "uuid-ossp";   -- butuh superuser

-- -------------------------------------------------------------
-- 1. Tabel Pengguna (User)
-- -------------------------------------------------------------
CREATE TABLE IF NOT EXISTS users (
    user_id       SERIAL       PRIMARY KEY,
    username      VARCHAR(50)  NOT NULL,
    email         VARCHAR(100) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    role          VARCHAR(20)  NOT NULL DEFAULT 'pelamar'
                  CHECK (role IN ('admin', 'pelamar')),
    created_at    TIMESTAMP    NOT NULL DEFAULT NOW()
);

-- -------------------------------------------------------------
-- 2. Tabel Kriteria (C1 - C10)
-- -------------------------------------------------------------
CREATE TABLE IF NOT EXISTS kriteria (
    kriteria_id   VARCHAR(5)   PRIMARY KEY,
    nama_kriteria VARCHAR(100) NOT NULL,
    jenis_atribut VARCHAR(10)  NOT NULL
                  CHECK (jenis_atribut IN ('benefit', 'cost')),
    satuan        VARCHAR(50),
    deskripsi     TEXT,
    created_at    TIMESTAMP    NOT NULL DEFAULT NOW()
);

-- Seed master kriteria sesuai README
INSERT INTO kriteria (kriteria_id, nama_kriteria, jenis_atribut, satuan, deskripsi) VALUES
    ('C1',  'Gaji Pokok & Tunjangan',         'benefit', 'Rupiah per bulan',          'Total pendapatan bulanan.'),
    ('C2',  'Jarak & Waktu Tempuh',           'cost',    'Kilometer',                  'Jarak dari rumah ke kantor atau waktu tempuh.'),
    ('C3',  'Fleksibilitas Kerja',            'benefit', 'Skala kerja',                'Sistem kerja (WFO / Hybrid / Remote).'),
    ('C4',  'Jenjang Karir',                  'benefit', 'Skala tingkat kejelasan',    'Kejelasan jalur promosi dan kenaikan jabatan.'),
    ('C5',  'Jam Kerja & Lemburan',           'cost',    'Frekuensi lembur per bulan', 'Frekuensi dan durasi lembur dalam sebulan.'),
    ('C6',  'Reputasi Perusahaan',            'benefit', 'Skala reputasi',             'Stabilitas keuangan dan nama baik perusahaan.'),
    ('C7',  'Budaya Kerja (Culture)',         'benefit', 'Skala budaya kerja',         'Lingkungan kerja suportif, minim politik kantor.'),
    ('C8',  'Fasilitas & Benefit Non-Tunai',  'benefit', 'Skala fasilitas',            'Asuransi, laptop, gym, makan siang, dll.'),
    ('C9',  'Biaya Transportasi & Parkir',    'cost',    'Rupiah per bulan',           'Bensin, tol, parkir jika WFO.'),
    ('C10', 'Pelatihan & Pengembangan',       'benefit', 'Skala program/pelatihan',    'Kursus/sertifikasi gratis dari perusahaan.')
ON CONFLICT (kriteria_id) DO NOTHING;

-- -------------------------------------------------------------
-- 3. Tabel Bobot Preferensi User
-- -------------------------------------------------------------
CREATE TABLE IF NOT EXISTS preferensi_user (
    preference_id SERIAL        PRIMARY KEY,
    user_id       INTEGER       NOT NULL REFERENCES users(user_id)    ON DELETE CASCADE,
    kriteria_id   VARCHAR(5)    NOT NULL REFERENCES kriteria(kriteria_id) ON DELETE CASCADE,
    nilai_asli    VARCHAR(255)  NOT NULL,
    nilai_bobot   SMALLINT      NOT NULL CHECK (nilai_bobot BETWEEN 1 AND 5),
    created_at    TIMESTAMP     NOT NULL DEFAULT NOW(),
    updated_at    TIMESTAMP     NOT NULL DEFAULT NOW(),
    UNIQUE (user_id, kriteria_id)
);

CREATE INDEX IF NOT EXISTS idx_preferensi_user ON preferensi_user(user_id);

-- -------------------------------------------------------------
-- 4. Tabel Perusahaan (Diisi Admin/Scraping)
-- -------------------------------------------------------------
CREATE TABLE IF NOT EXISTS perusahaan (
    perusahaan_id   SERIAL       PRIMARY KEY,
    nama_perusahaan VARCHAR(100) NOT NULL,
    posisi_tersedia VARCHAR(100),
    lokasi          VARCHAR(100),
    created_at      TIMESTAMP    NOT NULL DEFAULT NOW()
);

-- -------------------------------------------------------------
-- 5. Tabel Nilai Perusahaan (per kriteria)
-- -------------------------------------------------------------
CREATE TABLE IF NOT EXISTS nilai_perusahaan (
    nilai_id      SERIAL       PRIMARY KEY,
    perusahaan_id INTEGER      NOT NULL REFERENCES perusahaan(perusahaan_id) ON DELETE CASCADE,
    kriteria_id   VARCHAR(5)   NOT NULL REFERENCES kriteria(kriteria_id)      ON DELETE CASCADE,
    nilai_riil    VARCHAR(255) NOT NULL,
    nilai_skala   SMALLINT     NOT NULL CHECK (nilai_skala BETWEEN 1 AND 5),
    created_at    TIMESTAMP    NOT NULL DEFAULT NOW(),
    updated_at    TIMESTAMP    NOT NULL DEFAULT NOW(),
    UNIQUE (perusahaan_id, kriteria_id)
);

CREATE INDEX IF NOT EXISTS idx_nilai_perusahaan ON nilai_perusahaan(perusahaan_id);