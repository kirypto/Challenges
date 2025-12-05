function Watch-AocFile([int]$Day, [int]$Part, [int]$Test=1) {
    $LastPrint = $null
    $DaySourceFile = "./day$Day-part$Part.wat.js"

    while ($true) {
        $LastWrite = (Get-Item $DaySourceFile).LastWriteTime
        if ($LastWrite -ne $LastPrint) {
            Write-Host "`n`n$(Get-Date -Format 'o')" -ForegroundColor Green
            node --env-file=.env runner.js $Day $Part ".\day$Day-test$Test.txt"
            $LastPrint = $LastWrite
        }

        Start-Sleep -Seconds 1
    }
}
