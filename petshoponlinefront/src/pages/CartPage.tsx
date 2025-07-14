import { useCart } from '../hooks/useCart';
import HomeMenu from '../components/menu/Menu';

export default function CartPage() {
    const { cartItems, removeFromCart } = useCart();

    const totalPrice = cartItems.reduce((acc, item) => acc + item.price * item.quantity, 0);

    return (
        <div style={{ fontFamily: 'Segoe UI, Arial, sans-serif', background: '#f7f7f7', minHeight: '100vh' }}>
            <header style={{
                width: '100%',
                background: '#00a2ffff',
                boxShadow: '0 2px 8px rgba(46,139,87,0.12)',
                padding: '0.5rem 0',
                position: 'sticky',
                top: 0,
                zIndex: 10
            }}>
                <HomeMenu horizontal />
            </header>

            <main style={{ padding: '2rem', maxWidth: '1080px', margin: '0 auto' }}>
                <h1 style={{ fontSize: '2rem', fontWeight: 700, marginBottom: '2rem' }}>🛒 Carrinho de Compras</h1>

                {cartItems.length === 0 ? (
                    <p style={{ fontSize: '1.2rem' }}>Seu carrinho está vazio.</p>
                ) : (
                    <div style={{ display: 'flex', flexDirection: 'column', gap: '1.5rem' }}>
                        {cartItems.map(item => (
                            <div key={item.id} style={{
                                background: '#fff',
                                borderRadius: '12px',
                                padding: '1rem 1.5rem',
                                boxShadow: '0 4px 12px rgba(0,0,0,0.08)',
                                display: 'flex',
                                justifyContent: 'space-between',
                                alignItems: 'center'
                            }}>
                                <div>
                                    <h2 style={{ fontSize: '1.2rem', fontWeight: 600 }}>{item.name}</h2>
                                    <p style={{ margin: '0.3rem 0' }}>Quantidade: {item.quantity}</p>
                                    <p style={{ margin: 0, color: '#555' }}>Preço unitário: R$ {item.price.toFixed(2)}</p>
                                    <p style={{ margin: 0, color: '#000', fontWeight: 600 }}>Total: R$ {(item.price * item.quantity).toFixed(2)}</p>
                                </div>
                                <button
                                    onClick={() => removeFromCart(item.id)}
                                    style={{
                                        background: '#e53935',
                                        color: '#fff',
                                        border: 'none',
                                        borderRadius: '8px',
                                        padding: '0.6rem 1.2rem',
                                        fontWeight: 600,
                                        cursor: 'pointer',
                                        transition: 'background 0.2s'
                                    }}
                                    onMouseOver={e => e.currentTarget.style.background = '#c62828'}
                                    onMouseOut={e => e.currentTarget.style.background = '#e53935'}
                                >
                                    Remover
                                </button>
                            </div>
                        ))}

                        <div style={{
                            background: '#fff',
                            borderRadius: '12px',
                            padding: '1.5rem',
                            boxShadow: '0 4px 12px rgba(0,0,0,0.08)',
                            fontSize: '1.3rem',
                            fontWeight: 700,
                            textAlign: 'right'
                        }}>
                            Subtotal: R$ {totalPrice.toFixed(2)}
                        </div>
                    </div>
                )}
            </main>
        </div>
    );
}
