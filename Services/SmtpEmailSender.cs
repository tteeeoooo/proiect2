// using Microsoft.AspNetCore.Identity.UI.Services;
// using Microsoft.Extensions.Options;
// using System.Net;
// using System.Net.Mail;
// using System.Threading.Tasks;

// public class EmailSettings
// {
//     public string Host { get; set; }
//     public int Port { get; set; }
//     public string Username { get; set; }
//     public string Password { get; set; }
//     public bool EnableSsl { get; set; }
// }

// public class SmtpEmailSender : IEmailSender
// {
//     private readonly SmtpClient _smtpClient;
//     private readonly EmailSettings _emailSettings;

//     public SmtpEmailSender(IOptions<EmailSettings> emailSettings)
//     {
//         _emailSettings = emailSettings.Value; // Preia configurarea din appsettings.json
//         _smtpClient = new SmtpClient(_emailSettings.Host)
//         {
//             Port = _emailSettings.Port,
//             Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
//             EnableSsl = _emailSettings.EnableSsl,
//         };
//     }

//     public async Task SendEmailAsync(string email, string subject, string htmlMessage)
//     {
//         var mailMessage = new MailMessage
//         {
//             From = new MailAddress(_emailSettings.Username),
//             Subject = subject,
//             Body = htmlMessage,
//             IsBodyHtml = true
//         };

//         mailMessage.To.Add(email);

//         await _smtpClient.SendMailAsync(mailMessage);
//     }
// }