import { useEffect, useState } from "react";
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
        <div>
            <h2>Produtos</h2>
            <div style={{ display: "flex", flexWrap: "wrap", gap: 16 }}>
                {produtos.map(produto => (
                    <div key={produto.idProduto} style={{ border: "1px solid #ccc", padding: 16 }}>
                        <h3>{produto.nome}</h3>
                        <img src={produto.imagemUrl} alt={produto.nome} style={{ width: 120, height: 120, objectFit: "cover" }} />
                        <p>{produto.descricao}</p>
                        <p><b>Preço:</b> R$ {produto.preco}</p>
                        <a href={`/produtos/${produto.idProduto}`}>Ver detalhes</a>
                    </div>
                ))}
            </div>
        </div>
    );
}