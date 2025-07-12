
export default function HomeContent() {
    return (
        <>
            <h1 style={{
                color: '#2196f3',
                fontWeight: 800,
                fontSize: '2.7rem',
                marginBottom: '0.7rem',
                letterSpacing: '1px'
            }}>
                🐾 PetShop Online
            </h1>
            <h2 style={{
                color: '#333',
                fontWeight: 600,
                fontSize: '1.35rem',
                marginBottom: '1.2rem'
            }}>
                Novidades e soluções para você e seu pet!
            </h2>
            <p style={{
                color: '#444',
                fontSize: '1.13rem',
                marginBottom: '2rem',
                lineHeight: 1.7
            }}>
                O PetShop Online é uma plataforma moderna e intuitiva para compra de produtos para animais de estimação. Aqui você encontra rações, brinquedos, acessórios e medicamentos para cães, gatos, aves e outros pets. Nosso sistema foi desenvolvido para facilitar a vida dos tutores, oferecendo cadastro rápido, catálogo com filtros inteligentes, carrinho de compras, histórico de pedidos e área administrativa para gestão de produtos e estoque.
                <br /><br />
                O projeto utiliza arquitetura híbrida com SQL Server e MongoDB, garantindo segurança, performance e flexibilidade.
            </p>
            <a
                href="/produtos"
                style={{
                    display: 'inline-block',
                    background: 'linear-gradient(90deg, #2196f3 60%, #4ecdc4 100%)',
                    color: '#fff',
                    fontWeight: 600,
                    fontSize: '1.15rem',
                    borderRadius: '8px',
                    padding: '0.9rem 2.2rem',
                    textDecoration: 'none',
                    boxShadow: '0 2px 8px rgba(33,150,243,0.10)',
                    letterSpacing: '1px',
                    transition: 'background 0.2s, transform 0.2s'
                }}
                onMouseOver={e => (e.currentTarget.style.background = '#1976d2')}
                onMouseOut={e => (e.currentTarget.style.background = 'linear-gradient(90deg, #2196f3 60%, #4ecdc4 100%)')}
            >
                Conferir Catálogo
            </a>
        </>
    );
}