using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

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
