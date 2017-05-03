rem run this to set aspnet protection and security for app data dir
cacls c:\Inetpub\wwwroot\drs2.0\ /E /G aspnet:f /T
attrib -R c:\Inetpub\wwwroot\drs2.0\App_Data\DRS2.0.mdb
rem pause

