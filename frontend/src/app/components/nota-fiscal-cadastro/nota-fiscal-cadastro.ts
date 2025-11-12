import { Component, OnInit } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { NotaFiscal } from '../../models/nota-fiscal.model';
import { Produto } from '../../models/produto.model';

@Component({
  selector: 'app-nota-fiscal-cadastro',
  standalone: true,
  imports: [CommonModule, FormsModule, CurrencyPipe],
  templateUrl: './nota-fiscal-cadastro.html',
  styleUrls: ['./nota-fiscal-cadastro.css']
})
export class NotaFiscalCadastroComponent implements OnInit {
  pedidos: any[] = [];
  pedidoSelecionado: any = null;
  notas: NotaFiscal[] = [];
  mensagem: string = '';
  errorMessage: string = '';

  private baseUrl = 'http://localhost:5218/api';

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.carregarPedidos();
    this.carregarNotas();
  }

  carregarPedidos(): void {
    this.http.get<any[]>(`${this.baseUrl}/pedido`).subscribe({
      next: (data) => {
        console.log('📦 Pedidos recebidos da API:', data);

        this.pedidos = (data || []).filter(p => p.dsC_Status === 'Aberta');
      },
      error: (err) => {
        console.error(err);
        this.errorMessage = 'Erro ao carregar pedidos: ' + err.message;
      }
    });
  }

  carregarNotas(): void {
    this.http.get<NotaFiscal[]>(`${this.baseUrl}/notafiscal`).subscribe({
      next: (data) => {
        console.log('🧾 Notas fiscais recebidas:', data);
        this.notas = data || [];
      },
      error: (err) => {
        console.error(err);
        this.errorMessage = 'Erro ao carregar notas fiscais: ' + err.message;
      }
    });
  }

  selecionarPedido(pedido: any): void {
    this.pedidoSelecionado = pedido;
    this.mensagem = `🧾 Pedido do cliente ${pedido.dsC_Cliente} selecionado.`;
  }

  gerarNotaFiscal(): void {
    if (!this.pedidoSelecionado) {
      this.errorMessage = 'Selecione um pedido antes de gerar a nota fiscal.';
      return;
    }

    const novaNota: NotaFiscal = {
      NUM_NotaFiscal: 'NF-' + Math.floor(Math.random() * 10000),
      DSC_Status: 'Emitida',
      DT_Emissao: new Date(),
      NUM_ValorTotal: this.pedidoSelecionado.nuM_ValorTotal,
      Produtos: this.pedidoSelecionado.produtos,
      PedidoId: this.pedidoSelecionado.id,
    };

    this.http.post(`${this.baseUrl}/notafiscal`, novaNota).subscribe({
      next: () => {
        this.mensagem = '✅ Nota Fiscal criada com sucesso!';
        this.errorMessage = '';
        this.pedidoSelecionado = null;
        this.carregarNotas();
        this.carregarPedidos(); 
      },
      error: (err) => {
        console.error(err);
        this.errorMessage = '❌ Erro ao criar nota fiscal: ' + err.message;
      }
    });
  }
}
