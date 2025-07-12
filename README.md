# PetShop Database & API Setup

Complete setup for a PetShop application supporting both **MongoDB** and **SQL Server** databases, plus a **C# Web API**.

## 📁 Project Structure

```
PetShop/
├── 📊 Database Scripts/
│   ├── MongoDB/
│   │   ├── setup-petshop-db.js
│   │   ├── sample-data.js
│   │   └── mongosh-commands.txt
│   └── SQL Server/
│       ├── create-sqlserver-database.sql
│       ├── create-schema-only.sql
│       └── insert-sample-data.sql
├── 🚀 PetShopAPI/           # C# Web API (.NET 8)
│   ├── Controllers/
│   ├── Models/
│   ├── DTOs/
│   ├── Data/
│   └── Repositories/
└── 📖 Documentation/
    └── README.md
```

## Prerequisites

### Database Options
**MongoDB Setup**
- MongoDB running on localhost:27017
- MongoDB Compass or mongosh CLI

**SQL Server Setup**  
- SQL Server instance (local or remote)
- SQL Server Management Studio (SSMS) or Azure Data Studio

### API Setup
- .NET 8 SDK
- Visual Studio 2022 or VS Code

## Database Schema

The following collections will be created in the `petshop` database:

### 1. Administradores
- `_id`: ObjectId
- `nome`: String (required)
- `email`: String (required, unique)
- `senha`: String (required)

### 2. Clientes
- `_id`: ObjectId
- `nome`: String (required)
- `email`: String (required, unique)
- `telefone`: String (optional)
- `senha`: String (required)
- `enderecos`: Array of Objects
  - `_id_endereco`: ObjectId
  - `logradouro`: String
  - `numero`: String
  - `complemento`: String (optional)
  - `cidade`: String
  - `estado`: String
  - `cep`: String

### 3. Categorias
- `_id`: ObjectId
- `nome`: String (required)

### 4. Produtos
- `_id`: ObjectId
- `nome`: String (required)
- `descricao`: String (optional)
- `preco`: Decimal128 (required)
- `estoque`: Int32 (required)
- `idCategoria`: ObjectId (required, references Categorias)

### 5. Pedidos
- `_id`: ObjectId
- `idCliente`: ObjectId (required, references Clientes)
- `idEnderecoEntrega`: ObjectId (required, references Cliente.enderecos._id_endereco)
- `logradouro`: String (required)
- `numero`: String (required)
- `complemento`: String (optional)
- `cidade`: String (required)
- `estado`: String (required)
- `cep`: String (required)
- `dataPedido`: Date (required)
- `statusPedido`: String (required, enum: 'PENDENTE', 'PROCESSANDO', 'ENVIADO', 'ENTREGUE', 'CANCELADO')
- `valorTotal`: Decimal128 (required)
- `itens`: Array of Objects
  - `idProduto`: ObjectId (references Produtos)
  - `nomeProduto`: String
  - `quantidade`: Int32
  - `precoUnitario`: Decimal128

## MongoDB Setup

### Using MongoDB Compass
1. Open MongoDB Compass
2. Connect to `mongodb://localhost:27017`
3. Open the shell tab at the bottom
4. Copy and paste the contents of `setup-petshop-db.js`
5. Execute the script

### Using mongosh CLI
1. Open command prompt/terminal
2. Run: `mongosh mongodb://localhost:27017`
3. Execute: `load("setup-petshop-db.js")`

### Quick Copy-Paste Option
1. Open `mongosh-commands.txt`
2. Copy the commands and paste them into mongosh or Compass shell

### Insert MongoDB Sample Data
After creating collections, run `sample-data.js` to insert sample data.

## SQL Server Setup

### Using SQL Server Management Studio (SSMS)
1. Open SSMS and connect to your SQL Server instance
2. Open `create-sqlserver-database.sql` or `create-schema-only.sql`
3. Execute the script to create database and tables
4. Optionally run `insert-sample-data.sql` to add sample data

### Using Azure Data Studio
1. Connect to your SQL Server instance
2. Open and execute the SQL scripts in order:
   - `create-schema-only.sql` (creates tables)
   - `insert-sample-data.sql` (adds sample data)

## Files Overview

### MongoDB Files
- `setup-petshop-db.js` - Creates MongoDB collections with validation
- `sample-data.js` - Inserts sample data into MongoDB
- `mongosh-commands.txt` - Quick reference commands

### SQL Server Files  
- `create-sqlserver-database.sql` - Complete database with sample data
- `create-schema-only.sql` - Database schema only (tables, constraints, indexes)
- `insert-sample-data.sql` - Sample data insertion script

## Indexes Created

The following indexes are automatically created for optimal performance:
- `administradores.email` (unique)
- `clientes.email` (unique)
- `produtos.idCategoria`
- `pedidos.idCliente`
- `pedidos.statusPedido`
- `pedidos.dataPedido` (descending)

## Validation

All collections include JSON Schema validation to ensure data integrity:
- Required fields are enforced
- Data types are validated
- Enum values are restricted where applicable
- Array structures are validated

## Next Steps

After creating the collections, you can:
1. Insert sample data using `sample-data.js`
2. Connect your application using: `mongodb://localhost:27017/petshop`
3. Build a REST API or web application
