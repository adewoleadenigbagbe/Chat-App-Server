param([string]$server='',[string]$db='',[string]$filename='')

Invoke-sqlcmd -ServerInstance $server -Database $db -InputFile $-filename