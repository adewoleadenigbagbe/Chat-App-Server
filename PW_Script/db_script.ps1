param([string]$server='',[string]$filename='')


Invoke-sqlcmd -ServerInstance $server  -InputFile $filename