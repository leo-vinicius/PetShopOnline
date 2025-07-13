import { useNavigate } from 'react-router-dom';
import HomeMenu from '../components/menu/Menu';
import { useAuth } from '../context/AuthContext';
import authService from '../services/AuthService';

export default function ProfilePage() {
    const { auth, logout } = useAuth();
    const navigate = useNavigate();

    const handleLogout = async () => {
        try {
            await authService.logout(auth?.token);
        } catch { }
        logout();
        navigate('/');
    };

    if (!auth) {
        navigate('/');
        return null;
    }

    return (
        <div style={{ minHeight: '100vh', background: '#f7f7f7', width: '100vw' }}>
            <header style={{
                width: '100vw',
                background: '#00a2ffff',
                boxShadow: '0 2px 8px rgba(46,139,87,0.12)',
                padding: '0.5rem 0',
                position: 'sticky',
                top: 0,
                zIndex: 10
            }}>
                <HomeMenu horizontal />
            </header>
            <main style={{
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                justifyContent: 'center',
                minHeight: 'calc(100vh - 70px)'
            }}>
                <div style={{
                    background: '#fff',
                    borderRadius: '18px',
                    boxShadow: '0 8px 32px rgba(46,139,87,0.15)',
                    padding: '2.5rem 3rem',
                    maxWidth: '400px',
                    width: '100%',
                    marginBottom: '2rem',
                    textAlign: 'center'
                }}>
                    <h2 style={{
                        color: '#2196f3',
                        fontWeight: 700,
                        fontSize: '2rem',
                        marginBottom: '1.5rem'
                    }}>Meu Perfil</h2>
                    <div style={{ marginBottom: '1.5rem', fontSize: '1.1rem', color: '#333' }}>
                        <div><strong>ID:</strong> {auth.userId}</div>
                        <div><strong>Tipo:</strong> {auth.userType}</div>
                        <div><strong>Token:</strong> <span style={{ wordBreak: 'break-all', fontSize: '0.9rem' }}>{auth.token}</span></div>
                    </div>
                    <button
                        onClick={handleLogout}
                        style={{
                            background: 'linear-gradient(90deg, #d32f2f 60%, #f44336 100%)',
                            color: '#fff',
                            border: 'none',
                            borderRadius: '4px',
                            padding: '0.7rem',
                            fontWeight: 'bold',
                            cursor: 'pointer'
                        }}
                    >
                        Logout
                    </button>
                </div>
            </main>
        </div>
    );
}