using System.Globalization;
using TGS_Genesys_Teck.Models;
using TGS_Genesys_Teck.ORM;

namespace TGS_Genesys_Teck.Repositorio
{
    public class AgendamentoRepositorio
    {
        private TgsGenesysTeckContext _context;
        private List<ViewAgendamento> listAgendamentos;

        public AgendamentoRepositorio(TgsGenesysTeckContext context)
        {
            _context = context;
        }
        // Método para inserir um novo agendamento
        // Método para inserir um novo agendamento
        public bool InserirAgendamento(DateTime dtHoraAgendamento, DateOnly dataAgendamento, TimeOnly horario, int fkUsuarioId, int fkServicoId)
        {
            try
            {
                // Criando uma instância do modelo AtendimentoVM
                var agendamento = new TbAgendamento
                {
                    DtHoraAgendamento = dtHoraAgendamento,
                    DataAgendamento = dataAgendamento,
                    Horario = horario,
                    IdUsuario = fkUsuarioId,
                    IdServico = fkServicoId
                };

                // Adicionando o atendimento ao contexto
                _context.TbAgendamentos.Add(agendamento);
                _context.SaveChanges(); // Persistindo as mudanças no banco de dados

                return true; // Retorna true indicando sucesso
            }
            catch (Exception ex)
            {
                // Em caso de erro, pode-se logar a exceção (ex.Message)
                return false; // Retorna false em caso de erro
            }
        }

        public bool AlterarAgendamento(int id, string data, int servico, TimeOnly horario)
        {
            try
            {
                TbAgendamento agt = _context.TbAgendamentos.Find(id);
                DateOnly dtHoraAgendamento;
                if (agt != null)
                {
                    agt.IdAgendamento = id;
                    if (data != null)
                    {
                        if (DateOnly.TryParse(data, out dtHoraAgendamento))
                        {
                            agt.DataAgendamento = dtHoraAgendamento;
                        }
                    }

                    // Corrigido a verificação do tipo TimeOnly
                    if (horario != TimeOnly.MinValue)  // Verificando se o horário não é o valor padrão
                    {
                        agt.Horario = horario;
                    }

                    agt.IdServico = servico;
                    _context.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        


        // Método para listar todos os agendamentos
        public List<ViewAgendamentoVM> ListarAgendamentos()
        {
            List<ViewAgendamentoVM> listAgendamentos = new List<ViewAgendamentoVM>();

            // Recuperando todos os atendimentos do DbSet
            var listTb = _context.ViewAgendamentos.ToList();

            // Convertendo os atendimentos de TbAtendimento para AtendimentoVM
            foreach (var item in listTb)
            {
                var agendamento = new ViewAgendamentoVM
                {
                    IdAgendamento = item.IdAgendamento,
                    DtHoraAgendamento = item.DtHoraAgendamento,
                    DataAgendamento = item.DataAgendamento,
                    Horario = item.Horario,
                    TipoServico = item.TipoServico,
                    Valor = item.Valor,
                    Nome = item.Nome,
                    Email = item.Email,
                    Telefone = item.Telefone,

                };

                listAgendamentos.Add(agendamento);
            }

            return listAgendamentos;
        }

        public List<ViewAgendamentoVM> ListarAgendamentosCliente()
        {
            // Obtendo o ID do usuário a partir da variável de ambiente
            string nome = Environment.GetEnvironmentVariable("USUARIO_NOME");

            List<ViewAgendamentoVM> listAgendamentos = new List<ViewAgendamentoVM>();

            // Recuperando todos os agendamentos que correspondem ao ID do usuário
            var listTb = _context.ViewAgendamentos.Where(x => x.Nome == nome).ToList();

            // Convertendo cada agendamento para ViewAgendamentoVM
            foreach (var item in listTb)
            {
                var agendamento = new ViewAgendamentoVM
                {
                    IdAgendamento = item.IdAgendamento,
                    DtHoraAgendamento = item.DtHoraAgendamento,
                    DataAgendamento = item.DataAgendamento,
                    Horario = item.Horario,
                    TipoServico = item.TipoServico,
                    Valor = item.Valor,
                    Nome = item.Nome,
                    Email = item.Email,
                    Telefone = item.Telefone,
                };

                listAgendamentos.Add(agendamento);
            }

            return listAgendamentos;
        }

        // Método para atualizar um Agendamento
        public bool AtualizarAgendamento(int id, DateTime dtHoraAgendamento, DateOnly dataAgendamento, TimeOnly horario, int fkUsuarioId, int fkServicoId)
        {
            try
            {
                // Buscando o agendamento pelo ID
                var agendamento = _context.TbAgendamentos.FirstOrDefault(a => a.IdAgendamento == id);
                if (agendamento != null)
                {
                    // Atualizando os dados do atendimento
                    agendamento.DtHoraAgendamento = dtHoraAgendamento;
                    agendamento.DataAgendamento = dataAgendamento;
                    agendamento.Horario = horario;
                    agendamento.IdUsuario = fkUsuarioId;
                    agendamento.IdServico = fkServicoId;

                    // Salvando as mudanças
                    _context.SaveChanges();

                    return true; // Retorna true se a atualização foi bem-sucedida
                }
                else
                {
                    return false; // Retorna false se o agendamento não for encontrado
                }
            }
            catch (Exception ex)
            {
                // Log de exceção (opcional)
                Console.WriteLine($"Erro ao atualizar o agendamento com ID {id}: {ex.Message}");
                return false; // Retorna false em caso de erro
            }
        }

        // Método para excluir um agendamento
        public bool ExcluirAgendamento(int id)
        {
            try
            {


                var agt = _context.TbAgendamentos.Where(a => a.IdAgendamento == id).FirstOrDefault();
                if (agt != null)
                {
                    _context.TbAgendamentos.Remove(agt);

                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception)
            {

                return false;
            }
        }


        public List<AgendamentoVM> ConsultarAgendamento(string datap)
        {
            DateOnly data = DateOnly.ParseExact(datap, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            string dataFormatada = data.ToString("yyyy-MM-dd"); // Formato desejado: "yyyy-MM-dd"
            Console.WriteLine(dataFormatada);

            try
            {
                // Consulta ao banco de dados, convertendo para o tipo AtendimentoVM
                var ListaAgendamento = _context.TbAgendamentos
                    .Where(a => a.DataAgendamento == DateOnly.Parse(dataFormatada))
                    .Select(a => new AgendamentoVM
                    {
                        // Mapear as propriedades de TbAtendimento para AtendimentoVM
                        // Suponha que TbAtendimento tenha as propriedades Id, DataAtendimento, e outras:
                        Id = a.IdAgendamento,
                        DtHoraAgendamento = a.DtHoraAgendamento,
                        DataAgendamento = DateOnly.Parse(dataFormatada),
                        Horario = a.Horario,
                        IdServico = a.IdServico,
                        IdUsuario = a.IdUsuario
                    })
                    .ToList(); // Converte para uma lista

                return ListaAgendamento;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao consultar agendamentos: {ex.Message}");
                return new List<AgendamentoVM>(); // Retorna uma lista vazia em caso de erro
            }
        }

        public List<UsuarioVM> ListarNomesAgendamentos()
        {
            // Lista para armazenar os usuários com apenas Id e Nome
            List<UsuarioVM> listFun = new List<UsuarioVM>();

            // Obter apenas os campos Id e Nome da tabela TbUsuarios
            var listTb = _context.TbUsuarios
                                 .Select(u => new UsuarioVM
                                 {
                                     Id = u.IdUsuario,
                                     Nome = u.Nome
                                 })
                                 .ToList();

            // Retorna a lista já com os campos filtrados
            return listTb;
        }
    }
}