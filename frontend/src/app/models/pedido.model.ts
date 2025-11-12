import { Produto } from './produto.model';

export interface Pedido {
  Id?: number;
  DT_Pedido: Date;
  DSC_Cliente: string;
  NUM_ValorTotal: number;
  DSC_Status?: string;
  NUM_NotaFiscal?: string;
  Produtos: Produto[];
}
