using Microsoft.Extensions.Options;
using SignalR_Chat_Server.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Chat_Server.DAO
{
    public class GroupRepository
    {
        private readonly DataBaseSettings _databaseSettings = new DataBaseSettings();
        public GroupRepository(IOptions<DataBaseSettings> options)
        {
            _databaseSettings = options.Value;
        }

        public async Task<IEnumerable<string>> Groups()
        {
            var grouplist = new List<string>();
            using (var sqlConnection = new SqlConnection(_databaseSettings.ConnectionString))
            {
                await sqlConnection.OpenAsync();
                try
                {
                    var cmdText = "[dbo].[GetAllGroups]";
                    using (var sqlCommand = new SqlCommand(cmdText, sqlConnection))
                    {
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        var sqldataReader = await sqlCommand.ExecuteReaderAsync();

                        while(await sqldataReader.ReadAsync())
                        {
                            grouplist.Add(sqldataReader["GroupName"].ToString());
                        }
                        return grouplist;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace + ex.Message);
                    return null;
                }

            }

        }
    }
}
