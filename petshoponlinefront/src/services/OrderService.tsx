const API_URL = 'https://localhost:7000/api/pedidos';

function getAuthHeaders() {
    const auth = JSON.parse(localStorage.getItem('auth') || '{}');
    const token = auth.token;
    const headers: Record<string, string> = { 'Content-Type': 'application/json' };
    if (token) {
        headers['Authorization'] = `Bearer ${token}`;
    }
    return headers;
}

async function confirmarPedido(enderecoId: number) {
    const res = await fetch(`${API_URL}/confirmar`, {
        method: 'POST',
        headers: getAuthHeaders(),
        body: JSON.stringify({ enderecoId })
    });
    if (!res.ok) throw new Error('Erro ao confirmar pedido');
    return await res.json();
}

export default { confirmarPedido };