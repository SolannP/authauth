using System.Net.Mail;

namespace AuthAuthDomaineService;

public interface IMessageSending
{
    public void sendMessageToContact(string? host, int port, MailMessage mailMessage)
    {
        SmtpClient smtpClient = new SmtpClient();
    }

}