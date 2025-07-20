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

async function confirmarPedido(clienteId: number, enderecoEntregaId: number) {
    const res = await fetch(`https://localhost:7000/api/carrinho/${clienteId}/finalizar`, {
        method: 'POST',
        headers: getAuthHeaders(),
        body: JSON.stringify(enderecoEntregaId)
    });
    if (!res.ok) throw new Error('Erro ao confirmar pedido');
    return await res.json();
}

export default { confirmarPedido };