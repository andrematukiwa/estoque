import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router'; 
import { Produto } from '../../models/produto.model';

@Component({
  selector: 'app-produto-cadastro',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './produto-cadastro.html',
})
export class ProdutoCadastroComponent {
  novoProduto: Produto = { Id: 0, DSC_Name: '', NUM_UnitPrice: 0, QTD_STOCK: 0 };
  errorMessage: string = '';
  sucessoMessage: string = '';

  constructor(private http: HttpClient, private router: Router) { } 

  salvarProduto(): void {
    this.http.post('http://localhost:5218/api/produto', this.novoProduto).subscribe({
      next: () => {
        this.sucessoMessage = '✅ Produto cadastrado com sucesso!';
        this.novoProduto = { Id: 0, DSC_Name: '', NUM_UnitPrice: 0, QTD_STOCK: 0 };
      },
      error: (err) => {
        this.errorMessage = '❌ Erro ao cadastrar produto: ' + err.message;
      },
    });
  }

  irParaTelaInicial(): void {
    this.router.navigate(['/']); 
  }
}
