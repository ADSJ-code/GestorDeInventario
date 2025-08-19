using System.ComponentModel.DataAnnotations;

namespace GestorDeInventario.Web.Models;

public class ProdutoViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "Nome")]
    public string Nome { get; set; } = string.Empty;
    
    [Display(Name = "Quantidade")]
    public int Quantidade { get; set; }

    [Display(Name = "Preço")]
    public decimal Preco { get; set; }

    [Display(Name = "Data de Validade")]
    public DateTime? DataValidade { get; set; }

    [Display(Name = "Status")]
    public string StatusVencimento { get; private set; } = string.Empty;

    public string CorStatusVencimento { get; private set; } = "black";
    
    public string DataValidadeFormatada { get; private set; } = string.Empty;

    public ProdutoViewModel(Produto produto)
    {
        Id = produto.Id;
        Nome = produto.Nome;
        Quantidade = produto.Quantidade;
        Preco = produto.Preco;
        DataValidade = produto.DataValidade;
        CalcularStatus();
    }

    private void CalcularStatus()
    {
        if (DataValidade.HasValue)
        {
            DataValidadeFormatada = DataValidade.Value.ToString("dd/MM/yyyy");
            var hoje = DateTime.Today;
            var diasParaVencer = (DataValidade.Value - hoje).TotalDays;

            if (diasParaVencer < 0)
            {
                StatusVencimento = $"Vencido há {-diasParaVencer:F0} dia(s)";
                CorStatusVencimento = "red";
            }
            else if (diasParaVencer <= 30)
            {
                StatusVencimento = $"Vence em {diasParaVencer:F0} dia(s)";
                CorStatusVencimento = "orange";
            }
            else
            {
                StatusVencimento = "No prazo";
                CorStatusVencimento = "green";
            }
        }
        else
        {
            DataValidadeFormatada = "Indeterminada";
            StatusVencimento = "N/A";
            CorStatusVencimento = "blue";
        }

        if (Quantidade <= 30 && Quantidade > 0)
        {
            StatusVencimento += $" (Estoque baixo: {Quantidade} un.)";
            if (CorStatusVencimento == "green")
            {
                CorStatusVencimento = "orange";
            }
        }
        else if (Quantidade == 0)
        {
             StatusVencimento += " (Sem estoque)";
             CorStatusVencimento = "red";
        }
    }
}