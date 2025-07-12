# 🐾 PetShop - Projeto Completo

## 📋 Resumo Executivo

Este projeto implementa uma **plataforma completa de e-commerce para produtos de animais de estimação** usando uma **arquitetura híbrida de bancos de dados** com C# .NET 8 Web API.

### 🏗️ Arquitetura Híbrida Implementada

#### 📊 SQL Server (5CG5123HJ2\SQLEXPRESS)
**Responsabilidade:** Dados principais do negócio
- ✅ **Produtos** - Catálogo completo com estoque
- ✅ **Categorias** - Organização de produtos
- ✅ **Pedidos** - Transações e items
- ✅ **Endereços** - Endereços de entrega
- ✅ **Clientes** (referência) - IDs para linking

#### 🍃 MongoDB (localhost:27017)
**Responsabilidade:** Autenticação e administração
- ✅ **Administradores** - Usuários admin do sistema
- ✅ **Clientes** - Dados completos e endereços
- ✅ **Sessões** - Controle de tokens e login
- ✅ **Auditoria** - Logs de acesso e atividades

---

## 🗂️ Estrutura do Projeto

### 📁 Arquivos de Banco de Dados

#### MongoDB Scripts (Standalone - Sem Node.js)
```
/setup-petshop-db.js      - Cria collections e índices
/sample-data.js           - Dados de exemplo (3 admins)
/mongosh-commands.txt     - Comandos para execução manual
```

#### SQL Server Scripts  
```
/create-sqlserver-database.sql    - Database completo com dados
/create-schema-only.sql           - Apenas schema/estrutura
/insert-sample-data.sql           - Apenas dados de exemplo
```

### 📁 PetShopAPI Project

#### Controllers (/Controllers)
- ✅ **AuthController.cs** - Login/logout MongoDB
- ✅ **ProdutosController.cs** - CRUD produtos SQL Server
- ✅ **CategoriasController.cs** - CRUD categorias SQL Server
- ✅ **ClientesController.cs** - CRUD clientes híbrido
- ✅ **PedidosController.cs** - CRUD pedidos SQL Server
- ✅ **CarrinhoController.cs** - Gestão carrinho em memória

#### Models (/Models)
**SQL Server Models:**
- ✅ **Administrador.cs** - Entidade de admin
- ✅ **Cliente.cs** - Entidade de cliente
- ✅ **Categoria.cs** - Categorias de produtos
- ✅ **Produto.cs** - Produtos do catálogo
- ✅ **Endereco.cs** - Endereços de entrega
- ✅ **Pedido.cs** - Pedidos realizados
- ✅ **ItemPedido.cs** - Items dos pedidos

**MongoDB Models (/Models/MongoDB):**
- ✅ **AdminMongo.cs** - Administradores
- ✅ **ClienteMongo.cs** - Clientes completos
- ✅ **SessionMongo.cs** - Sessões de login

#### DTOs (/DTOs)
- ✅ **LoginRequestDto.cs** - Request de login
- ✅ **RegistroClienteDto.cs** - Registro de cliente
- ✅ **CriarProdutoDto.cs** - Criação de produto
- ✅ **CriarCategoriaDto.cs** - Criação de categoria
- ✅ **AdicionarCarrinhoDto.cs** - Adicionar ao carrinho
- ✅ **CriarPedidoDto.cs** - Criação de pedido

#### Data Contexts (/Data)
- ✅ **PetShopContext.cs** - Entity Framework para SQL Server
- ✅ **MongoContext.cs** - MongoDB context

#### Repositories (/Repositories)
**SQL Server Repositories:**
- ✅ **IAdministradorRepository.cs + AdministradorRepository.cs**
- ✅ **ICategoriaRepository.cs + CategoriaRepository.cs**
- ✅ **IProdutoRepository.cs + ProdutoRepository.cs**
- ✅ **IPedidoRepository.cs + PedidoRepository.cs**

**MongoDB Repositories (/Repositories/MongoDB):**
- ✅ **IAdminMongoRepository.cs + AdminMongoRepository.cs**
- ✅ **IClienteMongoRepository.cs + ClienteMongoRepository.cs**

#### Services (/Services)
- ✅ **AuthService.cs** - Autenticação híbrida completa
- ✅ **SessionRepository** - Gerenciamento de sessões

---

## 🔗 API Endpoints Implementados

### 🔐 Autenticação (`/api/auth`)
- ✅ `POST /api/auth/cliente/login` - Login cliente (MongoDB)
- ✅ `POST /api/auth/admin/login` - Login admin (MongoDB)  
- ✅ `POST /api/auth/cliente/registro` - Registro cliente (MongoDB)
- ✅ `POST /api/auth/logout` - Logout (invalida token)
- ✅ `GET /api/auth/validate` - Validação de token

### 🛍️ Produtos (`/api/produtos`)
- ✅ `GET /api/produtos` - Lista com filtros (SQL Server)
- ✅ `GET /api/produtos/{id}` - Busca por ID (SQL Server)
- ✅ `POST /api/produtos` - Criar produto (SQL Server - Admin)
- ✅ `PUT /api/produtos/{id}` - Atualizar produto (SQL Server - Admin)
- ✅ `DELETE /api/produtos/{id}` - Remover produto (SQL Server - Admin)
- ✅ `PATCH /api/produtos/{id}/estoque` - Atualizar estoque (SQL Server - Admin)

### 🏷️ Categorias (`/api/categorias`)
- ✅ `GET /api/categorias` - Listar categorias (SQL Server)
- ✅ `GET /api/categorias/{id}` - Buscar por ID (SQL Server)
- ✅ `POST /api/categorias` - Criar categoria (SQL Server - Admin)
- ✅ `PUT /api/categorias/{id}` - Atualizar categoria (SQL Server - Admin)
- ✅ `DELETE /api/categorias/{id}` - Remover categoria (SQL Server - Admin)

### 👥 Clientes (`/api/clientes`)
- ✅ `POST /api/clientes/registro` - Registro (MongoDB)
- ✅ `POST /api/clientes/login` - Login (MongoDB)
- ✅ `GET /api/clientes/{id}` - Buscar cliente (MongoDB)
- ✅ `POST /api/clientes/{id}/enderecos` - Adicionar endereço (MongoDB)
- ✅ `GET /api/clientes/{id}/enderecos` - Listar endereços (MongoDB)

### 📦 Pedidos (`/api/pedidos`)
- ✅ `GET /api/pedidos` - Listar todos (SQL Server - Admin)
- ✅ `GET /api/pedidos/cliente/{clienteId}` - Pedidos do cliente (SQL Server)
- ✅ `GET /api/pedidos/{id}` - Buscar por ID (SQL Server)
- ✅ `POST /api/pedidos` - Criar pedido (SQL Server)
- ✅ `PATCH /api/pedidos/{id}/status` - Atualizar status (SQL Server - Admin)
- ✅ `PATCH /api/pedidos/{id}/cancelar` - Cancelar pedido (SQL Server)

### 🛒 Carrinho (`/api/carrinho`)
- ✅ `GET /api/carrinho/{clienteId}` - Obter carrinho (Memória)
- ✅ `POST /api/carrinho/{clienteId}/adicionar` - Adicionar item
- ✅ `PUT /api/carrinho/{clienteId}/item/{produtoId}` - Atualizar quantidade
- ✅ `DELETE /api/carrinho/{clienteId}/item/{produtoId}` - Remover item
- ✅ `DELETE /api/carrinho/{clienteId}` - Limpar carrinho
- ✅ `POST /api/carrinho/{clienteId}/finalizar` - Finalizar compra

---

## ⚙️ Configuração de Execução

### 🗄️ Connection Strings Configuradas
```json
{
  "ConnectionStrings": {
    "SqlServerConnection": "Server=5CG5123HJ2\\SQLEXPRESS;Database=PetShop;Trusted_Connection=true;TrustServerCertificate=true;",
    "MongoDbConnection": "mongodb://localhost:27017",
    "MongoDbDatabase": "petshop"
  }
}
```

### 📦 Pacotes NuGet Instalados
- ✅ **Microsoft.EntityFrameworkCore.SqlServer** (8.0.0)
- ✅ **Microsoft.EntityFrameworkCore.Tools** (8.0.0)
- ✅ **MongoDB.Driver** (3.0.0)
- ✅ **Swashbuckle.AspNetCore** (6.7.3)

### 🛠️ Dependency Injection Configurado
```csharp
// SQL Server
builder.Services.AddDbContext<PetShopContext>(options =>
    options.UseSqlServer(connectionString));

// MongoDB
builder.Services.AddSingleton<MongoContext>();

// Repositories SQL Server
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();

// Repositories MongoDB
builder.Services.AddScoped<IAdminMongoRepository, AdminMongoRepository>();
builder.Services.AddScoped<IClienteMongoRepository, ClienteMongoRepository>();

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
```

---

## 🚀 Como Executar o Projeto

### 1️⃣ Preparar SQL Server Database
```sql
-- Conectar em: 5CG5123HJ2\SQLEXPRESS
-- Executar no SSMS:
CREATE DATABASE PetShop;
GO

-- Rodar script:
-- \create-schema-only.sql (estrutura)
-- \insert-sample-data.sql (dados exemplo - opcional)
```

### 2️⃣ Preparar MongoDB Database
```bash
# Conectar no mongosh:
mongosh mongodb://localhost:27017

# Executar scripts:
use petshop
load("setup-petshop-db.js")
load("sample-data.js")
```

### 3️⃣ Executar a API
```bash
# No diretório PetShopAPI:
dotnet restore
dotnet build
dotnet run

# OU usar o task criado no VS Code
```

### 4️⃣ Acessar a Documentação
- **Swagger UI:** https://localhost:7001/
- **API Base:** https://localhost:7001/api/

---

## 🧪 Dados de Teste Disponíveis

### 🍃 MongoDB (Autenticação)
```json
// Administradores
{
  "email": "admin1@petshop.com",
  "senha": "admin123"
}
{
  "email": "admin2@petshop.com", 
  "senha": "admin123"
}
{
  "email": "admin3@petshop.com",
  "senha": "admin123"
}

// Clientes (criar via /api/clientes/registro)
```

### 📊 SQL Server (Dados de Negócio)
```sql
-- Categorias
INSERT INTO Categorias VALUES 
('Rações', 'Alimentos para pets'),
('Brinquedos', 'Brinquedos e acessórios'),
('Medicamentos', 'Produtos veterinários');

-- Produtos
INSERT INTO Produtos VALUES
('Ração Premium Cães', 'Ração super premium...', 89.90, 1, 100, 1),
('Bola de Borracha', 'Bola resistente...', 15.50, 2, 50, 1),
('Antipulgas Spray', 'Produto antipulgas...', 45.00, 3, 30, 1);
```

---

## 🔒 Sistema de Autenticação

### 🎫 Fluxo de Tokens
1. **Login** → Gera token único baseado em timestamp
2. **Token** → Armazenado no MongoDB com expiração
3. **Validação** → Verificação em tempo real
4. **Logout** → Invalidação imediata do token

### ⏰ Expiração de Sessões
- **Clientes:** 24 horas
- **Administradores:** 8 horas
- **Limpeza:** Automática no startup

### 🛡️ Segurança
- ✅ Senhas hash SHA256
- ✅ Tokens únicos por sessão
- ✅ Validação de dados de entrada
- ✅ Verificação de autorização por endpoint

---

## 🎯 Casos de Uso Principais

### 👤 Cliente
1. **Registro/Login** → MongoDB
2. **Navegar produtos** → SQL Server
3. **Adicionar ao carrinho** → Memória
4. **Finalizar pedido** → SQL Server
5. **Acompanhar pedidos** → SQL Server

### 👨‍💼 Administrador  
1. **Login admin** → MongoDB
2. **Gerenciar produtos** → SQL Server
3. **Gerenciar categorias** → SQL Server
4. **Acompanhar pedidos** → SQL Server
5. **Atualizar status** → SQL Server

---

## ✅ Status de Implementação

### Concluído ✅
- [x] Arquitetura híbrida funcional
- [x] Autenticação completa (MongoDB)
- [x] CRUD produtos/categorias (SQL Server)
- [x] Sistema de pedidos (SQL Server)
- [x] Carrinho de compras (Memória)
- [x] Documentação Swagger
- [x] Scripts de banco standalone
- [x] API compilando e executando

### Pronto para Produção 🚀
- [x] Connection strings configuradas
- [x] Validação de dados
- [x] Tratamento de erros
- [x] Logs de aplicação
- [x] Repository pattern
- [x] Dependency injection
- [x] Clean architecture

---

## 📊 Vantagens da Arquitetura Híbrida

### 💪 SQL Server Benefits
- **ACID Compliance** para transações críticas
- **Relacionamentos complexos** entre entidades
- **Performance de consultas** analíticas
- **Integridade referencial** garantida

### 🚀 MongoDB Benefits
- **Flexibilidade de schema** para dados de usuário
- **Escalabilidade horizontal** para autenticação
- **Performance de leitura** para sessões
- **Estruturas aninhadas** para dados complexos

---

## 🎉 Projeto Finalizado

Este projeto implementa uma **solução completa e profissional** para e-commerce de produtos pet usando as melhores práticas de:

- ✅ **Clean Architecture**
- ✅ **Repository Pattern** 
- ✅ **Dependency Injection**
- ✅ **API RESTful**
- ✅ **Documentação OpenAPI/Swagger**
- ✅ **Arquitetura híbrida de dados**
- ✅ **Segurança e autenticação**

**Status:** 🟢 **COMPLETO E FUNCIONAL** 🟢

---

*Projeto desenvolvido em C# .NET 8 com arquitetura híbrida MongoDB + SQL Server*
