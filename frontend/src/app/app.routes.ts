import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./components/produto-listagem/produto-listagem')
        .then(m => m.ProdutoListagemComponent),
  },
  {
    path: 'produtos/novo',
    loadComponent: () =>
      import('./components/produto-cadastro/produto-cadastro')
        .then(m => m.ProdutoCadastroComponent),
  },
  {
    path: 'notas',
    loadComponent: () =>
      import('./components/nota-fiscal/nota-fiscal-listagem')
        .then(m => m.NotaFiscalListagemComponent),
  },
  {
    path: 'notas/nova',
    loadComponent: () =>
      import('./components/nota-fiscal-cadastro/nota-fiscal-cadastro')
        .then(m => m.NotaFiscalCadastroComponent),
  },
  {
    path: 'pedidos/novo',
    loadComponent: () =>
      import('./components/pedido-cadastro/pedido-cadastro')
        .then(m => m.PedidoCadastroComponent),
  },
  {
    path: 'vendas',
    loadComponent: () =>
      import('./components/venda-listagem/venda-listagem')
        .then(m => m.VendaListagemComponent),
  },
];
