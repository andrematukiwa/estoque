using EstoqueService.Data;
using EstoqueService.Models;
using Microsoft.EntityFrameworkCore;

namespace EstoqueService.Services
{
    public class PedidoService
    {
        private readonly AppDbContext _context;

        public PedidoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Pedido> CriarPedidoAsync(Pedido pedido)
        {
            if (pedido == null)
                throw new ArgumentException("Pedido inválido.");

            if (pedido.Produtos == null || pedido.Produtos.Count == 0)
                throw new ArgumentException("O pedido deve conter pelo menos um produto.");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var produtosParaPedido = new List<Produto>();

                foreach (var p in pedido.Produtos)
                {
                    if (p.Id <= 0)
                        throw new ArgumentException("ID do produto inválido.");

                    var produtoExistente = await _context.Produtos
                        .FirstOrDefaultAsync(x => x.Id == p.Id);

                    if (produtoExistente == null)
                        throw new ArgumentException($"Produto com ID {p.Id} não encontrado.");

                    if (produtoExistente.QTD_STOCK <= 0)
                        throw new InvalidOperationException($"Produto '{produtoExistente.DSC_Name}' sem estoque disponível.");

                    produtoExistente.QTD_STOCK -= 1;

                    produtosParaPedido.Add(produtoExistente);
                }

                pedido.Produtos = produtosParaPedido;
                pedido.DT_Pedido = DateTime.Now;
                pedido.NUM_ValorTotal = produtosParaPedido.Sum(x => x.NUM_UnitPrice);

                pedido.NUM_NotaFiscal = $"NF-{DateTime.Now:yyyyMMddHHmmss}";
                pedido.DSC_Status = "Aberta";

                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                await _context.Entry(pedido).Collection(p => p.Produtos).LoadAsync();

                return pedido;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<List<Pedido>> ListarPedidosAsync()
        {
            return await _context.Pedidos
                .Include(p => p.Produtos)
                .ToListAsync();
        }

        public async Task<Pedido?> BuscarPedidoPorIdAsync(int id)
        {
            return await _context.Pedidos
                .Include(p => p.Produtos)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> DeletarPedidoAsync(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Produtos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return false;

            foreach (var produto in pedido.Produtos ?? Enumerable.Empty<Produto>())
            {
                var produtoDb = await _context.Produtos.FindAsync(produto.Id);
                if (produtoDb != null)
                {
                    produtoDb.QTD_STOCK += 1; 
                }
            }

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task SalvarAlteracoesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<Pedido> ImprimirPedidoAsync(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Produtos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                throw new Exception("Pedido não encontrado.");

            if (pedido.DSC_Status == "Fechada")
                throw new Exception("Esse pedido já está fechado.");

            foreach (var produto in pedido.Produtos ?? new List<Produto>())
            {
                var produtoDb = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == produto.Id);

                if (produtoDb == null)
                    throw new Exception($"Produto '{produto.DSC_Name}' não encontrado.");

                if (produtoDb.QTD_STOCK <= 0)
                    throw new Exception($"Produto '{produtoDb.DSC_Name}' sem estoque suficiente.");

                produtoDb.QTD_STOCK -= 1; 
            }

            pedido.DSC_Status = "Fechada";
            await _context.SaveChangesAsync();

            return pedido;
        }

    }
}
