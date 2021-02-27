# WM recorder in powershell ...
#

while ($true)
{
  echo "New Import ... 1st recording is:"
  $wmrecs =Import-Csv c:\temp\scheduled.csv -Delimiter ";"

  echo $wmrecs[0].FileName, $wmrecs[0].RecordingTime
  $wmrecs | %{
      
  
    if ( ((get-date) -ge (get-date $_.RecordingTime)) -and ((get-date) -le (get-date $_.EndTime)) ) 
    {
      echo will record NOW
      echo $_.FileName, $_.RecordingTime
      start http://r-schmidt.com/oe1.html 
      sleep ((get-date $_.EndTime) - (get-date)).totalseconds # sleep until end time 
      
      ps opera|kill
      sleep 4
      
                    # must file rename here .. take youngest and rename to saved argument ...
    
      $files=ls c:\temp\*.mp3|sort LastWriteTime -Descending
      $lastf=$files[0]
      Rename-Item -Path $lastf.FullName -NewName "$($_.FileName).mp3"
           
       
    }
  }
  sleep 2
}
  

