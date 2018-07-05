using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Sebrae.Academico.InfraEstrutura.Core.Extensions.Others;

namespace Sebrae.Academico.InfraEstrutura.Core.Extensions.Linq
{
    public static class LinqExtensions
    {
        public static void Foreach<TClass>(this IEnumerable<TClass> collection, Action<TClass> action)
        {
            foreach (TClass @class in collection)
                action(@class);
        }

        public static IEnumerable<TClass> Distinct<TClass>(this IEnumerable<TClass> collection, params Expression<Func<TClass, object>>[] expressions)
        {
            return Enumerable.Distinct<TClass>(collection, (IEqualityComparer<TClass>)new ByMembersEqualityComparer<TClass>(expressions));
        }

        public static IEnumerable<TClass> Union<TClass>(this IEnumerable<TClass> collectionA, IEnumerable<TClass> collectionB, params Expression<Func<TClass, object>>[] expressions)
        {
            return Enumerable.Union<TClass>(LinqExtensions.Matherialize<TClass>(collectionA), LinqExtensions.Matherialize<TClass>(collectionB), (IEqualityComparer<TClass>)new ByMembersEqualityComparer<TClass>(expressions));
        }

        public static IEnumerable<TClass> Intersect<TClass>(this IEnumerable<TClass> collectionA, IEnumerable<TClass> collectionB, params Expression<Func<TClass, object>>[] expressions)
        {
            return Enumerable.Intersect<TClass>(collectionA, collectionB, (IEqualityComparer<TClass>)new ByMembersEqualityComparer<TClass>(expressions));
        }

        public static IEnumerable<TClass> Except<TClass>(this IEnumerable<TClass> collectionA, IEnumerable<TClass> collectionB, params Expression<Func<TClass, object>>[] expressions)
        {
            return Enumerable.Except<TClass>(collectionA, collectionB, (IEqualityComparer<TClass>)new ByMembersEqualityComparer<TClass>(expressions));
        }

        public static IEnumerable<TClass> Matherialize<TClass>(this IEnumerable<TClass> collection)
        {
            return (IEnumerable<TClass>)(collection as ICollection<TClass>) ?? (IEnumerable<TClass>)Enumerable.ToArray<TClass>(collection);
        }
    }
}
