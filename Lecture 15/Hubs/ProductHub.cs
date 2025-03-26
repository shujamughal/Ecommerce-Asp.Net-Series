using Microsoft.AspNetCore.SignalR;

namespace Project_Based_Learning.Hubs
{
    public class ProductHub : Hub
    {
        public async Task NotifyProductUpdate(int productId, string productName, string updateMessage)
        {
            await Clients.All.SendAsync("ReceiveProductUpdate", productId, productName, updateMessage);
        }
    }
}
