using System;
using Scentify.Models;
using System.Linq;

namespace Scentify.Data
{
    public class BitacoraErroresRepository
    {
        public void RegistrarError(string mensajeError)
        {
            try
            {
                int newId = MockDatabase.BitacoraErrores.Any() ? MockDatabase.BitacoraErrores.Max(e => e.BitacoraErroresID) + 1 : 1;
                MockDatabase.BitacoraErrores.Add(new BitacoraErrores
                {
                    BitacoraErroresID = newId,
                    MensajeError = mensajeError,
                    Fecha = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar en bitácora mock: " + ex.Message);
            }
        }
    }
}
