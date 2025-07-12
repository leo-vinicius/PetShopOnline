import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import authService from '../../services/AuthService';

export default function RegisterForm() {
    const [nome, setNome] = useState('');
    const [email, setEmail] = useState('');
    const [senha, setSenha] = useState('');
    const [telefone, setTelefone] = useState('');
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
        if (!nome || !email) {
            setError('Preencha todos os campos obrigatórios.');
            return;
        }
        try {
            const result = await authService.cadastro({ nome, email, senha, telefone });
            if (result.token) {
                login(result.token, result.userType, result.userId);
            }
        } catch (err: any) {
            setError(err.message || 'Erro ao cadastrar');
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
                placeholder="Nome"
                value={nome}
                onChange={e => setNome(e.target.value)}
                required
                style={{
                    padding: '0.7rem',
                    borderRadius: '4px',
                    border: '1px solid #ddd'
                }}
            />
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
            <input
                placeholder="Telefone"
                value={telefone}
                onChange={e => setTelefone(e.target.value)}
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
                Cadastrar
            </button>
            {error && (
                <div style={{ color: '#d32f2f', marginTop: '-0.5rem', fontSize: '0.95rem' }}>
                    {error}
                </div>
            )}
            <div style={{ marginTop: '0.5rem', textAlign: 'center', fontSize: '1rem' }}>
                Já possui cadastro?{' '}
                <span
                    style={{
                        color: '#2196f3',
                        cursor: 'pointer',
                        textDecoration: 'underline',
                        fontWeight: 500
                    }}
                    onClick={() => navigate('/login')}
                >
                    Clique aqui para fazer login
                </span>
            </div>
        </form>
    );
}