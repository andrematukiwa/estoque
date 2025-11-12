using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EstoqueService.Models
{
    [Table("TB_NOTA_FISCAL")]
    public class NotaFiscal
    {
        [Column("ID_NOTA_FISCAL")]
        public int Id { get; set; }

        [Column("NUM_NOTA_FISCAL")]
        public string NUM_NotaFiscal { get; set; } = string.Empty;

        [Column("DSC_STATUS")]
        public string DSC_Status { get; set; } = "Aberta";

        [Column("DT_EMISSAO")]
        public DateTime DT_Emissao { get; set; } = DateTime.Now;

        [Column("NUM_VALOR_TOTAL")]
        public decimal NUM_ValorTotal { get; set; }

        public List<Produto> Produtos { get; set; } = new();

        public int? PedidoId { get; set; }

        [JsonIgnore]
        public Pedido? Pedido { get; set; }
    }
}
