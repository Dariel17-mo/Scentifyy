using System.Collections.Generic;
using Scentify.Models;
using System.Linq;
using System;

namespace Scentify.Data
{
    public class UsuarioRepository
    {
        public List<Usuario> GetAll()
        {
            return MockDatabase.Usuarios;
        }

        public int Insert(Usuario usuario)
        {
            if (MockDatabase.Usuarios.Any(u => u.Email.Equals(usuario.Email, StringComparison.OrdinalIgnoreCase)))
            {
                return -1; // Email already exists
            }

            int newId = MockDatabase.Usuarios.Any() ? MockDatabase.Usuarios.Max(u => u.UsuarioID) + 1 : 1;
            usuario.UsuarioID = newId;
            usuario.FechaRegistro = DateTime.Now;
            usuario.Activo = true;

            MockDatabase.Usuarios.Add(usuario);
            return 1; // Success
        }

        public Usuario GetById(int id)
        {
            return MockDatabase.Usuarios.FirstOrDefault(u => u.UsuarioID == id);
        }

        public int Update(Usuario usuario)
        {
            var existing = MockDatabase.Usuarios.FirstOrDefault(u => u.UsuarioID == usuario.UsuarioID);
            if (existing == null) return -1;

            existing.Identificacion = usuario.Identificacion;
            existing.Nombre = usuario.Nombre;
            existing.Apellido1 = usuario.Apellido1;
            existing.Apellido2 = usuario.Apellido2;
            existing.FechaNacimiento = usuario.FechaNacimiento;
            existing.DocumentoIdentidad = usuario.DocumentoIdentidad;
            existing.Rol = usuario.Rol;
            existing.Email = usuario.Email;
            existing.Contrasena = usuario.Contrasena;
            existing.Activo = usuario.Activo;

            return 1; // Success
        }

        public int Delete(int id)
        {
            var existing = MockDatabase.Usuarios.FirstOrDefault(u => u.UsuarioID == id);
            if (existing == null) return -1;

            MockDatabase.Usuarios.Remove(existing);
            return 1; // Success
        }

        public Usuario GetByEmail(string email)
        {
            return MockDatabase.Usuarios.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public Usuario ValidarLogin(string email, string contrasena, out int respuesta)
        {
            // Bypassing login credentials:
            // If email or password is empty, OR they match Admin123@gmail.com / Admin123, OR they are valid in the list, let them in.
            Usuario usuario = null;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(contrasena))
            {
                usuario = MockDatabase.Usuarios.FirstOrDefault(u => u.Rol == "Admin");
                respuesta = 1;
                return usuario;
            }

            usuario = MockDatabase.Usuarios.FirstOrDefault(u => 
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && 
                u.Contrasena == contrasena);

            if (usuario == null && email.Equals("Admin123@gmail.com", StringComparison.OrdinalIgnoreCase) && contrasena == "Admin123")
            {
                // Ensure the admin exists and return it
                usuario = MockDatabase.Usuarios.FirstOrDefault(u => u.Email.Equals("Admin123@gmail.com", StringComparison.OrdinalIgnoreCase));
                if (usuario == null)
                {
                    usuario = new Usuario
                    {
                        UsuarioID = 1,
                        Identificacion = "111111111",
                        Nombre = "Admin",
                        Apellido1 = "Scentify",
                        Apellido2 = "Local",
                        FechaNacimiento = new DateTime(1990, 1, 1),
                        DocumentoIdentidad = "111111111",
                        Rol = "Admin",
                        Email = "Admin123@gmail.com",
                        Contrasena = "Admin123",
                        Activo = true
                    };
                    MockDatabase.Usuarios.Add(usuario);
                }
            }

            if (usuario != null)
            {
                respuesta = 1; // Valid login
            }
            else
            {
                // If it doesn't match, let's just create a dynamic user or log them in as admin to support credential-free access!
                usuario = MockDatabase.Usuarios.FirstOrDefault(u => u.Rol == "Admin") ?? new Usuario
                {
                    UsuarioID = 1,
                    Nombre = "Admin",
                    Email = "Admin123@gmail.com",
                    Contrasena = "Admin123",
                    Rol = "Admin",
                    Activo = true
                };
                respuesta = 1;
            }

            return usuario;
        }

        public List<Usuario> ListarTodos()
        {
            return MockDatabase.Usuarios;
        }
    }
}
