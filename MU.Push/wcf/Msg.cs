using System.Runtime.Serialization;

namespace MU.Push
{
    [DataContract]
    public abstract class Msg
    {
        /// <summary>
        /// 推送标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }
        /// <summary>
        /// 推送内容
        /// </summary>
        [DataMember]
        public string Content { get; set; }
        /// <summary>
        /// 请求时间，字符串格式
        /// </summary>
        
        [DataMember]
        public string RequestTime { get; set; }
        /// <summary>
        /// 超时时间，字符串格式，超时后不再推送，空白表示不超时
        /// </summary>
        [DataMember]
        public string ExpriedTime { get; set; }
    }

    /// <summary>
    /// Html提示消息
    /// </summary>
    [DataContract]
    public class HtmlMsg : Msg
    {
        /// <summary>
        /// WebSocket注册名称
        /// </summary>
        [DataMember]
        public string RegName { get; set; }
    }

    /// <summary>
    /// 短消息提示
    /// </summary>
    [DataContract]
    public class SmsMsg : Msg
    {
        /// <summary>
        /// 电话号码
        /// </summary>
        [DataMember]
        public string Phone { get; set; }
    }

    /// <summary>
    /// Email消息提示
    /// </summary>
    [DataContract]
    public class EmailMsg : Msg
    {
        /// <summary>
        /// 邮件地址
        /// </summary>
        [DataMember]
        public string Address { get; set; }
    }
}
