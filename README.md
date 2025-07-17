# PetShop Pro API - Guia de Teste

## Como executar a aplicação

1. Navegue até o diretório do projeto:
```bash
cd "c:\SourceCode\PetShopPro\PetShopPro.Api"
```

2. Execute a aplicação:
```bash
dotnet run
```

A API estará disponível em:
- HTTPS: https://localhost:7000
- HTTP: http://localhost:5000
- Swagger UI: https://localhost:7000/swagger

## Testando a API

### 1. Criar um Administrador
```http
POST /api/administradores
Content-Type: application/json

{
  "nome": "Admin Master",
  "email": "admin@petshop.com",
  "telefone": "11999999999",
  "senha": "admin123"
}
```

### 2. Login do Administrador
```http
POST /api/auth/administrador/login
Content-Type: application/json

{
  "email": "admin@petshop.com",
  "senha": "admin123"
}
```

### 3. Criar Categorias (requer token de admin)
```http
POST /api/categorias
Authorization: Bearer [TOKEN_DO_ADMIN]
Content-Type: application/json

{
  "nome": "Ração para Cães"
}
```

### 4. Criar Produtos (requer token de admin)
```http
POST /api/produtos
Authorization: Bearer [TOKEN_DO_ADMIN]
Content-Type: application/json

{
  "nome": "Ração Premium para Cães Adultos",
  "descricao": "Ração completa e balanceada para cães adultos",
  "preco": 89.90,
  "estoque": 50,
  "imagemUrl": "https://exemplo.com/racao.jpg",
  "categoriaId": 1
}
```

### 5. Cadastrar Cliente
```http
POST /api/clientes
Content-Type: application/json

{
  "nome": "João Silva",
  "email": "joao@email.com",
  "telefone": "11888888888",
  "senha": "cliente123"
}
```

### 6. Login do Cliente
```http
POST /api/auth/cliente/login
Content-Type: application/json

{
  "email": "joao@email.com",
  "senha": "cliente123"
}
```

### 7. Adicionar Endereço (requer token de cliente)
```http
POST /api/clientes/enderecos
Authorization: Bearer [TOKEN_DO_CLIENTE]
Content-Type: application/json

{
  "logradouro": "Rua das Flores, 123",
  "numero": 123,
  "bairro": "Centro",
  "cidade": "São Paulo",
  "estado": "SP",
  "cep": "01234567"
}
```

### 8. Listar Produtos
```http
GET /api/produtos
```

### 9. Adicionar Item ao Carrinho (requer token de cliente)
```http
POST /api/carrinho/items
Authorization: Bearer [TOKEN_DO_CLIENTE]
Content-Type: application/json

{
  "produtoId": 1,
  "quantidade": 2
}
```

### 10. Ver Carrinho (requer token de cliente)
```http
GET /api/carrinho
Authorization: Bearer [TOKEN_DO_CLIENTE]
```

### 11. Review do Pedido (requer token de cliente)
```http
POST /api/pedidos/review
Authorization: Bearer [TOKEN_DO_CLIENTE]
Content-Type: application/json

{
  "enderecoId": 1
}
```

### 12. Confirmar Pedido (requer token de cliente)
```http
POST /api/pedidos/confirmar
Authorization: Bearer [TOKEN_DO_CLIENTE]
Content-Type: application/json

{
  "enderecoId": 1
}
```

### 13. Listar Meus Pedidos (requer token de cliente)
```http
GET /api/pedidos
Authorization: Bearer [TOKEN_DO_CLIENTE]
```

## Estrutura do Banco de Dados

### SQL Server (5CG5123HJ2\SQLEXPRESS)
- **Categorias**: ID, Nome, Ativo
- **Produtos**: ID, Nome, Descrição, Preço, Estoque, Ativo, ImagemUrl, CategoriaID
- **Clientes**: ID, Nome, Email, Telefone, Senha (hash), Ativo
- **Endereços**: ID, Logradouro, Número, Bairro, Cidade, Estado, CEP, ClienteID
- **Administradores**: ID, Nome, Email, Telefone, Senha (hash), Ativo

### MongoDB (localhost:27017)
- **Pedidos**: ID, ClienteID, Nome, EnderecoID, EnderecoEntrega, Total, DataPedido, Status, Items[]

## Características da API

✅ **Arquitetura Vertical Slice** - Cada feature é independente
✅ **Autenticação JWT** - Separada para clientes e administradores
✅ **Autorização baseada em roles** - Admin/Cliente
✅ **Validação com FluentValidation**
✅ **Senhas com hash BCrypt**
✅ **Soft delete** - Entidades são desativadas, não deletadas
✅ **Documentação Swagger** - Interface web para testes
✅ **CORS habilitado** - Para integração com frontend
✅ **Carrinho de compras em memória** - Simula sessão de usuário
✅ **Pagamento simulado** - Aprovação automática
✅ **Controle de estoque** - Redução automática após pedido
✅ **Dual database** - SQL Server + MongoDB

## Fluxo Completo de Compra

1. **Cliente se cadastra** (`POST /api/clientes`)
2. **Cliente faz login** (`POST /api/auth/cliente/login`)
3. **Cliente adiciona endereço** (`POST /api/clientes/enderecos`)
4. **Cliente visualiza produtos** (`GET /api/produtos`)
5. **Cliente adiciona itens ao carrinho** (`POST /api/carrinho/items`)
6. **Cliente visualiza carrinho** (`GET /api/carrinho`)
7. **Cliente faz review do pedido** (`POST /api/pedidos/review`)
8. **Cliente confirma pedido** (`POST /api/pedidos/confirmar`)
9. **Sistema processa pagamento** (simulado - aprovação automática)
10. **Sistema reduz estoque e salva pedido no MongoDB**
11. **Cliente pode visualizar seus pedidos** (`GET /api/pedidos`)

A API está totalmente funcional e pronta para uso!
