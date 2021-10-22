using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace flexGateway.Server.Hubs
{
    public interface IServiceHub
    {
        Task StatusUpdate(string statusMessage);
    }

    public class ServiceHub : Hub<IServiceHub>
    {

    } 
}
