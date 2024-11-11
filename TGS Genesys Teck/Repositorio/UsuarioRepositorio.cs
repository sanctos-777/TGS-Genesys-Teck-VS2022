﻿using TGS_Genesys_Teck.Models;
using Microsoft.EntityFrameworkCore;
using TGS_Genesys_Teck.ORM;

namespace TGS_Genesys_Teck.Repositorio
{
    public class UsuarioRepositorio
    {
        private TgsGenesysTeckContext _context;
        public UsuarioRepositorio(TgsGenesysTeckContext context)
        {
            _context = context;
        }
        public bool InserirUsuario(string nome, string email, string telefone, string senha, int tipoUsuario)
        {
            try
            {
                TbUsuario usuario = new TbUsuario();
                usuario.Nome = nome;
                usuario.Email = email;
                usuario.Telefone = telefone;
                usuario.Senha = senha;
                usuario.TipoUsuario = tipoUsuario;

                _context.TbUsuarios.Add(usuario);  // Supondo que _context.TbUsuarios seja o DbSet para a entidade de usuários
                _context.SaveChanges();

                return true;  // Retorna true para indicar sucesso
            }
            catch (Exception ex)
            {
                // Trate o erro ou faça um log do ex.Message se necessário
                return false;  // Retorna false para indicar falha
            }
        }

        public List<UsuarioVM> ListarUsuarios()
        {
            List<UsuarioVM> listFun = new List<UsuarioVM>();

            var listTb = _context.TbUsuarios.ToList();

            foreach (var item in listTb)
            {
                var usuarios = new UsuarioVM
                {
                    Id = item.IdUsuario,
                    Nome = item.Nome,
                    Email = item.Email,
                    Telefone = item.Telefone,
                    Senha = item.Senha,
                    TipoUsuario = item.TipoUsuario,
                };

                listFun.Add(usuarios);
            }

            return listFun;
        }

        public bool AtualizarUsuario(int id, string nome, string email, string telefone, string senha, int tipoUsuario)
        {
            try
            {
                // Busca o usuário pelo ID
                var usuario = _context.TbUsuarios.FirstOrDefault(u => u.IdUsuario == id);
                if (usuario != null)
                {
                    // Atualiza os dados do usuário
                    usuario.Nome = nome;
                    usuario.Email = email;
                    usuario.Telefone = telefone;
                    usuario.Senha = senha;  // Não se esqueça de criptografar a senha antes de atualizar
                    usuario.TipoUsuario = tipoUsuario;

                    // Salva as mudanças no banco de dados
                    _context.SaveChanges();

                    return true;  // Retorna verdadeiro se a atualização for bem-sucedida
                }
                else
                {
                    return false;  // Retorna falso se o usuário não foi encontrado
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar o usuário com ID {id}: {ex.Message}");
                return false;
            }
        }

        public bool ExcluirUsuario(int id)
        {
            try
            {
                // Busca o usuário pelo ID
                var usuario = _context.TbUsuarios.FirstOrDefault(u => u.IdUsuario == id);

                // Se o usuário não for encontrado, lança uma exceção personalizada
                if (usuario == null)
                {
                    throw new KeyNotFoundException("Usuário não encontrado.");
                }   


                // Remove o usuário do banco de dados
                _context.TbUsuarios.Remove(usuario);
                _context.SaveChanges();  // Isso pode lançar uma exceção se houver dependências

                // Se tudo correr bem, retorna true indicando sucesso
                return true;

            }
            catch (Exception ex)
            {
                // Aqui tratamos qualquer erro inesperado e logamos para depuração
                Console.WriteLine($"Erro ao excluir o usuário com ID {id}: {ex.Message}");

                // Relança a exceção para ser capturada pelo controlador
                throw new Exception($"Erro ao excluir o usuário: {ex.Message}");
            }
        }


    }

}
