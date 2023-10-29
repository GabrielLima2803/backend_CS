using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

[Route("api/carrinho")]
[ApiController]
public class CarrinhoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CarrinhoController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("criar")]
    public async Task<IActionResult> CriarCarrinho()
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            var carrinho = new Carrinho();
            _context.Carrinhos.Add(carrinho);
            await _context.SaveChangesAsync();
            transaction.Commit();
            return Ok(carrinho);
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    [HttpPost("adicionar/{carrinhoId}/{itemId}")]
    public async Task<IActionResult> AdicionarItemAoCarrinho(int carrinhoId, int itemId)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            var item = await _context.Itens.FindAsync(itemId);
            var carrinho = await _context.Carrinhos.FindAsync(carrinhoId);

            if (item != null && carrinho != null)
            {
                var itemCarrinho = new ItemCarrinho
                {
                    ItemId = itemId,
                    CarrinhoId = carrinhoId
                };

                _context.ItensCarrinho.Add(itemCarrinho);
                await _context.SaveChangesAsync();
                transaction.Commit();
                return Ok();
            }

            transaction.Rollback();
            return NotFound();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }
[HttpDelete("remover/{carrinhoId}")]
public async Task<IActionResult> RemoverCarrinho(int carrinhoId)
{
    using var transaction = _context.Database.BeginTransaction();

    try
    {
        var carrinho = await _context.Carrinhos.FindAsync(carrinhoId);

        if (carrinho != null)
        {
            // Se desejar, pode remover automaticamente os itens associados a este carrinho.
            // Certifique-se de que o comportamento esteja de acordo com seus requisitos.
            var itensCarrinho = _context.ItensCarrinho
                .Where(ic => ic.CarrinhoId == carrinhoId);

            _context.ItensCarrinho.RemoveRange(itensCarrinho);

            _context.Carrinhos.Remove(carrinho);
            await _context.SaveChangesAsync();
            transaction.Commit();
            return Ok();
        }

        transaction.Rollback();
        return NotFound();
    }
    catch (Exception)
    {
        transaction.Rollback();
        throw;
    }
}
}
