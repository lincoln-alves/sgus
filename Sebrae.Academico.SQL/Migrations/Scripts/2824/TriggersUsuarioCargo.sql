GO

DROP TRIGGER [dbo].[TR_TB_UsuarioCargo_Delete_copy]

GO

CREATE TRIGGER [dbo].[TR_TB_UsuarioCargo_Delete]
ON [dbo].[TB_UsuarioCargo]
AFTER DELETE
AS
SET NOCOUNT ON
INSERT INTO HT_UsuarioCargo
	(ID_UsuarioCargo, ID_Usuario, ID_Cargo, IN_RegistroExcluido, DT_EventoTrigger)
SELECT
	ID_UsuarioCargo, ID_Usuario, ID_Cargo, '1', GETDATE()
	FROM Deleted
GO

DROP TRIGGER [dbo].[TR_TB_UsuarioCargo_Update_copy]

GO

CREATE TRIGGER [dbo].[TR_TB_UsuarioCargo_Update]
ON [dbo].[TB_UsuarioCargo]
AFTER UPDATE
AS
SET NOCOUNT ON
INSERT INTO HT_UsuarioCargo
	(ID_UsuarioCargo, ID_Usuario, ID_Cargo, IN_RegistroExcluido, DT_EventoTrigger)
SELECT
	ID_UsuarioCargo, ID_Usuario, ID_Cargo, '0', GETDATE()
	FROM Deleted

GO