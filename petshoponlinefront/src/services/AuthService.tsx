const API_URL = 'https://localhost:7000/api/Auth';

async function cadastro({ nome, email, senha, telefone }: { nome: string, email: string, senha: string, telefone?: string }) {
    const res = await fetch(`https://localhost:7000/api/Clientes`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ nome, email, senha, telefone }),
    });
    const data = await res.json();
    if (!res.ok) throw new Error(data.message || 'Erro no cadastro');
    return data;
}

async function login({ email, senha }: { email: string, senha: string }) {
    const res = await fetch(`https://localhost:7000/api/Auth/cliente/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, senha }),
    });
    const data = await res.json();
    if (!res.ok) throw new Error(data.message || 'Erro no login');
    return {
        token: data.token,
        userType: data.tipoUsuario || data.userType,
        userId: data.userId,
        nome: data.nome,
        email: data.email
    };
}

async function logout(token?: string) {
    await fetch(`${API_URL}/logout`, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        }
    });
}

export default { cadastro, login, logout };