import { Component, OnInit } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { NotaFiscal } from '../../models/nota-fiscal.model';
import { Produto } from '../../models/produto.model';
import { Pedido } from '../../models/pedido.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nota-fiscal-listagem',
  standalone: true,
  imports: [CommonModule, FormsModule, CurrencyPipe],
  templateUrl: './nota-fiscal-listagem.html',
  styleUrls: ['./nota-fiscal-listagem.css']
})
export class NotaFiscalListagemComponent implements OnInit {
  pedidos: Pedido[] = [];
  pedidoSelecionado: Pedido | null = null;
  notas: NotaFiscal[] = [];
  mensagem = '';
  errorMessage = '';
  private baseUrl = 'http://localhost:5218/api';

  constructor(private http: HttpClient, private router: Router) { }

  irParaTelaInicial() {
    this.router.navigate(['/']); 
  }

  ngOnInit(): void {
    this.carregarPedidos();
    this.carregarNotas();
  }

  carregarPedidos(): void {
    this.http.get<any[]>(`${this.baseUrl}/pedido`).subscribe({
      next: (data) => {
        console.log('📦 Pedidos recebidos:', data);

        this.pedidos = (data || [])
          .filter(p => p.dsC_Status === 'Aberta' || p.dsC_Status === 'aberta')
          .map(p => ({
            Id: p.id,
            DSC_Cliente: p.dsC_Cliente,
            NUM_ValorTotal: p.nuM_ValorTotal,
            DSC_Status: p.dsC_Status,
            NUM_NotaFiscal: p.nuM_NotaFiscal,
            DT_Pedido: p.dT_Pedido,
            Produtos: (p.produtos || []).map((prod: any) => ({
              Id: prod.id,
              DSC_Name: prod.dsC_Name,
              NUM_UnitPrice: prod.nuM_UnitPrice,
              QTD_STOCK: prod.qtD_STOCK
            }))
          }));
      },
      error: (err) => {
        console.error(err);
        this.errorMessage = 'Erro ao carregar pedidos.';
      }
    });
  }

  carregarNotas(): void {
    this.http.get<any[]>(`${this.baseUrl}/notafiscal`).subscribe({
      next: (data) => {
        console.log('🧾 Notas fiscais recebidas:', data);

        this.notas = (data || []).map(n => ({
          Id: n.id,
          NUM_NotaFiscal: n.nuM_NotaFiscal,
          DSC_Status: n.dsC_Status === 'Emitida' ? 'Fechada' : n.dsC_Status,
          DT_Emissao: n.dt_Emissao,
          NUM_ValorTotal: n.nuM_ValorTotal,
          PedidoId: n.pedidoId,
          Produtos: (n.produtos || []).map((prod: any) => ({
            Id: prod.id,
            DSC_Name: prod.dsC_Name,
            NUM_UnitPrice: prod.nuM_UnitPrice,
            QTD_STOCK: prod.qtD_STOCK
          }))
        }));
      },
      error: (err) => {
        console.error(err);
        this.errorMessage = 'Erro ao carregar notas fiscais.';
      }
    });
  }

  selecionarPedido(pedido: Pedido): void {
    this.pedidoSelecionado = pedido;
    this.mensagem = `🧾 Pedido do cliente ${pedido.DSC_Cliente} selecionado para faturamento.`;
    this.errorMessage = '';
  }

  gerarNotaFiscal(): void {
    if (!this.pedidoSelecionado) {
      this.errorMessage = 'Selecione um pedido antes de gerar a nota fiscal.';
      return;
    }

    const produtosParaEnviar = (this.pedidoSelecionado.Produtos || []).map(p => ({ Id: p.Id }));

    const novaNota = {
      NUM_NotaFiscal: this.pedidoSelecionado.NUM_NotaFiscal ?? ('NF-' + new Date().getTime()),
      DSC_Status: 'Fechada',
      NUM_ValorTotal: this.pedidoSelecionado.NUM_ValorTotal,
      PedidoId: this.pedidoSelecionado.Id,
      Produtos: produtosParaEnviar
    };

    this.http.post(`${this.baseUrl}/notafiscal/pedido/${this.pedidoSelecionado.Id}`, novaNota)
      .subscribe({
        next: (res) => {
          console.log('✅ Nota fiscal gerada:', res);
          this.mensagem = '✅ Nota fiscal gerada e pedido fechado com sucesso!';
          this.errorMessage = '';
          this.pedidoSelecionado = null;
          this.carregarNotas();
          this.carregarPedidos();
        },
        error: (err) => {
          console.error('❌ Erro ao gerar nota:', err);

          if (err?.status === 404) {
            this.http.post(`${this.baseUrl}/notafiscal`, novaNota).subscribe({
              next: () => {
                this.mensagem = '✅ Nota fiscal gerada (fallback)!';
                this.errorMessage = '';
                this.pedidoSelecionado = null;
                this.carregarNotas();
                this.carregarPedidos();
              },
              error: (err2) => {
                console.error(err2);
                this.errorMessage = 'Erro ao gerar nota fiscal.';
              }
            });
          } else {
            this.errorMessage = err?.error?.message ?? 'Erro ao gerar nota fiscal.';
          }
        }
      });
  }
}
