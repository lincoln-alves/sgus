using FluentMigrator;

namespace Sebrae.Academico.SQL.Migrations
{
    [Migration(NumerosMigracoes.MudancaFileServerServidor)]
    public class MudancaFileServerProtocolo : Migration
    {
        public override void Down()
        {
            Execute.Sql("DROP TABLE TB_ProtocoloFileServer");
        }

        public override void Up()
        {
            Execute.Sql(@"
                CREATE TABLE TB_ProtocoloFileServer (
	            Id_ProtocoloFileServer INT IDENTITY NOT NULL PRIMARY KEY, 
	            Id_Protocolo INT NOT NULL, 
	            Id_FileServer INT NOT NULL,
	            Id_Usuario INT NOT NULL,
                DT_Envio datetime NOT NULL,
	            FOREIGN KEY (Id_Protocolo) REFERENCES TB_Protocolo(Id_Protocolo) ON DELETE CASCADE,
	            FOREIGN KEY (Id_FileServer) REFERENCES TB_FileServer(Id_FileServer) ON DELETE CASCADE,
	            FOREIGN KEY (Id_Usuario) REFERENCES TB_Usuario(Id_Usuario) ON DELETE CASCADE
                )"
            );

            Execute.Sql("ALTER TABLE [dbo].[TB_FileServer] ADD [ID_ProtocoloFileServer] tinyint NULL");
        }
    }
}
