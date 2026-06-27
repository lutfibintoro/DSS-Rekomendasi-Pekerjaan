import { useState, useEffect } from 'react';
import { api } from '../api/client';
import { useNavigate } from 'react-router-dom';

type NilaiDetail = { kriteriaId: string; nilaiSkala: number; nilaiRiil: string };
type RekomendasiItem = {
  rank: number;
  perusahaanId: number;
  namaPerusahaan: string;
  posisiTersedia: string | null;
  lokasi: string | null;
  skorSaw: number;
  detailNilai: NilaiDetail[];
};

export function DashboardPelamar() {
  const navigate = useNavigate();
  const [items, setItems] = useState<RekomendasiItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    loadRekomendasi();
  }, []);

  const loadRekomendasi = async () => {
    try {
      setLoading(true);
      const res = await api.get<RekomendasiItem[]>('/api/rekomendasi');
      if (res.data) setItems(res.data);
    } catch (err: unknown) {
      const msg = err instanceof Error ? err.message : '';
      if (msg.includes('belum memiliki preferensi')) {
        setError('Anda belum mengisi preferensi. Silakan isi preferensi terlebih dahulu.');
      } else {
        setError(msg || 'Gagal memuat rekomendasi');
      }
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <div className="page"><p>Loading...</p></div>;
  if (error) return (
    <div className="page">
      <div className="alert alert-error">{error}</div>
      <button className="btn btn-primary" onClick={() => navigate('/preferensi')}>
        Isi Preferensi
      </button>
    </div>
  );

  return (
    <div className="page">
      <div className="dashboard-header">
        <h1>Rekomendasi Pekerjaan</h1>
        <p className="subtitle">Loker diurutkan dari yang paling cocok dengan preferensi Anda (metode SAW).</p>
        <button className="btn btn-outline" onClick={loadRekomendasi}>🔄 Refresh</button>
      </div>

      {items.length === 0 && <p className="empty">Belum ada loker tersedia.</p>}

      <div className="rekomendasi-list">
        {items.map(item => (
          <div key={item.perusahaanId} className={`rekom-card rank-${item.rank === 1 ? 'gold' : item.rank === 2 ? 'silver' : item.rank === 3 ? 'bronze' : ''}`}>
            <div className="rekom-rank">#{item.rank}</div>
            <div className="rekom-body">
              <h3>{item.namaPerusahaan}</h3>
              <p className="rekom-posisi">{item.posisiTersedia} · {item.lokasi}</p>
              <div className="rekom-nilai">
                {item.detailNilai.map(n => (
                  <span key={n.kriteriaId} className="nilai-chip" title={`Skala: ${n.nilaiSkala}`}>
                    {n.kriteriaId}={n.nilaiRiil}
                  </span>
                ))}
              </div>
            </div>
            <div className="rekom-score">
              <div className="score-value">{item.skorSaw.toFixed(2)}</div>
              <div className="score-label">Skor SAW</div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
