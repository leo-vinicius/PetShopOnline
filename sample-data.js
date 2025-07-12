// Conectar ao banco de dados 'petshop'
//use petshop

// Criação das coleções (opcional se já criadas com setup-petshop-db.js)
db.createCollection("administradores");
db.createCollection("clientes");
db.createCollection("categorias");
db.createCollection("produtos");
db.createCollection("pedidos");

// Administradores
db.administradores.insertMany([
    {
        nome: 'Leonardo Soares',
        email: 'admin1@petshop.com',
        senha: 'admin123'
    },
    {
        nome: 'Lucca Braga',
        email: 'admin2@petshop.com',
        senha: 'admin123'
    },
    {
        nome: 'Luca Domingues',
        email: 'admin3@petshop.com',
        senha: 'admin123'
    }
]);

// Categorias
db.categorias.insertMany([
    { nome: 'Rações' },
    { nome: 'Brinquedos' },
    { nome: 'Medicamentos' }
]);

// Buscar _id's das categorias para usar em Produtos
const categoriaRacoes = db.categorias.findOne({ nome: 'Rações' });
const categoriaBrinquedos = db.categorias.findOne({ nome: 'Brinquedos' });
const categoriaMedicamentos = db.categorias.findOne({ nome: 'Medicamentos' });

// Produtos
db.produtos.insertMany([
    {
        nome: 'Ração Premium 10kg',
        descricao: 'Ração para cães adultos',
        preco: NumberDecimal('129.90'),
        estoque: 50,
        idCategoria: categoriaRacoes._id
    },
    {
        nome: 'Osso de Nylon',
        descricao: 'Brinquedo resistente para cães',
        preco: NumberDecimal('39.90'),
        estoque: 100,
        idCategoria: categoriaBrinquedos._id
    },
    {
        nome: 'Vermífugo PetLife',
        descricao: 'Medicamento oral para vermes',
        preco: NumberDecimal('24.50'),
        estoque: 30,
        idCategoria: categoriaMedicamentos._id
    }
]);

// Buscar _id's dos produtos para usar em Pedidos
const produtoRacao = db.produtos.findOne({ nome: 'Ração Premium 10kg' });
const produtoOsso = db.produtos.findOne({ nome: 'Osso de Nylon' });
const produtoVermifugo = db.produtos.findOne({ nome: 'Vermífugo PetLife' });

// Clientes
// Gerando ObjectId para _id_endereco manualmente para poder referenciá-los
const idEnderecoJoao = new ObjectId();
const idEnderecoMaria = new ObjectId();

db.clientes.insertMany([
    {
        nome: 'João Silva',
        email: 'joao@email.com',
        telefone: '11999998888',
        senha: 'senha123',
        enderecos: [
            {
                _id_endereco: idEnderecoJoao,
                logradouro: 'Rua A',
                numero: '123',
                complemento: 'Apto 1',
                cidade: 'São Paulo',
                estado: 'SP',
                cep: '01000-000'
            }
        ]
    },
    {
        nome: 'Maria Souza',
        email: 'maria@email.com',
        telefone: '11888887777',
        senha: 'senha123',
        enderecos: [
            {
                _id_endereco: idEnderecoMaria,
                logradouro: 'Rua B',
                numero: '456',
                complemento: null,
                cidade: 'Campinas',
                estado: 'SP',
                cep: '13000-000'
            }
        ]
    }
]);

// Buscar _id's dos clientes para usar em Pedidos
const clienteJoao = db.clientes.findOne({ email: 'joao@email.com' });
const clienteMaria = db.clientes.findOne({ email: 'maria@email.com' });

// Pedidos
const dataPedido1 = new Date();
const dataPedido2 = new Date();

// Obter as cópias dos endereços para os pedidos
const enderecoJoaoPedido1 = clienteJoao.enderecos[0];
const enderecoMariaPedido2 = clienteMaria.enderecos[0];

db.pedidos.insertMany([
    {
        idCliente: clienteJoao._id,
        idEnderecoEntrega: enderecoJoaoPedido1._id_endereco,
        logradouro: enderecoJoaoPedido1.logradouro,
        numero: enderecoJoaoPedido1.numero,
        complemento: enderecoJoaoPedido1.complemento,
        cidade: enderecoJoaoPedido1.cidade,
        estado: enderecoJoaoPedido1.estado,
        cep: enderecoJoaoPedido1.cep,
        dataPedido: dataPedido1,
        statusPedido: 'PROCESSANDO',
        valorTotal: NumberDecimal('209.70'),
        itens: [
            {
                idProduto: produtoRacao._id,
                nomeProduto: produtoRacao.nome,
                quantidade: 1,
                precoUnitario: produtoRacao.preco
            },
            {
                idProduto: produtoOsso._id,
                nomeProduto: produtoOsso.nome,
                quantidade: 2,
                precoUnitario: produtoOsso.preco
            }
        ]
    },
    {
        idCliente: clienteMaria._id,
        idEnderecoEntrega: enderecoMariaPedido2._id_endereco,
        logradouro: enderecoMariaPedido2.logradouro,
        numero: enderecoMariaPedido2.numero,
        complemento: enderecoMariaPedido2.complemento,
        cidade: enderecoMariaPedido2.cidade,
        estado: enderecoMariaPedido2.estado,
        cep: enderecoMariaPedido2.cep,
        dataPedido: dataPedido2,
        statusPedido: 'PENDENTE',
        valorTotal: NumberDecimal('24.50'),
        itens: [
            {
                idProduto: produtoVermifugo._id,
                nomeProduto: produtoVermifugo.nome,
                quantidade: 1,
                precoUnitario: produtoVermifugo.preco
            }
        ]
    }
]);

print("✅ Dados inseridos com sucesso!");
print("📊 Resumo dos dados:");
print("- " + db.administradores.countDocuments() + " administradores");
print("- " + db.categorias.countDocuments() + " categorias");
print("- " + db.produtos.countDocuments() + " produtos");
print("- " + db.clientes.countDocuments() + " clientes");
print("- " + db.pedidos.countDocuments() + " pedidos");
