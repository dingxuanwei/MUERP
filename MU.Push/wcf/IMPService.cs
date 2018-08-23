using System.ServiceModel;
using System.ServiceModel.Web;

namespace MU.Push
{
    /// <summary>
    /// Message Push服务接口
    /// </summary>
    [ServiceContract]
    public interface IMPService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "DoWork/{name}")]
        string DoWork(string name);

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "PushHtmlMessage")]
        [OperationContract]
        string PushHtmlMessage(HtmlMsg msg);

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "PushSmsMessage")]
        [OperationContract]
        string PushSmsMessage(SmsMsg msg);

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "PushEmailMessage")]
        [OperationContract]
        string PushEmailMessage(EmailMsg msg);
    }
}
