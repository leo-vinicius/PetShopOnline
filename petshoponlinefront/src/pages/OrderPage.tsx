import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import HomeMenu from "../components/menu/Menu";
import addressService from "../services/AddressService";
import cartService from "../services/CartService";
import orderService from "../services/OrderService";

export default function CheckoutPage() {
    const [enderecos, setEnderecos] = useState<any[]>([]);
    const [enderecoId, setEnderecoId] = useState<number | null>(null);
    const [carrinho, setCarrinho] = useState<any>(null);
    const [loading, setLoading] = useState(true);
    const [msg, setMsg] = useState('');
    const [pagando, setPagando] = useState(false);
    const navigate = useNavigate();

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
            const res = await addressService.listarEnderecos();
            setEnderecos(res.data ?? res);
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
        const auth = JSON.parse(localStorage.getItem('auth') || '{}');
        const clienteId = auth.userId;
        if (!clienteId) {
            setMsg('Usuário não autenticado.');
            return;
        }
        const itens = (carrinho.items ?? carrinho.Itens).map((item: any) => ({
            produtoId: item.produtoId ?? item.ProdutoId,
            quantidade: item.quantidade ?? item.Quantidade
        }));

        setPagando(true);
        setMsg('Processando pagamento...');
        setTimeout(async () => {
            try {
                await orderService.confirmarPedido(clienteId, enderecoId);
                navigate('/order/success');
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
                                    onChange={e => {
                                        const val = e.target.value;
                                        setEnderecoId(val ? Number(val) : null);
                                    }}
                                    disabled={pagando}
                                >
                                    <option value="">Selecione...</option>
                                    {enderecos.map((e: any) => (
                                        <option
                                            key={e.id}
                                            value={e.id}
                                        >
                                            {(e.logradouro ?? e.Logradouro)}, {(e.numero ?? e.Numero)} - {(e.cidade ?? e.Cidade)}/{(e.estado ?? e.Estado)}
                                        </option>
                                    ))}
                                </select>
                                <button
                                    type="button"
                                    style={{
                                        marginLeft: 18,
                                        background: '#e3eafc',
                                        color: '#2196f3',
                                        border: 'none',
                                        borderRadius: 8,
                                        padding: '0.4rem 1rem',
                                        fontWeight: 600,
                                        cursor: 'pointer'
                                    }}
                                    onClick={() => navigate('/add-address')}
                                    disabled={pagando}
                                >
                                    + Novo Endereço
                                </button>
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