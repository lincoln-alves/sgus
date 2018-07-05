GO

ALTER TRIGGER [dbo].[TR_TB_UsuarioCertificadoCertame_Delete] ON [dbo].[TB_UsuarioCertificadoCertame] AFTER DELETE AS SET NOCOUNT ON
INSERT INTO HT_UsuarioCertificadoCertame
(
	ID_CertificadoCertame,
	ID_Usuario,
	VL_ChaveAutenticacao,
	DT_EventoTrigger,
	IN_RegistroExcluido,
	DT_DataDownload,
    VL_Status,
    VL_Nota
)
SELECT 
	ID_CertificadoCertame,
	ID_Usuario,
	VL_ChaveAutenticacao,
	GETDATE(),
	'1',
	DT_DataDownload,
    VL_Status,
    VL_Nota
FROM Deleted;

GO

ALTER TRIGGER [dbo].[TR_TB_UsuarioCertificadoCertame_Update] ON [dbo].[TB_UsuarioCertificadoCertame] AFTER UPDATE AS SET NOCOUNT ON
INSERT INTO HT_UsuarioCertificadoCertame
(
	ID_CertificadoCertame,
	ID_Usuario,
	VL_ChaveAutenticacao,
	DT_EventoTrigger,
	IN_RegistroExcluido,
	DT_DataDownload,
    VL_Status,
    VL_Nota
)
SELECT 
	ID_CertificadoCertame,
	ID_Usuario,
	VL_ChaveAutenticacao,
	GETDATE(),
	'0',
    DT_DataDownload,
    VL_Status,
    VL_Nota
FROM Deleted;

GO