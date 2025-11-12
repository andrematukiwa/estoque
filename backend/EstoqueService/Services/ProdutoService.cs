using EstoqueService.Data;
using EstoqueService.Models;
using Microsoft.EntityFrameworkCore;

namespace EstoqueService.Services
{
    public class ProdutoService
    {
        private readonly AppDbContext _context;

        public ProdutoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Produto> CriarProdutoAsync(Produto produto)
        {
            if (string.IsNullOrWhiteSpace(produto.DSC_Name))
                throw new ArgumentException("O nome do produto é obrigatório.");

            if (produto.NUM_UnitPrice <= 0)
                throw new ArgumentException("O preço do produto deve ser maior que zero.");

            if (produto.QTD_STOCK < 0)
                throw new ArgumentException("A quantidade em estoque não pode ser negativa.");

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return produto;
        }

        public async Task<Produto> AtualizarProdutoAsync(int id, Produto produtoAtualizado)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
                throw new KeyNotFoundException("Produto não encontrado.");

            if (string.IsNullOrWhiteSpace(produtoAtualizado.DSC_Name))
                throw new ArgumentException("O nome do produto é obrigatório.");

            if (produtoAtualizado.NUM_UnitPrice <= 0)
                throw new ArgumentException("O preço deve ser maior que zero.");

            if (produtoAtualizado.QTD_STOCK < 0)
                throw new ArgumentException("A quantidade em estoque não pode ser negativa.");

            produto.DSC_Name = produtoAtualizado.DSC_Name;
            produto.NUM_UnitPrice = produtoAtualizado.NUM_UnitPrice;
            produto.QTD_STOCK = produtoAtualizado.QTD_STOCK;

            await _context.SaveChangesAsync();
            return produto;
        }

        public async Task<List<Produto>> ListarProdutosAsync()
        {
            return await _context.Produtos.ToListAsync();
        }

        public async Task<Produto?> ObterProdutoPorIdAsync(int id)
        {
            return await _context.Produtos.FindAsync(id);
        }
    }
}
