using System;
using System.Collections.Generic;
using Scentify.Models;
using System.Linq;

namespace Scentify.Data
{
    public class ResenaRepository
    {
        public List<Resena> GetAll()
        {
            return MockDatabase.Resenas;
        }

        public Resena GetById(int id)
        {
            return MockDatabase.Resenas.FirstOrDefault(r => r.ResenaID == id);
        }

        public int Insert(Resena resena)
        {
            int newId = MockDatabase.Resenas.Any() ? MockDatabase.Resenas.Max(r => r.ResenaID) + 1 : 1;
            resena.ResenaID = newId;
            resena.Fecha = DateTime.Now;

            var user = MockDatabase.Usuarios.FirstOrDefault(u => u.UsuarioID == resena.UsuarioID);
            resena.UsuarioNombre = user != null ? $"{user.Nombre} {user.Apellido1}" : "Cliente Anónimo";
            resena.Email = user?.Email ?? "";

            var prod = MockDatabase.Productos.FirstOrDefault(p => p.ProductoID == resena.ProductoID);
            resena.ProductoNombre = prod?.Nombre ?? "";

            MockDatabase.Resenas.Add(resena);
            return 1;
        }

        public int Update(Resena resena)
        {
            var existing = MockDatabase.Resenas.FirstOrDefault(r => r.ResenaID == resena.ResenaID);
            if (existing == null) return -1;

            existing.Calificacion = resena.Calificacion;
            existing.Comentario = resena.Comentario;
            existing.ProductoID = resena.ProductoID;
            existing.UsuarioID = resena.UsuarioID;

            var user = MockDatabase.Usuarios.FirstOrDefault(u => u.UsuarioID == resena.UsuarioID);
            existing.UsuarioNombre = user != null ? $"{user.Nombre} {user.Apellido1}" : "Cliente Anónimo";
            existing.Email = user?.Email ?? "";

            var prod = MockDatabase.Productos.FirstOrDefault(p => p.ProductoID == resena.ProductoID);
            existing.ProductoNombre = prod?.Nombre ?? "";

            return 1;
        }

        public int Delete(int id)
        {
            var existing = MockDatabase.Resenas.FirstOrDefault(r => r.ResenaID == id);
            if (existing == null) return -1;

            MockDatabase.Resenas.Remove(existing);
            return 1;
        }
    }
}