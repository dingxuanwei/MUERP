using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MU.DBWapper
{
    public class DB
    {
        public static void Insert<T>(T model) where T : class
        {
            try
            {
                using (MUDB db = new MUDB())
                {
                    db.Set<T>().Add(model);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void InsertList<T>(IEnumerable<T> nmodel) where T : class
        {
            try
            {
                using (MUDB db = new MUDB())
                {
                    db.Set<T>().AddRange(nmodel);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void Update<T>(T model) where T : class
        {
            try
            {
                using (var db = new MUDB())
                {
                    if (db.Entry(model).State == EntityState.Detached)
                    {
                        db.Set<T>().Attach(model);
                        db.Entry(model).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void Delete<T>(T model) where T : class
        {
            try
            {
                using (var db = new MUDB())
                {
                    db.Entry(model).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void Delete<T>(Expression<Func<T, bool>> express) where T : class
        {
            try
            {
                using (var db = new MUDB())
                {
                    var list = db.Set<T>().Where(express).ToList();
                    foreach (var item in list)
                    {
                        db.Set<T>().Remove(item);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static IEnumerable<T> Select<T>(Expression<Func<T, bool>> express) where T : class
        {
            try
            {
                using (var db = new MUDB())
                {
                    return db.Set<T>().Where(express).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<T>();
            }
        }

        public static IEnumerable<T> View<T>(string sql)
        {
            try
            {
                using (var db = new MUDB())
                {
                    return db.Database.SqlQuery<T>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<T>();
            }
        }

        public static void Exec(string produce, params object[] parameters)
        {
            try
            {
                using (var db = new MUDB())
                {
                    db.Database.ExecuteSqlCommand(produce, parameters);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
