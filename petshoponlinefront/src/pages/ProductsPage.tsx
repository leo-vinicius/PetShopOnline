import { useCart } from '../hooks/useCart';
import HomeMenu from '../components/menu/Menu';

type Product = {
    id: number;
    name: string;
    price: number;
    imageUrl: string;
};

const mockProducts: Product[] = [
    {
        id: 1,
        name: 'Ração Premium para Cães',
        price: 129.90,
        imageUrl: 'https://via.placeholder.com/150',
    },
    {
        id: 2,
        name: 'Brinquedo Mordedor',
        price: 49.90,
        imageUrl: 'https://via.placeholder.com/150',
    },
    {
        id: 3,
        name: 'Areia Higiênica para Gatos',
        price: 39.99,
        imageUrl: 'https://via.placeholder.com/150',
    },
];

export default function ProductsPage() {
    const { addToCart } = useCart();

    const handleAdd = (product: Product) => {
        addToCart({
            id: product.id,
            name: product.name,
            price: product.price,
            quantity: 1,
        });
    };

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
                <h1 style={{ fontSize: '2rem', fontWeight: 700, marginBottom: '1.5rem' }}>Produtos</h1>
                <div style={{ display: 'flex', gap: '1.5rem', flexWrap: 'wrap' }}>
                    {mockProducts.map(product => (
                        <div key={product.id} style={{
                            background: '#fff',
                            borderRadius: '16px',
                            padding: '1rem',
                            width: '280px',
                            boxShadow: '0 4px 12px rgba(0,0,0,0.08)',
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'center'
                        }}>
                            <img src={product.imageUrl} alt={product.name} style={{ borderRadius: '8px', width: '100%', marginBottom: '1rem' }} />
                            <h2 style={{ fontSize: '1.2rem', fontWeight: 600, marginBottom: '0.5rem' }}>{product.name}</h2>
                            <p style={{ fontWeight: 500, color: '#333', marginBottom: '1rem' }}>R$ {product.price.toFixed(2)}</p>
                            <button
                                onClick={() => handleAdd(product)}
                                style={{
                                    background: '#2196f3',
                                    color: '#fff',
                                    border: 'none',
                                    borderRadius: '8px',
                                    padding: '0.6rem 1.5rem',
                                    fontWeight: 600,
                                    cursor: 'pointer',
                                    transition: 'background 0.2s'
                                }}
                                onMouseOver={e => e.currentTarget.style.background = '#1976d2'}
                                onMouseOut={e => e.currentTarget.style.background = '#2196f3'}
                            >
                                Adicionar ao Carrinho
                            </button>
                        </div>
                    ))}
                </div>
            </main>
        </div>
    );
}
