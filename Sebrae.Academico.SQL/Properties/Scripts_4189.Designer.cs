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
    public class Scripts_4189 {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Scripts_4189() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Sebrae.Academico.SQL.Properties.Scripts_4189", typeof(Scripts_4189).Assembly);
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
        ///   Looks up a localized string similar to /*
        ///Criar tabela de contabilização de medalhas ganhas por missão
        ///*/
        ///
        ///
        ///-- ----------------------------
        ///-- Table structure for TB_MissaoMedalha
        ///-- ----------------------------
        ///CREATE TABLE [dbo].[TB_MissaoMedalha] (
        ///[ID_MissaoMedalha] int NOT NULL IDENTITY(1,1),
        ///[QT_Medalhas] int NOT NULL ,
        ///[ID_Missao] int NOT NULL ,
        ///[ID_UsuarioTrilha] int NOT NULL ,
        ///[DT_Registro] datetime DEFAULT(getdate()) 
        ///)
        ///
        ///
        ///GO
        ///
        ///-- ----------------------------
        ///-- Indexes structure for table TB_MissaoMedalha
        ///-- ------- [rest of string was truncated]&quot;;.
        /// </summary>
        public static string CriarTabelaMissaoMedalha {
            get {
                return ResourceManager.GetString("CriarTabelaMissaoMedalha", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP TABLE [dbo].[TB_MissaoMedalha]
        ///GO.
        /// </summary>
        public static string RemoverTabelaMissaoMedalha {
            get {
                return ResourceManager.GetString("RemoverTabelaMissaoMedalha", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to -- =============================================
        ///-- Author:		Edgar Froes
        ///-- Create date: 31/08/2017
        ///-- Description:	Obtém as missões existentes e as
        ///--				concluídas pelo usuário no nível.
        ///-- =============================================
        ///CREATE PROCEDURE SP_MOCHILA_STATUS_MISSOES
        ///	@ID_TrilhaNivel INT,
        ///	@ID_UsuarioTrilha INT
        ///AS
        ///	BEGIN
        ///	SELECT
        ///		M.ID_Missao as ID,
        ///		MIN(M.DE_Objetivo) as Nome,
        ///		COUNT(DISTINCT SS_ITP.ID_ItemTrilha) as Concluidas,
        ///		COUNT(SS.ID_ItemTrilha) as Total
        ///	FROM TB_Mis [rest of string was truncated]&quot;;.
        /// </summary>
        public static string SP_MOCHILA_STATUS_MISSOES4189 {
            get {
                return ResourceManager.GetString("SP_MOCHILA_STATUS_MISSOES4189", resourceCulture);
            }
        }
    }
}
