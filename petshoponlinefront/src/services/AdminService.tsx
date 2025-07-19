const API_URL = 'https://localhost:7000/api/Produtos';

async function getProdutos(token: string) {
    const res = await fetch(API_URL, {
        headers: { 'Authorization': `Bearer ${token}` }
    });
    if (!res.ok) throw new Error('Erro ao buscar produtos');
    return await res.json();
}

async function atualizarEstoque(id: number, novoEstoque: number, token: string) {
    const res = await fetch(`${API_URL}/${id}/estoque`, {
        method: 'PATCH',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(novoEstoque)
    });
    if (!res.ok) throw new Error('Erro ao atualizar estoque');
}

async function adicionarProduto(produto: any, token: string) {
    const res = await fetch(API_URL, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(produto)
    });
    if (!res.ok) throw new Error('Erro ao adicionar produto');
    return await res.json();
}

async function editarProduto(id: number, produto: any, token: string) {
    const res = await fetch(`${API_URL}/${id}`, {
        method: 'PUT',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(produto)
    });
    if (!res.ok) throw new Error('Erro ao editar produto');
}

async function removerProduto(id: number, token: string) {
    const res = await fetch(`${API_URL}/${id}`, {
        method: 'DELETE',
        headers: { 'Authorization': `Bearer ${token}` }
    });
    if (!res.ok) throw new Error('Erro ao remover produto');
}

export default { getProdutos, atualizarEstoque, adicionarProduto, editarProduto, removerProduto };