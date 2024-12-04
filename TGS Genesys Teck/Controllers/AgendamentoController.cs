using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using TGS_Genesys_Teck.Models;
using TGS_Genesys_Teck.Repositorio;

namespace TGS_Genesys_Teck.Controllers
{
    public class AgendamentoController : Controller
    {
        private readonly AgendamentoRepositorio _agendamentoRepositorio;

        // Construtor para injetar o repositório
        public AgendamentoController(AgendamentoRepositorio agendamentoRepositorio)
        {
            _agendamentoRepositorio = agendamentoRepositorio;
        }

        public IActionResult Index()
        {
            // Criar a lista de SelectListItems, onde o 'Value' será o 'Id' e o 'Text' será o 'TipoServico'
            List<SelectListItem> tipoServico = new List<SelectListItem>
              {
                  new SelectListItem { Value = "0", Text = "Consultoria em TI" },
                  new SelectListItem { Value = "1", Text = "Desenvolvimento de Software" },
                   new SelectListItem { Value = "2", Text = "Suporte Técnico" },
                  new SelectListItem { Value = "3", Text = "Treinamento Corporativo" },
                   new SelectListItem { Value = "4", Text = "Auditoria de Sistemas" },
                  new SelectListItem { Value = "5", Text = "Implementação de ERP" }
              };

            // Passar a lista para a View usando ViewBag
            ViewBag.lstTipoServico = new SelectList(tipoServico, "Value", "Text");


            // Chama o método ListarNomesAgendamentos para obter a lista de usuários
            var usuarios = _agendamentoRepositorio.ListarNomesAgendamentos();

            if (usuarios != null && usuarios.Any())
            {
                // Cria a lista de SelectListItem
                var selectList = usuarios.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),  // O valor do item será o ID do usuário
                    Text = u.Nome             // O texto exibido será o nome do usuário
                }).ToList();

                // Passa a lista para o ViewBag para ser utilizada na view
                ViewBag.Usuarios = selectList;
            }

            var agendamentos = _agendamentoRepositorio.ListarAgendamentos();
            return View(agendamentos);
        }

        public IActionResult Cliente()
        {
            return View();
        }

        public IActionResult Cadastro()
        {
            return View();
        }

        public IActionResult InserirAgendamento(DateTime dtHoraAgendamento, DateOnly dataAgendamento, TimeOnly horario, int fkUsuarioId, int fkServicoId)
        {
            try
            {
                // Chama o método do repositório que realiza a inserção no banco de dados
                var resultado = _agendamentoRepositorio.InserirAgendamento(dtHoraAgendamento, dataAgendamento, horario, fkUsuarioId, fkServicoId);

                // Verifica o resultado da inserção
                if (resultado)
                {
                    return Json(new { success = true, message = "Agendamento inserido com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao inserir o agendamento. Tente novamente." });
                }
            }
            catch (Exception ex)
            {
                // Em caso de erro inesperado, captura e exibe o erro
                return Json(new { success = false, message = "Erro ao processar a solicitação. Detalhes: " + ex.Message });
            }
        }
        public IActionResult AtualizarAgendamento(int id, DateTime dtHoraAgendamento, DateOnly dataAgendamento, TimeOnly horario, int fkUsuarioId, int fkServicoId)
        {
            try
            {
                // Chama o repositório para atualizar o atendimento
                var resultado = _agendamentoRepositorio.AtualizarAgendamento(id, dtHoraAgendamento, dataAgendamento, horario, fkUsuarioId, fkServicoId);

                if (resultado)
                {
                    return Json(new { success = true, message = "Agendamento atualizado com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao atualizar o agendamento. Verifique se o agendamento existe." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erro ao processar a solicitação. Detalhes: " + ex.Message });
            }
        }
        public IActionResult ExcluirAgendamento(int id)
        {
            try
            {
                // Chama o repositório para excluir o atendimento
                var resultado = _agendamentoRepositorio.ExcluirAgendamento(id);

                if (resultado)
                {
                    return Json(new { success = true, message = "Agendamento excluído com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Não foi possível excluir o agendamento. Verifique se ele está vinculado a outros registros no sistema." });
                }
            }
            catch (Exception ex)
            {
                // Captura qualquer erro e inclui a mensagem detalhada da exceção
                return Json(new { success = false, message = "Erro ao processar a solicitação. Detalhes: " + ex.Message });
            }
        }	

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ConsultarAgendamento(string data)
        {

            var agendamento = _agendamentoRepositorio.ConsultarAgendamento(data);

            if (agendamento != null)
            {
                return Json(agendamento);
            }
            else
            {
                return NotFound();
            }

        }
    }
}
