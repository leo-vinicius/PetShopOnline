const API_URL = 'https://localhost:7000/api/carrinho';

function getAuthHeaders() {
    const auth = JSON.parse(localStorage.getItem('auth') || '{}');
    const token = auth.token;
    const headers: Record<string, string> = { 'Content-Type': 'application/json' };
    if (token) {
        headers['Authorization'] = `Bearer ${token}`;
    }
    return headers;
}

async function getCarrinho() {
    const res = await fetch(API_URL, {
        headers: getAuthHeaders()
    });
    if (!res.ok) throw new Error('Erro ao buscar carrinho');
    return await res.json();
}

async function adicionarItem(produtoId: number, quantidade: number) {
    const res = await fetch(`${API_URL}/items`, {
        method: 'POST',
        headers: getAuthHeaders(),
        body: JSON.stringify({ produtoId, quantidade })
    });
    if (!res.ok) throw new Error('Erro ao adicionar item');
    return await res.json();
}

async function removerItem(produtoId: number) {
    const res = await fetch(`${API_URL}/items/${produtoId}`, {
        method: 'DELETE',
        headers: getAuthHeaders()
    });
    if (!res.ok) throw new Error('Erro ao remover item');
    return await res.json();
}

async function atualizarQuantidade(produtoId: number, quantidade: number) {
    const res = await fetch(`${API_URL}/items/${produtoId}`, {
        method: 'PUT',
        headers: getAuthHeaders(),
        body: JSON.stringify({ quantidade })
    });
    if (!res.ok) throw new Error('Erro ao atualizar quantidade');
    return await res.json();
}

export default { getCarrinho, adicionarItem, removerItem, atualizarQuantidade };