# Definições de uso do projeto

Esse projeto deve obrigatoriamente utilizar o git flow descrito no site http://danielkummer.github.io/git-flow-cheatsheet/index.pt_BR.html e executado via a ferramenta source tree via botão do Git Flow.

Assim funcionalidades ficaram dentro de features com o nome sendo o número do redmine e o código só irá para produção quando for liberada uma release ou hotfix.

# Para instalar.

git clone https://gitlab.sebrae.com.br/na-dotnet/sgus.git

# Configurações não versionadas do web.config

## cstrings.config

O **cstrings.config** armazena a string de conexão, que não deve ser versionada.

Entre nas seguintes pastas e crie uma arquivo em cada um chamado *cstrings.config*:



```
Sebrae.Academico.WebForms
Services\Sebrae.Academico.Trilhas
Sebrae.Academico.Services
Sebrae.Academico.SQL
Sebrae.Academico.Test
```

O conteúdo do arquivo **cstrings.config** é o seguinte:

```xml
<?xml version="1.0" encoding="utf-8"?>
<connectionStrings>
  <add name="cnxSebraeAcademico" connectionString="Data Source=THOR\SEBRAE;Initial Catalog=UCSEBRAE_SGUS20_TRILHAS;User ID=sistema.sebrae;Password=ratpack@2000#;MultipleActiveResultSets=True; "/>
  <add name="cnxMoodle" connectionString="server=dbdev.intranet.ice;User Id=phpuser;database=UCSEBRAE_MOODLE25; password=ratpack;"/>
  <add name="cnxPortal" connectionString="server=dbdev.intranet.ice;User Id=phpuser;database=ucsebrae_portal30; password=ratpack;"/>
</connectionStrings>
```

## local.config

O **local.config** armazena configurações que não são versionadas.

Entre na pasta **Sebrae.Academico.WebForms** e crie um arquivo chamado *local.config* com o conteúdo abaixo:

```xml
<?xml version="1.0" encoding="utf-8"?>
<appSettings>
  <add key="ChartImageHandler" value="storage=file;timeout=20;Url=~/ChartImages/;"/>
  <add key="enviaremailcomErros" value="S"/>
  <add key="emaildedestinodoErro" value="desenvolvimento@icomunicacao.com.br"/>
  <add key="aspnet:MaxHttpCollectionKeys" value="10000"/>
  <add key="robo_sync" value="http://homolog.moodle25.icomunicacao.com.br/robo_sync/?&amp;nome=sebrae&amp;senha=Sebrae2022&amp;op={0}&amp;format=json"/>
  <add key="portal_url_node_id" value="http://homolog.portal20.icomunicacao.com.br/soap-test"/>
  
  <!--#1796 Keys customizadas para login automático-->
  <add key="debugLocal" value="N"/>
  <add key="localUser" value=""/>
</appSettings>
```