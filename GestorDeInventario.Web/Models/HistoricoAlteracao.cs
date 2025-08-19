using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestorDeInventario.Web.Models;

public class HistoricoAlteracao
{
    [Key]
    public int Id { get; set; }

    public int ProdutoId { get; set; }

    [ForeignKey("ProdutoId")]
    public virtual Produto? Produto { get; set; }

    [Required]
    public DateTime DataAlteracao { get; set; }

    [Required]
    [StringLength(100)]
    public string CampoAlterado { get; set; } = string.Empty;

    public string? ValorAntigo { get; set; }

    public string? ValorNovo { get; set; }
    
    [Required]
    [StringLength(50)]
    public string AutorAlteracao { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Motivo { get; set; } = string.Empty;
}