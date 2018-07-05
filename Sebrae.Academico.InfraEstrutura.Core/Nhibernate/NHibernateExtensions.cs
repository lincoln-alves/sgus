using System;
using System.Collections.Generic;

namespace Sebrae.Academico.InfraEstrutura.Core.Nhibernate
{
    public static class NHibernateExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
	        {
		        yield return element;
	        }
        }
    }
}
