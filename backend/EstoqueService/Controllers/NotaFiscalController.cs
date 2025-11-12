using Microsoft.AspNetCore.Mvc;
using EstoqueService.Models;
using EstoqueService.Services;

namespace EstoqueService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotaFiscalController : ControllerBase
    {
        private readonly NotaFiscalService _service;

        public NotaFiscalController(NotaFiscalService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var notas = await _service.ListarAsync();
            return Ok(notas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var nota = await _service.BuscarPorIdAsync(id);
            if (nota == null)
                return NotFound(new { message = "Nota fiscal não encontrada." });

            return Ok(nota);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] NotaFiscal nota)
        {
            var criada = await _service.CriarNotaAsync(nota);
            return CreatedAtAction(nameof(GetById), new { id = criada.Id }, criada);
        }

        [HttpPut("{id}/fechar")]
        public async Task<IActionResult> FecharNota(int id)
        {
            try
            {
                var notaFechada = await _service.FecharNotaAsync(id);
                return Ok(new
                {
                    success = true,
                    message = $"Nota fiscal {notaFechada.NUM_NotaFiscal} foi fechada com sucesso!",
                    nota = notaFechada
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost("pedido/{pedidoId}")]
        public async Task<IActionResult> CriarNotaDePedido(int pedidoId)
        {
            try
            {
                var nota = await _service.CriarNotaFiscalDePedidoAsync(pedidoId);
                return Ok(nota);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
