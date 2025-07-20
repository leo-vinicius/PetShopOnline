import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import HomeMenu from "../components/menu/Menu";
import cartService from "../services/CartService";

export default function CartPage() {
    const [carrinho, setCarrinho] = useState<any>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [msg, setMsg] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        carregarCarrinho();
        // eslint-disable-next-line
    }, []);

    async function carregarCarrinho() {
        setLoading(true);
        setError('');
        try {
            const res = await cartService.getCarrinho();
            // O backend pode retornar { data: { ...carrinho } }
            setCarrinho(res.data ?? res);
        } catch {
            setError('Erro ao carregar carrinho');
        }
        setLoading(false);
    }

    async function handleRemover(produtoId: number) {
        setMsg('');
        try {
            await cartService.removerItem(produtoId);
            setMsg('Item removido do carrinho!');
            await carregarCarrinho();
        } catch {
            setMsg('Erro ao remover item');
        }
    }

    if (loading) return <div style={{ padding: 40 }}>Carregando...</div>;
    if (error) return <div style={{ color: '#d32f2f', padding: 40 }}>{error}</div>;

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
                    <h2 style={{ color: '#2196f3', fontWeight: 800, fontSize: '2rem', marginBottom: '1.5rem' }}>🛒 Carrinho</h2>
                    {msg && <div style={{ color: '#267d4a', marginBottom: 12 }}>{msg}</div>}
                    {!carrinho?.items?.length && !carrinho?.Itens?.length ? (
                        <div style={{ color: '#888', fontSize: '1.2rem' }}>Seu carrinho está vazio.</div>
                    ) : (
                        <>
                            {(carrinho.items ?? carrinho.Itens).map((item: any) => (
                                <div key={item.produtoId ?? item.ProdutoId} style={{
                                    display: 'flex', alignItems: 'center', gap: 18, marginBottom: 18, borderBottom: '1px solid #e3eafc', paddingBottom: 12
                                }}>
                                    <img src={item.imagemUrl ?? item.ImagemUrl} alt={item.produtoNome ?? item.ProdutoNome} style={{ width: 70, height: 70, borderRadius: 8, objectFit: 'cover', background: '#f1f1f1' }} />
                                    <div style={{ flex: 1 }}>
                                        <div style={{ fontWeight: 700, color: '#2196f3', fontSize: '1.1rem' }}>{item.produtoNome ?? item.ProdutoNome}</div>
                                        <div style={{ color: '#444', fontSize: '1rem' }}>Qtd: {item.quantidade ?? item.Quantidade}</div>
                                        <div style={{ color: '#888', fontSize: '0.95rem' }}>Subtotal: R$ {(item.subtotal ?? item.Subtotal ?? ((item.precoUnitario ?? item.PrecoUnitario) * (item.quantidade ?? item.Quantidade))).toFixed(2)}</div>
                                    </div>
                                    <button
                                        style={{
                                            background: '#fdeaea',
                                            color: '#d32f2f',
                                            border: 'none',
                                            borderRadius: 8,
                                            padding: '0.4rem 1rem',
                                            fontWeight: 600,
                                            cursor: 'pointer'
                                        }}
                                        onClick={() => handleRemover(item.produtoId ?? item.ProdutoId)}
                                    >
                                        Remover
                                    </button>
                                </div>
                            ))}
                            <div style={{ textAlign: 'right', marginTop: 24, fontWeight: 700, fontSize: '1.2rem', color: '#267d4a' }}>
                                Total: R$ {(carrinho.total ?? carrinho.ValorTotal ?? carrinho.valorTotal ?? 0).toFixed(2)}
                            </div>
                            <div style={{ textAlign: 'right', marginTop: 24 }}>
                                <button
                                    style={{
                                        background: '#2196f3',
                                        color: '#fff',
                                        border: 'none',
                                        borderRadius: 8,
                                        padding: '0.7rem 2.2rem',
                                        fontWeight: 700,
                                        fontSize: '1.1rem',
                                        cursor: 'pointer'
                                    }}
                                    onClick={() => navigate('/order')}
                                >
                                    Finalizar Pedido
                                </button>
                            </div>
                        </>
                    )}
                </div>
            </div>
        </>
    );
}