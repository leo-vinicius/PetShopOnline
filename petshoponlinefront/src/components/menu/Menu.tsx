
const leftMenu = [
    { label: '🐾 PetShop Online', path: '/', isLogo: true },
    { label: 'Catálogo', path: '/produtos', icon: '' }
];

const rightMenu = [
    { label: 'Carrinho', path: '/carrinho', icon: '' },
    { label: 'Meus Pedidos', path: '/pedidos', icon: '' },
    { label: 'Perfil', path: '/login', icon: '' }
];

export default function HomeMenu({ horizontal = true }: { horizontal?: boolean }) {
    return (
        <nav style={{
            width: '100%',
            maxWidth: '1200px',
            margin: '0 auto',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'space-between',
            background: '#00a2ffff',
            padding: '0.5rem 2rem',
            boxShadow: '0 2px 8px rgba(46,139,87,0.12)',
            flexWrap: 'wrap',
            overflow: 'hidden'
        }}>
            <ul style={{
                display: 'flex',
                alignItems: 'center',
                gap: '2rem',
                listStyle: 'none',
                margin: 0,
                padding: 0
            }}>
                {leftMenu.map(item => (
                    <li key={item.path}>
                        <a
                            href={item.path}
                            style={{
                                display: 'flex',
                                alignItems: 'center',
                                color: '#fff',
                                fontWeight: item.isLogo ? 700 : 500,
                                fontSize: item.isLogo ? '1.7rem' : '1.1rem',
                                textDecoration: 'none',
                                letterSpacing: item.isLogo ? '1px' : undefined
                            }}
                        >
                            {item.icon && <span style={{ marginRight: '0.5rem', fontSize: '1.3rem' }}>{item.icon}</span>}
                            {item.label}
                        </a>
                    </li>
                ))}
            </ul>
            <ul style={{
                display: 'flex',
                alignItems: 'center',
                gap: '2rem',
                listStyle: 'none',
                margin: 0,
                padding: 0
            }}>
                {rightMenu.map(item => (
                    <li key={item.path}>
                        <a
                            href={item.path}
                            style={{
                                display: 'flex',
                                alignItems: 'center',
                                color: '#fff',
                                fontWeight: 500,
                                fontSize: '1.1rem',
                                textDecoration: 'none'
                            }}
                        >
                            {item.icon && <span style={{ marginRight: '0.5rem', fontSize: '1.3rem' }}>{item.icon}</span>}
                            {item.label}
                        </a>
                    </li>
                ))}
            </ul>
        </nav>
    );
}