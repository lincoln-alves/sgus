UPDATE ER
SET ER.ID_Cargo = UC.ID_Cargo
FROM TB_EtapaResposta ER
INNER JOIN TB_ProcessoResposta PR ON ER.ID_ProcessoResposta = PR.ID_ProcessoResposta
INNER JOIN TB_UsuarioCargo UC ON UC.ID_UsuarioCargo = PR.ID_UsuarioCargo
WHERE PR.ID_UsuarioCargo IS NOT NULL