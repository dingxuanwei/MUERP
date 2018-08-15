/*************************************************************************
 * 文件名称 ：ServiceBaseUtils.cs                          
 * 描述说明 ：定义数据服务基类中的工具类
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
 **************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using MUSystem.Data;
using System.Globalization;

namespace MUSystem.Core
{
    public partial class ServiceBase<T> where T : ModelBase, new()
    {

        /// <summary>
        /// 系统通用  单据保存函数
        /// 增加自动保存用户部门 工厂属性
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private static Dictionary<string, object> GetPersonDateForCreate(string tableName)
        {
            var user = FormsAuth.GetUserData().UserName;
            var usercode = FormsAuth.GetUserData().UserCode;
            var depno = FormsAuth.GetUserData().DepNo;
            var factorycode = FormsAuth.GetUserData().FactoryCode;
            var retdict = new Dictionary<string, object>();

            //过滤掉部门表和用户表，因为部门表和用户表的部门编号不是根据用户来增加的
            if (tableName.ToLower() == "sys_department" || tableName.ToLower() == "sys_user")
            {
               var dict = new Dictionary<string, object>
                {
                    {APP.FIELD_UPDATE_CODE, usercode},
                    {APP.FIELD_UPDATE_PERSON, user},
                    {APP.FIELD_UPDATE_DATE, DateTime.Now},
                    {APP.FIELD_CREATE_CODE, usercode},
                    {APP.FIELD_CREATE_PERSON, user},
                    {APP.FIELD_CREATE_DATE, DateTime.Now},
                };
                retdict = dict;
            }

            else
            {
                //增加自动保存用户部门 工厂属性
                var dict = new Dictionary<string, object>
                {
                    {APP.FIELD_UPDATE_CODE, usercode},
                    {APP.FIELD_UPDATE_PERSON, user},
                    {APP.FIELD_UPDATE_DATE, DateTime.Now},
                    {APP.FIELD_CREATE_CODE, usercode},
                    {APP.FIELD_CREATE_PERSON, user},
                    {APP.FIELD_CREATE_DATE, DateTime.Now},
                    {APP.FIELD_DepNo, depno},
                    {APP.FIELD_FactoryCode, factorycode}
                };

                retdict = dict;
            }

           
            
            return retdict;
        }

        private static Dictionary<string, object> GetPersonDateForUpdate()
        {
            var user = FormsAuth.GetUserData().UserName;
            var usercode = FormsAuth.GetUserData().UserCode;
            var dict = new Dictionary<string, object>
                {
                    {APP.FIELD_UPDATE_CODE, usercode},
                    {APP.FIELD_UPDATE_PERSON, user},
                    {APP.FIELD_UPDATE_DATE, DateTime.Now}
                };
            return dict;
        }

        protected ISelectBuilder<T> BuilderParse(ParamQuery param)
        {
            if (param == null)
            {
                param = new ParamQuery();
            }

            var data = param.GetData();
            var sFrom = data.From.Length == 0 ? typeof(T).Name : data.From;
            var selectBuilder = db.Select<T>(string.IsNullOrEmpty(data.Select) ? (sFrom + ".*") : data.Select)
                .From(sFrom)
                .Where(data.WhereSql)
                .GroupBy(data.GroupBy)
                .Having(data.Having)
                .OrderBy(data.OrderBy)
                .Paging(data.PagingCurrentPage, data.PagingItemsPerPage);
            return selectBuilder;
        }

        protected ISelectBuilder<T> BuilderParseSortSql(ParamQuery param,string sort="",string order="")
        {
            if (param == null)
            {
                param = new ParamQuery();
            }

            var data = param.GetData();
            var sFrom = data.From.Length == 0 ? typeof(T).Name : data.From;
            string orderBy = data.OrderBy;
            if (sort!="" && sort!=null)
            {
                int fieldCount = db.Select<int>("count(1)").From("bas_customerRank").Where("sortfield = '" + sort + "'").QuerySingle();
                if (fieldCount > 0)
                {
                    sFrom = sFrom + "  left join bas_customerRank b on a." + sort + "=b.sortfield";
                    orderBy = " b.sortnum " + order;
                }
            }

            var selectBuilder = db.Select<T>(string.IsNullOrEmpty(data.Select) ? (sFrom + ".*") : data.Select)
                .From(sFrom)
                .Where(data.WhereSql)
                .GroupBy(data.GroupBy)
                .Having(data.Having)
                .OrderBy(orderBy)
                .Paging(data.PagingCurrentPage, data.PagingItemsPerPage);
            return selectBuilder;
        }

        /// <summary>
        /// 将传递的param转换为sql语句
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ISelectBuilder<T> BuilderParseSql(ParamQuery param)
        {
            if (param == null)
            {
                param = new ParamQuery();
            }

            var data = param.GetData();
            var sFrom = data.From.Length == 0 ? typeof(T).Name : data.From;
            var selectBuilder = db.Select<T>(string.IsNullOrEmpty(data.Select) ? (sFrom + ".*") : data.Select)
                .From(sFrom)
                .Where(data.WhereSql)
                .GroupBy(data.GroupBy)
                .Having(data.Having)
                .OrderBy(data.OrderBy)
                .Paging(data.PagingCurrentPage, data.PagingItemsPerPage);
            return selectBuilder;
        }

        /// <summary>
        /// 如果是导出就不获取OrderBy
        /// </summary>
        /// <param name="param"></param>
        /// <param name="isExport"></param>
        /// <returns></returns>
        protected ISelectBuilder<T> BuilderParse(ParamQuery param, bool isExport)
        {
            if (param == null)
            {
                param = new ParamQuery();
            }

            var data = param.GetData();
            var sFrom = data.From.Length == 0 ? typeof(T).Name : data.From;

            if (isExport)
            {
                var selectBuilder = db.Select<T>(string.IsNullOrEmpty(data.Select) ? (sFrom + ".*") : data.Select)
                    .From(sFrom)
                    .Where(data.WhereSql)
                    .GroupBy(data.GroupBy)
                    .Having(data.Having)
                    .Paging(data.PagingCurrentPage, data.PagingItemsPerPage);
                return selectBuilder;
            }
            else
            {
                var selectBuilder = db.Select<T>(string.IsNullOrEmpty(data.Select) ? (sFrom + ".*") : data.Select)
                    .From(sFrom)
                    .Where(data.WhereSql)
                    .GroupBy(data.GroupBy)
                    .Having(data.Having)
                    .OrderBy(data.OrderBy)
                    .Paging(data.PagingCurrentPage, data.PagingItemsPerPage);
                return selectBuilder;
            }
        }

        protected IInsertBuilder BuilderParse(ParamInsert param)
        {
            var data = param.GetData();
            var insertBuilder = db.Insert(data.From.Length == 0 ? typeof(T).Name : data.From);
            string tableName = typeof(T).Name.ToString();
            var dict = GetPersonDateForCreate(tableName);

            foreach (var column in data.Columns.Where(column => !dict.ContainsKey(column.Key)))
                insertBuilder.Column(column.Key, column.Value);
            
            var properties = MUSystem.Utils.ZReflection.GetProperties(typeof(T));
            foreach (var item in dict.Where(item => properties.ContainsKey(item.Key.ToLower())))
                insertBuilder.Column(item.Key, item.Value);

            return insertBuilder;
        }

        protected IUpdateBuilder BuilderParse(ParamUpdate param)
        {
            var data = param.GetData();
            var updateBuilder = db.Update(data.Update.Length == 0 ? typeof(T).Name : data.Update);

            var dict = GetPersonDateForUpdate();
            foreach (var column in data.Columns.Where(column => !dict.ContainsKey(column.Key)))
                updateBuilder.Column(column.Key, column.Value);

            var properties = MUSystem.Utils.ZReflection.GetProperties(typeof(T));
            foreach (var item in dict.Where(item => properties.ContainsKey(item.Key.ToLower())))
                updateBuilder.Column(item.Key, item.Value);

            updateBuilder.Where(data.WhereSql);

            return updateBuilder;
        }

        protected IDeleteBuilder BuilderParse(ParamDelete param)
        {
            var data = param.GetData();
            var deleteBuilder = db.Delete(data.From.Length == 0 ? typeof(T).Name : data.From);
            deleteBuilder.Where(data.WhereSql);

            return deleteBuilder;
        }

        protected IStoredProcedureBuilder BuilderParse(ParamSP param)
        {
            var data = param.GetData();
            var spBuilder = db.StoredProcedure(data.Name);
            foreach(var item in data.Parameter)
                spBuilder.Parameter(item.Key, item.Value);

            foreach (var item in data.ParameterOut)
                spBuilder.ParameterOut(item.Key, item.Value);

            return spBuilder;
        }

        ///// <summary>
        ///// 获取自定义序列的grid数据
        ///// </summary>
        ///// <param name="param"></param>
        ///// <param name="columns"></param>
        ///// <returns></returns>
        //protected List<dynamic> queryRowForUserSort(ParamQuery param, string columns)
        //{
        //    columns = string.IsNullOrEmpty(columns) ? "*" : columns;
        //    var sql = BuilderParse(param).GetSql();
        //    return db.Sql(@"select " + columns + " from ( " + sql + " ) tb_temp").QueryMany<dynamic>();
        //}

        protected int queryRowCount(ParamQuery param, dynamic rows)
        {
            if (rows != null)
                if (null == param || param.GetData().PagingItemsPerPage == 0)
                    return rows.Count;

            var RowCountParam = param;
            var sql = BuilderParse(RowCountParam.Paging(1, 0).OrderBy(string.Empty)).GetSql();
            return db.Sql(@"select count(*) from ( " + sql + " ) tb_temp").QuerySingle<int>();
        }
        /// <summary>
        /// 跟进param参数先获取总数
        /// </summary>
        /// <param name="param"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public int queryCount(ParamQuery param)
        {
            var RowCountParam = param;
            var sql = BuilderParse(RowCountParam.Paging(1, 0).OrderBy(string.Empty)).GetSql();
            return db.Sql(@"select count(*) from ( " + sql + " ) tb_temp").QuerySingle<int>();
        }

        /// <summary>
        /// 获取datagrid对应的尾行
        /// </summary>
        /// <param name="param"></param>
        /// <param name="headerField">“总计”字样显示的位置： Field</param>
        /// <param name="fields">需要统计的字段，格式： Field1,Field2,Field3,...</param>
        /// <returns></returns>
        protected List<dynamic> FooterData(ParamQuery param, string headerField, string fields, bool isExport)
        {
            List<string> list = fields.Trim().Split(',').ToList();
            string footerSql = "";

            for (int i = 0; i < list.Count; i++)
            {
                string _fieldName = list[i].Substring(list[i].LastIndexOf(".") + 1, list[i].Length - list[i].LastIndexOf(".") - 1);
                footerSql += "SUM(" + list[i] + ") AS " + _fieldName + ",";
            }
            if (footerSql.Length <= 0)
            {
                return new List<dynamic>();
            }
            else
            {
                footerSql = "'总计:' as " + headerField + "," + footerSql.Substring(0, footerSql.Length - 1);
                var sql = BuilderParse(param.Paging(1, 0).OrderBy(string.Empty)).GetSql();
                param.Paging(1, 10);
                string resultSql = @"select " + footerSql + " from ( " + sql + " ) tb_temp";

                List<dynamic> result = db.Sql(resultSql).QueryMany<dynamic>();
                return result;
            }
        }

        /// <summary>
        /// 获取datagrid对应的尾行
        /// </summary>
        /// <param name="param"></param>
        /// <param name="headerField">“总计”字样显示的位置： Field</param>
        /// <param name="fields">需要统计的字段，格式： Field1,Field2,Field3,...</param>
        /// <returns></returns>
        protected dynamic FooterDataWithPage(ParamQuery param, string headerField, string fields, bool isExport)
        {
            List<string> list = fields.Trim().Split(',').ToList();
            string footerSql = "";

            for (int i = 0; i < list.Count; i++)
            {
                string _fieldName = list[i].Substring(list[i].LastIndexOf(".") + 1, list[i].Length - list[i].LastIndexOf(".") - 1);
                footerSql += "SUM(" + list[i] + ") AS " + _fieldName + ",";
            }
            if (footerSql.Length <= 0)
            {
                return new List<T>();
            }
            else
            {
                footerSql = "'总计:' as " + headerField + "," + footerSql.Substring(0, footerSql.Length - 1);
                string pageSql = BuilderParse(param, isExport).GetSql();

                CompareInfo Compare = CultureInfo.InvariantCulture.CompareInfo;
                int i = Compare.IndexOf(pageSql, "top", CompareOptions.IgnoreCase);
                if (i < 0)         //代表不存在top
                {
                    pageSql = pageSql.Replace("select", "select top 99999999 ");
                }

                if (param.GetData().PagingCurrentPage == 1)
                {
                    dynamic result = db.Sql(@"select " + footerSql + " from ( " + pageSql + " ) tb_temp").QueryMany<dynamic>();
                    return result;
                }
                else
                {
                    int index = pageSql.LastIndexOf("*");
                    pageSql = pageSql.Remove(index, 1);
                    pageSql = pageSql.Insert(index, footerSql);
                    dynamic result = db.Sql(pageSql).QueryMany<dynamic>();
                    return result;
                }
            }
        }

        /// <summary>
        /// 获取datagrid对应的尾行
        /// </summary>
        /// <param name="param"></param>
        /// <param name="headerField">“总计”字样显示的位置： Field</param>
        /// <param name="fields">需要统计的字段，格式： Field1,Field2,Field3,...</param>
        /// <returns></returns>
        public dynamic FooterPageData(ParamQuery param, string headerField, string fields, bool isExport = false)
        {
            List<string> list = fields.Trim().Split(',').ToList();
            string footerSql = "";

            for (int i = 0; i < list.Count; i++)
            {
                string _fieldName = list[i].Substring(list[i].LastIndexOf(".") + 1, list[i].Length - list[i].LastIndexOf(".") - 1);
                footerSql += "SUM(" + list[i] + ") AS " + _fieldName + ",";
            }
            if (footerSql.Length <= 0)
            {
                return new List<T>();
            }
            else
            {
                footerSql = "'总计:' as " + headerField + "," + footerSql.Substring(0, footerSql.Length - 1);
                string pageSql = BuilderParse(param, isExport).GetSql();

                CompareInfo Compare = CultureInfo.InvariantCulture.CompareInfo;
                int i = Compare.IndexOf(pageSql, "top", CompareOptions.IgnoreCase);
                if (i < 0)         //代表不存在top
                {
                    pageSql = pageSql.Replace("select", "select top 99999999 ");
                }

                if (param.GetData().PagingCurrentPage == 1)
                {
                    dynamic result = db.Sql(@"select " + footerSql + " from ( " + pageSql + " ) tb_temp").QueryMany<dynamic>();
                    return result;
                }
                else
                {
                    int index = pageSql.LastIndexOf("*");
                    pageSql = pageSql.Remove(index, 1);
                    pageSql = pageSql.Insert(index, footerSql);
                    dynamic result = db.Sql(pageSql).QueryMany<dynamic>();
                    return result;
                }
            }
        }


    }
}
