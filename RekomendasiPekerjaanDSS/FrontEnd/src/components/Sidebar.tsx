import { NavLink, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

export function Sidebar() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const isAdmin = user?.role === 'admin';

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <aside className="sidebar">
      <div className="sidebar-header">
        <h2>DSS Pekerjaan</h2>
        <p className="sidebar-user">{user?.username} ({user?.role})</p>
      </div>
      <nav className="sidebar-nav">
        {isAdmin ? (
          <>
            <NavLink to="/admin" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'}>
              <span>🏠</span> Home
            </NavLink>
            <NavLink to="/admin/loker-baru" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'}>
              <span>➕</span> Loker Baru
            </NavLink>
          </>
        ) : (
          <>
            <NavLink to="/dashboard" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'}>
              <span>🏠</span> Home
            </NavLink>
            <NavLink to="/preferensi" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'}>
              <span>⚙️</span> Preferensi
            </NavLink>
          </>
        )}
      </nav>
      <div className="sidebar-footer">
        <button className="btn btn-logout" onClick={handleLogout}>Logout</button>
      </div>
    </aside>
  );
}
