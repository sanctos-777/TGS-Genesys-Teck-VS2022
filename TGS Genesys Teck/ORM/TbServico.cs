using System;
using System.Collections.Generic;

namespace TGS_Genesys_Teck.ORM;

public partial class TbServico
{
    public int IdServico { get; set; }

    public string TipoServico { get; set; } = null!;

    public decimal Valor { get; set; }

    public virtual ICollection<TbAtendimento> TbAtendimentos { get; set; } = new List<TbAtendimento>();
}
