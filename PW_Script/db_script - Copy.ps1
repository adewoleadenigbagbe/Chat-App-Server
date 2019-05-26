param([string]$server='',[string]$filename='')



Invoke-sqlcmd -ServerInstance $server -Database $db -InputFile $-filename