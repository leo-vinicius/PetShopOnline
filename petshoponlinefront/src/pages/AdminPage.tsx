import { useEffect, useState } from "react";
import adminService from "../services/AdminService";

export default function AdminPage() {
    const [produtos, setProdutos] = useState<any[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [novoEstoque, setNovoEstoque] = useState<{ [id: number]: number }>({});
    const [editId, setEditId] = useState<number | null>(null);

    useEffect(() => {
        adminService.getProdutos()
            .then(res => {
                if (Array.isArray(res)) setProdutos(res);
                else if (Array.isArray(res.data)) setProdutos(res.data);
                else setProdutos([]);
            })
            .catch(() => setError('Erro ao carregar produtos'))
            .finally(() => setLoading(false));
    }, []);

    const handleEstoque = async (id: number) => {
        try {
            await adminService.atualizarEstoque(id, novoEstoque[id]);
            setProdutos(produtos.map(p => p.idProduto === id ? { ...p, estoque: novoEstoque[id] } : p));
            setEditId(null);
        } catch {
            setError('Erro ao atualizar estoque');
        }
    };

    const handleRemover = async (id: number) => {
        try {
            await adminService.removerProduto(id);
            setProdutos(produtos.filter(p => p.idProduto !== id));
        } catch {
            setError('Erro ao remover produto');
        }
    };

    // Adicionar e editar produto: pode ser expandido com formulários
    // Aqui apenas estrutura básica para manipulação

    return (
        <div style={{ minHeight: '100vh', background: '#f7f7f7', width: '100vw' }}>
            <h2>Administração de Produtos</h2>
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
                            <tr key={produto.id}>
                                <td>{produto.nome}</td>
                                <td>
                                    {editId === produto.id ? (
                                        <>
                                            <input
                                                type="number"
                                                value={novoEstoque[produto.id] ?? produto.estoque}
                                                onChange={e => setNovoEstoque({ ...novoEstoque, [produto.id]: Number(e.target.value) })}
                                                style={{ width: 60 }}
                                            />
                                            <button onClick={() => handleEstoque(produto.id)}>Salvar</button>
                                            <button onClick={() => setEditId(null)}>Cancelar</button>
                                        </>
                                    ) : (
                                        <>
                                            {produto.estoque}
                                            <button style={{ marginLeft: 8 }} onClick={() => setEditId(produto.id)}>Editar</button>
                                        </>
                                    )}
                                </td>
                                <td>R$ {produto.preco.toFixed(2)}</td>
                                <td>
                                    <button style={{ color: '#d32f2f' }} onClick={() => handleRemover(produto.id)}>Remover</button>
                                    {/* Adicionar/Editar produto pode ser implementado aqui */}
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
}