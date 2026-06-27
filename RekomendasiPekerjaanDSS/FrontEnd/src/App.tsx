import { BrowserRouter, Routes, Route, Navigate, Outlet } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import { ProtectedRoute, PelamarRoute, AdminRoute } from './components/ProtectedRoute';
import { Layout } from './components/Layout';
import { LoginPage } from './pages/LoginPage';
import { DashboardPelamar } from './pages/DashboardPelamar';
import { PreferensiPage } from './pages/PreferensiPage';
import { DashboardAdmin } from './pages/DashboardAdmin';
import { LokerBaruPage } from './pages/LokerBaruPage';

export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <Routes>
          <Route path="/login" element={<LoginPage />} />

          <Route element={<ProtectedRoute><Layout /></ProtectedRoute>}>
            <Route element={<PelamarRoute><Outlet /></PelamarRoute>}>
              <Route path="/dashboard" element={<DashboardPelamar />} />
              <Route path="/preferensi" element={<PreferensiPage />} />
            </Route>
            <Route element={<AdminRoute><Outlet /></AdminRoute>}>
              <Route path="/admin" element={<DashboardAdmin />} />
              <Route path="/admin/loker-baru" element={<LokerBaruPage />} />
            </Route>
          </Route>

          <Route path="*" element={<Navigate to="/login" replace />} />
        </Routes>
      </AuthProvider>
    </BrowserRouter>
  );
}
