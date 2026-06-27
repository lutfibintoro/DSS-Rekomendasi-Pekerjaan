# Docker — DSS Rekomendasi Pekerjaan

Semua service (PostgreSQL, Backend ASP.NET Core, Frontend React) dijalankan
lewat **Docker Compose**.

## Struktur file

```
.
├── docker-compose.yml              # orkestrasi 3 service
├── .env.example                    # template env vars
├── db/
│   └── init/
│       └── 01-schema.sql           # auto-run saat postgres pertama kali start
└── RekomendasiPekerjaanDSS/
    ├── BackEnd/
    │   ├── Dockerfile              # multi-stage build (sdk -> aspnet runtime)
    │   └── .dockerignore
    └── FrontEnd/
        ├── Dockerfile              # multi-stage build (node -> nginx)
        ├── nginx.conf              # SPA fallback + reverse proxy /api -> backend
        └── .dockerignore
```

## Cara Menjalankan

### 1. Pertama kali
```bash
cp .env.example .env
# (optional) edit .env kalau mau ubah password/port

docker compose up -d --build
```

### 2. Akses
| Service     | URL                              | Keterangan                  |
|-------------|----------------------------------|------------------------------|
| Frontend    | http://localhost                 | Web app (melalui nginx)      |
| Backend     | http://localhost:8080            | API langsung (untuk debug)   |
| OpenAPI/Swagger | http://localhost:8080/openapi/v1.json | saat mode Development |
| PostgreSQL  | `localhost:5432`                 | user/pass sesuai `.env`      |

> Frontend sudah otomatis reverse-proxy `/api/*` ke backend lewat nginx,
> jadi dari sisi client cukup pakai `fetch('/api/...')` tanpa perlu tahu
> alamat backend.

### 3. Perintah umum
```bash
# Lihat log semua service
docker compose logs -f

# Lihat log salah satu service
docker compose logs -f backend

# Restart salah satu service
docker compose restart backend

# Stop & hapus container (data DB tetap aman)
docker compose down

# Stop & hapus container + volume (DATA DB HILANG)
docker compose down -v

# Rebuild salah satu service setelah edit kode
docker compose up -d --build backend
```

### 4. Database

- Schema otomatis dibuat dari `db/init/01-schema.sql` saat container
  Postgres pertama kali start.
- Data disimpan di **named volume** `db-data` (persistent).
- Untuk reset database:
  ```bash
  docker compose down -v   # hapus volume
  docker compose up -d     # volume dibuat ulang, init SQL jalan lagi
  ```

### 5. Mengakses Postgres dari host
```bash
docker compose exec db psql -U dss_user -d dss_db
```

## Catatan Pengembangan

- **Hot-reload**: Docker image ini production-style. Untuk dev dengan hot-reload,
  jalankan service di host langsung (`dotnet run` di BackEnd, `npm run dev` di
  FrontEnd) — `vite.config.ts` sudah ada proxy `/api` ke `localhost:8080`.
- **Build context**: `backend` build pakai context `./RekomendasiPekerjaanDSS`
  (parent dir) karena ada `.slnx` di situ. Jangan pindahkan `Dockerfile` ke
  root tanpa ubah `context` di compose.
- **Port conflict**: Kalau port `80` di host sudah dipakai, ubah
  `FRONTEND_PORT=8081` di `.env`. Frontend tetap bisa di-reverse-proxy dari
  nginx host kalau perlu.