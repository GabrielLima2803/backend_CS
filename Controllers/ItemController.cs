using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

[Route("api/itens")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ItemController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult AdicionarItem(Item item)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            _context.Itens.Add(item);
            _context.SaveChanges();
            
            // Agora, crie um registro na tabela associativa ItemCarrinho
            var itemCarrinho = new ItemCarrinho
            {
                ItemId = item.Id,
                CarrinhoId = 1 // Supondo que o carrinho tenha o ID 1 (você pode ajustar isso)
            };
            _context.ItensCarrinho.Add(itemCarrinho);
            _context.SaveChanges();

            transaction.Commit();
            return Ok();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    [HttpGet]
    public IActionResult ListarItens()
    {
        // Recupere os itens associados a um carrinho específico
        var itens = _context.ItensCarrinho
            .Where(ic => ic.CarrinhoId == 1) // Supondo que o carrinho tenha o ID 1
            .Select(ic => ic.Item)
            .ToList();

        return Ok(itens);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteItem(int id)
    {
        var item = _context.Itens.Find(id);

        if (item == null)
        {
            return NotFound(); // Retorna 404 se o item não for encontrado
        }

        using var transaction = _context.Database.BeginTransaction();

        try
        {
            // Remova o registro na tabela associativa ItemCarrinho
            var itemCarrinho = _context.ItensCarrinho
                .FirstOrDefault(ic => ic.ItemId == id && ic.CarrinhoId == 1); // Supondo que o carrinho tenha o ID 1

            if (itemCarrinho != null)
            {
                _context.ItensCarrinho.Remove(itemCarrinho);
            }

            _context.Itens.Remove(item);
            _context.SaveChanges();
            transaction.Commit();
            return Ok();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }
}
