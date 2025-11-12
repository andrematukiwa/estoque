import { Component, OnInit } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Produto } from '../../models/produto.model';

@Component({
  selector: 'app-pedido-cadastro',
  standalone: true,
  imports: [CommonModule, FormsModule, CurrencyPipe],
  templateUrl: './pedido-cadastro.html'
})
export class PedidoCadastroComponent implements OnInit {
  produtos: Produto[] = [];
  produtosSelecionados: Produto[] = [];
  cliente: string = '';
  valorTotal: number = 0;
  mensagem: string = '';
  erro: string = '';

  private readonly baseUrl = 'http://localhost:5218/api';

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.carregarProdutos();
  }

  irParaTelaInicial() {
    this.router.navigate(['/']);
  }

  carregarProdutos(): void {
    this.http.get<any[]>(`${this.baseUrl}/produto`).subscribe({
      next: (data) => {
        this.produtos = data.map(p => ({
          Id: p.id,
          DSC_Name: p.dsC_Name,
          NUM_UnitPrice: p.nuM_UnitPrice,
          QTD_STOCK: p.qtD_STOCK
        }));
      },
      error: (err) => {
        console.error(err);
        this.erro = 'Erro ao carregar produtos.';
      }
    });
  }

  toggleProduto(produto: Produto): void {
    const index = this.produtosSelecionados.findIndex(p => p.Id === produto.Id);
    if (index > -1) {
      this.produtosSelecionados.splice(index, 1);
    } else {
      this.produtosSelecionados.push(produto);
    }
    this.calcularValorTotal();
  }

  calcularValorTotal(): void {
    this.valorTotal = this.produtosSelecionados.reduce((acc, p) => acc + (p.NUM_UnitPrice || 0), 0);
  }

  criarPedido(): void {
    if (!this.cliente.trim() || this.produtosSelecionados.length === 0) {
      this.erro = 'Preencha o nome do cliente e selecione ao menos um produto.';
      return;
    }

    const pedido = {
      DSC_Cliente: this.cliente,
      Produtos: this.produtosSelecionados,
      NUM_ValorTotal: this.valorTotal
    };

    this.http.post(`${this.baseUrl}/pedido`, pedido).subscribe({
      next: () => {
        this.mensagem = '✅ Pedido criado com sucesso!';
        this.erro = '';
        setTimeout(() => this.router.navigate(['/']), 1500);
      },
      error: (err) => {
        console.error(err);
        this.erro = 'Erro ao salvar pedido.';
      }
    });
  }
}
