# syntax=docker/dockerfile:1.7
# ============================================================
# Frontend — React + Vite + TypeScript, served by Nginx
# Build  : RekomendasiPekerjaanDSS/FrontEnd
# ============================================================

# ---------- Stage 1 : build ----------
FROM node:22-alpine AS build
WORKDIR /app

# Install deps dulu (cache layer)
COPY package*.json ./
RUN npm ci || npm install

# Copy source & build
COPY . .
RUN npm run build

# ---------- Stage 2 : nginx serve ----------
FROM nginx:1.27-alpine AS runtime

# Konfigurasi nginx (SPA fallback + reverse proxy /api -> backend)
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Static assets dari Vite
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80

HEALTHCHECK --interval=30s --timeout=3s --retries=3 \
    CMD wget -qO- http://127.0.0.1/ >/dev/null || exit 1

CMD ["nginx", "-g", "daemon off;"]