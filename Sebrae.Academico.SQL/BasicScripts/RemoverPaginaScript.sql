DECLARE @Left INT = (#EXPRESSAO#);

UPDATE
    Table_A
SET
    Table_A.QT_Left = Table_A.QT_Left - 2
FROM
    TB_Pagina AS Table_A
WHERE
    Table_A.QT_Left >= @Left

UPDATE
    Table_A
SET
    Table_A.QT_Right = Table_A.QT_Right - 2
FROM
    TB_Pagina AS Table_A
WHERE
    Table_A.QT_Right >= @Left