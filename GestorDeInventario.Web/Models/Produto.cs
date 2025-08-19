using System.ComponentModel.DataAnnotations;

namespace GestorDeInventario.Web.Models;

public class Produto
{
    public int Id { get; set; }

    [Display(Name = "Nome")]
    public string Nome { get; set; } = string.Empty;

    [Display(Name = "Quantidade")]
    public int Quantidade { get; set; }

    [Display(Name = "Preço")]
    [Range(0.00, 9999999.99, ErrorMessage = "O preço não pode ser negativo.")]
    public decimal Preco { get; set; }

    [Display(Name = "Data de Validade")]
    public DateTime? DataValidade { get; set; }
}