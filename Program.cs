using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;

public class Program
{
    // (O resto da sua classe Program continua igual, sem alterações)
    // ... (Main, MenuDeListagem, ListarProdutos, etc.)
    static List<Produto> inventario = new List<Produto>();
    static int proximoId = 1;
    static string arquivoDeInventario = "inventario.json";

    public static void Main(string[] args)
    {
        CarregarInventario();

        while (true)
        {
            Console.WriteLine("\n--- Gestor de Inventário ---");
            Console.WriteLine("1. Adicionar Produto");
            Console.WriteLine("2. Listar Produtos (com Ordenação)");
            Console.WriteLine("3. Atualizar Stock (com Busca por Nome)");
            Console.WriteLine("4. Sair e Guardar");
            Console.Write("Escolha uma opção: ");

            string? escolha = Console.ReadLine();

            switch (escolha)
            {
                case "1":
                    AdicionarProduto();
                    break;
                case "2":
                    MenuDeListagem();
                    break;
                case "3":
                    AtualizarStock();
                    break;
                case "4":
                    SalvarInventario();
                    Console.WriteLine("Inventário guardado. A sair...");
                    return;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }
        }
    }

    static void AdicionarProduto()
    {
        try
        {
            Console.Write("Nome do produto: ");
            string nomeInput = Console.ReadLine() ?? "";
            string nome = PadronizarNome(nomeInput);

            Console.Write("Preço do produto (ex: 24,99): ");
            string precoInput = Console.ReadLine()?.Replace("R$", "").Replace(",", ".").Trim() ?? "";
            double preco = Convert.ToDouble(precoInput, CultureInfo.InvariantCulture);

            Console.Write("Quantidade em stock: ");
            string quantidadeInput = Console.ReadLine()?.Trim() ?? "";
            int quantidade = Convert.ToInt32(quantidadeInput);

            DateTime validade;
            while (true) // Loop para validar a data
            {
                Console.Write("Data de validade (dd/mm/aaaa): ");
                string validadeInput = Console.ReadLine() ?? "";

                // Tenta converter a data que o utilizador digitou
                if (DateTime.TryParse(validadeInput, new CultureInfo("pt-BR"), DateTimeStyles.None, out validade))
                {
                    // Se a data for no passado, avisa mas permite (pode ser um lote antigo)
                    if (validade < DateTime.Today)
                    {
                        Console.WriteLine("Atenção: A data inserida já está vencida.");
                    }
                    break; // Sai do loop se a data for válida
                }
                else
                {
                    Console.WriteLine("Formato de data inválido. Por favor, use o formato dd/mm/aaaa.");
                }
            }

            Produto novoProduto = new Produto
            {
                Id = proximoId++,
                Nome = nome,
                Preco = preco,
                QuantidadeEmStock = quantidade,
                Validade = validade
            };

            inventario.Add(novoProduto);
            Console.WriteLine("Produto adicionado com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: Entrada inválida. {ex.Message}");
        }
    }

    static void MenuDeListagem()
    {
        Console.WriteLine("\n--- Opções de Ordenação ---");
        Console.WriteLine("1. Ordem Alfabética (A-Z)");
        Console.WriteLine("2. Ordem Alfabética (Z-A)");
        Console.WriteLine("3. Maior Stock");
        Console.WriteLine("4. Menor Stock");
        Console.Write("Escolha como ordenar: ");
        string? escolha = Console.ReadLine();

        List<Produto> inventarioOrdenado = new List<Produto>(inventario);

        switch (escolha)
        {
            case "1":
                inventarioOrdenado = inventario.OrderBy(p => p.Nome).ToList();
                break;
            case "2":
                inventarioOrdenado = inventario.OrderByDescending(p => p.Nome).ToList();
                break;
            case "3":
                inventarioOrdenado = inventario.OrderByDescending(p => p.QuantidadeEmStock).ToList();
                break;
            case "4":
                inventarioOrdenado = inventario.OrderBy(p => p.QuantidadeEmStock).ToList();
                break;
            default:
                Console.WriteLine("Opção de ordenação inválida. A mostrar por ID.");
                break;
        }
        ListarProdutos(inventarioOrdenado);
    }

    static void ListarProdutos(List<Produto> lista)
    {
        if (lista.Any())
        {
            Console.WriteLine("\n--- Lista de Produtos ---");
            foreach (var produto in lista)
            {
                string statusValidade = "";
                DateTime hoje = DateTime.Today;
                TimeSpan diferenca = produto.Validade.Subtract(hoje);
                int diasParaVencer = (int)diferenca.TotalDays;

        if (diasParaVencer < 0)
            {
                statusValidade = $" (VENCIDO HÁ {-diasParaVencer} DIAS)";
            }
            else if (diasParaVencer <= 30)
            {
                statusValidade = $" (VENCE EM {diasParaVencer} DIAS)";
            }

            Console.WriteLine($"ID: {produto.Id} | Nome: {produto.Nome} | Preço: R${produto.Preco:F2} | Stock: {produto.QuantidadeEmStock} | Validade: {produto.Validade:dd/MM/yyyy}{statusValidade}");
            }
        }
    else
        {
        Console.WriteLine("O inventário está vazio.");
        }
    }

    static string PadronizarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome)) return string.Empty;
        var palavras = nome.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < palavras.Length; i++)
        {
            if (palavras[i].Length > 0)
                palavras[i] = char.ToUpper(palavras[i][0]) + palavras[i].Substring(1);
        }
        return string.Join(" ", palavras);
    }

    static void AtualizarStock()
    {
        try
        {
            Console.Write("Digite o nome (ou parte do nome) do produto a ser atualizado: ");
            string? termoBusca = Console.ReadLine()?.ToLower();

            if (string.IsNullOrWhiteSpace(termoBusca))
            {
                Console.WriteLine("O nome para busca não pode ser vazio.");
                return;
            }

            var produtosEncontrados = inventario.Where(p => p.Nome.ToLower().Contains(termoBusca)).ToList();

            if (produtosEncontrados.Count == 0)
            {
                Console.WriteLine("Nenhum produto encontrado com esse nome.");
                return;
            }

            int idProduto;
            if (produtosEncontrados.Count > 1)
            {
                Console.WriteLine("Vários produtos encontrados. Por favor, escolha o ID:");
                ListarProdutos(produtosEncontrados);
                Console.Write("Digite o ID do produto exato: ");
                idProduto = Convert.ToInt32(Console.ReadLine());
            }
            else
            {
                idProduto = produtosEncontrados.First().Id;
                Console.WriteLine($"Produto encontrado: {produtosEncontrados.First().Nome}");
            }

            Produto? produtoParaAtualizar = inventario.FirstOrDefault(p => p.Id == idProduto);

            if (produtoParaAtualizar != null)
            {
                Console.Write($"Nova quantidade em stock para {produtoParaAtualizar.Nome}: ");
                int novaQuantidade = Convert.ToInt32(Console.ReadLine());
                produtoParaAtualizar.QuantidadeEmStock = novaQuantidade;
                Console.WriteLine("Stock atualizado com sucesso!");
            }
            else
            {
                Console.WriteLine("O ID escolhido não corresponde a nenhum dos produtos encontrados.");
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Erro: ID e quantidade devem ser números inteiros.");
        }
    }

    static void SalvarInventario()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(inventario, options);
        File.WriteAllText(arquivoDeInventario, jsonString);
    }

    static void CarregarInventario()
    {
        if (File.Exists(arquivoDeInventario))
        {
            string jsonString = File.ReadAllText(arquivoDeInventario);
            var inventarioCarregado = JsonSerializer.Deserialize<List<Produto>>(jsonString);
            if(inventarioCarregado != null)
            {
                inventario = inventarioCarregado;
                if (inventario.Any()) proximoId = inventario.Max(p => p.Id) + 1;
            }
        }
    }
}