const API_URL = 'https://localhost:7001/api/auth';

async function cadastro({ nome, email, senha, telefone }: { nome: string, email: string, senha: string, telefone?: string }) {
    const res = await fetch(`${API_URL}/cliente/registro`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ nome, email, senha, telefone }),
    });
    const data = await res.json();
    if (!res.ok) throw new Error(data.message || 'Erro no cadastro');
    // Após cadastro, pode realizar login automático se desejar
    return data;
}

async function login({ email, senha }: { email: string, senha: string }) {
    const res = await fetch(`${API_URL}/cliente/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, senha }),
    });
    const data = await res.json();
    if (!res.ok) throw new Error(data.message || 'Erro no login');
    return data;
}

export default { cadastro, login };