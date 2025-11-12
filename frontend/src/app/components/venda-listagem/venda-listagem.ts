import { Component, OnInit } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

interface Venda {
  id: number;
  dsc_Cliente: string;
  dt_Venda: string;
  produtos: { dsC_Name: string; nuM_UnitPrice: number }[];
  valorTotal: number;
}

@Component({
  selector: 'app-venda-listagem',
  standalone: true,
  imports: [CommonModule, CurrencyPipe, DatePipe],
  templateUrl: './venda-listagem.html',
  styleUrls: ['./venda-listagem.css']
})
export class VendaListagemComponent implements OnInit {
  vendas: Venda[] = [];
  carregando = true;
  erro = '';

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.carregarVendas();
  }

  carregarVendas(): void {
    this.http.get<Venda[]>('http://localhost:5218/api/venda').subscribe({
      next: (data) => {
        this.vendas = data.map(v => ({
          ...v,
          valorTotal: v.produtos.reduce((acc, prod) => acc + prod.nuM_UnitPrice, 0)
        }));
        this.carregando = false;
      },
      error: (err) => {
        console.error(err);
        this.erro = 'Erro ao carregar vendas.';
        this.carregando = false;
      }
    });
  }

  novaVenda(): void {
    this.router.navigate(['/pedidos/novo']);
  }

  imprimirNota(id: number) {
    this.http.put(`http://localhost:5218/api/pedido/${id}/imprimir`, {}).subscribe({
      next: (res: any) => {
        alert(res.message);
        this.carregarVendas(); 
      },
      error: (err) => {
        alert(err.error.message || 'Erro ao imprimir nota.');
      }
    });
  }

}
