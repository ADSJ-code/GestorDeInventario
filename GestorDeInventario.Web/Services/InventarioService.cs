using GestorDeInventario.Web.Data;
using GestorDeInventario.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace GestorDeInventario.Web.Services;

public class InventarioService : IInventarioService
{
    private readonly ApplicationDbContext _context;

    public InventarioService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Produto> ObterTodos()
    {
        return _context.Produtos.AsNoTracking().ToList();
    }

    public Produto? ObterPorId(int id)
    {
        return _context.Produtos.AsNoTracking().FirstOrDefault(p => p.Id == id);
    }

    public void Adicionar(Produto produto)
    {
        _context.Produtos.Add(produto);
        _context.SaveChanges();
    }

    public void Atualizar(Produto produto)
    {
        _context.Produtos.Update(produto);
        _context.SaveChanges();
    }

    public void Remover(int id)
    {
        var produto = _context.Produtos.Find(id);
        if (produto != null)
        {
            _context.Produtos.Remove(produto);
            _context.SaveChanges();
        }
    }
}