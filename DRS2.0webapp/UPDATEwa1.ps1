# do a partial update of bins to T: intermediate drive 

cd 'K:\My Projects here\DRS2.0\DRS2.0webapp\'
del T:\temp\TEMPdrsTESTfiles\drs2.0\*.*
del T:\temp\TEMPdrsTESTfiles\drs2.0\bin\*.*
copy .\bin\*.* T:\temp\TEMPdrsTESTfiles\drs2.0\bin\
copy  *.aspx T:\temp\TEMPdrsTESTfiles\drs2.0\
copy  *.asmx T:\temp\TEMPdrsTESTfiles\drs2.0\
copy  *.master T:\temp\TEMPdrsTESTfiles\drs2.0\
copy  aa.* T:\temp\TEMPdrsTESTfiles\drs2.0\
copy  .\Web.config T:\temp\TEMPdrsTESTfiles\drs2.0\
copy .\UPDATEwa2.ps1 T:\temp\TEMPdrsTESTfiles\drs2.0\