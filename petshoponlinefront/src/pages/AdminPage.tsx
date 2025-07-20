import { useEffect, useState } from 'react';
import HomeMenu from "../components/menu/Menu";
import adminService from '../services/AdminService';

export default function AdminPage() {
    const [produtos, setProdutos] = useState<any[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [novoEstoque, setNovoEstoque] = useState<{ [id: number]: number }>({});
    const [editId, setEditId] = useState<number | null>(null);

    // Formulário de novo produto
    const [novoProduto, setNovoProduto] = useState({
        nome: '',
        descricao: '',
        preco: '',
        estoque: '',
        imagemUrl: '',
        categoriaId: ''
    });
    const [msg, setMsg] = useState('');

    useEffect(() => {
        carregarProdutos();
    }, []);

    async function carregarProdutos() {
        setLoading(true);
        setError('');
        try {
            const res = await adminService.getProdutos();
            if (Array.isArray(res)) setProdutos(res);
            else if (Array.isArray(res.data)) setProdutos(res.data);
            else setProdutos([]);
        } catch {
            setError('Erro ao carregar produtos');
        }
        setLoading(false);
    }

    const handleEstoque = async (id: number) => {
        try {
            await adminService.atualizarEstoque(id, novoEstoque[id]);
            setProdutos(produtos.map(p => p.id === id ? { ...p, estoque: novoEstoque[id] } : p));
            setEditId(null);
        } catch {
            setError('Erro ao atualizar estoque');
        }
    };

    const handleRemover = async (id: number) => {
        try {
            await adminService.removerProduto(id);
            setProdutos(produtos.filter(p => p.id !== id));
        } catch {
            setError('Erro ao remover produto');
        }
    };

    const handleNovoProduto = async (e: React.FormEvent) => {
        e.preventDefault();
        setMsg('');
        try {
            const produto = {
                nome: novoProduto.nome,
                descricao: novoProduto.descricao,
                preco: parseFloat(novoProduto.preco),
                estoque: parseInt(novoProduto.estoque),
                imagemUrl: novoProduto.imagemUrl,
                categoriaId: parseInt(novoProduto.categoriaId)
            };
            await adminService.adicionarProduto(produto);
            setMsg('Produto adicionado com sucesso!');
            setNovoProduto({ nome: '', descricao: '', preco: '', estoque: '', imagemUrl: '', categoriaId: '' });
            carregarProdutos();
        } catch {
            setMsg('Erro ao adicionar produto');
        }
    };

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
                width: '100vw',
                padding: '2rem 0'
            }}>
                <div style={{
                    maxWidth: 900,
                    margin: '0 auto',
                    background: '#fff',
                    borderRadius: 16,
                    boxShadow: '0 4px 24px rgba(33,150,243,0.10)',
                    padding: '2.5rem 2.5rem'
                }}>
                    <h2 style={{ color: '#2196f3', fontWeight: 800, fontSize: '2rem', marginBottom: '1.5rem' }}>
                        Administração de Produtos
                    </h2>
                    <form onSubmit={handleNovoProduto} style={{
                        display: 'flex',
                        gap: 16,
                        flexWrap: 'wrap',
                        marginBottom: 32,
                        alignItems: 'flex-end'
                    }}>
                        <input
                            placeholder="Nome"
                            value={novoProduto.nome}
                            onChange={e => setNovoProduto({ ...novoProduto, nome: e.target.value })}
                            required
                            style={{ flex: 2, padding: 8, borderRadius: 6, border: '1px solid #ddd' }}
                        />
                        <input
                            placeholder="Descrição"
                            value={novoProduto.descricao}
                            onChange={e => setNovoProduto({ ...novoProduto, descricao: e.target.value })}
                            required
                            style={{ flex: 3, padding: 8, borderRadius: 6, border: '1px solid #ddd' }}
                        />
                        <input
                            placeholder="Preço"
                            type="number"
                            min="0"
                            step="0.01"
                            value={novoProduto.preco}
                            onChange={e => setNovoProduto({ ...novoProduto, preco: e.target.value })}
                            required
                            style={{ width: 110, padding: 8, borderRadius: 6, border: '1px solid #ddd' }}
                        />
                        <input
                            placeholder="Estoque"
                            type="number"
                            min="0"
                            value={novoProduto.estoque}
                            onChange={e => setNovoProduto({ ...novoProduto, estoque: e.target.value })}
                            required
                            style={{ width: 90, padding: 8, borderRadius: 6, border: '1px solid #ddd' }}
                        />
                        <input
                            placeholder="Categoria ID"
                            type="number"
                            min="1"
                            value={novoProduto.categoriaId}
                            onChange={e => setNovoProduto({ ...novoProduto, categoriaId: e.target.value })}
                            required
                            style={{ width: 110, padding: 8, borderRadius: 6, border: '1px solid #ddd' }}
                        />
                        <input
                            placeholder="Link da Imagem"
                            value={novoProduto.imagemUrl}
                            onChange={e => setNovoProduto({ ...novoProduto, imagemUrl: e.target.value })}
                            style={{ flex: 3, padding: 8, borderRadius: 6, border: '1px solid #ddd' }}
                        />
                        <button type="submit" style={{
                            background: 'linear-gradient(90deg, #2196f3 60%, #4ecdc4 100%)',
                            color: '#fff',
                            border: 'none',
                            borderRadius: 6,
                            padding: '0.7rem 2.2rem',
                            fontWeight: 700,
                            fontSize: '1.1rem',
                            cursor: 'pointer'
                        }}>
                            Adicionar Produto
                        </button>
                    </form>
                    {msg && <div style={{ color: msg.startsWith('Erro') ? '#d32f2f' : '#267d4a', marginBottom: 18 }}>{msg}</div>}
                    {loading ? <div>Carregando...</div> : error ? <div style={{ color: '#d32f2f' }}>{error}</div> : (
                        <div style={{ overflowX: 'auto' }}>
                            <table style={{ width: '100%', borderCollapse: 'collapse', background: '#fff', borderRadius: 10 }}>
                                <thead>
                                    <tr style={{ background: '#e3f2fd' }}>
                                        <th>Imagem</th>
                                        <th>Nome</th>
                                        <th>Descrição</th>
                                        <th>Estoque</th>
                                        <th>Preço</th>
                                        <th>Categoria</th>
                                        <th>Ações</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {produtos.map(produto => (
                                        <tr key={produto.id || produto.idProduto}>
                                            <td>
                                                {produto.imagemUrl &&
                                                    <img src={produto.imagemUrl} alt={produto.nome} style={{ width: 60, height: 60, objectFit: 'cover', borderRadius: 8, background: '#f1f1f1' }} />
                                                }
                                            </td>
                                            <td>{produto.nome}</td>
                                            <td style={{ maxWidth: 200, overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>{produto.descricao}</td>
                                            <td>
                                                {editId === (produto.id || produto.idProduto) ? (
                                                    <>
                                                        <input
                                                            type="number"
                                                            value={novoEstoque[produto.id || produto.idProduto] ?? produto.estoque}
                                                            onChange={e => setNovoEstoque({ ...novoEstoque, [produto.id || produto.idProduto]: Number(e.target.value) })}
                                                            style={{ width: 60 }}
                                                        />
                                                        <button onClick={() => handleEstoque(produto.id || produto.idProduto)} style={{ marginLeft: 6, color: '#2196f3' }}>Salvar</button>
                                                        <button onClick={() => setEditId(null)} style={{ marginLeft: 4 }}>Cancelar</button>
                                                    </>
                                                ) : (
                                                    <>
                                                        {produto.estoque}
                                                        {/* <button style={{ marginLeft: 8, color: '#2196f3' }} onClick={() => setEditId(produto.id || produto.idProduto)}>Editar</button> */}
                                                    </>
                                                )}
                                            </td>
                                            <td>R$ {produto.preco?.toFixed ? produto.preco.toFixed(2) : produto.preco}</td>
                                            <td>{produto.categoriaNome || produto.categoriaId}</td>
                                            <td>
                                                <button style={{ color: '#d32f2f' }} onClick={() => handleRemover(produto.id || produto.idProduto)}>Remover</button>
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    )}
                </div>
            </div>
        </>
    );
}