CREATE TABLE [dbo].[TB_EtapaEncaminhamentoUsuario] (
[ID_EtapaEncaminhamentoUsuario] int NOT NULL IDENTITY(1,1) ,
[ID_EtapaResposta] int NOT NULL ,
[ID_EtapaPermissaoNucleo] int NOT NULL ,
[ID_UsuarioEncaminhamento] int NOT NULL ,
[IN_StatusEncaminhamento] int NOT NULL DEFAULT ((0)) ,
[DT_SolicitacaoEncaminhamento] datetime NULL ,
[TX_Justificativa] text COLLATE Latin1_General_CI_AI NULL ,
[DT_UltimaAtualizacao] datetime NULL ,
[NM_UsuarioAtualizacao] varchar(255) COLLATE Latin1_General_CI_AI NULL ,
CONSTRAINT [PK__TB_Etapa__A0F294016287198E] PRIMARY KEY ([ID_EtapaEncaminhamentoUsuario]),
CONSTRAINT [fk_EtapaPermissaoNucleo_EtapaEncaminhamentoUsuario] FOREIGN KEY ([ID_EtapaPermissaoNucleo]) REFERENCES [dbo].[TB_EtapaPermissaoNucleo] ([ID_EtapaPermissaoNucleo]) ON DELETE CASCADE ON UPDATE CASCADE,
CONSTRAINT [fk_EtapaResposta_EtapaEncaminhamentoUsuario] FOREIGN KEY ([ID_EtapaResposta]) REFERENCES [dbo].[TB_EtapaResposta] ([ID_EtapaResposta]) ON DELETE CASCADE ON UPDATE CASCADE,
CONSTRAINT [fk_Usuario_EtapaEncaminhamentoUsuario] FOREIGN KEY ([ID_UsuarioEncaminhamento]) REFERENCES [dbo].[TB_Usuario] ([ID_Usuario]) ON DELETE CASCADE ON UPDATE CASCADE
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

CREATE UNIQUE INDEX [ix_EtapaResposta_EtapaPermissaoNucleo] ON [dbo].[TB_EtapaEncaminhamentoUsuario]
([ID_EtapaResposta] ASC, [ID_EtapaPermissaoNucleo] ASC) 
WITH (IGNORE_DUP_KEY = ON)
ON [PRIMARY]
GO