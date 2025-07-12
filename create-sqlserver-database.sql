-- SQL Server Database Creation Script for PetShop
-- Based on the provided database diagram

USE master;
GO

-- Create the PetShop database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'PetShop')
BEGIN
    CREATE DATABASE PetShop;
END
GO

USE PetShop;
GO

-- Create Categoria table
CREATE TABLE categoria (
    idCategoria INT IDENTITY(1,1) PRIMARY KEY,
    nome VARCHAR(100) NOT NULL
);

-- Create Produto table
CREATE TABLE produto (
    idProduto INT IDENTITY(1,1) PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    descricao TEXT,
    preco DECIMAL(10,2) NOT NULL,
    estoque INT NOT NULL DEFAULT 0,
    ativo TINYINT NOT NULL DEFAULT 1,
    categoriaId INT NOT NULL,
    FOREIGN KEY (categoriaId) REFERENCES categoria(idCategoria)
);

-- Create Cliente table
CREATE TABLE cliente (
    idCliente INT IDENTITY(1,1) PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    senha VARCHAR(100) NOT NULL,
    telefone VARCHAR(20)
);

-- Create Endereco table
CREATE TABLE endereco (
    idEndereco INT IDENTITY(1,1) PRIMARY KEY,
    logradouro VARCHAR(150) NOT NULL,
    numero INT NOT NULL,
    bairro VARCHAR(100),
    cidade VARCHAR(100) NOT NULL,
    estado VARCHAR(50) NOT NULL,
    cep VARCHAR(10) NOT NULL,
    clienteId INT NOT NULL,
    FOREIGN KEY (clienteId) REFERENCES cliente(idCliente)
);

-- Create Pedido table
CREATE TABLE pedido (
    idPedido INT IDENTITY(1,1) PRIMARY KEY,
    dataPedido DATETIME NOT NULL DEFAULT GETDATE(),
    status VARCHAR(20) NOT NULL DEFAULT 'PENDENTE',
    valorTotal DECIMAL(10,2) NOT NULL,
    clienteId INT NOT NULL,
    enderecoEntregaId INT NOT NULL,
    FOREIGN KEY (clienteId) REFERENCES cliente(idCliente),
    FOREIGN KEY (enderecoEntregaId) REFERENCES endereco(idEndereco),
    CONSTRAINT CHK_Status CHECK (status IN ('PENDENTE', 'PROCESSANDO', 'ENVIADO', 'ENTREGUE', 'CANCELADO'))
);

-- Create ItemPedido table (junction table for Pedido and Produto)
CREATE TABLE itemPedido (
    idItem INT IDENTITY(1,1) PRIMARY KEY,
    quantidade INT NOT NULL,
    precoUnitario DECIMAL(10,2) NOT NULL,
    produtoId INT NOT NULL,
    pedidoId INT NOT NULL,
    FOREIGN KEY (produtoId) REFERENCES produto(idProduto),
    FOREIGN KEY (pedidoId) REFERENCES pedido(idPedido)
);

-- Create indexes for better performance
CREATE INDEX IX_produto_categoriaId ON produto(categoriaId);
CREATE INDEX IX_endereco_clienteId ON endereco(clienteId);
CREATE INDEX IX_pedido_clienteId ON pedido(clienteId);
CREATE INDEX IX_pedido_enderecoEntregaId ON pedido(enderecoEntregaId);
CREATE INDEX IX_pedido_dataPedido ON pedido(dataPedido);
CREATE INDEX IX_pedido_status ON pedido(status);
CREATE INDEX IX_itemPedido_produtoId ON itemPedido(produtoId);
CREATE INDEX IX_itemPedido_pedidoId ON itemPedido(pedidoId);

-- Insert Categorias
INSERT INTO categoria (nome) VALUES
('Rações'),
('Brinquedos'),
('Medicamentos');

-- Insert Produtos
INSERT INTO produto (nome, descricao, preco, estoque, categoriaId) VALUES
('Ração Premium 10kg', 'Ração para cães adultos', 129.90, 50, 1),
('Osso de Nylon', 'Brinquedo resistente para cães', 39.90, 100, 2),
('Vermífugo PetLife', 'Medicamento oral para vermes', 24.50, 30, 3);

-- Insert Clientes
INSERT INTO cliente (nome, email, telefone, senha) VALUES
('João Silva', 'joao@email.com', '11999998888', 'senha123'),
('Maria Souza', 'maria@email.com', '11888887777', 'senha123');

-- Insert Endereços
INSERT INTO endereco (logradouro, numero, bairro, cidade, estado, cep, clienteId) VALUES
('Rua A', 123, 'Centro', 'São Paulo', 'SP', '01000-000', 1),
('Rua B', 456, 'Jardim', 'Campinas', 'SP', '13000-000', 2);

-- Insert Pedidos
INSERT INTO pedido (clienteId, enderecoEntregaId, status, valorTotal) VALUES
(1, 1, 'PROCESSANDO', 209.70),
(2, 2, 'PENDENTE', 24.50);

-- Insert Itens do Pedido
INSERT INTO itemPedido (pedidoId, produtoId, quantidade, precoUnitario) VALUES
(1, 1, 1, 129.90),  -- João: 1x Ração Premium
(1, 2, 2, 39.90),   -- João: 2x Osso de Nylon
(2, 3, 1, 24.50);   -- Maria: 1x Vermífugo

PRINT 'Database PetShop created successfully!';
PRINT 'Tables created:';
PRINT '- administrador';
PRINT '- categoria';
PRINT '- produto';
PRINT '- cliente';
PRINT '- endereco';
PRINT '- pedido';
PRINT '- itemPedido';
PRINT 'Sample data inserted successfully!';

GO
