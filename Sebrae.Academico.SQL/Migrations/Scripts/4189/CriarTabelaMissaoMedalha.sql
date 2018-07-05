/*
Criar tabela de contabilização de medalhas ganhas por missão
*/


-- ----------------------------
-- Table structure for TB_MissaoMedalha
-- ----------------------------
CREATE TABLE [dbo].[TB_MissaoMedalha] (
[ID_MissaoMedalha] int NOT NULL IDENTITY(1,1),
[QT_Medalhas] int NOT NULL ,
[ID_Missao] int NOT NULL ,
[ID_UsuarioTrilha] int NOT NULL ,
[DT_Registro] datetime DEFAULT(getdate()) 
)


GO

-- ----------------------------
-- Indexes structure for table TB_MissaoMedalha
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table TB_MissaoMedalha
-- ----------------------------
ALTER TABLE [dbo].[TB_MissaoMedalha] ADD PRIMARY KEY ([ID_MissaoMedalha])
GO

-- ----------------------------
-- Foreign Key structure for table [dbo].[TB_MissaoMedalha]
-- ----------------------------
ALTER TABLE [dbo].[TB_MissaoMedalha] ADD FOREIGN KEY ([ID_Missao]) REFERENCES [dbo].[TB_Missao] ([ID_Missao]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO
ALTER TABLE [dbo].[TB_MissaoMedalha] ADD FOREIGN KEY ([ID_UsuarioTrilha]) REFERENCES [dbo].[TB_UsuarioTrilha] ([ID_UsuarioTrilha]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO
