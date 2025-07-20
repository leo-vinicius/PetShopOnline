import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import HomeMenu from "../components/menu/Menu";
import { listarProdutos } from "../services/ProductService";

export default function ProductsPage() {
    const [produtos, setProdutos] = useState<any[]>([]);
    const [erro, setErro] = useState<string | null>(null);

    useEffect(() => {
        listarProdutos()
            .then(res => {
                if (Array.isArray(res)) setProdutos(res);
                else if (Array.isArray(res.data)) setProdutos(res.data);
                else setProdutos([]);
            })
            .catch(() => setErro("Erro ao carregar produtos"));
    }, []);

    if (erro) return <div>{erro}</div>;

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
                padding: '2rem 0',
                fontFamily: 'Segoe UI, Arial, sans-serif'
            }}>
                <h2 style={{
                    color: '#2196f3',
                    fontWeight: 800,
                    fontSize: '2.2rem',
                    marginBottom: '2rem',
                    textAlign: 'center',
                    letterSpacing: '1px'
                }}>
                    🛍️ Produtos
                </h2>
                <div style={{
                    display: "flex",
                    flexWrap: "wrap",
                    gap: 40,
                    justifyContent: "center"
                }}>
                    {produtos.map(produto => (
                        <div key={produto.id}
                            style={{
                                background: '#fff',
                                borderRadius: '20px',
                                boxShadow: '0 8px 32px rgba(33,150,243,0.13)',
                                padding: '2rem 2rem 1.5rem 2rem',
                                width: 320,
                                display: 'flex',
                                flexDirection: 'column',
                                alignItems: 'center',
                                transition: 'box-shadow 0.2s, transform 0.2s',
                                border: 'none',
                                position: 'relative'
                            }}
                            onMouseOver={e => {
                                (e.currentTarget as HTMLDivElement).style.boxShadow = '0 12px 32px rgba(33,150,243,0.18)';
                                (e.currentTarget as HTMLDivElement).style.transform = 'translateY(-6px) scale(1.03)';
                            }}
                            onMouseOut={e => {
                                (e.currentTarget as HTMLDivElement).style.boxShadow = '0 8px 32px rgba(33,150,243,0.13)';
                                (e.currentTarget as HTMLDivElement).style.transform = 'none';
                            }}
                        >
                            <img
                                src={produto.imagemUrl}
                                alt={produto.nome}
                                style={{
                                    width: 180,
                                    height: 180,
                                    objectFit: "cover",
                                    borderRadius: '14px',
                                    marginBottom: '1.2rem',
                                    background: '#f1f1f1',
                                    border: '2px solid #e3eafc',
                                    boxShadow: '0 2px 12px rgba(33,150,243,0.08)'
                                }}
                            />
                            <h3 style={{
                                fontSize: '1.25rem',
                                fontWeight: 800,
                                color: '#2196f3',
                                margin: '0 0 0.5rem 0',
                                textAlign: 'center',
                                letterSpacing: '0.5px'
                            }}>{produto.nome}</h3>
                            <p style={{
                                color: '#444',
                                fontSize: '1.05rem',
                                margin: '0 0 0.7rem 0',
                                textAlign: 'center',
                                minHeight: 48,
                                lineHeight: 1.5
                            }}>{produto.descricao}</p>
                            <div style={{
                                display: 'flex',
                                alignItems: 'center',
                                gap: 12,
                                marginBottom: '1.1rem'
                            }}>
                                <span style={{
                                    color: '#2196f3',
                                    fontWeight: 900,
                                    fontSize: '1.35rem',
                                    letterSpacing: '1px'
                                }}>
                                    R$ {produto.preco.toFixed(2)}
                                </span>
                                {produto.estoque > 0 ? (
                                    <span style={{
                                        color: '#267d4a',
                                        background: '#eafaf1',
                                        borderRadius: 8,
                                        padding: '0.18rem 0.7rem',
                                        fontWeight: 600,
                                        fontSize: '0.98rem'
                                    }}>
                                        Em estoque
                                    </span>
                                ) : (
                                    <span style={{
                                        color: '#d32f2f',
                                        background: '#fdeaea',
                                        borderRadius: 8,
                                        padding: '0.18rem 0.7rem',
                                        fontWeight: 600,
                                        fontSize: '0.98rem'
                                    }}>
                                        Indisponível
                                    </span>
                                )}
                            </div>
                            <div style={{
                                color: '#888',
                                fontSize: '0.98rem',
                                marginBottom: '1.2rem'
                            }}>
                                <b>Categoria:</b> {produto.categoriaNome}
                            </div>
                            <Link
                                to={`/produtos/${produto.id}`}
                                state={{ produto }}
                                style={{
                                    display: 'inline-block',
                                    background: 'linear-gradient(90deg, #2196f3 60%, #4ecdc4 100%)',
                                    color: '#fff',
                                    fontWeight: 700,
                                    fontSize: '1.08rem',
                                    borderRadius: '10px',
                                    padding: '0.7rem 1.7rem',
                                    textDecoration: 'none',
                                    boxShadow: '0 2px 12px rgba(33,150,243,0.10)',
                                    letterSpacing: '1px',
                                    transition: 'background 0.2s, transform 0.2s'
                                }}
                                onMouseOver={e => (e.currentTarget.style.background = '#1976d2')}
                                onMouseOut={e => (e.currentTarget.style.background = 'linear-gradient(90deg, #2196f3 60%, #4ecdc4 100%)')}
                            >
                                Ver detalhes
                            </Link>
                        </div>
                    ))}
                </div>
            </div>
        </>
    );
}