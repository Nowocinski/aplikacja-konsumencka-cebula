using System.Net.Mail;

namespace WebApplication.Infrastructure.Extensions
{
    public static class SendEmailExtensions
    {
        public static void SendEmail(string email)
        {
            SmtpClient client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587
            };

            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential("FakultetBillenium@gmail.com", "haslo4321");
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            MailMessage msg = new MailMessage
            {
                From = new MailAddress("FakultetBillenium@gmail.com")
            };

            msg.To.Add(new MailAddress(email));
            msg.Subject = "Potwierdzenie poprawnego utworzenia konta";
            msg.IsBodyHtml = true;
            msg.Body = string.Format("<html><head></head><body><b>Twoje konto zostało utworzone.</b><br/><br/><br/>Wiadomość została wygenerowana automatycznie. Proszę na nią nie odpowiadać.</body></html>");
            client.Send(msg);
        }
    }
}
