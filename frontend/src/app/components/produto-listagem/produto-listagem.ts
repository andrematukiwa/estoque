import { Component, OnInit } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Produto } from '../../models/produto.model';

@Component({
  selector: 'app-produto-listagem',
  standalone: true,
  imports: [CommonModule, CurrencyPipe],
  templateUrl: './produto-listagem.html',
})
export class ProdutoListagemComponent implements OnInit {
  produtos: Produto[] = [];
  errorMessage: string = '';

  private readonly baseUrl = 'http://localhost:5218/api/produto';

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.carregarProdutos();
  }

  carregarProdutos(): void {
    this.http.get<any[]>(this.baseUrl).subscribe({
      next: (data) => {
        this.produtos = data.map(p => ({
          Id: p.id,
          DSC_Name: p.dsC_Name,
          NUM_UnitPrice: p.nuM_UnitPrice,
          QTD_STOCK: p.qtD_STOCK
        }));
      },
      error: (err) => {
        this.errorMessage = 'Erro ao carregar produtos: ' + err.message;
      }
    });
  }

  irParaNotaFiscal(): void {
    this.router.navigate(['/notas']);
  }

  irParaPedidos(): void {
    this.router.navigate(['/pedidos/novo']);
  }

  irParaCadastroProduto(): void {
    this.router.navigate(['/produtos/novo']);
  }
}
