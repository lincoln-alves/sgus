using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Sebrae.Academico.InfraEstrutura.Core.Extensions.Others
{
    internal static class ExpressionHelper
    {
        public static TValue GetValue<TMember, TValue>(Expression<Func<TMember, TValue>> exp, TMember obj)
        {
            return (TValue)ExpressionHelper.GetValue(exp.Body, (object)obj);
        }

        private static object GetValue(Expression expression, object value)
        {
            if (value == null)
                return (object)null;
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    MemberExpression memberExpression = (MemberExpression)expression;
                    object instance1 = ExpressionHelper.GetValue(memberExpression.Expression, value);
                    if (instance1 != null)
                        return ExpressionHelper.GetValue(memberExpression.Member, instance1);
                    else
                        return (object)null;
                case ExpressionType.Parameter:
                    return value;
                case ExpressionType.Call:
                    MethodCallExpression methodCallExpression = (MethodCallExpression)expression;
                    if (!ExpressionHelper.SupportsMethod(methodCallExpression))
                        throw new NotSupportedException((string)(object)methodCallExpression.Method + (object)" is not supported");
                    object instance2 = ExpressionHelper.GetValue(methodCallExpression.Method.IsStatic ? methodCallExpression.Arguments[0] : methodCallExpression.Object, value);
                    if (instance2 != null)
                        return ExpressionHelper.GetValue((MethodBase)methodCallExpression.Method, instance2);
                    else
                        return (object)null;
                case ExpressionType.Convert:
                    UnaryExpression unaryExpression = (UnaryExpression)expression;
                    return ExpressionHelper.Convert(unaryExpression.Type, ExpressionHelper.GetValue(unaryExpression.Operand, value));
                default:
                    throw new MemberException();
            }
        }

        private static object Convert(Type type, object value)
        {
            return Expression.Lambda((Expression)Expression.Convert((Expression)Expression.Constant(value), type), new ParameterExpression[0]).Compile().DynamicInvoke(new object[0]);
        }

        private static object GetValue(MemberInfo memberInfo, object instance)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).GetValue(instance);
                case MemberTypes.Method:
                    return ExpressionHelper.GetValue((MethodBase)memberInfo, instance);
                case MemberTypes.Property:
                    return ExpressionHelper.GetValue((PropertyInfo)memberInfo, instance);
                default:
                    throw new MemberException();
            }
        }

        private static object GetValue(PropertyInfo propertyInfo, object instance)
        {
            return propertyInfo.GetGetMethod(true).Invoke(instance, (object[])null);
        }

        private static object GetValue(MethodBase method, object instance)
        {
            if (!method.IsStatic)
                return method.Invoke(instance, (object[])null);
            return method.Invoke((object)null, new object[1]
      {
        instance
      });
        }

        private static bool SupportsMethod(MethodCallExpression methodCallExpression)
        {
            if (!methodCallExpression.Method.IsStatic || methodCallExpression.Arguments.Count != 1)
                return methodCallExpression.Arguments.Count == 0;
            else
                return true;
        }

        private static MemberInfo GetMember(Expression exp)
        {
            if (exp is MemberExpression)
                return ((MemberExpression)exp).Member;
            if (exp is UnaryExpression)
                return ((MemberExpression)((UnaryExpression)exp).Operand).Member;
            else
                throw new MemberException();
        }

        internal static MemberInfo GetMember<TMember>(Expression<Func<TMember>> exp)
        {
            return ExpressionHelper.GetMember(exp.Body);
        }

        internal static MemberInfo GetMember<TClass, TMember>(Expression<Func<TClass, TMember>> exp)
        {
            return ExpressionHelper.GetMember(exp.Body);
        }
    }
}
