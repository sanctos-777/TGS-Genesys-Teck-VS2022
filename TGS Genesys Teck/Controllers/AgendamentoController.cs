using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TGS_Genesys_Teck.Models;
using TGS_Genesys_Teck.ORM;
using TGS_Genesys_Teck.Repositorio;

namespace TGS_Genesys_Teck.Controllers
{
    public class AgendamentoController : Controller
    {
        private readonly TgsGenesysTeckContext _context;
        private readonly AgendamentoRepositorio _agendamentoRepositorio;

        // Construtor para injetar o reposit�rio
        public AgendamentoController(AgendamentoRepositorio agendamentoRepositorio, TgsGenesysTeckContext context)
        {
            _agendamentoRepositorio = agendamentoRepositorio;
            _context = context;
        }

        public IActionResult Index()
        {
            var servicos = new ServicoRepositorio(_context);
            var nomeServicos = servicos.ListarNomesServicos();
            if (nomeServicos != null && nomeServicos.Any())
            {
                // Cria a lista de SelectListItem
                var selectList = nomeServicos.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),  // O valor do item ser� o ID do usu�rio
                    Text = u.TipoServico             // O texto exibido ser� o nome do usu�rio
                }).ToList();

                // Passa a lista para o ViewBag para ser utilizada na view
                ViewBag.lstTipoServico = selectList;
            }

            // Chama o m�todo ListarNomesAgendamentos para obter a lista de usu�rios
            var usuarios = _agendamentoRepositorio.ListarNomesAgendamentos();

            if (usuarios != null && usuarios.Any())
            {
                // Cria a lista de SelectListItem
                var selectList = usuarios.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),  // O valor do item ser� o ID do usu�rio
                    Text = u.Nome             // O texto exibido ser� o nome do usu�rio
                }).ToList();

                // Passa a lista para o ViewBag para ser utilizada na view
                ViewBag.Usuarios = selectList;
            }

            var listaHorario = new List<SelectListItem>
            {
                new SelectListItem { Value = "8", Text = "08:00:00" },
                new SelectListItem { Value = "10", Text = "10:00:00" },
                new SelectListItem { Value = "13", Text = "13:00:00" },
                new SelectListItem { Value = "15", Text = "15:00:00" },
                new SelectListItem { Value = "17", Text = "17:00:00" },
                new SelectListItem { Value = "19", Text = "19:00:00" }
            };

            ViewBag.lstHorarios = listaHorario;

            var agendamentos = _agendamentoRepositorio.ListarAgendamentos();
            return View(agendamentos);
        }

        public IActionResult Cliente()
        {

            var servicos = new ServicoRepositorio(_context);
            var nomeServicos = servicos.ListarNomesServicos();
            if (nomeServicos != null && nomeServicos.Any())
            {
                // Cria a lista de SelectListItem
                var selectList = nomeServicos.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),  // O valor do item ser� o ID do usu�rio
                    Text = u.TipoServico             // O texto exibido ser� o nome do usu�rio
                }).ToList();

                // Passa a lista para o ViewBag para ser utilizada na view
                ViewBag.lstTipoServico = selectList;
            }

            // Chama o m�todo ListarNomesAgendamentos para obter a lista de usu�rios
            var usuarios = _agendamentoRepositorio.ListarNomesAgendamentos();

            if (usuarios != null && usuarios.Any())
            {
                // Cria a lista de SelectListItem
                var selectList = usuarios.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),  // O valor do item ser� o ID do usu�rio
                    Text = u.Nome             // O texto exibido ser� o nome do usu�rio
                }).ToList();

                // Passa a lista para o ViewBag para ser utilizada na view
                ViewBag.Usuarios = selectList;
            }

            var listaHorario = new List<SelectListItem>
            {
                new SelectListItem { Value = "8", Text = "08:00:00" },
                new SelectListItem { Value = "10", Text = "10:00:00" },
                new SelectListItem { Value = "13", Text = "13:00:00" },
                new SelectListItem { Value = "15", Text = "15:00:00" },
                new SelectListItem { Value = "17", Text = "17:00:00" },
                new SelectListItem { Value = "19", Text = "19:00:00" }
            };

            ViewBag.lstHorarios = listaHorario;

            var agendamentos = _agendamentoRepositorio.ListarAgendamentosCliente();
            return View(agendamentos);
        }

        public IActionResult Cadastro()
        {
            var servicos = new ServicoRepositorio(_context);
            var nomeServicos = servicos.ListarNomesServicos();
            if (nomeServicos != null && nomeServicos.Any())
            {
                // Cria a lista de SelectListItem
                var selectList = nomeServicos.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),  // O valor do item ser� o ID do usu�rio
                    Text = u.TipoServico             // O texto exibido ser� o nome do usu�rio
                }).ToList();

                // Passa a lista para o ViewBag para ser utilizada na view
                ViewBag.lstTipoServico = selectList;
            }

            return View();
        }

        public IActionResult InserirAgendamento(DateTime dtHoraAgendamento, DateOnly dataAgendamento, TimeOnly horario, int fkUsuarioId, int fkServicoId)
        {
            try
            {
                // Chama o m�todo do reposit�rio que realiza a inser��o no banco de dados
                var resultado = _agendamentoRepositorio.InserirAgendamento(dtHoraAgendamento, dataAgendamento, horario, fkUsuarioId, fkServicoId);

                // Verifica o resultado da inser��o
                if (resultado)
                {
                    return Json(new { success = true, message = "Atendimento inserido com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao inserir o agendamento. Tente novamente." });
                }
            }
            catch (Exception ex)
            {
                // Em caso de erro inesperado, captura e exibe o erro
                return Json(new { success = false, message = "Erro ao processar a solicita��o. Detalhes: " + ex.Message });
            }
        }

        public IActionResult InserirAgendamentoCliente(DateTime dtHoraAgendamento, DateOnly dataAtendimento, TimeOnly horario, int fkUsuarioId, int fkServicoId)
        {
            string id = Environment.GetEnvironmentVariable("USUARIO_ID");
            int IdUsuario = Int32.Parse(id);
            try
            {
                // Chama o m�todo do reposit�rio que realiza a inser��o no banco de dados
                var resultado = _agendamentoRepositorio.InserirAgendamento(dtHoraAgendamento, dataAtendimento, horario, IdUsuario, fkServicoId);

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

        public IActionResult AlterarAgendamento(int id, string data, int servico, TimeOnly horario)
        {

            var rs = _agendamentoRepositorio.AlterarAgendamento(id, data, servico, horario);
            if (rs)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public IActionResult ExcluirAgendamento(int id)
        {

            var rs = _agendamentoRepositorio.ExcluirAgendamento(id);
            return Json(new { success = rs });

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
