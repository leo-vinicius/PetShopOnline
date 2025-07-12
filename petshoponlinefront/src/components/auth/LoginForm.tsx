import { useState } from 'react';
import { useAuth } from '../../context/AuthContext';
import authService from '../../services/AuthService';

export default function LoginForm() {
    const [email, setEmail] = useState('');
    const [senha, setSenha] = useState('');
    const [error, setError] = useState('');
    const { login } = useAuth();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError('');
        try {
            const result = await authService.login({ email, senha });
            if (result.token) {
                login(result.token, result.userType, result.userId);
            }
        } catch (err: any) {
            setError(err.message || 'Erro ao logar');
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <input placeholder="Email" value={email} onChange={e => setEmail(e.target.value)} required type="email" />
            <input placeholder="Senha" value={senha} onChange={e => setSenha(e.target.value)} required type="password" />
            <button type="submit">Entrar</button>
            {error && <div style={{ color: 'red' }}>{error}</div>}
        </form>
    );
}