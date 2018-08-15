/*************************************************************************
 * 文件名称 ：ServiceBaseQuery.cs                          
 * 描述说明 ：定义数据服务基类中的查询处理
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
 **************************************************************************/

using System.Collections.Generic;
using System.Dynamic;

namespace MUSystem.Core
{
    public partial class ServiceBase<T> where T : ModelBase, new()
    {
        public List<T> GetModelList(ParamQuery param = null)
        {
            var result = new List<T>();
            Logger("获取实体列表", () =>result = BuilderParse(param).QueryMany());
            return result;
        }

        public List<dynamic> GetDynamicList(ParamQuery param = null)
        {
            var result = new List<dynamic>();
            Logger("获取动态列表", () =>result = BuilderParse(param).QueryManyDynamic());
            return result;
        }

        public List<dynamic> GetDynamicListBySort(ParamQuery param = null,string sort="",string order="")
        {
            var result = new List<dynamic>();
            Logger("获取动态列表", () => result = BuilderParseSortSql(param,sort,order).QueryManyDynamic());
            return result;
        }

        public dynamic GetModelListWithPaging(ParamQuery param = null)
        {
            dynamic result = new ExpandoObject();
            result.rows = this.GetModelList(param);
            result.total = this.queryRowCount(param, result.rows);
            return result;
        }

        public dynamic GetDynamicListWithPaging(ParamQuery param = null)
        {
            dynamic result = new ExpandoObject();
            result.rows = this.GetDynamicList(param);
            result.total = this.queryRowCount(param, result.rows);
            return result;
        }

        /// <summary>
        /// 获取具体的分页数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public dynamic GetDynamicPagingList(ParamQuery param = null)
        {
            dynamic result = new ExpandoObject();
            result.rows = this.GetDynamicList(param);
            return result;
        }

        /// <summary>
        /// 获取具体的分页数据
        /// 自带自定义排序功能
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public dynamic GetDynamicPagingListBySort(ParamQuery param = null,string sort="",string order="")
        {
            dynamic result = new ExpandoObject();
            result.rows = this.GetDynamicListBySort(param,sort,order);
            return result;
        }

        /// <summary>
        /// 带尾行的datagrid列表,所有页面全部统计
        /// </summary>
        /// <param name="param"></param>
        /// <param name="headerField">总计显示在那一列</param>
        /// <param name="fields">需要统计的列：A,B,C</param>
        /// <returns></returns>
        public dynamic GetDynamicListWithFooterAndPaging(ParamQuery param = null, string headerField = null, string fields = null, bool isExport = false)
        {
            dynamic result = new ExpandoObject();
            result.rows = this.GetDynamicList(param);
            result.footer = this.FooterData(param, headerField, fields, isExport);
            result.total = this.queryRowCount(param, result.rows);
            return result;
        }

        /// <summary>
        /// 带尾行的datagrid列表,每页单独统计
        /// </summary>
        /// <param name="param"></param>
        /// <param name="headerField">总计显示在那一列</param>
        /// <param name="fields">需要统计的列：A,B,C</param>
        /// <returns></returns>
        public dynamic GetDynamicListWithPagingAndFooter(ParamQuery param = null, string headerField = null, string fields = null, bool isExport = false)
        {
            dynamic result = new ExpandoObject();
            result.rows = this.GetDynamicList(param);
            result.footer = this.FooterDataWithPage(param, headerField, fields, isExport);
            result.total = this.queryRowCount(param, result.rows);
            return result;
        }

        public T GetModel(ParamQuery param)
        {
            var result = new T();
            Logger("获取实体对象", () => result = BuilderParse(param).QuerySingle());
            //if (result == null) result = new T();
            return result;
        }

        public dynamic GetDynamic(ParamQuery param)
        {
            var result = new ExpandoObject();
            Logger("获取动态对象", () => result = BuilderParse(param).QuerySingleDynamic());
            return result;
        }

        public TField GetField<TField>(ParamQuery param)
        {
            var result = default(TField);
            Logger("获取字段", () => result = BuilderParse(param).QueryValue<TField>());
            return result;
        }
    }
}
