using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

public class DBUtils<T>
{
    public static List<T> DataToListObject(DataTable dt)
    {
        var list = new List<T>();
        var plist = new List<PropertyInfo>(typeof(T).GetProperties());

        for (int row = 0; row < dt.Rows.Count; row++)
        {
            //T t = Activator.CreateInstance<T>();
            T t = default(T);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                PropertyInfo info = plist.Find(p => p.Name.ToLower() == dt.Columns[i].ColumnName.ToLower());
                if (info != null)
                {
                    if (!Convert.IsDBNull(dt.Rows[row][i]))
                    {
                        bool cannull = false;                           //属性有问号
                        string typename = "";
                        if (info.PropertyType.Name == "Nullable`1")
                        {
                            typename = info.PropertyType.GenericTypeArguments[0].Name;
                            cannull = true;
                        }
                        else
                        {
                            typename = info.PropertyType.Name;
                            cannull = false;
                        }
                        object obj = dt.Rows[row][i];
                        if (obj == null && cannull)
                        {
                            info.SetValue(t, null, null);
                        }
                        else
                        {
                            switch (typename)
                            {
                                case "String":
                                    info.SetValue(t, Convert.ToString(obj), null);
                                    break;
                                case "Int32":
                                    info.SetValue(t, Convert.ToInt32(obj), null);
                                    break;
                                case "Double":
                                    info.SetValue(t, Convert.ToDouble(obj), null);
                                    break;
                                case "Decimal":
                                    info.SetValue(t, Convert.ToDecimal(obj), null);
                                    break;
                                case "DateTime":
                                    info.SetValue(t, Convert.ToDateTime(obj), null);
                                    break;
                                case "Int64":
                                    info.SetValue(t, Convert.ToInt64(obj), null);
                                    break;
                                default:
                                    info.SetValue(t, obj, null);
                                    break;
                            }
                        }
                    }
                }
            }
            list.Add(t);
        }

        return list;
    }

    public static List<T> Data2Entity(DataTable dt)
    {
        List<T> list = new List<T>( );
        PropertyInfo[] ps = typeof(T).GetProperties( );
        foreach (DataRow r in dt.Rows)
        {
            T t = default(T);
            t = Activator.CreateInstance<T>( );
            foreach (var item in ps)
            {
                if (r.Table.Columns.Contains(item.Name))
                {
                    object v = r[item.Name];
                    if (v.GetType( ) == typeof(System.DBNull)) v = null;
                    item.SetValue(t, v, null);
                }
            }
            list.Add(t);
        }
        return list;
    }

    public static PageList<T> DataToPageList(DataTable dt, int page = 1, int size = 10)
    {
        PageList<T> pagelist = new PageList<T>();
        pagelist.page = page;
        pagelist.size = size;
        pagelist.total = dt.Rows.Count;
        var list = new List<T>();
        var plist = new List<PropertyInfo>(typeof(T).GetProperties());

        for (int row = page * size - size; row < dt.Rows.Count && row < page * size; row++)
        {
            T t = Activator.CreateInstance<T>();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                PropertyInfo info = plist.Find(p => p.Name.ToLower() == dt.Columns[i].ColumnName.ToLower());
                if (info != null)
                {
                    if (!Convert.IsDBNull(dt.Rows[row][i]))
                    {
                        bool cannull = false;                           //属性有问号
                        string typename = "";
                        if (info.PropertyType.Name == "Nullable`1")
                        {
                            typename = info.PropertyType.GenericTypeArguments[0].Name;
                            cannull = true;
                        }
                        else
                        {
                            typename = info.PropertyType.Name;
                            cannull = false;
                        }
                        object obj = dt.Rows[row][i];
                        if (obj == null && cannull)
                        {
                            info.SetValue(t, null, null);
                        }
                        else
                        {
                            switch (typename)
                            {
                                case "String":
                                    info.SetValue(t, Convert.ToString(obj), null);
                                    break;
                                case "Int16":
                                    info.SetValue(t, Convert.ToInt16(obj), null);
                                    break;
                                case "Int32":
                                    info.SetValue(t, Convert.ToInt32(obj), null);
                                    break;
                                case "Int64":
                                    info.SetValue(t, Convert.ToInt64(obj), null);
                                    break;
                                case "Double":
                                    info.SetValue(t, Convert.ToDouble(obj), null);
                                    break;
                                case "Decimal":
                                    info.SetValue(t, Convert.ToDecimal(obj), null);
                                    break;
                                case "DateTime":
                                    info.SetValue(t, Convert.ToDateTime(obj), null);
                                    break;
                                case "Boolean":
                                    info.SetValue(t, Convert.ToBoolean(obj), null);
                                    break;
                                case "Byte":
                                    info.SetValue(t, Convert.ToByte(obj), null);
                                    break;
                                case "SByte":
                                    info.SetValue(t, Convert.ToSByte(obj), null);
                                    break;
                                case "Char":
                                    info.SetValue(t, Convert.ToChar(obj), null);
                                    break;
                                case "Single":
                                    info.SetValue(t, Convert.ToSingle(obj), null);
                                    break;
                                default:
                                    info.SetValue(t, obj, null);
                                    break;
                            }
                        }
                    }
                }
            }
            list.Add(t);
        }
        pagelist.rows = list.ToArray();
        return pagelist;
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="tbname">数据表名</param>
    /// <param name="page">页码</param>
    /// <param name="size">页面大小</param>
    /// <param name="filter">查询条件</param>
    /// <returns>分页查询结果</returns>
    public static PageList<T> SelectListPagination(string tbname, int page, int size, string filter = "")
    {
        string cols = GetFields();
        string sqlc = "";
        string sql = "";

        if (string.IsNullOrWhiteSpace(filter))
        {
            sqlc = string.Format("select count(*) from {0}", tbname);
            sql = string.Format("select {3} from {0} limit {1},{2}", tbname, (page - 1) * size, size, cols);
        }
        else
        {
            sqlc = string.Format("select count(*) from {0} where {1}", tbname, filter);
            sql = string.Format("select {4} from {0} where {1} limit {2},{3}", tbname, filter, (page - 1) * size, size, cols);
        }
        PageList<T> pagelist = new PageList<T>();
        int total = Convert.ToInt32(DbHelperSQL.GetSingle(sqlc));
        var datatable = DbHelperSQL.Query(sql).Tables[0];
        var list = DataToListObject(datatable);
        pagelist = new PageList<T>(list.ToArray(), page, size, total);

        return pagelist;
    }

    /// <summary>
    /// 根据属性获取要查询的列
    /// </summary>
    /// <returns>查询列</returns>
    private static string GetFields()
    {
        StringBuilder sb = new StringBuilder();
        Type objType = typeof(T);
        foreach (var propInfo in objType.GetProperties())
        {
            object[] objAttrs = propInfo.GetCustomAttributes().ToArray();
            if (objAttrs.Length > 0)
            {
                bool hasattr = false;
                for (int i = 0; i < objAttrs.Length; i++)
                {
                    if (objAttrs[i] is DisplayAttribute)
                    {
                        string name = (objAttrs[i] as DisplayAttribute).Name;
                        sb.AppendFormat("{0} as '{1}',", name, propInfo.Name);
                        hasattr = true;
                    }
                }
                if (!hasattr)
                {
                    sb.AppendFormat("{0},", propInfo.Name);
                }
            }
            else
            {
                sb.AppendFormat("{0},", propInfo.Name);
            }
        }
        return sb.ToString().TrimEnd(',');
    }
}

public class PageList<T>
{
    /// <summary>
    /// 数据内容
    /// </summary>
    public T[] rows { get; set; }
    /// <summary>
    /// 页码
    /// </summary>
    public int page { get; set; }
    /// <summary>
    /// 页面数量
    /// </summary>
    public int size { get; set; }
    /// <summary>
    /// 总数
    /// </summary>
    public int total { get; set; }

    public PageList() { }

    public PageList(T[] rows, int page, int size, int total)
    {
        this.rows = rows;
        this.page = page;
        this.size = size;
        this.total = total;
    }
}


