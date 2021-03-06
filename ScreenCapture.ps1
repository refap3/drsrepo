[Reflection.Assembly]::LoadWithPartialName("System.Drawing") | Out-Null
function screenshot([Drawing.Rectangle]$bounds, $path) {
   $bmp = New-Object Drawing.Bitmap $bounds.width, $bounds.height
   $graphics = [Drawing.Graphics]::FromImage($bmp)

   $graphics.CopyFromScreen($bounds.Location, [Drawing.Point]::Empty, $bounds.size)

   $bmp.Save($path)

   $graphics.Dispose()
   $bmp.Dispose()
}
sleep 180 # sleep time after startup ...
$sc=Get-WmiObject -Class Win32_DesktopMonitor 
$bounds = [Drawing.Rectangle]::FromLTRB(0, 0, $sc.ScreenWidth, $sc.ScreenHeight)
$bmpFile="c:\temp\screen$((get-date).ToString("yyyy.MM.dd")).png"
screenshot $bounds $bmpFile


