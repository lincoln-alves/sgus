GO

ALTER TRIGGER [dbo].[TR_TB_Cargo_Update] ON [dbo].[TB_Cargo] AFTER UPDATE AS SET NOCOUNT ON
INSERT INTO HT_Cargo
	(ID_Cargo, NM_Cargo, VL_TipoCargo, ID_CargoPai, IN_Ativo, VL_Order, IN_RegistroExcluido, DT_EventoTrigger)
SELECT
	ID_Cargo, NM_Cargo, VL_TipoCargo, ID_CargoPai, IN_Ativo, VL_Order, '0', GETDATE()
	FROM Deleted

GO

ALTER TRIGGER [dbo].[TR_TB_Cargo_Delete] ON [dbo].[TB_Cargo] FOR DELETE AS SET NOCOUNT ON
INSERT INTO HT_Cargo
	(ID_Cargo, NM_Cargo, VL_TipoCargo, ID_CargoPai, IN_Ativo, VL_Order, IN_RegistroExcluido, DT_EventoTrigger)
SELECT
	ID_Cargo, NM_Cargo, VL_TipoCargo, ID_CargoPai, IN_Ativo, VL_Order, '1', GETDATE()
	FROM Deleted

GO