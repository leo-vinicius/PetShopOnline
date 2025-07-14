import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import HomeMenu from '../components/menu/Menu';
import { useAuth } from '../context/AuthContext';
import adminService from '../services/AdminService';

export default function AdminPage() {
    const { auth } = useAuth();
    const navigate = useNavigate();
    const [produtos, setProdutos] = useState<any[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [novoEstoque, setNovoEstoque] = useState<{ [id: number]: number }>({});
    const [editId, setEditId] = useState<number | null>(null);

    useEffect(() => {
        if (!auth) {
            setLoading(false);
            setError('Você precisa estar logado.');
            return;
        }
        adminService.getProdutos(auth.token)
            .then(setProdutos)
            .catch(() => setError('Erro ao carregar produtos'))
            .finally(() => setLoading(false));
    }, [auth, navigate]);

    const handleEstoque = async (id: number) => {
        try {
            await adminService.atualizarEstoque(id, novoEstoque[id], auth!.token);
            setProdutos(produtos.map(p => p.idProduto === id ? { ...p, estoque: novoEstoque[id] } : p));
        } catch {
            setError('Erro ao atualizar estoque');
        }
    };

    const handleRemover = async (id: number) => {
        try {
            await adminService.removerProduto(id, auth!.token);
            setProdutos(produtos.filter(p => p.idProduto !== id));
        } catch {
            setError('Erro ao remover produto');
        }
    };

    // Adicionar e editar produto: pode ser expandido com formulários
    // Aqui apenas estrutura básica para manipulação

    return (
        <div style={{ minHeight: '100vh', background: '#f7f7f7', width: '100vw' }}>
            <HomeMenu horizontal />
            <main style={{ maxWidth: 900, margin: '2rem auto', background: '#fff', borderRadius: 12, boxShadow: '0 2px 12px #0001', padding: '2rem' }}>
                <h2 style={{ color: '#2196f3', marginBottom: '2rem' }}>Administração de Produtos</h2>
                {loading ? <div>Carregando...</div> : error ? <div style={{ color: '#d32f2f' }}>{error}</div> : (
                    <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                        <thead>
                            <tr style={{ background: '#e3f2fd' }}>
                                <th>Nome</th>
                                <th>Estoque</th>
                                <th>Preço</th>
                                <th>Ações</th>
                            </tr>
                        </thead>
                        <tbody>
                            {produtos.map(produto => (
                                <tr key={produto.idProduto}>
                                    <td>{produto.nome}</td>
                                    <td>
                                        {editId === produto.idProduto ? (
                                            <>
                                                <input
                                                    type="number"
                                                    value={novoEstoque[produto.idProduto] ?? produto.estoque}
                                                    onChange={e => setNovoEstoque({ ...novoEstoque, [produto.idProduto]: Number(e.target.value) })}
                                                    style={{ width: 60 }}
                                                />
                                                <button onClick={() => handleEstoque(produto.idProduto)}>Salvar</button>
                                                <button onClick={() => setEditId(null)}>Cancelar</button>
                                            </>
                                        ) : (
                                            <>
                                                {produto.estoque}
                                                <button style={{ marginLeft: 8 }} onClick={() => setEditId(produto.idProduto)}>Editar</button>
                                            </>
                                        )}
                                    </td>
                                    <td>R$ {produto.preco.toFixed(2)}</td>
                                    <td>
                                        <button style={{ color: '#d32f2f' }} onClick={() => handleRemover(produto.idProduto)}>Remover</button>
                                        {/* Adicionar/Editar produto pode ser implementado aqui */}
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                )}
            </main>
        </div>
    );
}