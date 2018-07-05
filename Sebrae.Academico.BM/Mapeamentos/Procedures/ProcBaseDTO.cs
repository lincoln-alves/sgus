using System.Data;
using System.Data.SqlClient;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Mapeamentos.Procedures
{
    public abstract class ProcBaseDTO<T> : ProcBase<T>
        where T : class
    {

        protected abstract T ObterObjetoDTO(IDataReader dr);
 
    }
}
