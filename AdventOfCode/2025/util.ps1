function Watch-AocFile([String]$file) {
    $LastPrint = $null

    while ($true) {
        $LastWrite = (Get-Item $file).LastWriteTime
        if ($LastWrite -ne $LastPrint) {
            Write-Host "`n`n$(Get-Date -Format 'o')" -ForegroundColor Green
            node $file
            $LastPrint = $LastWrite
        }

        Start-Sleep -Seconds 1
    }
}
