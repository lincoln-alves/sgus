﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sebrae.Academico.SQL.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Scripts_3400 {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Scripts_3400() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Sebrae.Academico.SQL.Properties.Scripts_3400", typeof(Scripts_3400).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GO
        ///INSERT INTO TB_ProdutoSebrae (NM_Nome) VALUES (&apos;PDF&apos;);
        ///GO
        ///INSERT INTO TB_ProdutoSebrae (NM_Nome) VALUES (&apos;AOE&apos;);
        ///GO
        ///INSERT INTO TB_ProdutoSebrae (NM_Nome) VALUES (&apos;DET&apos;);
        ///GO
        ///INSERT INTO TB_ProdutoSebrae (NM_Nome) VALUES (&apos;ALI&apos;);
        ///GO
        ///INSERT INTO TB_ProdutoSebrae (NM_Nome) VALUES (&apos;Boas Práticas de Gestão de Estoque&apos;);
        ///GO
        ///INSERT INTO TB_ProdutoSebrae (NM_Nome) VALUES (&apos;Crescer com Design&apos;);
        ///GO
        ///INSERT INTO TB_ProdutoSebrae (NM_Nome) VALUES (&apos;Design de Embalagem para Artesanato&apos;);
        ///GO
        ///INSERT INT [rest of string was truncated]&quot;;.
        /// </summary>
        public static string CommitCriarEstruturaProdutoSebrae {
            get {
                return ResourceManager.GetString("CommitCriarEstruturaProdutoSebrae", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM TABLE TB_Cargo WHERE NM_Cargo = &quot;Sebrae UF&quot;.
        /// </summary>
        public static string ExcluindoCargo {
            get {
                return ResourceManager.GetString("ExcluindoCargo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO TB_Cargo (NM_Cargo, IN_ATIVO, VL_Sigla, ID_UF, VL_TipoCargo) 
        ///values(&apos;Sebrae UF&apos;, 1, &apos;Sebrae UF&apos;, 1, 0).
        /// </summary>
        public static string IncluindoCargoSebraeUF {
            get {
                return ResourceManager.GetString("IncluindoCargoSebraeUF", resourceCulture);
            }
        }
    }
}
