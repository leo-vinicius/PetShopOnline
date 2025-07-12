import LoginForm from '../components/auth/LoginForm';
import HomeMenu from '../components/menu/Menu';

export default function LoginPage() {
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
                    }}>Login</h2>
                    <LoginForm />
                </div>
            </main>
        </div>
    );
}