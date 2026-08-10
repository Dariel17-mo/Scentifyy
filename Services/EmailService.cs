// Services/EmailService.cs
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Scentify.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task EnviarComprobanteAsync(string destinatarioEmail, string destinatarioNombre,
            int pedidoId, decimal monto, string metodoPago, string estado, string? codigoTransaccion)
        {
            var section = _config.GetSection("Email");
            string fromName = section["FromName"];
            string from = section["From"];
            string smtp = section["Smtp"];
            int port = int.Parse(section["Port"]);
            string user = section["User"];
            string pass = section["Pass"];

            string asunto = $"Comprobante de compra • Pedido #{pedidoId}";
            string cuerpoHtml = $@"
<!doctype html>
<html lang='es'>
  <body style='font-family:Segoe UI, Arial, sans-serif;'>
    <div style='max-width:600px;margin:auto;border:1px solid #eee;border-radius:10px;padding:24px'>
      <h2 style='color:#4b0082;margin:0 0 8px'>Scentify</h2>
      <p style='margin:0 0 16px;color:#555'>Hola <strong>{WebUtility.HtmlEncode(destinatarioNombre)}</strong>,</p>
      <p style='margin:0 0 16px;color:#555'>Tu pago se registró correctamente. Estos son los detalles del comprobante:</p>
      <table style='width:100%;border-collapse:collapse'>
        <tr>
          <td style='padding:8px;border-bottom:1px solid #eee'>Pedido</td>
          <td style='padding:8px;border-bottom:1px solid #eee'>#{pedidoId}</td>
        </tr>
        <tr>
          <td style='padding:8px;border-bottom:1px solid #eee'>Monto</td>
          <td style='padding:8px;border-bottom:1px solid #eee'>₡{monto:N2}</td>
        </tr>
        <tr>
          <td style='padding:8px;border-bottom:1px solid #eee'>Método de pago</td>
          <td style='padding:8px;border-bottom:1px solid #eee'>{WebUtility.HtmlEncode(metodoPago)}</td>
        </tr>
        <tr>
          <td style='padding:8px;border-bottom:1px solid #eee'>Estado</td>
          <td style='padding:8px;border-bottom:1px solid #eee'>{WebUtility.HtmlEncode(estado)}</td>
        </tr>
        {(string.IsNullOrWhiteSpace(codigoTransaccion) ? "" : $@"
        <tr>
          <td style='padding:8px;border-bottom:1px solid #eee'>Código de transacción</td>
          <td style='padding:8px;border-bottom:1px solid #eee'>{WebUtility.HtmlEncode(codigoTransaccion)}</td>
        </tr>")}
      </table>
      <p style='margin:16px 0 0;color:#555'>Gracias por tu compra, te agradecemos tu preferencia.</p>
      <p style='margin:4px 0 0;color:#999;font-size:12px'>Este es un comprobante automático, no respondas a este correo.</p>
    </div>
  </body>
</html>";

            using var msg = new MailMessage
            {
                From = new MailAddress(from, fromName, Encoding.UTF8),
                Subject = asunto,
                SubjectEncoding = Encoding.UTF8,
                Body = cuerpoHtml,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };
            msg.To.Add(new MailAddress(destinatarioEmail, destinatarioNombre, Encoding.UTF8));

            using var client = new SmtpClient(smtp, port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(user, pass),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            // Enviar (async)
            await client.SendMailAsync(msg);
        }

        public async Task EnviarRecuperacionDeContrasenaAsync(string destinatarioEmail, string destinatarioNombre, string contrasenaActual)
        {
            var section = _config.GetSection("Email");
            string fromName = section["FromName"];
            string from = section["From"];
            string smtp = section["Smtp"];
            int port = int.Parse(section["Port"]);
            string user = section["User"];
            string pass = section["Pass"];

            string asunto = "Recuperación de Contraseña • Scentify";
            string cuerpoHtml = $@"
<!doctype html>
<html lang='es'>
  <body style='font-family:Segoe UI, Arial, sans-serif;'>
    <div style='max-width:600px;margin:auto;border:1px solid #eee;border-radius:10px;padding:24px'>
      <h2 style='color:#C9A84C;margin:0 0 8px'>Scentify</h2>
      <p style='margin:0 0 16px;color:#555'>Hola <strong>{WebUtility.HtmlEncode(destinatarioNombre)}</strong>,</p>
      <p style='margin:0 0 16px;color:#555'>Hemos recibido una solicitud para recuperar tu contraseña de acceso.</p>
      
      <div style='background:rgba(201,168,76,0.1); padding:20px; text-align:center; border-radius:8px; margin: 20px 0;'>
         <p style='margin:0; font-size:12px; color:#666; text-transform:uppercase; letter-spacing:2px;'>Tu contraseña actual es:</p>
         <h3 style='margin:10px 0 0; color:#080808; font-size:24px; letter-spacing:4px;'>{WebUtility.HtmlEncode(contrasenaActual)}</h3>
      </div>

      <p style='margin:16px 0 0;color:#DC3545;font-weight:bold;font-size:14px'>⚠ Por tu seguridad, te pedimos que no compartas esta contraseña con nadie y la borres de tu correo una vez hayas ingresado, o actualízala desde tu perfil.</p>
      <p style='margin:16px 0 0;color:#555'>Si no lograste solicitar este recordatorio, ignora este mensaje.</p>
      <p style='margin:4px 0 0;color:#999;font-size:12px'>Scentify • Privilege & Luxury</p>
    </div>
  </body>
</html>";

            using var msg = new MailMessage
            {
                From = new MailAddress(from, fromName, Encoding.UTF8),
                Subject = asunto,
                SubjectEncoding = Encoding.UTF8,
                Body = cuerpoHtml,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };
            msg.To.Add(new MailAddress(destinatarioEmail, destinatarioNombre, Encoding.UTF8));

            using var client = new SmtpClient(smtp, port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(user, pass),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            await client.SendMailAsync(msg);
        }
    }
}
