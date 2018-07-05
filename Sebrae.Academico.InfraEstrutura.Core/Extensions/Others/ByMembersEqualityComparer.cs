using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Sebrae.Academico.InfraEstrutura.Core.Extensions.Others
{
    public class ByMembersEqualityComparer<TClass> : IEqualityComparer<TClass>
    {
        private readonly Expression<Func<TClass, object>>[] _expressions;

        public ByMembersEqualityComparer(params Expression<Func<TClass, object>>[] expressions)
        {
            this._expressions = expressions;
        }

        public bool Equals(TClass x, TClass y)
        {
            if (!Enumerable.Any<Expression<Func<TClass, object>>>((IEnumerable<Expression<Func<TClass, object>>>)this._expressions))
                return object.Equals((object)x, (object)y);
            foreach (Expression<Func<TClass, object>> exp in this._expressions)
            {
                if (!object.Equals(ExpressionHelper.GetValue<TClass, object>(exp, x), ExpressionHelper.GetValue<TClass, object>(exp, y)))
                    return false;
            }
            return true;
        }

        public int GetHashCode(TClass obj)
        {
            if (object.Equals((object)default(TClass), (object)obj))
                return 0;
            int num = 0;
            foreach (Expression<Func<TClass, object>> exp in this._expressions)
            {
                object obj1 = ExpressionHelper.GetValue<TClass, object>(exp, obj);
                if (obj1 != null)
                    num ^= obj1.GetHashCode();
            }
            return num;
        }
    }

}
