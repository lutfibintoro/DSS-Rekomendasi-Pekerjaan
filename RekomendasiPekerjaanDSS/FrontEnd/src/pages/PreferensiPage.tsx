import { useState, useEffect } from 'react';
import { api } from '../api/client';
import { useNavigate } from 'react-router-dom';

type Kriteria = {
  kriteriaId: string;
  namaKriteria: string;
  jenisAtribut: string;
  skala: Record<string, string>;
};

type PreferensiItem = {
  kriteriaId: string;
  nilaiAsli: string;
};

export function PreferensiPage() {
  const navigate = useNavigate();
  const [kriterias, setKriterias] = useState<Kriteria[]>([]);
  const [values, setValues] = useState<Record<string, string>>({});
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [message, setMessage] = useState('');

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      setLoading(true);
      const res = await api.get<Kriteria[]>('/api/kriteria');
      if (res.data) {
        // Sort C1, C2, ..., C10
        res.data.sort((a, b) => a.kriteriaId.localeCompare(b.kriteriaId));
        setKriterias(res.data);

        // Load existing preferences if any
        try {
          const prefRes = await api.get<PreferensiItem[]>('/api/preferences');
          if (prefRes.data) {
            const loaded: Record<string, string> = {};
            prefRes.data.forEach(p => { loaded[p.kriteriaId] = p.nilaiAsli; });
            setValues(loaded);
          }
        } catch {
          // No existing prefs, that's ok
        }
      }
    } catch {
      setMessage('Gagal memuat data kriteria');
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (id: string, val: string) => {
    setValues(prev => ({ ...prev, [id]: val }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSaving(true);
    setMessage('');
    try {
      const preferences = kriterias.map(k => ({
        kriteriaId: k.kriteriaId,
        nilaiAsli: values[k.kriteriaId] || '',
      }));
      await api.post('/api/preferences', { preferences });
      setMessage('Preferensi berhasil disimpan!');
      setTimeout(() => navigate('/dashboard'), 1500);
    } catch (err: unknown) {
      setMessage(err instanceof Error ? err.message : 'Gagal menyimpan');
    } finally {
      setSaving(false);
    }
  };

  const handleReset = async () => {
    if (!confirm('Hapus semua preferensi?')) return;
    try {
      await api.delete('/api/preferences');
      setValues({});
      setMessage('Preferensi dihapus');
    } catch (err: unknown) {
      setMessage(err instanceof Error ? err.message : 'Gagal');
    }
  };

  if (loading) return <div className="page"><p>Loading...</p></div>;

  return (
    <div className="page">
      <h1>Preferensi</h1>
      <p className="subtitle">Isi preferensi Anda untuk mendapatkan rekomendasi pekerjaan terbaik.</p>

      {message && <div className={`alert ${message.includes('berhasil') ? 'alert-success' : 'alert-error'}`}>{message}</div>}

      <form onSubmit={handleSubmit} className="preferensi-form">
        <div className="preferensi-grid">
          {kriterias.map(k => (
            <div key={k.kriteriaId} className="form-group">
              <label>
                {k.kriteriaId} — {k.namaKriteria}
                <span className={`badge ${k.jenisAtribut === 'benefit' ? 'badge-benefit' : 'badge-cost'}`}>
                  {k.jenisAtribut}
                </span>
              </label>

              {(k.kriteriaId === 'C1' || k.kriteriaId === 'C2' || k.kriteriaId === 'C9') ? (
                <input
                  type="text"
                  placeholder={k.kriteriaId === 'C1' ? 'Rp 8.500.000' : k.kriteriaId === 'C2' ? '12 Km' : 'Rp 400.000'}
                  value={values[k.kriteriaId] || ''}
                  onChange={e => handleChange(k.kriteriaId, e.target.value)}
                  required
                />
              ) : (
                <select
                  value={values[k.kriteriaId] || ''}
                  onChange={e => handleChange(k.kriteriaId, e.target.value)}
                  required
                >
                  <option value="">— Pilih —</option>
                  {Object.entries(k.skala || {}).map(([skala, deskripsi]) => (
                    <option key={skala} value={deskripsi}>
                      {skala}: {deskripsi}
                    </option>
                  ))}
                </select>
              )}
            </div>
          ))}
        </div>

        <div className="preferensi-actions">
          <button className="btn btn-primary" type="submit" disabled={saving}>
            {saving ? 'Menyimpan...' : 'Simpan Preferensi'}
          </button>
          <button className="btn btn-secondary" type="button" onClick={handleReset}>
            Reset
          </button>
        </div>
      </form>
    </div>
  );
}
