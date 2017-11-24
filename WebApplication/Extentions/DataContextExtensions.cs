using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApplication.Extentions
{
    public static class DataContextExtensions
    {
        public static TEntity CreateIfNotExists<TEntity>(this DbSet<TEntity> set,
            TEntity toAdd,
            Expression<Func<TEntity, bool>> predicate = null) where TEntity : class
        {
            var entity = predicate != null ? set.FirstOrDefault(predicate) : set.FirstOrDefault();
            if (entity == null) return set.Add(toAdd).Entity;
            return entity;
        }
    }
}
