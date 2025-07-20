import { useLocation, useParams } from "react-router-dom";
import HomeMenu from "../components/menu/Menu";

export default function ProductDetailPage() {
    const { id } = useParams<{ id: string }>();
    const location = useLocation();
    const produto = location.state?.produto;

    if (!produto) return (
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
                borderRadius: '18px',
                boxShadow: '0 8px 32px rgba(33,150,243,0.12)',
                padding: '2.5rem 3rem',
                textAlign: 'center'
            }}>
                Produto não encontrado.
            </div>
        </div>
    );

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
                    maxWidth: 650,
                    width: '100%',
                    display: 'flex',
                    gap: '2.5rem',
                    alignItems: 'center'
                }}>
                    <div style={{
                        flex: '0 0 260px',
                        display: 'flex',
                        alignItems: 'center',
                        justifyContent: 'center'
                    }}>
                        <img
                            src={produto.imagemUrl}
                            alt={produto.nome}
                            style={{
                                width: 220,
                                height: 220,
                                objectFit: "cover",
                                borderRadius: '16px',
                                boxShadow: '0 4px 24px rgba(33,150,243,0.10)',
                                border: '2px solid #e3eafc',
                                background: '#f1f1f1'
                            }}
                        />
                    </div>
                    <div style={{ flex: 1 }}>
                        <h2 style={{
                            color: '#2196f3',
                            fontWeight: 800,
                            fontSize: '2.3rem',
                            marginBottom: '0.6rem',
                            letterSpacing: '1px'
                        }}>{produto.nome}</h2>
                        <p style={{
                            color: '#444',
                            fontSize: '1.15rem',
                            marginBottom: '1.2rem',
                            lineHeight: 1.7
                        }}>{produto.descricao}</p>
                        <div style={{
                            display: 'flex',
                            alignItems: 'center',
                            gap: 18,
                            marginBottom: '1.2rem'
                        }}>
                            <span style={{
                                color: '#2196f3',
                                fontWeight: 900,
                                fontSize: '2rem',
                                letterSpacing: '1px'
                            }}>
                                R$ {produto.preco.toFixed(2)}
                            </span>
                            {produto.estoque > 0 ? (
                                <span style={{
                                    color: '#267d4a',
                                    background: '#eafaf1',
                                    borderRadius: 8,
                                    padding: '0.2rem 0.8rem',
                                    fontWeight: 600,
                                    fontSize: '1rem'
                                }}>
                                    Em estoque
                                </span>
                            ) : (
                                <span style={{
                                    color: '#d32f2f',
                                    background: '#fdeaea',
                                    borderRadius: 8,
                                    padding: '0.2rem 0.8rem',
                                    fontWeight: 600,
                                    fontSize: '1rem'
                                }}>
                                    Indisponível
                                </span>
                            )}
                        </div>
                        <div style={{
                            color: '#888',
                            fontSize: '1.02rem',
                            marginBottom: '1.5rem'
                        }}>
                            <b>Categoria:</b> {produto.categoriaNome}
                        </div>
                        <button
                            style={{
                                background: 'linear-gradient(90deg, #2196f3 60%, #4ecdc4 100%)',
                                color: '#fff',
                                fontWeight: 700,
                                fontSize: '1.1rem',
                                borderRadius: '10px',
                                padding: '0.85rem 2.2rem',
                                border: 'none',
                                boxShadow: '0 2px 12px rgba(33,150,243,0.10)',
                                letterSpacing: '1px',
                                cursor: produto.estoque > 0 ? 'pointer' : 'not-allowed',
                                opacity: produto.estoque > 0 ? 1 : 0.6,
                                transition: 'background 0.2s, transform 0.2s'
                            }}
                            disabled={produto.estoque <= 0}
                            onMouseOver={e => {
                                if (produto.estoque > 0) e.currentTarget.style.background = '#1976d2';
                            }}
                            onMouseOut={e => {
                                if (produto.estoque > 0) e.currentTarget.style.background = 'linear-gradient(90deg, #2196f3 60%, #4ecdc4 100%)';
                            }}
                        >
                            Adicionar ao Carrinho
                        </button>
                    </div>
                </div>
            </div>
        </>
    );
}