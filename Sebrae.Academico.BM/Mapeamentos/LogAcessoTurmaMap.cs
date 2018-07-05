using FluentNHibernate.Mapping;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BM.Mapeamentos
{
    public class LGAcessoTurmaMap : ClassMap<LogAcessoTurma>
    {

        public LGAcessoTurmaMap()
        {
            Table("LG_AcessoTurma");
            LazyLoad();
            CompositeId().KeyProperty(x => x.IDMatriculaTurma, "ID_MatriculaTurma")
                         .KeyProperty(x => x.DataAcesso, "DT_Acesso");
        }
    }
}
