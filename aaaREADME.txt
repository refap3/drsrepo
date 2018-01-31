Bloody stream URL changed after 9.1.18 .....
----------------------------------------------------

from: mms://apasf.apa.at/oe1_live_worldwide
to: http://mp3ooe1.apasf.sf.apa.at


full URL seen in WM recorda is this: http://mp3ooe1.apasf.sf.apa.at/;stream.mp3


WTF WTF WTF !
----------------------






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
 
 
 
    
 
    
