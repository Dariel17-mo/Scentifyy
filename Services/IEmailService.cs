// Services/IEmailService.cs
using System.Threading.Tasks;

namespace Scentify.Services
{
    public interface IEmailService
    {
        Task EnviarComprobanteAsync(string destinatarioEmail, string destinatarioNombre,
            int pedidoId, decimal monto, string metodoPago, string estado, string? codigoTransaccion);
            
        Task EnviarRecuperacionDeContrasenaAsync(string destinatarioEmail, string destinatarioNombre, string contrasenaActual);
    }
}
