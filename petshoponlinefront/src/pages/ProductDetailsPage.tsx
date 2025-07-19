import { useNavigate, useParams } from 'react-router-dom';
import { useCart } from '../hooks/useCart';

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

export default function ProductDetailPage() {
    const { id } = useParams();
    const { addItem } = useCart();
    const navigate = useNavigate();
    const product = mockProducts.find(p => p.id === Number(id));

    if (!product) return (
        <div style={{ fontFamily: 'Segoe UI, Arial, sans-serif', background: '#f7f7f7', minHeight: '100vh', padding: '2rem' }}>
            <div style={{ maxWidth: 500, margin: '4rem auto', background: '#fff', borderRadius: 16, boxShadow: '0 4px 16px #2196f333', padding: '2rem', textAlign: 'center' }}>
                <h2 style={{ color: '#d32f2f' }}>Produto não encontrado</h2>
                <button
                    style={{
                        background: '#2196f3',
                        color: '#fff',
                        border: 'none',
                        borderRadius: '8px',
                        padding: '0.7rem 1.5rem',
                        fontWeight: 600,
                        cursor: 'pointer',
                        marginTop: '1.5rem'
                    }}
                    onClick={() => navigate('/produtos')}
                >
                    Voltar ao Catálogo
                </button>
            </div>
        </div>
    );

    return (
        <div style={{ fontFamily: 'Segoe UI, Arial, sans-serif', background: '#f7f7f7', minHeight: '100vh', padding: '2rem' }}>
            <div style={{
                maxWidth: 500,
                margin: '4rem auto',
                background: '#fff',
                borderRadius: 16,
                boxShadow: '0 4px 16px #2196f333',
                padding: '2.5rem',
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center'
            }}>
                <img src={product.imageUrl} alt={product.name} style={{ borderRadius: '12px', width: '80%', marginBottom: '2rem', boxShadow: '0 2px 8px #2196f333' }} />
                <h1 style={{ fontSize: '2rem', fontWeight: 700, color: '#1976d2', marginBottom: '1rem', textAlign: 'center' }}>{product.name}</h1>
                <p style={{ fontWeight: 500, color: '#333', fontSize: '1.3rem', marginBottom: '2rem' }}>R$ {product.price.toFixed(2)}</p>
                <button
                    style={{
                        background: 'linear-gradient(90deg, #2196f3 60%, #4ecdc4 100%)',
                        color: '#fff',
                        border: 'none',
                        borderRadius: '8px',
                        padding: '0.9rem 2.2rem',
                        fontWeight: 600,
                        fontSize: '1.15rem',
                        boxShadow: '0 2px 8px rgba(33,150,243,0.10)',
                        letterSpacing: '1px',
                        transition: 'background 0.2s, transform 0.2s',
                        cursor: 'pointer'
                    }}
                    onMouseOver={e => (e.currentTarget.style.background = '#1976d2')}
                    onMouseOut={e => (e.currentTarget.style.background = 'linear-gradient(90deg, #2196f3 60%, #4ecdc4 100%)')}
                    onClick={() => addItem({
                        id: product.id.toString(),
                        name: product.name,
                        price: product.price,
                        quantity: 1,
                        imageUrl: product.imageUrl,
                    })}
                >
                    Adicionar ao Carrinho
                </button>
                <button
                    style={{
                        background: '#e3f2fd',
                        color: '#1976d2',
                        border: 'none',
                        borderRadius: '8px',
                        padding: '0.7rem 1.5rem',
                        fontWeight: 600,
                        cursor: 'pointer',
                        marginTop: '1.5rem'
                    }}
                    onClick={() => navigate('/produtos')}
                >
                    Voltar ao Catálogo
                </button>
            </div>
        </div>
    );
}