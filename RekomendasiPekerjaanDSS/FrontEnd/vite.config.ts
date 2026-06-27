import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [plugin()],
    server: {
        port: 64990,
        // Proxy /api/* ke backend ASP.NET Core saat dev mode (di luar docker).
        // Di dalam docker, nginx di container frontend yang handle proxy ini.
        proxy: {
            '/api': {
                target: 'http://localhost:8080',
                changeOrigin: true,
            },
        },
    },
});