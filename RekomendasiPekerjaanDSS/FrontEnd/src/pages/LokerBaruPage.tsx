import { useState, useEffect } from 'react';
import { api } from '../api/client';
import { useNavigate } from 'react-router-dom';

type Kriteria = { kriteriaId: string; namaKriteria: string; jenisAtribut: string };

type NilaiInput = { kriteriaId: string; nilaiRiil: string };

export function LokerBaruPage() {
  const navigate = useNavigate();
  const [kriterias, setKriterias] = useState<Kriteria[]>([]);
  const [nama, setNama] = useState('');
  const [posisi, setPosisi] = useState('');
  const [lokasi, setLokasi] = useState('');
  const [nilai, setNilai] = useState<Record<string, string>>({});
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    const load = async () => {
      try {
        const res = await api.get<Kriteria[]>('/api/kriteria');
        if (res.data) {
          res.data.sort((a, b) => a.kriteriaId.localeCompare(b.kriteriaId));
          setKriterias(res.data);
          const init: Record<string, string> = {};
          res.data.forEach(k => { init[k.kriteriaId] = ''; });
          setNilai(init);
        }
      } catch {
        setError('Gagal memuat kriteria');
      } finally {
        setLoading(false);
      }
    };
    load();
  }, []);

  const updateNilai = (id: string, value: string) => {
    setNilai(prev => ({ ...prev, [id]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!nama || !posisi || !lokasi) { setError('Isi nama, posisi, dan lokasi'); return; }

    const nilaiPerKriteria: NilaiInput[] = Object.entries(nilai)
      .filter(([_, v]) => v.trim())
      .map(([k, v]) => ({ kriteriaId: k, nilaiRiil: v }));

    if (nilaiPerKriteria.length === 0) { setError('Isi minimal satu nilai kriteria'); return; }

    setSaving(true);
    setError('');
    try {
      await api.post('/api/loker', {
        namaPerusahaan: nama,
        posisiTersedia: posisi,
        lokasi,
        nilaiPerKriteria,
      });
      navigate('/admin');
    } catch (err: unknown) {
      setError(err instanceof Error ? err.message : 'Gagal menyimpan');
    } finally {
      setSaving(false);
    }
  };

  if (loading) return <div className="page"><p>Loading...</p></div>;

  return (
    <div className="page">
      <h1>Tambah Loker Baru</h1>

      {error && <div className="alert alert-error">{error}</div>}

      <form onSubmit={handleSubmit} className="loker-form">
        <div className="form-row">
          <div className="form-group">
            <label>Nama Perusahaan</label>
            <input type="text" value={nama} onChange={e => setNama(e.target.value)} required />
          </div>
          <div className="form-group">
            <label>Posisi</label>
            <input type="text" value={posisi} onChange={e => setPosisi(e.target.value)} required />
          </div>
          <div className="form-group">
            <label>Lokasi</label>
            <input type="text" value={lokasi} onChange={e => setLokasi(e.target.value)} required />
          </div>
        </div>

        <h3>Nilai per Kriteria</h3>
        <div className="nilai-grid">
          {kriterias.map(k => (
            <div key={k.kriteriaId} className="nilai-input-group">
              <label>{k.kriteriaId} — {k.namaKriteria}</label>
              <input
                type="text"
                placeholder={k.kriteriaId === 'C1' ? 'Rp 8.500.000' : k.kriteriaId === 'C2' ? '12 Km' : k.kriteriaId === 'C9' ? 'Rp 400.000' : 'Full WFO / Hybrid / Full Remote'}
                value={nilai[k.kriteriaId] || ''}
                onChange={e => updateNilai(k.kriteriaId, e.target.value)}
              />
            </div>
          ))}
        </div>

        <button className="btn btn-primary" type="submit" disabled={saving}>
          {saving ? 'Menyimpan...' : 'Simpan Loker'}
        </button>
      </form>
    </div>
  );
}
