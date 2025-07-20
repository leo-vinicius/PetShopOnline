import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { buscarProdutoPorId } from "../services/ProductService";

export default function ProductDetailPage() {
    const { id } = useParams<{ id: string }>();
    const [produto, setProduto] = useState<any | null>(null);
    const [erro, setErro] = useState<string | null>(null);

    useEffect(() => {
        if (id) {
            buscarProdutoPorId(Number(id))
                .then(setProduto)
                .catch(() => setErro("Produto não encontrado"));
        }
    }, [id]);

    if (erro) return <div>{erro}</div>;
    if (!produto) return <div>Carregando...</div>;

    return (
        <div>
            <h2>{produto.nome}</h2>
            <img src={produto.imagemUrl} alt={produto.nome} style={{ width: 200, height: 200, objectFit: "cover" }} />
            <p>{produto.descricao}</p>
            <p><b>Preço:</b> R$ {produto.preco}</p>
            <p><b>Estoque:</b> {produto.estoque}</p>
            <p><b>Categoria:</b> {produto.categoriaNome}</p>
        </div>
    );
}