using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
namespace PracaInzynierska_RentIt.Server.Models.AspNetUsersEntity.Email;
public class EmailService
{
    private readonly string _fromEmail;
    private readonly string _smtpServer;
    private readonly int _port;
    private readonly string _password;
    public EmailService(string smtpServer, int port, string fromEmail, string password)
    {
        _fromEmail = fromEmail;
        _smtpServer = smtpServer;
        _port = port;
        _password = password;
    }
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("RentIt", _fromEmail));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;
        var bodyBuilder = new BodyBuilder { HtmlBody = body };
        message.Body = bodyBuilder.ToMessageBody();
        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(_smtpServer, _port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_fromEmail, _password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}