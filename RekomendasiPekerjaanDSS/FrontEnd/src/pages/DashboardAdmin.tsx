import { useState, useEffect } from 'react';
import { api } from '../api/client';
import { useNavigate } from 'react-router-dom';

type NilaiLoker = { nilaiId: number; perusahaanId: number; kriteriaId: string; nilaiRiil: string; nilaiSkala: number };
type Loker = {
  perusahaanId: number;
  namaPerusahaan: string;
  posisiTersedia: string | null;
  lokasi: string | null;
  nilai: NilaiLoker[];
};

export function DashboardAdmin() {
  const navigate = useNavigate();
  const [items, setItems] = useState<Loker[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => { loadLoker(); }, []);

  const loadLoker = async () => {
    try {
      setLoading(true);
      const res = await api.get<Loker[]>('/api/loker');
      if (res.data) setItems(res.data);
    } catch {
      // handled by api client (redirect on 401)
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: number) => {
    if (!confirm('Hapus loker ini?')) return;
    try {
      await api.delete(`/api/loker/${id}`);
      setItems(prev => prev.filter(i => i.perusahaanId !== id));
    } catch {
      alert('Gagal menghapus');
    }
  };

  if (loading) return <div className="page"><p>Loading...</p></div>;

  return (
    <div className="page">
      <div className="dashboard-header">
        <h1>Dashboard Admin</h1>
        <p className="subtitle">Daftar loker yang tersedia. Anda bisa menambah atau menghapus loker.</p>
        <button className="btn btn-primary" onClick={() => navigate('/admin/loker-baru')}>➕ Tambah Loker</button>
      </div>

      {items.length === 0 && <p className="empty">Belum ada loker.</p>}

      <div className="admin-table-wrap">
        <table className="admin-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Perusahaan</th>
              <th>Posisi</th>
              <th>Lokasi</th>
              <th>Nilai (Skala)</th>
              <th>Aksi</th>
            </tr>
          </thead>
          <tbody>
            {items.map(item => (
              <tr key={item.perusahaanId}>
                <td>{item.perusahaanId}</td>
                <td><strong>{item.namaPerusahaan}</strong></td>
                <td>{item.posisiTersedia}</td>
                <td>{item.lokasi}</td>
                <td>
                  <div className="nilai-mini">
                    {item.nilai.map(n => (
                      <span key={n.kriteriaId} className="nilai-chip" title={n.nilaiRiil}>
                        {n.kriteriaId}={n.nilaiSkala}
                      </span>
                    ))}
                  </div>
                </td>
                <td>
                  <button className="btn btn-danger btn-sm" onClick={() => handleDelete(item.perusahaanId)}>Hapus</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
