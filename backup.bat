SET mypath=%~dp0
CD %mypath:~0,-1%
SET "SOLUTION_FILE_PATH=%CD%\Sebrae.Academico.sln"
SET "BUILD_LOCATION=%CD%\BUILDS"

@echo off
for /f "tokens=2 delims==" %%a in ('wmic OS Get localdatetime /value') do set "dt=%%a"
set "YY=%dt:~2,2%" & set "YYYY=%dt:~0,4%" & set "MM=%dt:~4,2%" & set "DD=%dt:~6,2%"
set "HH=%dt:~8,2%" & set "Min=%dt:~10,2%" & set "Sec=%dt:~12,2%"
set "fullstamp=%DD%%MM%%YYYY%%HH%%Min%%Sec%"


ROBOCOPY "%CD%"/"BUILDS/Sebrae.Academico.Services "%CD%"/"BUILDS/Backups/Sebrae.Academico.Services/%fullstamp% /e
ROBOCOPY "%CD%"/"BUILDS/Sebrae.Academico.UI "%CD%"/"BUILDS/Backups/Sebrae.Academico.UI/%fullstamp% /e
ROBOCOPY "%CD%"/"BUILDS/Sebrae.Academico.WebForms "%CD%"/"BUILDS/Backups/Sebrae.Academico.WebForms/%fullstamp% /e


ROBOCOPY "%CD%"/BUILDS/CRON "%CD%"/BUILDS/Backups/CRON/%fullstamp% /e
ROBOCOPY "%CD%"/CRON "%CD%"/BUILDS/CRON /e