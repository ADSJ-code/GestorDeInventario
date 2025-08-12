public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public double Preco { get; set; }
    public int QuantidadeEmStock { get; set; }
    public DateTime Validade { get; set; } // Nova propriedade
}