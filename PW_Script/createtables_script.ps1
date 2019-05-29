Invoke-sqlcmd -ServerInstance 'devAnsibleEnv\SQLEXPRESS' -Database 'ChatDB' -InputFile 'C:\GitRepo.CompiledCode\Chat-App-Server\Sql Script\ChatDb_Tables.sql'
