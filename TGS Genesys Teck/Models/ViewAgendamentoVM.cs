namespace TGS_Genesys_Teck.Models
{
    public class ViewAgendamentoVM
    {
        public int IdAgendamento { get; set; }

        public DateTime DtHoraAgendamento { get; set; }

        public DateOnly DataAgendamento { get; set; }

        public TimeOnly Horario { get; set; }

        public string TipoServico { get; set; } = null!;

        public decimal Valor { get; set; }

        public string Nome { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Telefone { get; set; } = null!;
    }
}
