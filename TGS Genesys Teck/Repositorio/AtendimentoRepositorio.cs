using System.Globalization;
using TGS_Genesys_Teck.Models;
using TGS_Genesys_Teck.ORM;

namespace TGS_Genesys_Teck.Repositorio
{
    public class AtendimentoRepositorio
    {
        private TgsGenesysTeckContext _context;
        public AtendimentoRepositorio(TgsGenesysTeckContext context)
        {
            _context = context;
        }
        // Método para inserir um novo atendimento
        public bool InserirAtendimento(DateTime dtHoraAgendamento, DateOnly dataAtendimento, TimeOnly horario, int fkUsuarioId, int fkServicoId)
        {
            try
            {
                // Criando uma instância do modelo AtendimentoVM
                var atendimento = new TbAtendimento
                {
                    DtHoraAgendamento = dtHoraAgendamento,
                    DataAtendimento = dataAtendimento,
                    Horario = horario,
                    IdUsuario = fkUsuarioId,
                    IdServico = fkServicoId
                };

                // Adicionando o atendimento ao contexto
                _context.TbAtendimentos.Add(atendimento);
                _context.SaveChanges(); // Persistindo as mudanças no banco de dados

                return true; // Retorna true indicando sucesso
            }
            catch (Exception ex)
            {
                // Em caso de erro, pode-se logar a exceção (ex.Message)
                return false; // Retorna false em caso de erro
            }
        }

        // Método para listar todos os atendimentos
        public List<ViewAtendimentoVM> ListarAtendimentos()
        {
            List<ViewAtendimentoVM> listAtendimentos = new List<ViewAtendimentoVM>();

            // Recuperando todos os atendimentos do DbSet
            var listTb = _context.ViewAgendamentos.ToList();

            // Convertendo os atendimentos de TbAtendimento para AtendimentoVM
            foreach (var item in listTb)
            {
                var atendimento = new ViewAtendimentoVM
                {
                    IdAtendimento = item.IdAtendimento,
                    DtHoraAgendamento = item.DtHoraAgendamento,
                    DataAtendimento = item.DataAtendimento,
                    Horario = item.Horario,
                    TipoServico = item.TipoServico,
                    Valor = item.Valor,
                    Nome = item.Nome,
                    Email = item.Email,
                    Telefone = item.Telefone,

                };

                listAtendimentos.Add(atendimento);
            }

            return listAtendimentos;
        }

        // Método para atualizar um atendimento
        public bool AtualizarAtendimento(int id, DateTime dtHoraAgendamento, DateOnly dataAtendimento, TimeOnly horario, int fkUsuarioId, int fkServicoId)
        {
            try
            {
                // Buscando o atendimento pelo ID
                var atendimento = _context.TbAtendimentos.FirstOrDefault(a => a.IdAtendimento == id);
                if (atendimento != null)
                {
                    // Atualizando os dados do atendimento
                    atendimento.DtHoraAgendamento = dtHoraAgendamento;
                    atendimento.DataAtendimento = dataAtendimento;
                    atendimento.Horario = horario;
                    atendimento.IdUsuario = fkUsuarioId;
                    atendimento.IdServico = fkServicoId;

                    // Salvando as mudanças
                    _context.SaveChanges();

                    return true; // Retorna true se a atualização foi bem-sucedida
                }
                else
                {
                    return false; // Retorna false se o atendimento não for encontrado
                }
            }
            catch (Exception ex)
            {
                // Log de exceção (opcional)
                Console.WriteLine($"Erro ao atualizar o atendimento com ID {id}: {ex.Message}");
                return false; // Retorna false em caso de erro
            }
        }

        // Método para excluir um atendimento
        public bool ExcluirAtendimento(int id)
        {
            try
            {
                // Buscando o atendimento pelo ID
                var atendimento = _context.TbAtendimentos.FirstOrDefault(a => a.IdAtendimento == id);

                if (atendimento == null)
                {
                    throw new KeyNotFoundException("Atendimento não encontrado.");
                }

                // Removendo o atendimento
                _context.TbAtendimentos.Remove(atendimento);
                _context.SaveChanges(); // Persistindo a exclusão

                return true; // Retorna true indicando sucesso
            }
            catch (Exception ex)
            {
                // Em caso de erro, pode-se logar a exceção
                Console.WriteLine($"Erro ao excluir o atendimento com ID {id}: {ex.Message}");

                // Lançando a exceção novamente para ser tratada pelo controlador
                throw new Exception($"Erro ao excluir o atendimento: {ex.Message}");
            }
        }

        public List<AgendamentoVM> ConsultarAgendamento(string datap)
        {
            DateOnly data = DateOnly.ParseExact(datap, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string dataFormatada = data.ToString("yyyy-MM-dd"); // Formato desejado: "yyyy-MM-dd"

            try
            {
                // Consulta ao banco de dados, convertendo para o tipo AtendimentoVM
                var ListaAgendamento = _context.TbAtendimentos
                    .Where(a => a.DataAtendimento == DateOnly.Parse(dataFormatada))
                    .Select(a => new AgendamentoVM
                    {
                        // Mapear as propriedades de TbAtendimento para AtendimentoVM
                        // Suponha que TbAtendimento tenha as propriedades Id, DataAtendimento, e outras:
                        Id = a.IdUsuario,
                        DtHoraAgendamento = a.DtHoraAgendamento,
                        DataAtendimento = DateOnly.Parse(dataFormatada),
                        Horario = a.Horario,
                        IdUsuario = a.IdUsuario,
                        IdServico = a.IdServico
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
    }
}