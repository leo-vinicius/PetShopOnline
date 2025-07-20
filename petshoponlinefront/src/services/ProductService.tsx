const API_URL = "/api/Produtos";

function getAuthHeaders() {
    const auth = JSON.parse(localStorage.getItem('auth') || '{}');
    const token = auth.token;
    const headers: Record<string, string> = { 'Content-Type': 'application/json' };
    if (token) {
        headers['Authorization'] = `Bearer ${token}`;
    }
    return headers;
}

export async function listarProdutos() {
    const res = await fetch('https://localhost:7000/api/produtos', {
        method: 'GET',
        headers: getAuthHeaders()
    });
    if (!res.ok) throw new Error('Erro ao buscar produtos');
    return await res.json();
}

export async function buscarProdutoPorId(id: number) {
    const res = await fetch(`${API_URL}/${id}`, {
        method: 'GET',
        headers: getAuthHeaders()
    });
    if (!res.ok) throw new Error("Produto não encontrado");
    return await res.json();
}