DECLARE @NAME NVARCHAR(MAX) = (SELECT TOP 1 NAME FROM sys.objects WHERE object_id = OBJECT_ID(N'#SCRIPT_NAME#') AND TYPE IN ( N'P', N'PC' ));
DECLARE @DROP_COMMAND NVARCHAR(MAX) = 'DROP PROCEDURE ' + @NAME;
IF @NAME IS NOT NULL BEGIN EXECUTE sp_executesql @DROP_COMMAND END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

