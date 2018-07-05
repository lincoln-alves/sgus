::git reset --hard & git pull & git checkout develop & git pull origin develop & 
msbuild deploy.proj /p:VisualStudioVersion=12.0 /nologo /verbosity:minimal /clp:ErrorsOnly
echo %errorlevel%