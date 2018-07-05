ALTER TABLE TB_ItemQuestionario ADD IN_RespostaObrigatoria bit NULL DEFAULT ((1))
GO

UPDATE TB_ItemQuestionario SET IN_RespostaObrigatoria = 1
GO

ALTER TABLE TB_ItemQuestionarioParticipacao ADD IN_RespostaObrigatoria bit NULL DEFAULT ((1))
GO

UPDATE TB_ItemQuestionarioParticipacao SET IN_RespostaObrigatoria = 1
GO
