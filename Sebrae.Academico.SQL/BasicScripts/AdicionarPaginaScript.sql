DECLARE @Left INT = (#EXPRESSAO#);

UPDATE
    Table_A
SET
    Table_A.QT_Left = Table_A.QT_Left + 2
FROM
    TB_Pagina AS Table_A
WHERE
    Table_A.QT_Left >= @Left
	AND (Table_A.TX_CaminhoRelativo IS NULL OR Table_A.TX_CaminhoRelativo NOT LIKE '#CAMINHO_NOVA_PAGINA#')

UPDATE
    Table_A
SET
    Table_A.QT_Right = Table_A.QT_Right + 2
FROM
    TB_Pagina AS Table_A
WHERE
    Table_A.QT_Right >= @Left
	AND (Table_A.TX_CaminhoRelativo IS NULL OR Table_A.TX_CaminhoRelativo NOT LIKE '#CAMINHO_NOVA_PAGINA#')