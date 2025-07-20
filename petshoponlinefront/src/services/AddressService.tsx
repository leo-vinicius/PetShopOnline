const API_URL = 'https://localhost:7000/api/clientes/enderecos';

function getAuthHeaders() {
    const auth = JSON.parse(localStorage.getItem('auth') || '{}');
    const token = auth.token;
    const headers: Record<string, string> = { 'Content-Type': 'application/json' };
    if (token) {
        headers['Authorization'] = `Bearer ${token}`;
    }
    return headers;
}

async function criarEndereco(endereco: any) {
    const res = await fetch(API_URL, {
        method: 'POST',
        headers: getAuthHeaders(),
        body: JSON.stringify(endereco)
    });
    if (!res.ok) throw new Error('Erro ao criar endereço');
    return await res.json();
}

async function listarEnderecos() {
    const res = await fetch(API_URL, {
        headers: getAuthHeaders()
    });
    if (!res.ok) throw new Error('Erro ao buscar endereços');
    return await res.json();
}

export default { criarEndereco, listarEnderecos };