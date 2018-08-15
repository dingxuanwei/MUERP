using MUSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUSystem.DBModel
{
    public class oms_orderService : ServiceBase<oms_order>
    {

    }

    public class oms_order : ModelBase
    {
        //[Identity]
        //public int ID { get; set; }
        [PrimaryKey]
        public string BillNo { get; set; }
        public string MONo { get; set; }
        public string CustomerCode { get; set; }
        public string CustOrder { get; set; }
        public string OrderNo { get; set; }
        public string PatternNo { get; set; }
        public string Currency { get; set; }
        public decimal? TotalQty { get; set; }
        public decimal? Price { get; set; }
        public decimal? Amount { get; set; }
        public decimal? ProductPar { get; set; }
        public string MaterialType { get; set; }
        public string TradeTerms { get; set; }
        public string PaymentType { get; set; }
        public DateTime? BillDate { get; set; }
        public string SeasonName { get; set; }
        public string ModelName { get; set; }
        public string StyleNo { get; set; }
        public string StyleName { get; set; }
        public string BrandCode { get; set; }
        public string Brand { get; set; }
        public string ItemCode { get; set; }
        public decimal? StandardTime { get; set; }
        public string Manufacturer { get; set; }
        public string Salesman { get; set; }
        public string OrderType { get; set; }
        public string SizeTypeList { get; set; }
        public string ColorTypeList { get; set; }
        public string BomSetFlag { get; set; }
        public string BomType { get; set; }
        public string Biller { get; set; }
        public string Description { get; set; }
        public string OrderState { get; set; }
        public string OrderStateCode { get; set; }
        public bool? Submit { get; set; }
        public string SubmitState { get; set; }
        public string SubmitCode { get; set; }
        public string SubmitPerson { get; set; }
        public DateTime? SubmitDate { get; set; }
        public bool? BomCheckFlag { get; set; }
        public string BomCheckState { get; set; }
        public string BomCheckCode { get; set; }
        public string BomCheckPerson { get; set; }
        public DateTime? BomCheckDate { get; set; }
        public bool? CheckFlag { get; set; }
        public string CheckState { get; set; }
        public string CheckCode { get; set; }
        public string CheckPerson { get; set; }
        public DateTime? CheckDate { get; set; }
        public string ApproveState { get; set; }
        public string ApproveCode { get; set; }
        public string ApprovePerson { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveRemark { get; set; }
        public string CreateCode { get; set; }
        public string CreatePerson { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateCode { get; set; }
        public string UpdatePerson { get; set; }
        public DateTime? UpdateDate { get; set; }
        //做大货订单成本核算添加新增字段2016-09-16
        //新增的字段(2016-8-30)
        public string CostCurrency { get; set; }
        public decimal? OutPurchaseToatl { get; set; }
        public decimal? InPurchaseToatl { get; set; }
        public decimal? PurchaseToatl { get; set; }
        public decimal? ProcessingExpense { get; set; }
        public decimal? TaxCost { get; set; }
        public decimal? OtherCost { get; set; }
        public decimal? Profits { get; set; }
        public decimal? QuotaCost { get; set; }
        public decimal? ManageCost { get; set; }
        public decimal? QuotedPrice { get; set; }
        public decimal? ExchangeRate { get; set; }

        public decimal? OrderQuotedPrice { get; set; }

        public string SizeModel { get; set; }
        /// <summary>
        /// 生产工厂自定义编码
        /// </summary>
        public string FacSetCode { get; set; }
        /// <summary>
        /// 生产工厂GUID
        /// </summary>
        public string FactoryCode { get; set; }
        /// <summary>
        /// 生产工厂描述
        /// </summary>
        public string FactoryName { get; set; }
        /// <summary>
        /// 用户的部门编号
        /// </summary>
        public string DepNo { get; set; }
        ///// <summary>
        ///// 生产工厂描述
        ///// </summary>
        //public string Nick { get; set; }

        /// <summary>
        /// 样板倍数
        /// </summary>
        public double TempletMultiple { get; set; }
        /// <summary>
        /// 裁剪状态
        /// </summary>
        public int TailorStatus { get; set; }
        /// <summary>
        /// 海关BillNo
        /// </summary>
        public string HSBillNo { get; set; }

        /// <summary>
        /// 海关编号
        /// </summary>
        public string HSCode { get; set; }
        /// <summary>
        /// 海关名称
        /// </summary>
        public string HSName { get; set; }
        /// <summary>
        /// 海关描述
        /// </summary>
        public string HSModel { get; set; }
        /// <summary>
        /// 尺寸表审核状态
        /// </summary>
        public string SizeApproval { get; set; }
        /// <summary>
        /// 结算号码
        /// </summary>
        public string PaymentCode { get; set; }
        /// <summary>
        /// 工厂价格
        /// </summary>
        public decimal FactoryPrice { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public decimal? Weight1 { get; set; }
        /// <summary>
        /// 成分
        /// </summary>
        public string Component { get; set; }
        /// <summary>
        /// 成分
        /// </summary>
        public string SaleType { get; set; }
        /// <summary>
        /// 单价涨幅
        /// </summary>
        public decimal? PriceIncrease { get; set; }
        public string POList { get; set; }
        public string DescPrice { get; set; }
    }
}
