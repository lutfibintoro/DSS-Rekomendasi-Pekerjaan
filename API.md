# API Spec — DSS Rekomendasi Pekerjaan

Spesifikasi REST API untuk aplikasi DSS rekomendasi pekerjaan. Format lengkap
OpenAPI 3.0 ada di [`api-spec.yaml`](./api-spec.yaml).

- **Base URL** (dev): `http://localhost:5000/api`
- **Auth**: Basic Auth → `Authorization: Basic base64(email:password)`
  (tidak ada token yang disimpan di database; frontend mengirim `email:password` langsung pada setiap request, backend memvalidasi ulang tiap kali)
- **Content-Type**: `application/json`

---

## Ringkasan Endpoint

| Method | Endpoint                       | Akses        | Halaman Terkait                    |
|--------|--------------------------------|--------------|------------------------------------|
| POST   | `/auth/register`               | public       | Halaman Registrasi                 |
| POST   | `/auth/login`                  | public       | Halaman Login                      |
| GET    | `/auth/me`                     | any login    | -                                  |
| GET    | `/kriteria`                    | any login    | Halaman Preferensi, Dashboard Admin|
| GET    | `/kriteria/{id}`               | any login    | Halaman Preferensi                 |
| GET    | `/preferences`                 | pelamar      | Halaman Preferensi                 |
| POST   | `/preferences`                 | pelamar      | Halaman Preferensi                 |
| DELETE | `/preferences`                 | pelamar      | Halaman Preferensi                 |
| GET    | `/loker`                       | admin        | Dashboard Admin                    |
| POST   | `/loker`                       | admin        | Halaman Loker Baru                 |
| GET    | `/loker/{id}`                  | any login    | Dashboard Admin/Detail             |
| PUT    | `/loker/{id}`                  | admin        | Dashboard Admin                    |
| DELETE | `/loker/{id}`                  | admin        | Dashboard Admin                    |
| GET    | `/loker/{id}/nilai`            | any login    | Dashboard Admin                    |
| GET    | `/rekomendasi`                 | pelamar      | Dashboard Pelamar                  |

---

## Alur Halaman → Endpoint

### 1. Login & Registrasi
- Submit form registrasi → `POST /auth/register`
- Submit form login → `POST /auth/login` (backend hanya validasi kredensial; **tidak ada token**)
- Simpan `email` & `password` di frontend (mis. `sessionStorage`) untuk dipakai pada request berikutnya
- Header untuk semua request berikutnya:
  ```
  Authorization: Basic base64(email:password)
  ```
  Backend akan memvalidasi ulang `email` & `password` tersebut pada setiap request.

### 2. Dashboard Pelamar (`/rekomendasi`)
- Pertama kali buka → frontend panggil `GET /preferences`. Jika 404,
  arahkan user ke halaman Preferensi.
- Jika preferensi sudah ada → panggil `GET /rekomendasi` untuk dapat daftar
  loker terurut.
- Sidebar menu:
  - **Home** → panggil ulang `GET /rekomendasi`.
  - **Preferensi** → buka form Preferensi.

### 3. Halaman Preferensi (Pelamar)
- Mount komponen → `GET /kriteria` untuk render label & opsi dropdown tiap kriteria.
- Jika user sudah pernah input → `GET /preferences` lalu prefill form-nya.
- Submit form → `POST /preferences` (payload berisi 10 item, lihat contoh).
- Tombol "Reset" → `DELETE /preferences`.

### 4. Dashboard Admin (`/loker`)
- Mount → `GET /loker` untuk render tabel daftar loker.
- Tiap baris punya tombol **Hapus** → `DELETE /loker/{id}`.
- Sidebar menu:
  - **Home** → refresh `GET /loker`.
  - **Loker Baru** → buka form input perusahaan + nilai per kriteria.

### 5. Halaman Loker Baru (Admin)
- Mount → `GET /kriteria` untuk render field nilai per-kriteria.
- Submit → `POST /loker` (sekaligus insert 10 baris `nilai_perusahaan`).

---

## Contoh Payload

### `POST /auth/register`
```json
{
  "username": "budi_santoso",
  "email": "budi@example.com",
  "password": "password123",
  "role": "pelamar"
}
```

### `POST /auth/login`
```json
{
  "email": "budi@example.com",
  "password": "password123"
}
```
Response (tidak ada token — frontend menyimpan email & password untuk dipakai di header Basic Auth):
```json
{
  "success": true,
  "data": {
    "user": { "user_id": 1, "username": "budi_santoso", "email": "budi@example.com", "role": "pelamar" }
  }
}
```

Contoh header yang dikirim frontend pada request berikutnya:
```
Authorization: Basic YnVkaUBleGFtcGxlLmNvbTpwYXNzd29yZDEyMw==
   (di mana base64-nya adalah "budi@example.com:password123")
```

### `POST /preferences`
```json
{
  "preferences": [
    { "kriteria_id": "C1", "nilai_asli": "8500000" },
    { "kriteria_id": "C2", "nilai_asli": "12" },
    { "kriteria_id": "C3", "nilai_asli": "Hybrid (3 hari WFO, 2 hari WFH)" },
    { "kriteria_id": "C4", "nilai_asli": "Cukup / Standar" },
    { "kriteria_id": "C5", "nilai_asli": "Tepat waktu (9-to-5), jarang lembur" },
    { "kriteria_id": "C6", "nilai_asli": "Sangat Baik / Sangat Jelas" },
    { "kriteria_id": "C7", "nilai_asli": "Baik / Jelas" },
    { "kriteria_id": "C8", "nilai_asli": "BPJS + Asuransi Swasta + Laptop" },
    { "kriteria_id": "C9", "nilai_asli": "400000" },
    { "kriteria_id": "C10", "nilai_asli": "Tidak Ada / Minim" }
  ]
}
```
Response (setelah backend konversi `nilai_asli` → `nilai_bobot`):
```json
{
  "success": true,
  "data": [
    { "kriteria_id": "C1", "nilai_asli": "8500000", "nilai_bobot": 3 },
    { "kriteria_id": "C2", "nilai_asli": "12",       "nilai_bobot": 2 },
    { "kriteria_id": "C3", "nilai_asli": "Hybrid ...", "nilai_bobot": 3 },
    { "kriteria_id": "C4", "nilai_asli": "Cukup / Standar", "nilai_bobot": 3 },
    { "kriteria_id": "C5", "nilai_asli": "Tepat waktu ...",  "nilai_bobot": 1 },
    { "kriteria_id": "C6", "nilai_asli": "Sangat Baik ...",  "nilai_bobot": 5 },
    { "kriteria_id": "C7", "nilai_asli": "Baik / Jelas",     "nilai_bobot": 4 },
    { "kriteria_id": "C8", "nilai_asli": "BPJS + ...",       "nilai_bobot": 3 },
    { "kriteria_id": "C9", "nilai_asli": "400000",           "nilai_bobot": 2 },
    { "kriteria_id": "C10","nilai_asli": "Tidak Ada / Minim","nilai_bobot": 1 }
  ]
}
```

### `POST /loker` (admin)
```json
{
  "nama_perusahaan": "PT. Teknologi Maju",
  "posisi_tersedia": "Backend Developer",
  "lokasi": "Jakarta Selatan",
  "nilai_per_kriteria": [
    { "kriteria_id": "C1", "nilai_riil": "Rp 10.000.000", "nilai_skala": 3 },
    { "kriteria_id": "C2", "nilai_riil": "8 Km",          "nilai_skala": 2 },
    { "kriteria_id": "C3", "nilai_riil": "Hybrid",        "nilai_skala": 4 }
  ]
}
```

### `GET /rekomendasi`
Response (urut descending by `skor_saw`):
```json
{
  "success": true,
  "data": [
    {
      "rank": 1,
      "perusahaan_id": 3,
      "nama_perusahaan": "PT. Teknologi Maju",
      "posisi_tersedia": "Backend Developer",
      "lokasi": "Jakarta Selatan",
      "skor_saw": 0.8525,
      "detail_nilai": [
        { "kriteria_id": "C1", "nilai_skala": 3 },
        { "kriteria_id": "C2", "nilai_skala": 2 }
      ]
    },
    {
      "rank": 2,
      "perusahaan_id": 1,
      "nama_perusahaan": "CV. Solusi Digital",
      "skor_saw": 0.7810
    }
  ]
}
```

---

## Catatan Perhitungan SAW (Backend)

Dokumentasi internal untuk tim backend saat implementasi `GET /rekomendasi`:

1. **Ambil preferensi** user yang sedang login → array `w_j` (1 ≤ w_j ≤ 5).
2. **Ambil semua loker + nilai per kriteria** → bentuk matriks keputusan `X[i][j]`.
3. **Normalisasi**:
   - Benefit → `r_ij = X[i][j] / max_j(X[*][j])`
   - Cost    → `r_ij = min_j(X[*][j]) / X[i][j]`
4. **Skor SAW** per alternatif: `V_i = Σ (r_ij × w_j)`
5. **Sort** descending, tambahkan field `rank`.

Hasil boleh di-cache per-user selama preferensi tidak berubah.