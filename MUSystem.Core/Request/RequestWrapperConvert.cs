/*************************************************************************
 * 文件名称 ：RequestWrapperConvert.cs                          
 * 描述说明 ：请求包装 参数转换处理
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
**************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace MUSystem.Core
{
    public partial class RequestWrapper
    {
        #region 私有方法
        private void parseColumns(XElement settings, Action<string,object> Column) {
            var columns = settings.Element("columns");//todo 拓展功能
            var ignoreCols = columns != null ? getXmlElementAttr(columns, "ignore").Split(',') : new string[0];
            var includeCols = columns != null ? getXmlElementAttr(columns, "include").Split(',') : new string[0];
            ignores.AddRange(ignoreCols);

            foreach (var key in this.Except(ignores))
            {
                if (includeCols.Length == 0 || includeCols.Contains(key))
                    Column(key, this[key]);
            }
        }

        private void parseWhere(XElement settings, Action<string, string, Func<WhereData, string>, string, object[]> AndWhere)
        {
            var where = settings.Element("where");
            if (where == null) return;

            var defaultForAll = string.Equals(getXmlElementAttr(where, "defaultForAll"), "true", StringComparison.OrdinalIgnoreCase);
            var defaultCp = CpHelper.Parse(getXmlElementAttr(where, "defaultCp"));
            var defaultIgnoreEmpty = string.Equals(getXmlElementAttr(where, "defaultIgnoreEmpty"), "true", StringComparison.OrdinalIgnoreCase);

            var handledList = new List<string>();
            foreach (var item in where.Elements("field"))
            {
                var name = getXmlElementAttr(item, "name");
                var cp = CpHelper.Parse(getXmlElementAttr(item, "cp"), defaultCp);
                var ignoreEmpty = getXmlElementAttr(item, "ignoreEmpty") == string.Empty ? defaultIgnoreEmpty : string.Equals(getXmlElementAttr(item, "ignoreEmpty"), "true", StringComparison.OrdinalIgnoreCase);
                var variable = getXmlElementAttr(item, "variable",name);
                string value = Convert.ToString(this[variable]);
                value = string.IsNullOrEmpty(value) ? "" : value.Replace("'", "''");  //修改特殊处理单引号，如果有单引号，转义之
                var extend = getXmlElementAttr(item, "extend").Split(',');

                if (!string.IsNullOrEmpty(value) || !ignoreEmpty)
                    AndWhere(getFieldName(name, true), value, cp, variable, extend);

                handledList.Add(getFieldName(variable));
                handledList.Add(getAliasName(variable));
            }

            if (defaultForAll)
            {
                var unHandledKeys = this.Except(handledList);
                foreach (var name in unHandledKeys)
                {
                    var value = this[name];
                    if (!string.IsNullOrEmpty(value) || !defaultIgnoreEmpty)
                        AndWhere(name, value, defaultCp, name,new object[]{});
                }
            }
        }
        #endregion

        #region ToParamQuery
        public ParamQuery ToParamQuery() {
            var pQuery = new ParamQuery();
            var settings = XElement.Parse(settingXml);
            var defaultOrderBy = getXmlElementAttr(settings, "defaultOrderBy");
            pQuery.Select(getXmlElementValue(settings, "select"));

            //获取分页及排序信息
            var page = parseInt(this.request["page"],1);
            var rows = parseInt(this.request["rows"],0);
            var orderby = string.Join(" ", getFieldName(this["sort"],true), this["order"]).Trim();
            if (string.IsNullOrEmpty(orderby))
                orderby = defaultOrderBy;

            var sFrom = getXmlElementValue(settings, "from");
            if (string.IsNullOrEmpty(sFrom))
                sFrom = getXmlElementValue(settings, "table");

            var fromMatches = new Regex(@"\$variable\[([a-zA-Z_][a-zA-Z0-9_]*)\]", RegexOptions.Multiline).Matches(sFrom);
            foreach (Match match in fromMatches)
                sFrom = sFrom.Replace(match.Groups[0].ToString(), this[match.Groups[1].ToString()]);

            pQuery.From(sFrom)
                .Paging(page, rows)
                .OrderBy(orderby);

            parseWhere(settings, (name, value, compare, variable, extend) =>
            { 
                pQuery.AndWhere(name, value, compare,extend); 
            });
            return pQuery;
        }
        public ParamQuery ToParamQueryTask(string tableName)
        {
            var pQuery = new ParamQuery();
            var settings = XElement.Parse(settingXml);
            var defaultOrderBy = getXmlElementAttr(settings, "defaultOrderBy");
            pQuery.Select(getXmlElementValue(settings, "select"));

            //获取分页及排序信息
            var page = parseInt(this.request["page"], 1);
            var rows = parseInt(this.request["rows"], 0);
            var orderby = string.Join(" ", getFieldName(this["sort"], true), this["order"]).Trim();
            if (string.IsNullOrEmpty(orderby))
                orderby = defaultOrderBy;

            var sFrom = getXmlElementValue(settings, "from");
            if (string.IsNullOrEmpty(sFrom))
                sFrom = getXmlElementValue(settings, "table");

            var fromMatches = new Regex(@"\$variable\[([a-zA-Z_][a-zA-Z0-9_]*)\]", RegexOptions.Multiline).Matches(sFrom);
            foreach (Match match in fromMatches)
                sFrom = sFrom.Replace(match.Groups[0].ToString(), this[match.Groups[1].ToString()]);

            pQuery.From(tableName)
                .Paging(page, rows)
                .OrderBy(orderby);

            parseWhere(settings, (name, value, compare, variable, extend) =>
            {
                pQuery.AndWhere(name, value, compare, extend);
            });
            return pQuery;
        }
        /// <summary>
        /// Get方法带参数的方法 - 参数类型 field1,field2,... 这些字段将不进行where条件过滤
        /// </summary>
        /// <param name="notFilterField">不进行过滤的字段</param>
        /// <returns></returns>
        public ParamQuery ToParamQuery(string notFilterField)
        {
            List<string> notFilterFieldList = notFilterField.Split(',').ToList();

            var pQuery = new ParamQuery();
            var settings = XElement.Parse(settingXml);
            var defaultOrderBy = getXmlElementAttr(settings, "defaultOrderBy");
            pQuery.Select(getXmlElementValue(settings, "select"));

            //获取分页及排序信息
            var page = parseInt(this.request["page"], 1);
            var rows = parseInt(this.request["rows"], 0);
            var orderby = string.Join(" ", getFieldName(this["sort"], true), this["order"]).Trim();
            if (string.IsNullOrEmpty(orderby))
                orderby = defaultOrderBy;

            var sFrom = getXmlElementValue(settings, "from");
            if (string.IsNullOrEmpty(sFrom))
                sFrom = getXmlElementValue(settings, "table");

            var fromMatches = new Regex(@"\$variable\[([a-zA-Z_][a-zA-Z0-9_]*)\]", RegexOptions.Multiline).Matches(sFrom);
            foreach (Match match in fromMatches)
                sFrom = sFrom.Replace(match.Groups[0].ToString(), this[match.Groups[1].ToString()]);

            pQuery.From(sFrom)
                .Paging(page, rows)
                .OrderBy(orderby);

            parseWhere(settings, (name, value, compare, variable, extend) =>
            {
                if (!notFilterFieldList.Contains(name))
                {
                    pQuery.AndWhere(name, value, compare, extend);
                }
            });
            return pQuery;
        }

        private int parseInt(string str, int defaults = 0) 
        { 
            int value;
            if (!int.TryParse(str,out value)) value = defaults;
            return value;
        }
        #endregion

        #region ToParamUpdate
        public ParamUpdate ToParamUpdate()
        {
            var settings = XElement.Parse(settingXml);
            var pUpdate = ParamUpdate.Instance().Update(getXmlElementValue(settings, "table"));

            var list = new List<string>();
            parseWhere(settings, (name, value, compare, variable,extend) =>
            {
                pUpdate.AndWhere(name, value, compare, extend);
                list.Add(variable);
            });

            parseColumns(settings, (name, value) =>
            {
                if (list.IndexOf(name)<0)
                    pUpdate.Column(name, value);
            });
            return pUpdate;
        }
        #endregion

        #region ToParamInsert
        public ParamInsert ToParamInsert()
        {
            var settings = XElement.Parse(settingXml);
            var pInsert = ParamInsert.Instance().Insert(getXmlElementValue(settings, "table"));
            parseColumns(settings, (name, value) => pInsert.Column(name, value));
            return pInsert;
        }
        #endregion

        #region ToParamDelete
        public ParamDelete ToParamDelete()
        {
            var settings = XElement.Parse(settingXml);
            var pDelete = ParamDelete.Instance().From(getXmlElementValue(settings, "table"));
            parseWhere(settings, (name, value, compare, variable, extend) => pDelete.AndWhere(name, value, compare, extend));

            return pDelete;
        }
        #endregion
    }
}