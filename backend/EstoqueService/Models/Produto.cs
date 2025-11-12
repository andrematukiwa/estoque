using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EstoqueService.Models
{
    [Table("TB_PRODUTO")]
    public class Produto
    {
        [Column("ID_PRODUTO")]
        public int Id { get; set; }

        [Column("DSC_NAME")]
        public string DSC_Name { get; set; } = string.Empty;

        [Column("NUM_UNIT_PRICE")]
        public decimal NUM_UnitPrice { get; set; }

        [Column("QTD_STOCK")]
        public int QTD_STOCK { get; set; }

        [Column("ID_PEDIDO")]
        public int? PedidoId { get; set; }

        [JsonIgnore]
        public Pedido? Pedido { get; set; }

        [Column("ID_NOTA_FISCAL")]
        public int? NotaFiscalId { get; set; }

        [JsonIgnore]
        public NotaFiscal? NotaFiscal { get; set; }
    }
}
