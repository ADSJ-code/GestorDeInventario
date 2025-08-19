using GestorDeInventario.Web.Models;
using System.Collections.Generic;

namespace GestorDeInventario.Web.Services;

public interface IInventarioService
{
    List<Produto> ObterTodos();
    Produto? ObterPorId(int id);
    void Adicionar(Produto produto);
    void Atualizar(Produto produto);
    void Remover(int id);
}