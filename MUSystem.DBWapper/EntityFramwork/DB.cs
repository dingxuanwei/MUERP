using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace MU.DBWapper
{
    public class DB
    {
        private static ILog log = LogManager.GetLogger("SysAppender");

        /// <summary>
        /// 新增单条数据
        /// </summary>
        /// <typeparam name="T">表的实体类</typeparam>
        /// <param name="model">模型数据</param>
        public static int Insert<T>(T model) where T : class
        {
            try
            {
                int rows = 0;
                using (MUDB db = new MUDB())
                {
                    db.Set<T>().Add(model);
                    rows += db.SaveChanges();
                }
                return rows;
            }
            catch (Exception ex)
            {
                WriteException(ex);
                return 0;
            }
        }

        /// <summary>
        /// 新增多条数据
        /// </summary>
        /// <typeparam name="T">表的实体类</typeparam>
        /// <param name="models">多条模型数据</param>
        public static int InsertList<T>(IEnumerable<T> models) where T : class
        {
            try
            {
                int rows = 0;
                using (MUDB db = new MUDB())
                {
                    db.Set<T>().AddRange(models);
                    rows += db.SaveChanges();
                }
                return rows;
            }
            catch (Exception ex)
            {
                WriteException(ex);
                return 0;
            }
        }


        /// <summary>
        /// 更新单条数据
        /// </summary>
        /// <typeparam name="T">表的实体类</typeparam>
        /// <param name="model">模型数据</param>
        public static int Update<T>(T model) where T : class
        {
            try
            {
                int rows = 0;
                using (var db = new MUDB())
                {
                    if (db.Entry(model).State == EntityState.Detached)
                    {
                        db.Set<T>().Attach(model);
                        db.Entry(model).State = EntityState.Modified;
                    }
                    rows += db.SaveChanges();
                }
                return rows;
            }
            catch (Exception ex)
            {
                WriteException(ex);
                return 0;
            }
        }

        /// <summary>
        /// 使用查询条件更新数据
        /// </summary>
        /// <typeparam name="T">表的实体类</typeparam>
        /// <param name="action">更新数据动作</param>
        /// <param name="where">表达式</param>
        public static int Update<T>(Action<T> action, Expression<Func<T, bool>> where) where T : class
        {
            try
            {
                int rows = 0;
                using (var db = new MUDB())
                {
                    var list = db.Set<T>().Where(where);
                    foreach (var model in list)
                    {
                        action.Invoke(model);
                        db.Entry(model).State = EntityState.Modified;
                    }
                    rows += db.SaveChanges();
                }
                return 0;
            }
            catch (Exception ex)
            {
                WriteException(ex);
                return 0;
            }
        }

        /// <summary>
        /// 删除单条数据
        /// </summary>
        /// <typeparam name="T">表的实体类</typeparam>
        /// <param name="model">模型数据</param>
        /// <returns>删除行数</returns>
        public static int Delete<T>(T model) where T : class
        {
            try
            {
                int rows = 0;
                using (var db = new MUDB())
                {
                    db.Entry(model).State = EntityState.Deleted;
                    rows += db.SaveChanges();
                }
                return rows;
            }
            catch (Exception ex)
            {
                WriteException(ex);
                return 0;
            }
        }

        /// <summary>
        /// 按条件删除数据
        /// </summary>
        /// <typeparam name="T">表的实体类</typeparam>
        /// <param name="where">linq</param>
        /// <returns>删除行数</returns>
        public static int Delete<T>(Expression<Func<T, bool>> where) where T : class
        {
            try
            {
                if (where == null) throw new ArgumentNullException();
                int rows = 0;
                using (var db = new MUDB())
                {
                    var list = db.Set<T>().Where(where).ToList();
                    foreach (var item in list)
                    {
                        db.Set<T>().Remove(item);
                        rows += db.SaveChanges();
                    }
                }
                return rows;
            }
            catch (Exception ex)
            {
                WriteException(ex);
                return 0;
            }
        }


        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <typeparam name="T">表的实体类</typeparam>
        /// <param name="where">linq表达式</param>
        public static IEnumerable<T> Select<T>(Expression<Func<T, bool>> where = null) where T : class
        {
            try
            {
                where = where ?? (p => true);
                using (var db = new MUDB())
                {
                    return db.Set<T>().Where(where).ToList();
                }
            }
            catch (Exception ex)
            {
                WriteException(ex);
                return new List<T>();
            }
        }

        /// <summary>
        /// 根据条件分页查询
        /// </summary>
        /// <typeparam name="T">表的实体类</typeparam>
        /// <param name="page">页码</param>
        /// <param name="size">页面大小</param>
        /// <param name="where">linq表达式</param>
        /// <returns></returns>
        public static DataGrid<T> SelectPage<T, TKey>(int page, int size, Expression<Func<T, TKey>> orderby, Expression<Func<T, bool>> where = null) where T : class
        {
            try
            {
                where = where ?? (p => true);
                var total = Count(where);
                if (total <= 0) return new DataGrid<T>() { total = 0, rows = new List<T>(), note = "没有查询到相关数据" };
                DataGrid<T> grid = new DataGrid<T>();
                grid.total = total;
                grid.note = "查询成功";

                using (var db = new MUDB())
                {
                    grid.rows = db.Set<T>().Where(where).OrderBy(orderby).Skip((page - 1) * size).Take(size).ToList();
                }
                return grid;
            }
            catch (Exception ex)
            {
                WriteException(ex);
                return new DataGrid<T>() { total = 0, rows = new List<T>(), note = "查询故障，请联系开发人员" };
            }
        }

        /// <summary>
        /// 创建一个原始 SQL 查询，该查询将返回给定泛型类型的元素。类型可以是包含与从查询返回的列名匹配的属性的任何类型，也可以是简单的基元类型。该类型不必是实体类型。即使返回对象的类型是实体类型，上下文也决不会跟踪此查询的结果。使用
        /// System.Data.Entity.DbSet`1.SqlQuery(System.String,System.Object[]) 方法可返回上下文跟踪的实体。与接受
        /// SQL 的任何 API 一样，对任何用户输入进行参数化以便避免 SQL 注入攻击是十分重要的。您可以在 SQL 查询字符串中包含参数占位符，然后将参数值作为附加参数提供。您提供的任何参数值都将自动转换为
        /// DbParameter。context.Database.SqlQuery&lt;Post&gt;("SELECT * FROM dbo.Posts WHERE
        /// Author = @p0", userSuppliedAuthor); 或者，您还可以构造一个 DbParameter 并将它提供给 SqlQuery。这允许您在
        /// SQL 查询字符串中使用命名参数。context.Database.SqlQuery&lt;Post&gt;("SELECT * FROM dbo.Posts
        /// WHERE Author = @author", new SqlParameter("@author", userSuppliedAuthor));
        /// </summary>
        /// <typeparam name="T">表的实体类</typeparam>
        /// <param name="sql">查询字符串</param>
        /// <param name="parameters">要应用于命令字符串的参数</param>
        public static IEnumerable<T> View<T>(string sql, params object[] parameters)
        {
            try
            {
                using (var db = new MUDB())
                {
                    return db.Database.SqlQuery<T>(sql, parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                WriteException(ex);
                return new List<T>();
            }
        }

        /// <summary>
        /// 对数据库执行给定的 DDL/DML 命令。与接受 SQL 的任何 API 一样，对任何用户输入进行参数化以便避免 SQL 注入攻击是十分重要的。您可以在
        /// SQL 查询字符串中包含参数占位符，然后将参数值作为附加参数提供。您提供的任何参数值都将自动转换为 DbParameter。context.Database.ExecuteSqlCommand("UPDATE
        /// dbo.Posts SET Rating = 5 WHERE Author = @p0", userSuppliedAuthor); 或者，您还可以构造一个
        /// DbParameter 并将它提供给 SqlQuery。这允许您在 SQL 查询字符串中使用命名参数。context.Database.ExecuteSqlCommand("UPDATE
        /// dbo.Posts SET Rating = 5 WHERE Author = @author", new SqlParameter("@author",
        /// userSuppliedAuthor));
        /// </summary>
        /// <param name="sql">命令字符串</param>
        /// <param name="parameters">要应用于命令字符串的参数</param>
        public static int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            try
            {
                using (var db = new MUDB())
                {
                    return db.Database.ExecuteSqlCommand(sql, parameters);
                }
            }
            catch (Exception ex)
            {
                WriteException(ex);
                return 0;
            }
        }

        /// <summary>
        /// 执行SQL，带事务
        /// </summary>
        /// <param name="transactionalBehavior">对于此命令控制事务的创建</param>
        /// <param name="sql">命令字符串</param>
        /// <param name="parameters">要应用于命令字符串的参数</param>
        public static int ExecuteSqlCommand(TransactionalBehavior transactionalBehavior, string sql, params object[] parameters)
        {
            try
            {
                using (var db = new MUDB())
                {
                    return db.Database.ExecuteSqlCommand(transactionalBehavior, sql, parameters);
                }
            }
            catch (Exception ex)
            {
                WriteException(ex);
                return 0;
            }
        }

        public static IQueryable Set<T>() where T: class
        {
            try
            {
                using (var db = new MUDB())
                {
                    return db.Set<T>();
                }
            }
            catch (Exception ex)
            {
                WriteException(ex);
                return null;
            }
        }

        /// <summary>
        /// 根据条件查询数据量，返回-1表示错误
        /// </summary>
        /// <typeparam name="T">表的实体类</typeparam>
        /// <param name="where">linq表达式</param>
        public static int Count<T>(Expression<Func<T, bool>> where) where T : class
        {
            try
            {
                where = where ?? (p => true);
                using (var db = new MUDB())
                {
                    return db.Set<T>().Count(where);
                }
            }
            catch (Exception ex)
            {
                WriteException(ex);
                return -1;
            }
        }

        private static void WriteException(Exception ex)
        {
            log.Error(ex);
        }
    }

    public class DataGrid<T>
    {
        public int total { get; set; }
        public IEnumerable<T> rows { get; set; }
        public string note { get; set; }
    }
}
