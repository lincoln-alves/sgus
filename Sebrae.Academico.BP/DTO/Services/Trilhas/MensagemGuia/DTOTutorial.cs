using Sebrae.Academico.Dominio.Enumeracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP.DTO.Services.Trilhas.MensagemGuia
{
    public class DTOTutorial
    {
        private int _Id;
        private string _conteudo;
        private int _ordem;
        private enumCategoriaTrilhaTutorial _categoria;

        public int Id
        {
            get
            {
                return _Id;
            }
        }

        public string Conteudo
        {
            get
            {
                return _conteudo;
            }
        }

        public int Ordem
        {
            get
            {
                return _ordem;
            }
        }

        public enumCategoriaTrilhaTutorial Categoria
        {
            get
            {
                return _categoria;
            }
        }

        public DTOTutorial(int Id, string conteudo, int ordem, enumCategoriaTrilhaTutorial categoria)
        {
            _Id = Id;
            _conteudo = conteudo;
            _ordem = ordem;
            _categoria = categoria;
        }
    }
}
