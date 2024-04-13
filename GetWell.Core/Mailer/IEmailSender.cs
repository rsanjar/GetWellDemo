using System.Threading.Tasks;

namespace GetWell.Core.Mailer;

public interface IEmailSender
{
    Task<bool> SendEmailAsync(string email, string subject, string htmlBody);
}