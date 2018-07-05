SET WPATH=%~dp0
php -r "require '%WPATH%cron.php'; cron();"