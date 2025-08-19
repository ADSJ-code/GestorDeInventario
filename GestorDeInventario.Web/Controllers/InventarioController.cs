using System.Globalization;
using GestorDeInventario.Web.Data;
using GestorDeInventario.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestorDeInventario.Web.Controllers;

public class InventarioController : Controller
{
    private readonly ApplicationDbContext _context;

    public InventarioController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string sortOrder, string searchString)
    {
        ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        ViewData["QuantitySortParm"] = sortOrder == "Quantity" ? "quantity_desc" : "Quantity";
        ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
        ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
        ViewData["CurrentFilter"] = searchString;
        var produtosQuery = from p in _context.Produtos select p;
        if (!String.IsNullOrEmpty(searchString))
        {
            produtosQuery = produtosQuery.Where(p => p.Nome.ToLower().Contains(searchString.ToLower()));
        }
        switch (sortOrder)
        {
            case "name_desc":
                produtosQuery = produtosQuery.OrderByDescending(p => p.Nome);
                break;
            case "Quantity":
                produtosQuery = produtosQuery.OrderBy(p => p.Quantidade);
                break;
            case "quantity_desc":
                produtosQuery = produtosQuery.OrderByDescending(p => p.Quantidade);
                break;
            case "Price":
                produtosQuery = produtosQuery.OrderBy(p => p.Preco);
                break;
            case "price_desc":
                produtosQuery = produtosQuery.OrderByDescending(p => p.Preco);
                break;
            case "Date":
                produtosQuery = produtosQuery.OrderBy(p => p.DataValidade);
                break;
            case "date_desc":
                produtosQuery = produtosQuery.OrderByDescending(p => p.DataValidade);
                break;
            default:
                produtosQuery = produtosQuery.OrderBy(p => p.Nome);
                break;
        }
        var produtos = await produtosQuery.AsNoTracking().ToListAsync();
        var viewModel = produtos.Select(p => new ProdutoViewModel(p)).ToList();
        return View(viewModel);
    }
    
    public async Task<IActionResult> Detalhes(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var produto = await _context.Produtos.FirstOrDefaultAsync(m => m.Id == id);
        if (produto == null)
        {
            return NotFound();
        }
        ViewBag.Historico = await _context.HistoricoAlteracoes
            .Where(h => h.ProdutoId == id)
            .OrderByDescending(h => h.DataAlteracao)
            .Take(5)
            .ToListAsync();
        
        var viewModel = new ProdutoViewModel(produto);
        return View(viewModel);
    }

    public IActionResult Criar()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Criar(Produto produto)
    {
        if (Request.Form.TryGetValue("Preco", out var precoStringValues))
        {
            string precoString = precoStringValues.ToString().Replace(".", ",");
            var culture = new CultureInfo("pt-BR");
            if (decimal.TryParse(precoString, NumberStyles.Number, culture, out decimal precoConvertido))
            {
                produto.Preco = precoConvertido;
            }
        }

        var produtosExistentes = await _context.Produtos.AsNoTracking().ToListAsync();
        if (produtosExistentes.Any(p => p.Nome.Equals(produto.Nome, StringComparison.OrdinalIgnoreCase)))
        {
            ModelState.AddModelError("Nome", "Já existe um produto com este nome.");
        }
        if (produto.DataValidade.HasValue && produto.DataValidade.Value < new DateTime(2000, 1, 1))
        {
            ModelState.AddModelError("DataValidade", "A data de validade não pode ser anterior ao ano 2000.");
        }

        if (ModelState.IsValid)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            produto.Nome = textInfo.ToTitleCase(produto.Nome.ToLower());
            _context.Add(produto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(produto);
    }
    
    public async Task<IActionResult> Editar(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null)
        {
            return NotFound();
        }
        ViewBag.Historico = await _context.HistoricoAlteracoes
            .Where(h => h.ProdutoId == id)
            .OrderByDescending(h => h.DataAlteracao)
            .Take(5)
            .ToListAsync();
        return View(produto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Produto produtoEditado, string motivo)
    {
        if (id != produtoEditado.Id)
        {
            return NotFound();
        }

        if (Request.Form.TryGetValue("Preco", out var precoStringValues))
        {
            string precoString = precoStringValues.ToString().Replace(".", ",");
            var culture = new CultureInfo("pt-BR");
            if (decimal.TryParse(precoString, NumberStyles.Number, culture, out decimal precoConvertido))
            {
                produtoEditado.Preco = precoConvertido;
            }
        }
        
        var produtoOriginal = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        if (produtoOriginal == null)
        {
            return NotFound();
        }
        if (string.IsNullOrWhiteSpace(motivo))
        {
             ModelState.AddModelError("", "O motivo da alteração é obrigatório.");
        }

        if (ModelState.IsValid)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            string motivoFormatado = textInfo.ToTitleCase(motivo.ToLower());
            var dataAlteracao = DateTime.UtcNow;
            var autor = "Sistema";

            if (produtoOriginal.Nome != produtoEditado.Nome)
            {
                var historico = new HistoricoAlteracao { ProdutoId = id, DataAlteracao = dataAlteracao, AutorAlteracao = autor, Motivo = motivoFormatado, CampoAlterado = "Nome", ValorAntigo = produtoOriginal.Nome, ValorNovo = produtoEditado.Nome };
                _context.Add(historico);
            }
            if (produtoOriginal.Quantidade != produtoEditado.Quantidade)
            {
                var historico = new HistoricoAlteracao { ProdutoId = id, DataAlteracao = dataAlteracao, AutorAlteracao = autor, Motivo = motivoFormatado, CampoAlterado = "Quantidade", ValorAntigo = produtoOriginal.Quantidade.ToString(), ValorNovo = produtoEditado.Quantidade.ToString() };
                _context.Add(historico);
            }
            if (produtoOriginal.Preco != produtoEditado.Preco)
            {
                var historico = new HistoricoAlteracao { ProdutoId = id, DataAlteracao = dataAlteracao, AutorAlteracao = autor, Motivo = motivoFormatado, CampoAlterado = "Preço", ValorAntigo = produtoOriginal.Preco.ToString("C"), ValorNovo = produtoEditado.Preco.ToString("C") };
                _context.Add(historico);
            }
            if (produtoOriginal.DataValidade != produtoEditado.DataValidade)
            {
                var historico = new HistoricoAlteracao { ProdutoId = id, DataAlteracao = dataAlteracao, AutorAlteracao = autor, Motivo = motivoFormatado, CampoAlterado = "Data de Validade", ValorAntigo = produtoOriginal.DataValidade?.ToString("dd/MM/yyyy") ?? "N/A", ValorNovo = produtoEditado.DataValidade?.ToString("dd/MM/yyyy") ?? "N/A" };
                _context.Add(historico);
            }
            try
            {
                produtoEditado.Nome = textInfo.ToTitleCase(produtoEditado.Nome.ToLower());
                _context.Update(produtoEditado);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(produtoEditado.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Historico = await _context.HistoricoAlteracoes
            .Where(h => h.ProdutoId == id)
            .OrderByDescending(h => h.DataAlteracao)
            .Take(5)
            .ToListAsync();
        return View(produtoEditado);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DesfazerAlteracao(int id)
    {
        var historico = await _context.HistoricoAlteracoes.FindAsync(id);
        if (historico == null)
        {
            return NotFound();
        }
        var produto = await _context.Produtos.FindAsync(historico.ProdutoId);
        if (produto == null)
        {
            return NotFound();
        }
        string valorAtualRevertido = "";
        switch (historico.CampoAlterado)
        {
            case "Nome":
                valorAtualRevertido = produto.Nome;
                produto.Nome = historico.ValorAntigo ?? "";
                break;
            case "Quantidade":
                valorAtualRevertido = produto.Quantidade.ToString();
                produto.Quantidade = int.Parse(historico.ValorAntigo ?? "0");
                break;
            case "Preço":
                valorAtualRevertido = produto.Preco.ToString("C");
                produto.Preco = decimal.Parse(historico.ValorAntigo ?? "0", NumberStyles.Currency, new CultureInfo("pt-BR"));
                break;
            case "Data de Validade":
                 valorAtualRevertido = produto.DataValidade?.ToString("dd/MM/yyyy") ?? "N/A";
                if (historico.ValorAntigo == "N/A" || historico.ValorAntigo is null)
                {
                    produto.DataValidade = null;
                }
                else
                {
                    produto.DataValidade = DateTime.ParseExact(historico.ValorAntigo!, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                break;
        }
        var historicoDoDesfazer = new HistoricoAlteracao
        {
            ProdutoId = produto.Id,
            DataAlteracao = DateTime.UtcNow,
            AutorAlteracao = "Sistema",
            Motivo = $"Reversão: restaurado valor '{historico.ValorAntigo}'",
            CampoAlterado = historico.CampoAlterado,
            ValorAntigo = valorAtualRevertido,
            ValorNovo = historico.ValorAntigo
        };
        _context.Add(historicoDoDesfazer);
        _context.Update(produto);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Editar), new { id = produto.Id });
    }

    public async Task<IActionResult> Remover(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var produto = await _context.Produtos.FirstOrDefaultAsync(m => m.Id == id);
        if (produto == null)
        {
            return NotFound();
        }
        
        var viewModel = new ProdutoViewModel(produto);
        return View(viewModel);
    }

    [HttpPost, ActionName("Remover")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoverConfirmado(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto != null)
        {
            var historicoParaRemover = _context.HistoricoAlteracoes.Where(h => h.ProdutoId == id);
            _context.HistoricoAlteracoes.RemoveRange(historicoParaRemover);
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool ProdutoExists(int id)
    {
        return _context.Produtos.Any(e => e.Id == id);
    }
}