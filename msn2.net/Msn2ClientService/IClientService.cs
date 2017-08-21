using System.ServiceModel;
using System.ServiceModel.Web;

namespace Msn2ClientService
{
    [ServiceContract]
    public interface IClientService
    {
        [OperationContract]
        [WebGet]
        void Suspend();

        [OperationContract]
        [WebGet]
        string GetVersion();
    }
}
