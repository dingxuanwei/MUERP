/*************************************************************************
 * 文件名称 ：ServiceBase.cs                          
 * 描述说明 ：定义数据服务基类
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
 **************************************************************************/

using System.Dynamic;
using MUSystem.Data;
using System.Data;
using System.Data.SqlClient;

namespace MUSystem.Core {
    public class ServiceBase : ServiceBase<ModelBase> {
        public ServiceBase(string Module)
            : base(Module) {

        }
    }

    public partial class ServiceBase<T> where T : ModelBase, new() {
        private static string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["Sys"].ConnectionString;

        public ServiceBase() {
            ModuleName = AttributeHelper.GetModuleAttribute(this.GetType());
            Msg = new AjaxMessge();
        }

        public ServiceBase(string moduleName) {
            ModuleName = moduleName;
            Msg = new AjaxMessge();
        }

        ~ServiceBase() {
            try {
                db.Dispose();
            } catch {
            }
        }

        public static ServiceBase<T> Instance() {
            return new ServiceBase<T>();
        }

        public int StoredProcedure(ParamSP param) {
            var result = 0;
            Logger("执存储过程", () => result = BuilderParse(param).Execute());
            return result;
        }

        public dynamic ScrollKeys(string key, string value, ParamQuery where = null) {
            dynamic result = new ExpandoObject();
            Logger("获取上一条下一条数据", () => {
                result.current = value;
                var pFirst = ParamQuery.Instance().Select("top 1 " + key).OrderBy(key);
                var pPrevious = ParamQuery.Instance().Select("top 1 " + key).AndWhere(key, value, Cp.Less).OrderBy(key + " desc");
                var pNext = ParamQuery.Instance().Select("top 1 " + key).AndWhere(key, value, Cp.Greater).OrderBy(key);
                var pLast = ParamQuery.Instance().Select("top 1 " + key).OrderBy(key + " desc");

                if (where != null) {
                    foreach (var item in where.GetData().Where) {
                        pFirst.AndWhere(item.Data.Column, item.Data.Value, item.Compare, item.Data.Extend);
                        pPrevious.AndWhere(item.Data.Column, item.Data.Value, item.Compare, item.Data.Extend);
                        pNext.AndWhere(item.Data.Column, item.Data.Value, item.Compare, item.Data.Extend);
                        pLast.AndWhere(item.Data.Column, item.Data.Value, item.Compare, item.Data.Extend);
                    }
                }

                result.first = GetField<string>(pFirst) ?? value;
                result.previous = GetField<string>(pPrevious) ?? value;
                result.next = GetField<string>(pNext) ?? value;
                result.last = GetField<string>(pLast) ?? value;

                result.previousEnable = !object.Equals(result.previous, result.current);
                result.nextEnable = !object.Equals(result.next, result.current);
                result.firstEnable = result.previousEnable && !object.Equals(result.first, result.current);
                result.lastEnable = result.nextEnable && !object.Equals(result.last, result.current);
            });
            return result;
        }

        public string GetRuleNewKey(string field, string Prefix, int numberLength, ParamQuery pQuery = null)
        {
            string result = string.Empty;
            string table = typeof(T).Name;
            result = NewKey.BillNoplus(db, table, field, Prefix, numberLength, pQuery);
            return result;
        }
        /// <summary>
        /// 获取自定义前缀规则的编号，直接接收表名参数；增加人：丁贺涛；时间：2017.12.23
        /// </summary>
        /// <param name="table"></param>
        /// <param name="field"></param>
        /// <param name="Prefix"></param>
        /// <param name="numberLength"></param>
        /// <param name="pQuery"></param>
        /// <returns></returns>
        public string GetRuleNewKeyNotTypeofT(IDbContext db, string table, string field, string Prefix,string datestringFormat, int numberLength, ParamQuery pQuery = null)
        {
            string result = string.Empty;
            result = NewKey.BillNoAndDateplus(db, table, field, Prefix, datestringFormat, numberLength, pQuery);
            return result;
        }

        public string GetNewKey(string field, string rule, int qty = 1, ParamQuery pQuery = null)
        {
            var result = string.Empty;

            Logger("获取新主键", () => 
            {
                for (var i = 0; i < qty; i++) {
                    string newkey, table = typeof(T).Name; ;
                    switch (rule) {
                        case "guid":
                            newkey = NewKey.guid();
                            break;
                        case "datetime":
                            newkey = NewKey.datetime();
                            break;
                        case "dateplus":
                            newkey = NewKey.dateplus(db, table, field, "yyyyMMdd", 4);
                            break;
                        case "maxplus":
                            newkey = NewKey.maxplus(db, table, field, pQuery);
                            break;
                        case "rowmaxplus":
                            newkey = NewKey.rowmaxplus(db, table, field, pQuery);
                            break;
                        default:
                            newkey = NewKey.maxplus(db, table, field, pQuery);
                            break;
                    }

                    result += "," + newkey;
                }
            });

            return result.Trim(',');
        }

        #region 独有的方式，操作数据库
        public static DataTable GetData_Pro(string proName, IDataParameter[] parameters, string connString = "") {
            connString = string.IsNullOrEmpty(connString) ? ConnStr : connString;
            using (SqlConnection sqlconn = new SqlConnection(connString)) {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlconn;
                cmd.CommandText = proName;
                cmd.CommandType = CommandType.StoredProcedure;
                // 创建参数
                foreach (var p in parameters) {
                    cmd.Parameters.Add(p);
                }
                //  sqlconn.Open();
                SqlDataAdapter dp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                // 填充dataset
                dp.Fill(ds);

                sqlconn.Close();
                return ds.Tables[0];
            }
        }

        /// <summary>
        /// 独有的方式，操作数据库
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public static DataTable GetData_SqlStr(string sqlStr, string connString = "") {
            connString = string.IsNullOrEmpty(connString) ? ConnStr : connString;
            using (SqlConnection sqlconn = new SqlConnection(connString)) {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlconn;
                cmd.CommandText = sqlStr;
                cmd.CommandType = CommandType.Text;
                //  sqlconn.Open();
                SqlDataAdapter dp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                // 填充dataset
                dp.Fill(ds);
                sqlconn.Close();
                return ds.Tables[0];
            }
        }

        public static int ExecuteNonQuery(string sqlStr, string connString = "") {
            connString = string.IsNullOrEmpty(connString) ? ConnStr : connString;
            using (SqlConnection sqlconn = new SqlConnection(connString)) {
                SqlCommand cmd = new SqlCommand();
                sqlconn.Open();
                cmd.Connection = sqlconn;

                cmd.CommandText = sqlStr;
                cmd.CommandType = CommandType.Text;
                int ret = cmd.ExecuteNonQuery();
                sqlconn.Close();
                return ret;
            }
        }
        #endregion 独有的方式，操作数据库

        #region 变量
        private IDbContext _db;
        protected IDbContext db {
            get {
                if (_db == null)
                    _db = Db.Context(ModuleName);

                return _db;
            }
        }

        public AjaxMessge Msg { get; set; }
        public string ModuleName { get; set; }

        #endregion
    }
}
