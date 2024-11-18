namespace TGS_Genesys_Teck.Models
{
    public class ServicoVM
    {
        public int Id { get; set; }

        public string TipoServico { get; set; } = null!;

        public decimal Valor { get; set; }
    }
}
