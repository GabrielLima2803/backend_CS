using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/itemcarrinho")]
[ApiController]
public class ItemCarrinhoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ItemCarrinhoController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("listar/{carrinhoId}")]
    public async Task<IActionResult> ListarItensDoCarrinho(int carrinhoId)
    {
        var itens = await _context.ItensCarrinho
            .Where(ic => ic.CarrinhoId == carrinhoId)
            .Select(ic => ic.Item)
            .ToListAsync();

        return Ok(itens);
    }
}
