import { useEffect, useState } from "react";
import HomeMenu from "../components/menu/Menu";
import cartService from "../services/CartService";
import orderService from "../services/OrderService";

export default function CheckoutPage() {
    const [enderecos, setEnderecos] = useState<any[]>([]);
    const [enderecoId, setEnderecoId] = useState<number | null>(null);
    const [carrinho, setCarrinho] = useState<any>(null);
    const [loading, setLoading] = useState(true);
    const [msg, setMsg] = useState('');
    const [pagando, setPagando] = useState(false);

    useEffect(() => {
        carregarCarrinho();
        carregarEnderecos();
    }, []);

    async function carregarCarrinho() {
        try {
            const res = await cartService.getCarrinho();
            setCarrinho(res.data ?? res);
        } catch {
            setMsg('Erro ao carregar carrinho');
        }
        setLoading(false);
    }

    async function carregarEnderecos() {
        try {
            const auth = JSON.parse(localStorage.getItem('auth') || '{}');
            const token = auth.token;
            const res = await fetch('https://localhost:7000/api/clientes/enderecos', {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });
            if (res.ok) {
                const data = await res.json();
                setEnderecos(data.data ?? data);
            }
        } catch {
            setMsg('Erro ao carregar endereços');
        }
    }

    async function handleFinalizarPedido(e: React.FormEvent) {
        e.preventDefault();
        setMsg('');
        if (!enderecoId) {
            setMsg('Selecione um endereço de entrega.');
            return;
        }
        setPagando(true);
        setMsg('Processando pagamento...');
        // Simula pagamento
        setTimeout(async () => {
            try {
                await orderService.confirmarPedido(enderecoId);
                setMsg('Pedido realizado com sucesso! 🎉');
                setCarrinho(null);
            } catch {
                setMsg('Erro ao finalizar pedido');
            }
            setPagando(false);
        }, 2000);
    }

    if (loading) return <div style={{ padding: 40 }}>Carregando...</div>;

    return (
        <>
            <header style={{
                width: '100vw',
                background: '#00a2ffff',
                boxShadow: '0 2px 8px rgba(46,139,87,0.12)',
                padding: '0.5rem 0',
                position: 'sticky',
                top: 0,
                zIndex: 10
            }}>
                <HomeMenu horizontal />
            </header>
            <div style={{
                minHeight: '100vh',
                background: '#f7f7f7',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                fontFamily: 'Segoe UI, Arial, sans-serif'
            }}>
                <div style={{
                    background: '#fff',
                    borderRadius: '20px',
                    boxShadow: '0 8px 32px rgba(33,150,243,0.13)',
                    padding: '2.5rem 3.5rem',
                    maxWidth: 700,
                    width: '100%'
                }}>
                    <h2 style={{ color: '#2196f3', fontWeight: 800, fontSize: '2rem', marginBottom: '1.5rem' }}>Finalizar Pedido</h2>
                    {msg && <div style={{ color: pagando ? '#2196f3' : '#267d4a', marginBottom: 12 }}>{msg}</div>}
                    {!carrinho?.items?.length && !carrinho?.Itens?.length ? (
                        <div style={{ color: '#888', fontSize: '1.2rem' }}>Seu carrinho está vazio.</div>
                    ) : (
                        <form onSubmit={handleFinalizarPedido}>
                            <div style={{ marginBottom: 18 }}>
                                <label style={{ fontWeight: 600, color: '#444' }}>Endereço de entrega:</label>
                                <select
                                    style={{ marginLeft: 12, padding: 6, borderRadius: 6, border: '1px solid #e3eafc' }}
                                    value={enderecoId ?? ''}
                                    onChange={e => setEnderecoId(Number(e.target.value))}
                                    disabled={pagando}
                                >
                                    <option value="">Selecione...</option>
                                    {enderecos.map((e: any) => (
                                        <option key={e.idEndereco ?? e._id_endereco} value={e.idEndereco ?? e._id_endereco}>
                                            {e.logradouro}, {e.numero} - {e.cidade}/{e.estado}
                                        </option>
                                    ))}
                                </select>
                            </div>
                            <div style={{ marginBottom: 18 }}>
                                <div style={{ fontWeight: 600, color: '#444', marginBottom: 6 }}>Resumo do Carrinho:</div>
                                {(carrinho.items ?? carrinho.Itens).map((item: any) => (
                                    <div key={item.produtoId ?? item.ProdutoId} style={{ display: 'flex', alignItems: 'center', gap: 12, marginBottom: 8 }}>
                                        <span style={{ color: '#2196f3', fontWeight: 700 }}>{item.produtoNome ?? item.ProdutoNome}</span>
                                        <span style={{ color: '#444' }}>Qtd: {item.quantidade ?? item.Quantidade}</span>
                                        <span style={{ color: '#888' }}>Subtotal: R$ {(item.subtotal ?? item.Subtotal ?? ((item.precoUnitario ?? item.PrecoUnitario) * (item.quantidade ?? item.Quantidade))).toFixed(2)}</span>
                                    </div>
                                ))}
                                <div style={{ textAlign: 'right', fontWeight: 700, fontSize: '1.1rem', color: '#267d4a', marginTop: 10 }}>
                                    Total: R$ {(carrinho.total ?? carrinho.ValorTotal ?? carrinho.valorTotal ?? 0).toFixed(2)}
                                </div>
                            </div>
                            <button
                                type="submit"
                                disabled={pagando}
                                style={{
                                    background: '#2196f3',
                                    color: '#fff',
                                    border: 'none',
                                    borderRadius: 8,
                                    padding: '0.7rem 2.2rem',
                                    fontWeight: 700,
                                    fontSize: '1.1rem',
                                    cursor: pagando ? 'not-allowed' : 'pointer',
                                    marginTop: 10
                                }}
                            >
                                {pagando ? 'Processando...' : 'Finalizar Pedido'}
                            </button>
                        </form>
                    )}
                </div>
            </div>
        </>
    );
}