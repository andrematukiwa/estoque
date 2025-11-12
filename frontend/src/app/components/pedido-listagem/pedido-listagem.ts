import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Pedido } from '../../models/pedido.model';
import { Produto } from '../../models/produto.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-pedido-listagem',
  templateUrl: './pedido-listagem.html',
  standalone: true,
  imports: [CommonModule, FormsModule],
})
export class PedidoListagemComponent implements OnInit {
  pedidos: Pedido[] = [];
  produtos: Produto[] = [];
  novoPedido: Pedido = {
    DT_Pedido: new Date(),
    DSC_Cliente: '',
    NUM_ValorTotal: 0,
    Produtos: [],
  };
  valorTotal: number = 0;
  errorMessage = '';

  private baseUrl = 'http://localhost:5218';

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.carregarProdutos();
    this.carregarPedidos();
  }

  carregarProdutos(): void {
    this.http.get<Produto[]>(`${this.baseUrl}/produto`).subscribe({
      next: (data) => (this.produtos = data),
      error: (err) => (this.errorMessage = 'Erro ao carregar produtos: ' + err.message),
    });
  }

  carregarPedidos(): void {
    this.http.get<Pedido[]>(`${this.baseUrl}/pedidos`).subscribe({
      next: (data) => (this.pedidos = data),
      error: (err) => (this.errorMessage = 'Erro ao carregar pedidos: ' + err.message),
    });
  }

  toggleProduto(produto: Produto): void {
    const index = this.novoPedido.Produtos.findIndex(p => p.Id === produto.Id);
    if (index > -1) {
      this.novoPedido.Produtos.splice(index, 1);
    } else {
      this.novoPedido.Produtos.push(produto);
    }
    this.calcularTotal();
  }

  calcularTotal(): void {
    this.valorTotal = this.novoPedido.Produtos.reduce(
      (sum, p) => sum + (p.NUM_UnitPrice || 0),
      0
    );
  }

  criarPedido(): void {
    if (this.novoPedido.Produtos.length === 0) {
      alert('Selecione pelo menos um produto.');
      return;
    }

    this.http.post(`${this.baseUrl}/pedidos`, this.novoPedido).subscribe({
      next: () => {
        alert('Pedido criado com sucesso!');
        this.novoPedido = {
          DT_Pedido: new Date(),
          DSC_Cliente: '',
          NUM_ValorTotal: 0,
          Produtos: [],
        };
        this.carregarPedidos();
        this.valorTotal = 0;
      },
      error: (err) => {
        this.errorMessage = 'Erro ao criar pedido: ' + err.message;
      },
    });
  }

  criarNotaDePedido(pedidoId?: number): void {
    if (!pedidoId) return;
    this.http.post(`${this.baseUrl}/notafiscal/pedido/${pedidoId}`, {}).subscribe({
      next: () => {
        alert('✅ Nota fiscal criada a partir do pedido!');
        this.carregarPedidos();
      },
      error: (err) => {
        console.error('Erro ao criar nota fiscal:', err);
        this.errorMessage = '❌ Erro ao criar nota fiscal a partir do pedido.';
      }
    });
  }
}
