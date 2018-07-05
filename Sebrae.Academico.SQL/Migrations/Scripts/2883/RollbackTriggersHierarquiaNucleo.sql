GO

ALTER TRIGGER [dbo].[TR_TB_HierarquiaNucleo_Update] ON [dbo].[TB_HierarquiaNucleo] AFTER UPDATE AS SET NOCOUNT ON
INSERT INTO HT_HierarquiaNucleo
(
	ID_HierarquiaNucleo,
	NM_HierarquiaNucleo,
	DT_EventoTrigger,
	IN_RegistroExcluido
)
SELECT 
	ID_HierarquiaNucleo,
	NM_HierarquiaNucleo,
	GETDATE(),
	'0'
FROM Deleted;

GO

ALTER TRIGGER [dbo].[TR_TB_HierarquiaNucleo_Delete] ON [dbo].[TB_HierarquiaNucleo] FOR DELETE AS SET NOCOUNT ON
INSERT INTO HT_HierarquiaNucleo
(
	ID_HierarquiaNucleo,
	NM_HierarquiaNucleo,
	DT_EventoTrigger,
	IN_RegistroExcluido
)
SELECT 
	ID_HierarquiaNucleo,
	NM_HierarquiaNucleo,
	GETDATE(),
	'1'
FROM Deleted;

GO