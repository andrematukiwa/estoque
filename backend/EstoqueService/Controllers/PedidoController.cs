using EstoqueService.Models;
using EstoqueService.Services;
using Microsoft.AspNetCore.Mvc;

namespace EstoqueService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoService _pedidoService;

        public PedidoController(PedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
        {
            try
            {
                var pedidos = await _pedidoService.ListarPedidosAsync();
                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao listar pedidos: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedidoPorId(int id)
        {
            try
            {
                var pedido = await _pedidoService.BuscarPedidoPorIdAsync(id);
                if (pedido == null)
                    return NotFound(new { message = "Pedido não encontrado." });

                return Ok(pedido);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao buscar pedido: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Pedido>> PostPedido(Pedido pedido)
        {
            try
            {
                var novoPedido = await _pedidoService.CriarPedidoAsync(pedido);
                return CreatedAtAction(nameof(GetPedidoPorId), new { id = novoPedido.Id }, novoPedido);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(int id)
        {
            try
            {
                bool removido = await _pedidoService.DeletarPedidoAsync(id);
                if (!removido)
                    return NotFound(new { message = "Pedido não encontrado." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao deletar pedido: {ex.Message}" });
            }
        }

        [HttpPut("{id}/fechar")]
        public async Task<IActionResult> FecharPedido(int id)
        {
            try
            {
                var pedido = await _pedidoService.BuscarPedidoPorIdAsync(id);
                if (pedido == null)
                    return NotFound(new { message = "Pedido (nota fiscal) não encontrado." });

                if (pedido.DSC_Status == "Fechada")
                    return BadRequest(new { message = "Nota já está fechada." });

                pedido.DSC_Status = "Fechada";
                await _pedidoService.SalvarAlteracoesAsync();

                return Ok(new { message = "Nota fiscal impressa e fechada com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao fechar nota: {ex.Message}" });
            }
        }
        [HttpPut("{id}/imprimir")]
        public async Task<IActionResult> ImprimirPedido(int id)
        {
            try
            {
                var pedido = await _pedidoService.ImprimirPedidoAsync(id);
                return Ok(new { message = "Nota impressa com sucesso!", pedido });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
