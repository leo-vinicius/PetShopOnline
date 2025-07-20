const API_URL = 'https://localhost:7000/api/produtos';

function getAuthHeaders() {
    const auth = JSON.parse(localStorage.getItem('auth') || '{}');
    const token = auth.token;
    const headers: Record<string, string> = { 'Content-Type': 'application/json' };
    if (token) {
        headers['Authorization'] = `Bearer ${token}`;
    }
    return headers;
}

async function getProdutos() {
    const res = await fetch(API_URL, {
        headers: getAuthHeaders()
    });
    if (!res.ok) throw new Error('Erro ao buscar produtos');
    return await res.json();
}

async function atualizarEstoque(id: number, novoEstoque: number) {
    const res = await fetch(`${API_URL}/${id}/estoque`, {
        method: 'PATCH',
        headers: getAuthHeaders(),
        body: JSON.stringify(novoEstoque)
    });
    if (!res.ok) throw new Error('Erro ao atualizar estoque');
}

async function adicionarProduto(produto: any) {
    const res = await fetch(API_URL, {
        method: 'POST',
        headers: getAuthHeaders(),
        body: JSON.stringify(produto)
    });
    if (!res.ok) throw new Error('Erro ao adicionar produto');
    return await res.json();
}

async function editarProduto(id: number, produto: any) {
    const res = await fetch(`${API_URL}/${id}`, {
        method: 'PUT',
        headers: getAuthHeaders(),
        body: JSON.stringify(produto)
    });
    if (!res.ok) throw new Error('Erro ao editar produto');
}

async function removerProduto(id: number) {
    const res = await fetch(`${API_URL}/${id}`, {
        method: 'DELETE',
        headers: getAuthHeaders()
    });
    if (!res.ok) throw new Error('Erro ao remover produto');
}

export default { getProdutos, atualizarEstoque, adicionarProduto, editarProduto, removerProduto };