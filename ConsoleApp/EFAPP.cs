using System.Collections;
namespace ConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    public class EFAPP : DbContext
    {
        public EFAPP()
            : base("name=EFAPP")
        {
        }

        public virtual DbSet<TUsers> Users { get; set; }
        public virtual DbSet<TRoles> Roles { get; set; }
        public virtual DbSet<TUsersRoles> UsersRoles { get; set; }
    }

    public class DB
    {
        public static void Add<T>(T model) where T : class
        {
            try
            {
                using (EFAPP db = new EFAPP())
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

        public static void AddRange<T>(IEnumerable<T> nmodel) where T : class
        {
            try
            {
                using (EFAPP db = new EFAPP())
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
                using (var db = new EFAPP())
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
                using (var db = new EFAPP())
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
                using (var db = new EFAPP())
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
                using (var db = new EFAPP())
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
                using (var db = new EFAPP())
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
                using (var db = new EFAPP())
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

    public class sys_company
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }

        public sys_company() { }
        public sys_company(string name)
        {
            this.Name = name;
        }
    }

    public class v_test
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
