// MongoDB Shell Script for PetShop Database Setup
// Run this in mongosh or MongoDB Compass shell
// Connect to: mongodb://localhost:27017

// Switch to petshop database (creates it if it doesn't exist)
use("petshop");

// Create Administradores collection
db.createCollection("administradores", {
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: ["nome", "email", "senha"],
            properties: {
                nome: { 
                    bsonType: "string",
                    description: "Nome do administrador é obrigatório"
                },
                email: { 
                    bsonType: "string",
                    pattern: "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$",
                    description: "Email válido é obrigatório"
                },
                senha: { 
                    bsonType: "string",
                    minLength: 6,
                    description: "Senha com pelo menos 6 caracteres é obrigatória"
                }
            }
        }
    }
});

print("✅ Coleção 'administradores' criada");

// Create Clientes collection
db.createCollection("clientes", {
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: ["nome", "email", "senha"],
            properties: {
                nome: { 
                    bsonType: "string",
                    description: "Nome do cliente é obrigatório"
                },
                email: { 
                    bsonType: "string",
                    pattern: "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$",
                    description: "Email válido é obrigatório"
                },
                telefone: { 
                    bsonType: "string",
                    description: "Telefone é opcional"
                },
                senha: { 
                    bsonType: "string",
                    minLength: 6,
                    description: "Senha com pelo menos 6 caracteres é obrigatória"
                },
                enderecos: {
                    bsonType: "array",
                    description: "Array de endereços do cliente",
                    items: {
                        bsonType: "object",
                        required: ["_id_endereco", "logradouro", "numero", "cidade", "estado", "cep"],
                        properties: {
                            _id_endereco: { bsonType: "objectId" },
                            logradouro: { bsonType: "string" },
                            numero: { bsonType: "string" },
                            complemento: { bsonType: "string" },
                            cidade: { bsonType: "string" },
                            estado: { bsonType: "string" },
                            cep: { 
                                bsonType: "string",
                                pattern: "^[0-9]{5}-?[0-9]{3}$",
                                description: "CEP no formato 00000-000"
                            }
                        }
                    }
                }
            }
        }
    }
});

print("✅ Coleção 'clientes' criada");

// Create Categorias collection
db.createCollection("categorias", {
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: ["nome"],
            properties: {
                nome: { 
                    bsonType: "string",
                    description: "Nome da categoria é obrigatório"
                }
            }
        }
    }
});

print("✅ Coleção 'categorias' criada");

// Create Produtos collection
db.createCollection("produtos", {
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: ["nome", "preco", "estoque", "idCategoria"],
            properties: {
                nome: { 
                    bsonType: "string",
                    description: "Nome do produto é obrigatório"
                },
                descricao: { 
                    bsonType: "string",
                    description: "Descrição do produto é opcional"
                },
                preco: { 
                    bsonType: "decimal",
                    minimum: 0,
                    description: "Preço deve ser um valor decimal positivo"
                },
                estoque: { 
                    bsonType: "int",
                    minimum: 0,
                    description: "Estoque deve ser um número inteiro não negativo"
                },
                idCategoria: { 
                    bsonType: "objectId",
                    description: "ID da categoria é obrigatório"
                }
            }
        }
    }
});

print("✅ Coleção 'produtos' criada");

// Create Pedidos collection
db.createCollection("pedidos", {
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: ["idCliente", "idEnderecoEntrega", "logradouro", "numero", "cidade", "estado", "cep", "dataPedido", "statusPedido", "valorTotal", "itens"],
            properties: {
                idCliente: { 
                    bsonType: "objectId",
                    description: "ID do cliente é obrigatório"
                },
                idEnderecoEntrega: { 
                    bsonType: "objectId",
                    description: "ID do endereço de entrega é obrigatório"
                },
                logradouro: { bsonType: "string" },
                numero: { bsonType: "string" },
                complemento: { bsonType: "string" },
                cidade: { bsonType: "string" },
                estado: { bsonType: "string" },
                cep: { 
                    bsonType: "string",
                    pattern: "^[0-9]{5}-?[0-9]{3}$"
                },
                dataPedido: { 
                    bsonType: "date",
                    description: "Data do pedido é obrigatória"
                },
                statusPedido: { 
                    bsonType: "string",
                    enum: ["PENDENTE", "PROCESSANDO", "ENVIADO", "ENTREGUE", "CANCELADO"],
                    description: "Status deve ser um dos valores permitidos"
                },
                valorTotal: { 
                    bsonType: "decimal",
                    minimum: 0,
                    description: "Valor total deve ser positivo"
                },
                itens: {
                    bsonType: "array",
                    minItems: 1,
                    description: "Pedido deve ter pelo menos um item",
                    items: {
                        bsonType: "object",
                        required: ["idProduto", "nomeProduto", "quantidade", "precoUnitario"],
                        properties: {
                            idProduto: { bsonType: "objectId" },
                            nomeProduto: { bsonType: "string" },
                            quantidade: { 
                                bsonType: "int",
                                minimum: 1,
                                description: "Quantidade deve ser pelo menos 1"
                            },
                            precoUnitario: { 
                                bsonType: "decimal",
                                minimum: 0,
                                description: "Preço unitário deve ser positivo"
                            }
                        }
                    }
                }
            }
        }
    }
});

print("✅ Coleção 'pedidos' criada");

// Create indexes for better performance
db.administradores.createIndex({ email: 1 }, { unique: true });
print("✅ Índice único criado em administradores.email");

db.clientes.createIndex({ email: 1 }, { unique: true });
print("✅ Índice único criado em clientes.email");

db.produtos.createIndex({ idCategoria: 1 });
print("✅ Índice criado em produtos.idCategoria");

db.produtos.createIndex({ nome: "text", descricao: "text" });
print("✅ Índice de texto criado em produtos para busca");

db.pedidos.createIndex({ idCliente: 1 });
print("✅ Índice criado em pedidos.idCliente");

db.pedidos.createIndex({ statusPedido: 1 });
print("✅ Índice criado em pedidos.statusPedido");

db.pedidos.createIndex({ dataPedido: -1 });
print("✅ Índice criado em pedidos.dataPedido (decrescente)");

// Display summary
print("\n🎉 SETUP CONCLUÍDO COM SUCESSO!");
print("=================================");
print("Database: petshop");
print("Coleções criadas:");
print("  📋 administradores");
print("  👥 clientes");
print("  🏷️  categorias");
print("  🛍️  produtos");
print("  📦 pedidos");
print("\n📊 Índices criados para performance otimizada");
print("🔒 Validação de esquema ativa em todas as coleções");
print("\nPróximos passos:");
print("1. Inserir dados de exemplo");
print("2. Testar as validações");
print("3. Desenvolver API REST");
