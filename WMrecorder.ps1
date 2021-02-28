# WM recorder in powershell ...
#
$BrowserName = "opera"
$wmrecs =Import-Csv c:\temp\scheduled.csv -Delimiter ";" | Select @{Name="RecordingTime";Expression={(Get-date $_.RecordingTime)}}, @{Name="EndTime";Expression={(get-date $_.EndTime)}}, @{Name="Length";Expression={[int32]$_.Length}}, Filename  | where Recordingtime -gt (get-date)
if ($wmrecs.Length -gt 0 ) {echo "$($wmrecs.length) recording(s) ... Next @ $($wmrecs[0].RecordingTime) to $($wmrecs[0].EndTime.Hour.ToString('00')):$($wmrecs[0].EndTime.Minute.ToString('00')) - $($wmrecs[0].FileName)" }
while ($true)
{
   $wmrecs | %{
    if ( ((get-date) -ge ( $_.RecordingTime)) -and ((get-date) -le ( $_.EndTime)) ) 
    {
      echo "record NOW $($_.FileName) $($_.RecordingTime)"
      start http://r-schmidt.com/oe1.html 
      sleep (($_.EndTime) - (get-date)).totalseconds # sleep until end time 
      
      ps $BrowserName|kill
      sleep 3

      # rename out file 
      $files=ls c:\temp\*.mp3|sort LastWriteTime -Descending
      $lastf=$files[0]
      Rename-Item -Path $lastf.FullName -NewName "$($_.FileName).mp3"
    }
  }
  sleep 2
}
  

