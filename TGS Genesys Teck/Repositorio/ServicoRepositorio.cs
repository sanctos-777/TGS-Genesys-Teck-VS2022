﻿using TGS_Genesys_Teck.Models;
using TGS_Genesys_Teck.ORM;

namespace TGS_Genesys_Teck.Repositorio
{
    public class ServicoRepositorio
    {
        private TgsGenesysTeckContext _context;
        public ServicoRepositorio(TgsGenesysTeckContext context)
        {
            _context = context;
        }
        public bool InserirServico(string tipoServico, decimal valor)
        {
            try
            {
                TbServico servico = new TbServico();
                servico.TipoServico = tipoServico;
                servico.Valor = valor;


                _context.TbServicos.Add(servico);  // Supondo que _context.TbServicos seja o DbSet para a entidade de usuários
                _context.SaveChanges();

                return true;  // Retorna true para indicar sucesso
            }
            catch (Exception ex)
            {
                // Trate o erro ou faça um log do ex.Message se necessário
                return false;  // Retorna false para indicar falha
            }
        }

        public List<ServicoVM> ListarServicos()
        {
            List<ServicoVM> listSer = new List<ServicoVM>();

            var listTb = _context.TbServicos.ToList();

            foreach (var item in listTb)
            {
                var servicos = new ServicoVM
                {
                    Id = item.IdServico,
                    TipoServico = item.TipoServico,
                    Valor = item.Valor,
                };

                listSer.Add(servicos);
            }

            return listSer;
        }

        public bool AtualizarServico(int id, string tipoServico, decimal valor)
        {
            try
            {
                // Busca o Servico pelo ID
                var servico = _context.TbServicos.FirstOrDefault(u => u.IdServico == id);
                if (servico != null)
                {
                    // Atualiza os dados do servico
                    servico.TipoServico = tipoServico;
                    servico.Valor = valor;


                    // Salva as mudanças no banco de dados
                    _context.SaveChanges();

                    return true;  // Retorna verdadeiro se a atualização for bem-sucedida
                }
                else
                {
                    return false;  // Retorna falso se o servico não foi encontrado
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar o servico com ID {id}: {ex.Message}");
                return false;
            }
        }

        public bool ExcluirServico(int id)
        {
            try
            {
                // Busca o servico pelo ID
                var servico = _context.TbServicos.FirstOrDefault(u => u.IdServico == id);

                // Se o usuário não for encontrado, lança uma exceção personalizada
                if (servico == null)
                {
                    throw new KeyNotFoundException("Servico não encontrado.");
                }


                // Remove o usuário do banco de dados
                _context.TbServicos.Remove(servico);
                _context.SaveChanges();  // Isso pode lançar uma exceção se houver dependências

                // Se tudo correr bem, retorna true indicando sucesso
                return true;

            }
            catch (Exception ex)
            {
                // Aqui tratamos qualquer erro inesperado e logamos para depuração
                Console.WriteLine($"Erro ao excluir o servico com ID {id}: {ex.Message}");

                // Relança a exceção para ser capturada pelo controlador
                throw new Exception($"Erro ao excluir o servico: {ex.Message}");
            }
        }
    }
}