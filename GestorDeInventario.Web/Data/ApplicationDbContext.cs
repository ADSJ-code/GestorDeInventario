using GestorDeInventario.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace GestorDeInventario.Web.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Produto> Produtos { get; set; }
    public DbSet<HistoricoAlteracao> HistoricoAlteracoes { get; set; }
}