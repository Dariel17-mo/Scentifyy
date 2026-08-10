using System;
using System.Collections.Generic;
using Scentify.Models;
using System.Linq;

namespace Scentify.Data
{
    public class BitacoraTransaccionesRepository
    {
        public List<BitacoraTransacciones> Listar()
        {
            return MockDatabase.BitacoraTransacciones;
        }

        public List<BitacoraTransacciones> ConsultarPorUsuario(int? usuarioId)
        {
            if (usuarioId.HasValue)
            {
                return MockDatabase.BitacoraTransacciones.Where(b => b.UsuarioID == usuarioId.Value).ToList();
            }
            return MockDatabase.BitacoraTransacciones;
        }

        public void RegistrarBitacora(string tabla, string accion, int? usuarioID, string descripcion)
        {
            int newId = MockDatabase.BitacoraTransacciones.Any() ? MockDatabase.BitacoraTransacciones.Max(b => b.BitacoraTransaccionesID) + 1 : 1;
            MockDatabase.BitacoraTransacciones.Add(new BitacoraTransacciones
            {
                BitacoraTransaccionesID = newId,
                Tabla = tabla,
                Accion = accion,
                UsuarioID = usuarioID,
                Fecha = DateTime.Now,
                Descripcion = descripcion
            });
        }
    }
}