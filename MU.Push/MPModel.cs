using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace MU.Push
{
    public class MPModel : DbContext
    {
        /// <summary>
        /// 消息推送服务器数据库实体
        /// </summary>
        public MPModel()
            : base("name=SqlServer")
        {
        }

        public virtual DbSet<MsgToBeSent> MsgToBeSents { get; set; }
        public virtual DbSet<MsgSent> MsgSents { get; set; }
    }

    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MsgType
    {
        Html = 1,
        Sms = 2,
        Email = 4
    }

    /// <summary>
    /// 待发送消息表
    /// </summary>
    public class MsgToBeSent
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(500)]
        public string Content { get; set; }
        [MaxLength(50)]
        public string RequestTime { get; set; }
        [MaxLength(50)]
        public string ExpriedTime { get; set; }
        public int MType { get; set; }
        [MaxLength(50)]
        public string RegName { get; set; }
        [MaxLength(50)]
        public string Phone { get; set; }
        [MaxLength(50)]
        public string Address { get; set; }
    }

    /// <summary>
    /// 已发送消息表
    /// </summary>
    public class MsgSent
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(500)]
        public string Content { get; set; }
        [MaxLength(50)]
        public string RequestTime { get; set; }
        [MaxLength(50)]
        public string ExpriedTime { get; set; }
        public int MType { get; set; }
        [MaxLength(50)]
        public string RegName { get; set; }
        [MaxLength(50)]
        public string Phone { get; set; }
        [MaxLength(50)]
        public string Address { get; set; }
        public DateTime SendTime { get; set; }
    }
}