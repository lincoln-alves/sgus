SET WPATH=%~dp0
php -r "require '%WPATH%RemoverMatriculasDuplicadas.php'; cron();"