# Sitemaps

```mermaid
flowchart TD
    Login["/login<br/>Login · Register"]
    DPel["/dashboard<br/>Rekomendasi Pekerjaan"]
    Pref["/preferensi<br/>Preferensi Kriteria"]
    DAdm["/admin<br/>Daftar Loker"]
    LBru["/admin/loker-baru<br/>Tambah Loker"]

    Login -->|"pelamar"| DPel
    Login -->|"admin"| DAdm
    DPel --> Pref
    Pref --> DPel
    DAdm --> LBru
    LBru --> DAdm
```