import { useState } from 'react';
import { useAuth } from '../../context/AuthContext';
import authService from '../../services/AuthService';

export default function RegisterForm() {
    const [nome, setNome] = useState('');
    const [email, setEmail] = useState('');
    const [senha, setSenha] = useState('');
    const [telefone, setTelefone] = useState('');
    const [error, setError] = useState('');
    const { login } = useAuth();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError('');
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
        <form onSubmit={handleSubmit}>
            <input placeholder="Nome" value={nome} onChange={e => setNome(e.target.value)} required />
            <input placeholder="Email" value={email} onChange={e => setEmail(e.target.value)} required type="email" />
            <input placeholder="Senha" value={senha} onChange={e => setSenha(e.target.value)} required type="password" />
            <input placeholder="Telefone" value={telefone} onChange={e => setTelefone(e.target.value)} />
            <button type="submit">Cadastrar</button>
            {error && <div style={{ color: 'red' }}>{error}</div>}
        </form>
    );
}