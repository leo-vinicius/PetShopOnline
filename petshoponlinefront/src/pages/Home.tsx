
export default function Home() {
    return (
        <div style={{ padding: '2rem' }}>
            <h1>PetShop Online</h1>
            <nav>
                <ul>
                    <li><a href="/produtos">Catálogo de Produtos</a></li>
                    <li><a href="/login">Login/Cadastro</a></li>
                    <li><a href="/carrinho">Carrinho de Compras</a></li>
                    <li><a href="/pedidos">Meus Pedidos</a></li>
                    <li><a href="/favoritos">Favoritos</a></li>
                    <li><a href="/admin">Área Administrativa</a></li>
                </ul>
            </nav>
        </div>
    );
}