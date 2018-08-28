namespace MU.DBWapper
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using MU.Models;

    public class MUDB : DbContext
    {
        public MUDB()
            : base("name=MUDB")
        {
        }

        public virtual DbSet<Book> MyEntities { get; set; }

        
    }

}