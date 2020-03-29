using System.Threading.Tasks;

namespace HistorialClinico.Services.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

}
