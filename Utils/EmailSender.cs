using System.Net.Mail;

namespace TheOrchidArchade.Utils
{
    public class EmailSender
    {
        public static bool SendEmail(string userEmail, string message,ILogger logger)
        {
            var smtpUser = Environment.GetEnvironmentVariable("SMTP_USER");
            var smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(smtpUser);
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Authentication code";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = message;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPassword);
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Port = 587;

            try
            {
                logger.LogInformation("Sending authentication message to mail: " + userEmail);
                client.Send(mailMessage);
                return true;
            }
            catch (SmtpException smtpEx)
            {
                logger.LogError($"SMTP Exception: {smtpEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception sending email: {ex.Message}");
                return false;
            }

        }


        public static void Send(MailMessage message)
        {
            var client = new SmtpClient() { EnableSsl = true };
            try
            {
                client.Send(message);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
