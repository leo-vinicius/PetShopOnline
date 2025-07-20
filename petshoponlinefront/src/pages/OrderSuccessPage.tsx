import HomeMenu from "../components/menu/Menu";
import { useNavigate } from "react-router-dom";

export default function OrderSuccessPage() {
    const navigate = useNavigate();
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
                    maxWidth: 500,
                    width: '100%',
                    textAlign: 'center'
                }}>
                    <h2 style={{ color: '#267d4a', fontWeight: 800, fontSize: '2rem', marginBottom: '1.5rem' }}>Pedido realizado com sucesso! 🎉</h2>
                    <p style={{ color: '#444', fontSize: '1.1rem', marginBottom: 24 }}>Obrigado por comprar conosco.</p>
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
                        onClick={() => navigate('/produtos')}
                    >
                        Voltar para Produtos
                    </button>
                </div>
            </div>
        </>
    );
}