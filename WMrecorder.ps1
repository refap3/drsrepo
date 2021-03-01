# WM recorder in powershell ...
#
$BuildNumber=1.12
$BrowserName = "opera"
while ($true)
{
  $wmrecs =Import-Csv c:\temp\scheduled.csv -Delimiter ";" | `
  Select @{Name="RecordingTime";Expression={(Get-date $_.RecordingTime)}}, @{Name="EndTime";Expression={(get-date $_.EndTime)}}, @{Name="Length";Expression={[int32]$_.Length}}, Filename, Link  | where Recordingtime -gt ((get-date).AddSeconds(-30)) | sort Recordingtime 
#  if ($wmrecs.Length -gt 0 ) {echo "$($wmrecs.length) recording(s) ... Next @ $($wmrecs[0].RecordingTime) to $($wmrecs[0].EndTime.Hour.ToString('00')):$($wmrecs[0].EndTime.Minute.ToString('00')) - $($wmrecs[0].FileName)" }
  
  # find the 1st recordable entry  and display it, focc off iff nothing ....
  if ($wmrecs -ne $null) 
  {
    $nextRec = $wmrecs[0]
    $ssecs=(($nextRec.RecordingTime) - (get-date)).totalseconds
    $smins=$ssecs/60
    echo "V: $BuildNumber-will record this: $nextRec in $($smins.tostring('0')) minutes."
 
    if ($ssecs -gt 0 ) {sleep  $ssecs } # sleep until start time 
    echo "recording NOW: $($nextRec.RecordingTime) to $($nextRec.EndTime.Hour.ToString('00')):$($nextRec.EndTime.Minute.ToString('00')) - $($nextRec.FileName)"
    start $nextRec.Link
    sleep (($nextRec.EndTime) - (get-date)).totalseconds # sleep until end time 
      
    ps $BrowserName|kill -ErrorAction SilentlyContinue
    sleep 3

    # rename out file 
    $files=ls c:\temp\*.mp3|sort LastWriteTime -Descending
    if ($files -ne $null)
    {
      $lastf=$files[0]
      Rename-Item -Path $lastf.FullName -NewName "$($nextRec.FileName).mp3"
    }
    else {echo "No output mp3 file found ... !"}
    
    sleep 2
  }
  else 
  {
    echo "Nothing found ... sleep and retry!"
    sleep 5
  }
}
  

