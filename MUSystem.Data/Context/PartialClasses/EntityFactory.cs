﻿using System;

namespace MUSystem.Data
{
    public partial class DbContext
    {
        public IDbContext EntityFactory(IEntityFactory entityFactory)
        {
            Data.EntityFactory = entityFactory;
            return this;
        }
    }
}
