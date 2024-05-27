
# Lanchonete - wa-lanchonete_api

API desenvolvida para atender as funcionalidades de uma lanchonete proposta como projeto da Pós Tech FIAP.


## Stack utilizada

**Back-end:** .NET 6, C#

**Banco de dados:** PostgresSQL


## Rodando Localmente

Instale o projeto wa-lanchonete_api

1º Passo : Clone o projeto

2º Passo : Configurando projeto no Docker - acesse o terminal dentro da pasta onde o  projeto foi clonado e execute o comando abaixo para que seja configurado (docker precisa estar aberto)
- API
- Banco de dados
- Pgadmin : gerenciador do banco de dados

```bash
  docker-compose up --build
```

3º Passo: **(OPCIONAL)** Configurando Pgadmin para visualização do banco de dados.
Acesse no navegador http://localhost:5050/browser/
- Usuário: admin@admin.com
- Senha: admin

Siga os passos do gif abaixo:
![App Screenshot](https://raw.githubusercontent.com/WilliaMarques7/wa-lanchonete_api/main/Configurar%20PGADMIN.gif)

## Documentação da API

API foi configurada na porta 3001: http://localhost:3001/swagger/index.html

Existe a própria documentação da API configurada no Swagger, abaixo está apenas um exemplo para realizar um pedido.

#### Retorna os produtos cadastrados

```http
  GET /api/products
```

| Parâmetro   | 
| :---------- | 
| `No parameters`      | 

#### Cadastrar um pedido

```http
  POST /api/order

  BODY
  {
    "customerId": 1,
    "orderItems": [
      {
        "productId": 1,
        "quantity": 1,
        "price": 10
      },
      {
        "productId": 2,
        "quantity": 2,
        "price": 10
      },
      {
        "productId": 3,
        "quantity": 1,
        "price": 3
      }
    ]
  }
```

| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `customerId` | `int` | **Obrigatório**. Código 1 do Cliente Anônimo (Já cadastrado na tabela Customer) |
| `productId` | `int` | **Obrigatório**. Código do produto cadastrado em Produtos |
| `quantity` | `int` | **Obrigatório**. Quantidade de itens selecionados |
| `price` | `decimal` | **Obrigatório**. Valor total dos itens selecionado |


## Autores

- [@alvarovianello](https://github.com/alvarovianello)
- [@WilliaMarques7](https://github.com/https://github.com/WilliaMarques7)
