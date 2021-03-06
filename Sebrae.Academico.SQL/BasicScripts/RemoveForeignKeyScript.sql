DECLARE @CONSTRAINT_NAME VARCHAR(255) = 
	(
		SELECT C.CONSTRAINT_NAME
		FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C
		INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS FK ON C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME
		INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK ON C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME
		INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU ON C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME
		WHERE FK.TABLE_NAME = '#TABLE_NAME#' AND CU.COLUMN_NAME = '#COLUMN_NAME#'
	)

EXEC ('ALTER TABLE #TABLE_NAME# DROP CONSTRAINT ' + @CONSTRAINT_NAME)