﻿DECLARE @CONSTRAINT_NAME VARCHAR(255) = 
	(
		SELECT CONSTRAINT_NAME
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE TABLE_NAME LIKE 'TB_HierarquiaNucleo' AND CONSTRAINT_TYPE LIKE 'PRIMARY KEY'
	)

EXEC ('ALTER TABLE TB_HierarquiaNucleo DROP CONSTRAINT ' + @CONSTRAINT_NAME)