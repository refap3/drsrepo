27.02.21
--------------------------

Managed to find out why deployment on VM would not run, did this: 

1. Build only from x86 CPU
2. install AccessDatabaseEngine.exe (x86 NOT x64!!)
3. in IIS appPool enable 32-bit applications 
4. in AppPool sed identity to administrator -- i know it sucks !
5. AppPool: V4, integrated ....



25.02.21
--------------------------------
managed to run IISexpress on port 80 BUT no foccing debugger in Webapp?!?!?!?
see this link: https://docs.microsoft.com/en-us/iis/extensions/using-iis-express/handling-url-binding-failures-in-iis-express 
essentially i used the command: netsh http add urlacl url=http://localhost:80/ user=everyone
then the project ran on port 80 ?!?!?!?

created drs3.1 (3.0 was debug focced) branch since bloody ORF switched from static player url to js player .... WTF

a possible path to the new solution looks like this: 
-------------------------------------------------------

0. use Windows Server 2012 R2 ...
1. use a fixed url (live OE1 player) and a browser that autoplays from there (i.e. Opera) https://oe1.orf.at/player/live
2. from a background job start and stop the browser at the saved points in time
3. install media center (25) and WDM driver
4. make MC default speaker and define default play zone as diskwriter
5. convert .wav files to MP3(192kbit) [or FLAC] 
6. rename MP3 files according to schedule and move them to target directory

alternatives
-------------

1. leave web site as IS and create schedule file from XP Machine 
2. add a job to read and handle schedule file 

pro: easier to implement
con: need 2 vms for recording




4.1.20
----------------------------

reactivated CLONE aka record everything ... 
could ot get past security dialog so i created a .btm to handle it
also drs2 connection pointed to some old shit




29.4.18
--------------------------------

Looks like oe1.orf.at switched to https only. 
changed the BASE URL in functions and installed SP3 on WMRECORDA VM




Bloody stream URL changed after 9.1.18 .....
----------------------------------------------------

from: mms://apasf.apa.at/oe1_live_worldwide
to: http://mp3ooe1.apasf.sf.apa.at

Foccing bloody stream changed 19.3.19 because of HISS and CLICKS -- dunno if it helps though!
------------------------------------------------------------------------------------------------------

new: http://mp3stream3.apasf.apa.at/;stream.mp3


full URL seen in WM recorda is this: http://mp3ooe1.apasf.sf.apa.at/;stream.mp3


WTF WTF WTF !
----------------------

also the output file changed to .mp3 from .asf wtf! again 

looks like wmrecorda file are in sync now ... 
test does NOT work !?!?!?


Moved the focc to GITHUB after 2017 web redesign 3.5.2017
---------------------------------------------------------

heres what changed: 

[K:\My Projects here\DRS2.0]gpl
Updating 59ccc43..15c5812
Fast-forward
 DRS2.0/DRS2.0.vbproj                   |    1 +
 DRS2.0Lib/DRS2.0Lib/Functions.vb       |  104 +-
 RegExTEst/Module1.vb                   |    2 +-
 RegExTEst/oe1_ORF_at - before 2017.htm | 2336 +++++++++++++++++
 RegExTEst/oe1_ORF_at.htm               | 4369 +++++++++++++++-----------------
 RegExTEst/oe1_ORF_at_detail.htm        |  374 +++
 6 files changed, 4805 insertions(+), 2381 deletions(-)
 create mode 100644 RegExTEst/oe1_ORF_at - before 2017.htm
 create mode 100644 RegExTEst/oe1_ORF_at_detail.htm
 
 essentially only Functions.vb -- so it should do to replace this one file !
 ------------------------
 
 
 



BLOODY blues and troubles collected here 
---------------------------------------------

Source control  is still on deprec .... 


HOWTOdeploy web APP f.ckr
--------------------------

  1. Build the web app to file system i.e. c:\temp\drs2.0 
  2. zip everything and check that bin and App_dat folder present 
  3. on TARGET:
    ö delete everything below inetpub\wwwroot\drs2.0 
	ö ad new files from zip 
	ö on DRS2.0 dir: 
	  give ASPNET user fullcontrol
	  goto advanced
	  check inherit permissions and replace below
	  click apply 
	  (fckin cacls script does NOT really werke!)
 4. mite have to copy new .EXEs and .DLLs to \4dos directory 
 5. Complete Testplan:
   ö check if DRS2.0 works
   ö check iff entries in editeur can be edited/deleted
   ö check iff correct entries are seen both in DRS2.0 as in WebApp!
   ö reboot ...
timesync.btm is a new sync script since the old mechanism did not seem to work !
10:34 06.06.2013

   
   
    



BLOODY SOURCE SAFE 
-----------------------------------

set the working folder in DRS2.0 AND NOT IN DRS2.0ROOT
                          =============================

be SURE ALWAYS to check into Source SAFE the LOCAL folders in MyDocs ARE NOT THE SOURCE !!






1. check out to loacal dir NOT on MyDocs folder and work there !
   check out recursive and build tree !
   check out to c:\temp -- this will ad subdir drs2.0 THEN 
   
2. make access db writeable 
3. on WMRECORDA on each f.ckin deployment need to set the ASPNET user 
   security to full control !!
   since all is overwritten you have to do this for all writeable access !
   
4. on a new PUBLISH all is overwritten at the target -- the mdb DB for example !
   this is NOT GOODE 
   
   ---> create a PROPER setup project 
   NO run aspnetsec.bat after deployment or so 
   
   DO EXACTLY this:
   
   -----------------------------------
   -----------------------------------
   !                                  !
   ! NEVER EVER Publish to wmrecorda  !
   ! this will kill the MDB !!!       !
   !                                  !
   -----------------------------------
   -----------------------------------
   
   
   BUILD new project on local IIS (not on the f.ckin Cassini Web Server) 
   run partialUPDATEwebsite.btm
   run aspnetsec.bat on wmrecorda machine 
   
   
5. In Source Safe even the object libraries are stored - can we remove this - this is TRASH !!

6. LIST OF CURRENT BUGz
-----------------------------------------------------------------

  ö cannot edit wmrecorda entry. This is made up at insert time - no goode
  
  ö schedWatcher kept disappearing after changes to thread handling during clock sync.
  removed thread handling again and corrected bugs in Sync class maybe this will help
  NO id did NOT - re introduced seperate threads ...  
  
  ö Lost last version of .mdb DUE to bloody PUBLISHING !
  
  ö add the SAUGALLYOUCAN mode 
    DONE is in Testing on WMRECORDA
  
  ö exclude.txt wird per filename alleine gelsesn - dasn geht NIE so bei WinApp und WebApp
    im Augenblick sind das 2 VERSCHIEDENE Dateien !!
    --------------------------------
    CORRECT THIS SOMETIME !!!!
    --------------------------------
    
   
   
-- more to come i guess !



7. HOWTO Config for AUTOEXEC 
-------------------------------------------------

  Autologon & AUTOSTART these apps: 
  -----------------------------------
    BGInfo 
    DRS2.0 
    WMR 11
    schedWatcher
    aspnetsec.bat 
    
  Rebooting: 
  -----------
    daily 6:15 via rebuht.bat file, this looks like this: 
    
    pskill schedWatcher
    pskill WMR11
    psshutdown -r -e p:1:1
    
 Configure DRS2.0 options as this: 
 ------------------------------------
 
    AUTOEXEC = true
    DaysaheadInAutoexec = 3 (3 days will be pregenerated) 
    FILESERVINGHOST = wmrecorda 
    RECORDERHOST = wmrecorda 
    
 WMR11 bug/config prob 
 -------------------------------
    be SURE that in settings // Recording Mode // AutoRecord 
    is cleared !! OTHERWISE WMP starts during recording. plays etc ..
    symptom is that files are named 1.asf 
    
 finally DO HAVE faith !
 
 
 
    
 
    
