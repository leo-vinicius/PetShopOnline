# PetShop Hybrid API (.NET 9)

Uma Web API completa em **C# (.NET 9)** para gerenciamento de uma loja de produtos para animais de estimação usando **arquitetura híbrida de bancos de dados** com **documentação Swagger avançada**.

## 🚀 **Novidades .NET 9**

### ⚡ Performance e Recursos
- **Runtime otimizado** para melhor performance
- **Compilação AOT** (Ahead-of-Time) support
- **Memory usage** reduzido
- **Startup time** mais rápido
- **GC improvements** para menor latência

### 📚 **Swagger Documentation Premium**
- **UI customizada** com tema PetShop
- **Autenticação integrada** com Bearer tokens
- **Exemplos automáticos** para requests
- **Performance monitoring** em tempo real
- **Keyboard shortcuts** para navegação
- **Responsive design** para mobile/tablet

## 🏗️ Arquitetura Híbrida

### 📊 SQL Server (5CG5123HJ2\SQLEXPRESS)
**Dados Principais do Negócio:**
- **Produtos** - Catálogo de produtos
- **Categorias** - Categorias de produtos  
- **Pedidos** - Pedidos e itens
- **Endereços** - Endereços de entrega
- **Clientes** - Dados básicos dos clientes (apenas referência)

### 🍃 MongoDB (localhost:27017)
**Autenticação e Administração:**
- **Administradores** - Usuários administrativos
- **Clientes** - Dados completos de clientes e endereços
- **Sessões** - Controle de tokens e sessões
- **Auditoria** - Logs de login e atividades

## 🔗 Principais Endpoints

### 🔐 Autenticação (`/api/auth`)
- `POST /api/auth/cliente/login` - Login de cliente (MongoDB)
- `POST /api/auth/admin/login` - Login de administrador (MongoDB)
- `POST /api/auth/cliente/registro` - Registro de cliente (MongoDB)
- `POST /api/auth/logout` - Logout (invalida token)
- `GET /api/auth/validate` - Valida token

### �️ Produtos (`/api/produtos`)
- `GET /api/produtos` - Lista produtos com filtros (SQL Server)
- `GET /api/produtos/{id}` - Busca produto por ID (SQL Server)
- `POST /api/produtos` - Cria produto (SQL Server - Admin)
- `PUT /api/produtos/{id}` - Atualiza produto (SQL Server - Admin)
- `PATCH /api/produtos/{id}/estoque` - Atualiza estoque (SQL Server - Admin)

### 📦 Pedidos (`/api/pedidos`)
- `GET /api/pedidos` - Lista pedidos (SQL Server - Admin)
- `POST /api/pedidos` - Cria pedido (SQL Server)
- `GET /api/pedidos/cliente/{clienteId}` - Pedidos do cliente (SQL Server)
- `PATCH /api/pedidos/{id}/status` - Atualiza status (SQL Server - Admin)

### 🛒 Carrinho (`/api/carrinho`)
- `GET /api/carrinho/{clienteId}` - Obtém carrinho (Memória)
- `POST /api/carrinho/{clienteId}/adicionar` - Adiciona item
- `POST /api/carrinho/{clienteId}/finalizar` - Finaliza compra

## ⚙️ Configuração Dual Database

### Connection Strings
```json
{
  "ConnectionStrings": {
    "SqlServerConnection": "Server=5CG5123HJ2\\SQLEXPRESS;Database=PetShop;Trusted_Connection=true;TrustServerCertificate=true;",
    "MongoDbConnection": "mongodb://localhost:27017",
    "MongoDbDatabase": "petshop"
  }
}
```

### Pré-requisitos
- **.NET 9 SDK** (mais recente)
- **SQL Server** (5CG5123HJ2\SQLEXPRESS)
- **MongoDB** (localhost:27017)
- **Visual Studio 2022** (v17.8+) ou VS Code

## 🚀 Instalação e Execução

### 1. Preparar Bancos de Dados

**SQL Server:**
```sql
-- Execute no SQL Server Management Studio conectado a 5CG5123HJ2\SQLEXPRESS
-- Rode o script: create-schema-only.sql
-- Opcionalmente: insert-sample-data.sql
```

**MongoDB:**
```bash
# Execute no mongosh conectado a localhost:27017
mongosh mongodb://localhost:27017
load("../setup-petshop-db.js")
load("../sample-data.js")
```

### 2. Executar a API

```bash
# Restaurar pacotes
dotnet restore

# Executar aplicação
dotnet run
```

### 3. Acessar Documentação
- **Swagger UI:** https://localhost:7001/swagger/
- **API Base:** https://localhost:7001/api/
- **OpenAPI Spec:** https://localhost:7001/swagger/v1/swagger.json

### 🎨 **Swagger Features Premium**
- **🎯 Interface personalizada** com tema PetShop
- **🔐 Autenticação integrada** - clique em "Authorize"
- **💡 Exemplos automáticos** para todos os endpoints
- **⏱️ Performance metrics** em tempo real
- **📱 Design responsivo** para todos os dispositivos
- **⌨️ Keyboard shortcuts** (Ctrl+/ para buscar)
- **🎨 Visual enhancements** com ícones e cores

## � Fluxo de Autenticação

### Login de Cliente
```json
POST /api/auth/cliente/login
{
  "email": "joao@email.com",
  "senha": "senha123"
}

Response:
{
  "message": "Login realizado com sucesso.",
  "token": "638123456789_AbCdEf...",
  "userId": "507f1f77bcf86cd799439011",
  "userType": "cliente"
}
```

### Login de Administrador
```json
POST /api/auth/admin/login
{
  "email": "admin1@petshop.com", 
  "senha": "admin123"
}

Response:
{
  "message": "Login de administrador realizado com sucesso.",
  "token": "638123456789_XyZwVu...",
  "userId": "507f1f77bcf86cd799439012",
  "userType": "administrador"
}
```

## 🔒 Segurança e Sessões

### Gerenciamento de Tokens
- **Tokens únicos** gerados por sessão
- **Expiração automática** (24h cliente, 8h admin)
- **Armazenamento seguro** no MongoDB
- **Invalidação de logout**

### Validação de Tokens
```http
GET /api/auth/validate
Authorization: Bearer 638123456789_AbCdEf...
```

## 📊 Vantagens da Arquitetura Híbrida

### SQL Server Benefits
✅ **ACID Compliance** para transações financeiras
✅ **Relacionamentos complexos** entre produtos/pedidos
✅ **Performance otimizada** para consultas analíticas
✅ **Backup e recovery** robustos

### MongoDB Benefits  
✅ **Flexibilidade de schema** para dados de usuário
✅ **Escalabilidade horizontal** para autenticação
✅ **Performance de leitura** para sessões
✅ **Estruturas aninhadas** para endereços

## � Manutenção

### Limpeza de Sessões Expiradas
```csharp
// Executado automaticamente no startup
await sessionRepository.CleanupExpiredAsync();
```

### Sincronização de Dados
- **Clientes:** ID do MongoDB como referência no SQL Server
- **Pedidos:** Referência cruzada entre sistemas
- **Consistência:** Validação em ambos os bancos

## 📈 Monitoramento

### Logs da Aplicação
- **Conexões de banco:** Startup logs
- **Autenticação:** Login/logout tracking
- **Erros:** Exception handling

### Health Checks
```http
GET /api/auth/validate  # MongoDB health
GET /api/produtos       # SQL Server health
```

## 🧪 Dados de Exemplo

### MongoDB (Autenticação)
- 3 Administradores pré-cadastrados
- Sistema de registro para clientes
- Sessões de exemplo

### SQL Server (Dados de Negócio)
- 3 Categorias (Rações, Brinquedos, Medicamentos)
- 3 Produtos de exemplo
- Estrutura completa de pedidos

## � Troubleshooting

### Erro de Conexão SQL Server
```
Verificar se o serviço SQL Server está rodando em 5CG5123HJ2\SQLEXPRESS
Validar as credenciais de conexão
```

### Erro de Conexão MongoDB
```
Verificar se MongoDB está rodando em localhost:27017
Testar conexão: mongosh mongodb://localhost:27017
```

### Token Inválido
```
Tokens expiram automaticamente
Realizar novo login para obter token válido
```
