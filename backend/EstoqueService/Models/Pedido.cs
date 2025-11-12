using System.ComponentModel.DataAnnotations.Schema;

namespace EstoqueService.Models
{
    [Table("TB_PEDIDO")] 
    public class Pedido
    {
        [Column("ID_PEDIDO")]
        public int Id { get; set; }

        [Column("DT_PEDIDO")]
        public DateTime DT_Pedido { get; set; }

        [Column("DSC_CLIENTE")]
        public string DSC_Cliente { get; set; } = string.Empty;

        [Column("NUM_VALOR_TOTAL")]
        public decimal NUM_ValorTotal { get; set; }

        public List<Produto>? Produtos { get; set; }
        [Column("NUM_NOTA_FISCAL")]
        public string NUM_NotaFiscal { get; set; } = string.Empty;

        [Column("DSC_STATUS")]
        public string DSC_Status { get; set; } = "Aberta"; 

    }
}
