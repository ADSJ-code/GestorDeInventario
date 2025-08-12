# Gestor de Inventário em C#

![C#](https://img.shields.io/badge/C%23-512BD4?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)

## Sumário

Este projeto é uma aplicação de linha de comando (`console`) desenvolvida em C# e na plataforma .NET. A aplicação funciona como um sistema de gestão de inventário para uma pequena loja, permitindo ao utilizador realizar operações CRUD (Criar, Ler, Atualizar) sobre uma lista de produtos.

O projeto foi construído para validar a competência em C# listada no meu currículo, demonstrando conhecimento em Programação Orientada a Objetos (OOP), manipulação de dados, persistência em arquivos e lógica de negócio.

---

## Funcionalidades Principais

* **Gestão de Produtos:** Permite adicionar novos produtos com nome, preço, quantidade em stock e data de validade.
* **Persistência de Dados:** O inventário é guardado automaticamente num arquivo `inventario.json` ao sair e é carregado ao iniciar, garantindo que os dados não são perdidos entre sessões.
* **Listagem com Ordenação:** O utilizador pode listar todos os produtos e escolher diferentes critérios de ordenação (alfabética A-Z/Z-A, maior/menor stock).
* **Busca Inteligente:** Para atualizar o stock, o utilizador pode buscar por um produto pelo seu nome (ou parte dele). Se múltiplos resultados forem encontrados, o sistema lista-os e pede ao utilizador para especificar o ID correto.
* **Sistema de Alerta de Validade:** A aplicação analisa a data de validade dos produtos e, ao listá-los, exibe um aviso para produtos já vencidos ou que irão vencer nos próximos 30 dias.
* **Validação e Padronização:** O sistema valida as entradas do utilizador (para números e datas) e padroniza automaticamente os nomes dos produtos para o formato "Title Case".

---

## Stack de Tecnologias

* **Linguagem:** C#
* **Plataforma:** .NET 8
* **Persistência:** Serialização/Desserialização para formato JSON.

---

## Como Rodar o Projeto

1.  **Pré-requisito:** Ter o [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) instalado.
2.  **Clone o repositório** e navegue para a pasta:
    ```bash
    git clone [https://github.com/ADSJ-code/gestor-de-inventario-csharp.git](https://github.com/ADSJ-code/gestor-de-inventario-csharp.git)
    cd gestor-de-inventario-csharp
    ```
3.  **Execute a aplicação:**
    ```bash
    dotnet run
    ```
    A aplicação irá iniciar diretamente no seu terminal.