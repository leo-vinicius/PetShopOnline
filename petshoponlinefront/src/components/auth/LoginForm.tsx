import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import authService from '../../services/AuthService';

export default function LoginForm() {
    const [email, setEmail] = useState('');
    const [senha, setSenha] = useState('');
    const [error, setError] = useState('');
    const { login } = useAuth();
    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError('');
        if (senha.length < 6) {
            setError('A senha deve ter pelo menos 6 caracteres.');
            return;
        }
        if (!email) {
            setError('Informe o email.');
            return;
        }
        try {
            const result = await authService.login({ email, senha });
            if (result.token) {
                localStorage.setItem('auth', JSON.stringify(result));
                login(result.token, result.userType, result.userId);
                navigate('/');
            }
        } catch (err: any) {
            setError(err.message || 'Email ou senha incorretos');
        }
    };

    return (
        <form
            onSubmit={handleSubmit}
            style={{
                display: 'flex',
                flexDirection: 'column',
                gap: '1rem',
                background: '#fff',
                borderRadius: '8px',
                boxShadow: '0 2px 12px rgba(0,0,0,0.08)',
                padding: '2rem',
                maxWidth: '400px',
                margin: '0 auto'
            }}
        >
            <input
                placeholder="Email"
                value={email}
                onChange={e => setEmail(e.target.value)}
                required
                type="email"
                style={{
                    padding: '0.7rem',
                    borderRadius: '4px',
                    border: '1px solid #ddd'
                }}
            />
            <input
                placeholder="Senha"
                value={senha}
                onChange={e => setSenha(e.target.value)}
                required
                type="password"
                style={{
                    padding: '0.7rem',
                    borderRadius: '4px',
                    border: '1px solid #ddd'
                }}
            />
            <button
                type="submit"
                style={{
                    background: 'linear-gradient(90deg, #2196f3 60%, #4ecdc4 100%)',
                    color: '#fff',
                    border: 'none',
                    borderRadius: '4px',
                    padding: '0.7rem',
                    fontWeight: 'bold',
                    cursor: 'pointer'
                }}
            >
                Entrar
            </button>
            {error && (
                <div style={{ color: '#d32f2f', marginTop: '-0.5rem', fontSize: '0.95rem' }}>
                    {error}
                </div>
            )}
            <div style={{ marginTop: '0.5rem', textAlign: 'center', fontSize: '1rem' }}>
                Ainda não tem cadastro?{' '}
                <span
                    style={{
                        color: '#2196f3',
                        cursor: 'pointer',
                        textDecoration: 'underline',
                        fontWeight: 500
                    }}
                    onClick={() => navigate('/cadastro')}
                >
                    Clique aqui para se registrar
                </span>
            </div>
        </form>
    );
}