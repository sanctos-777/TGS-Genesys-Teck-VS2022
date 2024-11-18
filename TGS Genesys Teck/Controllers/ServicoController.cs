using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using TGS_Genesys_Teck.Models;
using TGS_Genesys_Teck.Repositorio;

namespace TGS_Genesys_Teck.Controllers
{
    public class ServicoController : Controller
    {
        private readonly ServicoRepositorio _servicoRepositorio;
        private readonly ILogger<ServicoController> _logger;

        public ServicoController(ServicoRepositorio servicoRepositorio, ILogger<ServicoController> logger)
        {
            _servicoRepositorio = servicoRepositorio;
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<SelectListItem> tipoServico = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "Montagem de computador" },
                new SelectListItem { Value = "1", Text = "Configura��o de software" },
                new SelectListItem { Value = "2", Text = "Formata��o" },
                new SelectListItem { Value = "3", Text = "Venda de hardware sob encomenda" },
                new SelectListItem { Value = "4", Text = "Assist�ncia remota" },
                new SelectListItem { Value = "5", Text = "Atendimento � domicilio   " }
            };

            // Passar a lista para a View usando ViewBag
            ViewBag.lstTipoServico = new SelectList(tipoServico, "Value", "Text");

            var Servicos = _servicoRepositorio.ListarServicos();
            return View(Servicos);
        }

        public IActionResult InserirServico(string tipoServico, decimal valor)
        {
            try
            {
                // Chama o m�todo do reposit�rio que realiza a inser��o no banco de dados
                var resultado = _servicoRepositorio.InserirServico(tipoServico, valor);

                // Verifica o resultado da inser��o
                if (resultado)
                {
                    // Se o resultado for verdadeiro, significa que o servico foi inserido com sucesso
                    return Json(new { success = true, message = "Servico inserido com sucesso!" });
                }
                else
                {
                    // Se o resultado for falso, significa que houve um erro ao tentar inserir o servico
                    return Json(new { success = false, message = "Erro ao inserir o servico. Tente novamente." });
                }
            }
            catch (Exception ex)
            {
                // Em caso de erro inesperado, captura e exibe o erro
                return Json(new { success = false, message = "Erro ao processar a solicita��o. Detalhes: " + ex.Message });
            }
        }
        public IActionResult AtualizarServico(int id, string tipoServico, decimal valor)
        {
            try
            {
                // Chama o reposit�rio para atualizar o servico
                var resultado = _servicoRepositorio.AtualizarServico(id, tipoServico, valor);

                if (resultado)
                {
                    return Json(new { success = true, message = "Servi�o atualizado com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao atualizar o servi�o. Verifique se o servi�o existe." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erro ao processar a solicita��o. Detalhes: " + ex.Message });
            }
        }
        public IActionResult ExcluirServico(int id)
        {
            try
            {
                // Chama o reposit�rio para excluir o usu�rio
                var resultado = _servicoRepositorio.ExcluirServico(id);

                if (resultado)
                {
                    return Json(new { success = true, message = "Usu�rio exclu�do com sucesso!" });
                }
                else
                {
                    // Se o resultado for falso, voc� pode fornecer uma mensagem mais espec�fica.
                    return Json(new { success = false, message = "N�o foi poss�vel excluir o usu�rio. Verifique se ele est� vinculado a outros registros no sistema." });
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
    }
}
