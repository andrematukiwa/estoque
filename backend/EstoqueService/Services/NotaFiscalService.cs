using EstoqueService.Models;
using EstoqueService.Data;
using Microsoft.EntityFrameworkCore;

namespace EstoqueService.Services
{
    public class NotaFiscalService
    {
        private readonly AppDbContext _context;

        public NotaFiscalService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<NotaFiscal>> ListarAsync()
        {
            return await _context.NotasFiscais
                .Include(n => n.Produtos)
                .ToListAsync();
        }

        public async Task<NotaFiscal?> BuscarPorIdAsync(int id)
        {
            return await _context.NotasFiscais
                .Include(n => n.Produtos)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<NotaFiscal> CriarNotaAsync(NotaFiscal nota)
        {
            var produtosExistentes = new List<Produto>();
            foreach (var p in nota.Produtos)
            {
                var produtoDb = await _context.Produtos.FindAsync(p.Id);
                if (produtoDb == null)
                    throw new Exception($"Produto {p.Id} não encontrado.");

                produtosExistentes.Add(produtoDb);
            }

            nota.Produtos = produtosExistentes;

            nota.NUM_ValorTotal = nota.Produtos.Sum(p => p.NUM_UnitPrice);

            _context.NotasFiscais.Add(nota);
            await _context.SaveChangesAsync();
            return nota;
        }

        public async Task<NotaFiscal?> FecharNotaAsync(int id)
        {
            var nota = await _context.NotasFiscais
                .Include(n => n.Produtos)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (nota == null)
                throw new Exception("Nota fiscal não encontrada.");

            if (nota.DSC_Status == "Fechada")
                throw new Exception("Nota já está fechada.");

            foreach (var produto in nota.Produtos)
            {
                var produtoDb = await _context.Produtos.FindAsync(produto.Id);
                if (produtoDb == null)
                    throw new Exception($"Produto {produto.Id} não encontrado.");

                if (produtoDb.QTD_STOCK < 1)
                    throw new Exception($"Produto '{produtoDb.DSC_Name}' sem saldo suficiente em estoque.");
            }

            foreach (var produto in nota.Produtos)
            {
                var produtoDb = await _context.Produtos.FindAsync(produto.Id);
                if (produtoDb != null)
                {
                    produtoDb.QTD_STOCK -= 1; 
                    _context.Produtos.Update(produtoDb);
                }
            }

            nota.DSC_Status = "Fechada";
            _context.NotasFiscais.Update(nota);

            await _context.SaveChangesAsync();

            return nota;
        }
        public async Task<NotaFiscal> CriarNotaFiscalDePedidoAsync(int pedidoId)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Produtos)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);

            if (pedido == null)
                throw new Exception("Pedido não encontrado.");

            if (pedido.DSC_Status == "Fechada")
                throw new Exception("Este pedido já foi faturado.");

            // Cria a nota fiscal
            var nota = new NotaFiscal
            {
                NUM_NotaFiscal = pedido.NUM_NotaFiscal ?? $"NF-{DateTime.Now:yyyyMMddHHmmss}",
                DSC_Status = "Fechada", // ou "Emitida" se quiser mudar o texto
                Produtos = pedido.Produtos.ToList(),
                NUM_ValorTotal = pedido.NUM_ValorTotal,
                DT_Emissao = DateTime.Now,
                PedidoId = pedido.Id
            };

            // Marca o pedido como fechado
            pedido.DSC_Status = "Fechada";

            _context.Pedidos.Update(pedido);
            _context.NotasFiscais.Add(nota);

            // Atualiza estoque dos produtos
            foreach (var produto in pedido.Produtos)
            {
                var produtoDb = await _context.Produtos.FindAsync(produto.Id);
                if (produtoDb == null)
                    throw new Exception($"Produto {produto.Id} não encontrado.");

                if (produtoDb.QTD_STOCK <= 0)
                    throw new Exception($"Produto '{produtoDb.DSC_Name}' sem estoque suficiente.");

                produtoDb.QTD_STOCK -= 1;
                _context.Produtos.Update(produtoDb);
            }

            // Salva tudo de uma vez
            await _context.SaveChangesAsync();

            // 🔄 Recarrega o pedido atualizado (garantir que status persistiu)
            await _context.Entry(pedido).ReloadAsync();

            return nota;
        }
    }
}
