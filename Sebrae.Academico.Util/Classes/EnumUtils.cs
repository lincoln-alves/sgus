using System;
using System.ComponentModel;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.Util.Classes
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Obtém a descrição de uma propriedade de um enum fazendo uso do Data Attribute [Description("Texto")]. Por exemplo: uma propriedade chamada FooDescricao com a notação [Description("Foo descrição")] vai retornar a string "Foo descrição", que poderá ser usado como um Label deste enum.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum element)
        {
            var type = element.GetType();

            var memberInfo = type.GetMember(element.ToString());

            if (memberInfo.Length <= 0) return element.ToString();

            var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? ((DescriptionAttribute) attributes[0]).Description : element.ToString();
        }
    }
}