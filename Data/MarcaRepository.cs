using System.Collections.Generic;
using Scentify.Models;
using System.Linq;

namespace Scentify.Data
{
    public class MarcaRepository
    {
        public List<Marca> GetAll()
        {
            return MockDatabase.Marcas;
        }

        public int Insert(Marca marca)
        {
            int newId = MockDatabase.Marcas.Any() ? MockDatabase.Marcas.Max(m => m.MarcaID) + 1 : 1;
            marca.MarcaID = newId;
            MockDatabase.Marcas.Add(marca);
            return 1;
        }

        public Marca GetById(int id)
        {
            return MockDatabase.Marcas.FirstOrDefault(m => m.MarcaID == id);
        }

        public int Update(Marca marca)
        {
            var existing = MockDatabase.Marcas.FirstOrDefault(m => m.MarcaID == marca.MarcaID);
            if (existing == null) return -1;
            existing.Nombre = marca.Nombre;
            return 1;
        }

        public int Delete(int id)
        {
            var existing = MockDatabase.Marcas.FirstOrDefault(m => m.MarcaID == id);
            if (existing == null) return -1;
            MockDatabase.Marcas.Remove(existing);
            return 1;
        }
    }
}
