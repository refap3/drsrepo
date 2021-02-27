# do a partial update of bins from  T: to inetpup ...

cd 'T:\temp\TEMPdrsTESTfiles\drs2.0\'
del C:\inetpub\wwwroot\drs2.0\*.*
del C:\inetpub\wwwroot\drs2.0\bin\*.*
copy .\bin\*.* C:\inetpub\wwwroot\drs2.0\bin
copy  *.aspx C:\inetpub\wwwroot\drs2.0\
copy  *.asmx C:\inetpub\wwwroot\drs2.0\
copy  *.master C:\inetpub\wwwroot\drs2.0\
copy  aa.* C:\inetpub\wwwroot\drs2.0\
copy  .\Web.config C:\inetpub\wwwroot\drs2.0\
