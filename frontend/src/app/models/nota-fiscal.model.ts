import { Produto } from './produto.model';

export interface NotaFiscal {
  Id?: number;
  NUM_NotaFiscal?: string;
  DSC_Status?: string;
  DT_Emissao?: string | Date;
  NUM_ValorTotal?: number;
  Produtos: Produto[];
  PedidoId?: number | null;
}
