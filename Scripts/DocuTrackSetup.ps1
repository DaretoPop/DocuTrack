# ----------------------------------------------------
# DeleteDocuTrack.ps1
#   Schedules "DocuTrackConfiguration" (a .bat) to run once, 3 minutes from now.
# ----------------------------------------------------

### 1. CONFIGURE:
$TaskName   = 'DocuTrackConfiguration'
$ScriptPath = 'C:\Users\Admin\Desktop\publish\DocuTrackConfiguration.bat'
$ScriptArgs = ''   # e.g. '-foo bar' or leave empty if none

# 2. Figure out how to invoke it
$ext = [IO.Path]::GetExtension($ScriptPath)
if ($ext -ieq '.bat') {
    $exe  = 'cmd.exe'
    # wrap your .bat path in quotes in case of spaces
    $args = "/c `"$ScriptPath`" $ScriptArgs"
} else {
    $exe  = $ScriptPath
    $args = $ScriptArgs
}

# 3. Build the Scheduled Task action
if ([string]::IsNullOrWhiteSpace($args)) {
    $action = New-ScheduledTaskAction -Execute $exe
} else {
    $action = New-ScheduledTaskAction -Execute $exe -Argument $args
}

# 4. Build the one‐time trigger for now + 3 minutes
$runTime = (Get-Date).AddMinutes(2)
$trigger = New-ScheduledTaskTrigger -Once -At $runTime

# 5. Optional settings
$settings = New-ScheduledTaskSettingsSet `
    -AllowStartIfOnBatteries `
    -DontStopIfGoingOnBatteries `
    -StartWhenAvailable

# 6. Register (or overwrite) the task
if (Get-ScheduledTask -TaskName $TaskName -ErrorAction SilentlyContinue) {
    Unregister-ScheduledTask -TaskName $TaskName -Confirm:$false
}

Register-ScheduledTask `
    -TaskName    $TaskName `
    -Action      $action `
    -Trigger     $trigger `
    -Description "Run $TaskName (batch) after 3 minutes" `
    -Settings    $settings

Write-Host " Scheduled batch task '$TaskName' to run at $($runTime.ToString('u'))"

