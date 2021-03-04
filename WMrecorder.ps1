# WM recorder in powershell ...
#
$BuildNumber=1.35
# Setup adjust recording time range ...
# Grace: accept if record start is before now - grace
# Before: Begin recording after before seconds, ie. 5: 5 secs later 
# After: Time to record AFTER EndTime

$secondsGrace = -30
$secondsBefore=5 
$secondsAfter=30


$BrowserName = "opera"
$schedFile="c:\temp\scheduled.csv"

# add debug view support via this: 



$WinAPI = @"
    public class WinAPI
    {
    [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
    public static extern void OutputDebugString(string message);
    }
"@

Add-Type $WinAPI -Language CSharp


function Write-DbgView ($s)
{
  [WinAPI]::OutputDebugString($s)
}

function echo_debug ($s)
{
  echo $s
  Write-DbgView $s
}

# cleanup from possible previous crash ....
ps $BrowserName -ErrorAction SilentlyContinue|kill -ErrorAction SilentlyContinue

Try
{
  while ($true) # use another outer loop for break inside inner loop 
  {
    while ($true)
    {

      Write-DbgView "$BuildNumber :refresh SCHEDULE file "
      if ((Test-Path $schedFile) -eq $false) { Write-DbgView "no schedule found, wait & retry";sleep 10 ; break}
      $wmrecs =Import-Csv $schedFile -Delimiter ";" | `
      Select @{Name="RecordingTime";Expression={(Get-date $_.RecordingTime)}}, @{Name="EndTime";Expression={(get-date $_.EndTime)}}, @{Name="Length";Expression={[int32]$_.Length}}, Filename, Link, @{Name="AirTime";Expression={(Get-date $_.AirTime)}} | where Recordingtime -gt ((get-date).AddSeconds($secondsGrace)) | sort Recordingtime 
      #  if ($wmrecs.Length -gt 0 ) {echo "$($wmrecs.length) recording(s) ... Next @ $($wmrecs[0].RecordingTime) to $($wmrecs[0].EndTime.Hour.ToString('00')):$($wmrecs[0].EndTime.Minute.ToString('00')) - $($wmrecs[0].FileName)" }
  
      # find the 1st recordable entry  and display it, focc off iff nothing ....
      if ($wmrecs -ne $null) 
      {
        $nextRec = $wmrecs[0]
        $ssecs=(($nextRec.RecordingTime) - (get-date)).totalseconds + $secondsBefore
        $smins=$ssecs/60
    
        # if enough time until next recording sleep and re-read schedule again 
        if ($smins -gt 10) {Write-DbgView "$($smins.tostring('0')) mins to go for $($nextRec.Filename) ..." ; sleep 60 ; break }
    
        echo_debug "V: $BuildNumber-will record this: $nextRec in $($smins.tostring('0')) minutes."
 
        if ($ssecs -gt 0 ) {sleep  $ssecs } # sleep until start time 
        # compute maximum sec after recording ... Problem if next Recording starts @ this.EndTime ....
        $secondsAfterUsed=$secondsAfter
        if ($wmrecs.length -gt 1) 
        { 
          if ($nextRec.EndTime -eq $wmrecs[1].RecordingTime)
          {
            $secondsAfterUsed=0
            # echo "end: $($nextRec.EndTime) nextrec: $($wmrecs[1].RecordingTime)"
          }
        }
        echo_debug "recording NOW: $($nextRec.RecordingTime) to $($nextRec.EndTime.Hour.ToString('00')):$($nextRec.EndTime.Minute.ToString('00')) - $($nextRec.FileName) with slack: $secondsAfterUsed"
        start $nextRec.Link
        sleep ((($nextRec.EndTime) - (get-date)).totalseconds + $secondsAfterUsed) # sleep until end time 
      
        ps $BrowserName -ErrorAction SilentlyContinue|kill -ErrorAction SilentlyContinue
        sleep 3

        # rename and process DateTime Attributes on out file
        $files=ls c:\temp\*.mp3|sort LastWriteTime -Descending
        if ($files -ne $null)
        {
          $lastf=$files[0]
          # process AirTime 
          $lastf.CreationTime=$nextRec.AirTime
          $lastf.LastWriteTime=$lastf.CreationTime
          
          Rename-Item -Path $lastf.FullName -NewName "$($nextRec.FileName).mp3"
          
        }
        else {Write-Host -ForegroundColor Red "No output mp3 file found ... !"}
    
        # test exception handling and restart in between ...
        # throw "OMG I FUCKED UPPPPPPP ........................"
             
        echo_debug  "Recording: $($nextRec.Filename) ... done"
        sleep 2
      }
      else 
      {
        Write-DbgView "Nothing found ... sleep and retry!"
        sleep 5
      }
    }
  } 
}
catch 
{
  Write-Output "Bloody exception with $PSCommandPath resulted in  $($PSItem.tostring())"
  Write-DbgView "Bloody exception with $PSCommandPath resulted in  $($PSItem.tostring())"
  sleep 3
  Write-Output "$(Get-Date) Restart time "
  # restart your foccing self
  & $PSCommandPath 
}
