using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using TGS_Genesys_Teck.Models;
using TGS_Genesys_Teck.Repositorio;

namespace TGS_Genesys_Teck.Controllers
{
    public class AgendamentoController : Controller
    {
        private readonly AtendimentoRepositorio _atendimentoRepositorio;

        // Construtor para injetar o reposit�rio
        public AgendamentoController(AtendimentoRepositorio agendamentoRepositorio)
        {
            _atendimentoRepositorio = agendamentoRepositorio;
        }

        public IActionResult Index()
        {
            // Criar a lista de SelectListItems, onde o 'Value' ser� o 'Id' e o 'Text' ser� o 'TipoServico'
            List<SelectListItem> tipoServico = new List<SelectListItem>
              {
                  new SelectListItem { Value = "0", Text = "Consultoria em TI" },
                  new SelectListItem { Value = "1", Text = "Desenvolvimento de Software" },
                   new SelectListItem { Value = "2", Text = "Suporte T�cnico" },
                  new SelectListItem { Value = "3", Text = "Treinamento Corporativo" },
                   new SelectListItem { Value = "4", Text = "Auditoria de Sistemas" },
                  new SelectListItem { Value = "5", Text = "Implementa��o de ERP" }
              };

            // Passar a lista para a View usando ViewBag
            ViewBag.lstTipoServico = new SelectList(tipoServico, "Value", "Text");
            var atendimentos = _atendimentoRepositorio.ListarAtendimentos();
            return View(atendimentos);
        }

        public IActionResult Cliente()
        {
            return View();
        }

        public IActionResult Cadastro()
        {
            return View();
        }

        public IActionResult InserirAtendimento(DateTime dtHoraAgendamento, DateOnly dataAtendimento, TimeOnly horario, int fkUsuarioId, int fkServicoId)
        {
            try
            {
                // Chama o m�todo do reposit�rio que realiza a inser��o no banco de dados
                var resultado = _atendimentoRepositorio.InserirAtendimento(dtHoraAgendamento, dataAtendimento, horario, fkUsuarioId, fkServicoId);

                // Verifica o resultado da inser��o
                if (resultado)
                {
                    return Json(new { success = true, message = "Atendimento inserido com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao inserir o atendimento. Tente novamente." });
                }
            }
            catch (Exception ex)
            {
                // Em caso de erro inesperado, captura e exibe o erro
                return Json(new { success = false, message = "Erro ao processar a solicita��o. Detalhes: " + ex.Message });
            }
        }
        public IActionResult AtualizarAtendimento(int id, DateTime dtHoraAgendamento, DateOnly dataAtendimento, TimeOnly horario, int fkUsuarioId, int fkServicoId)
        {
            try
            {
                // Chama o reposit�rio para atualizar o atendimento
                var resultado = _atendimentoRepositorio.AtualizarAtendimento(id, dtHoraAgendamento, dataAtendimento, horario, fkUsuarioId, fkServicoId);

                if (resultado)
                {
                    return Json(new { success = true, message = "Atendimento atualizado com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao atualizar o atendimento. Verifique se o atendimento existe." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erro ao processar a solicita��o. Detalhes: " + ex.Message });
            }
        }
        public IActionResult ExcluirAtendimento(int id)
        {
            try
            {
                // Chama o reposit�rio para excluir o atendimento
                var resultado = _atendimentoRepositorio.ExcluirAtendimento(id);

                if (resultado)
                {
                    return Json(new { success = true, message = "Atendimento exclu�do com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "N�o foi poss�vel excluir o atendimento. Verifique se ele est� vinculado a outros registros no sistema." });
                }
            }
            catch (Exception ex)
            {
                // Captura qualquer erro e inclui a mensagem detalhada da exce��o
                return Json(new { success = false, message = "Erro ao processar a solicita��o. Detalhes: " + ex.Message });
            }
        }	

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ConsultarAgendamento(string data)
        {

            var agendamento = _atendimentoRepositorio.ConsultarAgendamento(data);

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
