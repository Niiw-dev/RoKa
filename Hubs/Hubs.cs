using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace RoKa.Hubs
{
    public class EmailNotificationHub : Hub
    {
        // Método que el servidor usará para notificar al cliente
        public async Task NotifyEmailSent(string message)
        {
            // Envía a todos los clientes conectados (puedes cambiar a grupos o a un cliente específico)
            await Clients.All.SendAsync("ReceiveEmailNotification", message);
        }
    }
}