using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Common.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> queryable, bool condition, Expression<Func<T, bool>> predicate) =>
            condition ? queryable.Where(predicate) : queryable;

        public static IEnumerable<T> Where<T>(this IEnumerable<T> enumerable, bool condition, Func<T, bool> predicate) =>
            condition ? enumerable.Where(predicate) : enumerable;
    }
}
