using System.Collections.Generic;
using Scentify.Models;
using System.Linq;

namespace Scentify.Data
{
    public class CategoriaRepository
    {
        public List<Categoria> GetAll()
        {
            return MockDatabase.Categorias;
        }

        public int Insert(Categoria categoria)
        {
            int newId = MockDatabase.Categorias.Any() ? MockDatabase.Categorias.Max(c => c.CategoriaID) + 1 : 1;
            categoria.CategoriaID = newId;
            MockDatabase.Categorias.Add(categoria);
            return 1;
        }

        public Categoria GetById(int id)
        {
            return MockDatabase.Categorias.FirstOrDefault(c => c.CategoriaID == id);
        }

        public int Update(Categoria categoria)
        {
            var existing = MockDatabase.Categorias.FirstOrDefault(c => c.CategoriaID == categoria.CategoriaID);
            if (existing == null) return -1;
            existing.Nombre = categoria.Nombre;
            return 1;
        }

        public int Delete(int id)
        {
            var existing = MockDatabase.Categorias.FirstOrDefault(c => c.CategoriaID == id);
            if (existing == null) return -1;
            MockDatabase.Categorias.Remove(existing);
            return 1;
        }
    }
}
