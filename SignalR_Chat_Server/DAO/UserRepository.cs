using Microsoft.Extensions.Options;
using SignalR_Chat_Server.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using NLog;


namespace SignalR_Chat_Server.DAO
{
    public class UserRepository
    {
        private readonly DataBaseSettings _databaseSettings = new DataBaseSettings();
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public UserRepository(IOptions<DataBaseSettings> options)
        {
            _databaseSettings = options.Value;
        }
        public async Task<Tuple<string,string>> CreateUser(User user)
        {
            using (var sqlConnection = new SqlConnection(_databaseSettings.ConnectionString))
            {
                try
                {
                    await sqlConnection.OpenAsync();
                    var username = await GetUserByName(user.UserName.ToLower().Trim());
                    if(username != string.Empty)
                    {
                        return Tuple.Create("10", "Name must be Unique..Choose another username ");
                    }
                    var cmdText = "[dbo].[CreateUser]";
                    using (var sqlCommand = new SqlCommand(cmdText, sqlConnection))
                    {
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                        sqlCommand.Parameters.AddWithValue("@username", user.UserName.ToLower().Trim());
                        sqlCommand.Parameters.AddWithValue("@password", user.Password.ToLower().Trim());

                        var rowsAffected = await sqlCommand.ExecuteNonQueryAsync();
                        if(rowsAffected > 0 )
                        {
                            return Tuple.Create("00", "Succefully Created");
                        }
                        return Tuple.Create("10", "Try again Later");

                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message + ex.StackTrace);
                    return Tuple.Create("10", "Try again Later");
                }

            }
        }
        public async Task<User> GetUser(string username ,string password)
        {
            using (var sqlConnection = new SqlConnection(_databaseSettings.ConnectionString))
            {
                await sqlConnection.OpenAsync();

                try
                {

                    var cmdText = "[dbo].[GetUser]";
                
                    using (var sqlCommand = new SqlCommand(cmdText, sqlConnection))
                    {
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                        sqlCommand.Parameters.AddWithValue("@username", username);
                        sqlCommand.Parameters.AddWithValue("@password", password);

                        var sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                        var user = new User();
                        while (await sqlDataReader.ReadAsync())
                        {

                            user.UserName = sqlDataReader["UserName"].ToString();
                            user.Password = sqlDataReader["Password"].ToString();
                        }
                        return user;
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message + ex.StackTrace);
                    return null;
                }

            }
        }
        private async Task<string> GetUserByName(string username)
        {
            using (var sqlConnection = new SqlConnection(_databaseSettings.ConnectionString))
            {
                try
                {
                    await sqlConnection.OpenAsync();

                    var cmdText = "[dbo].[GetUserName]";
                    using (var sqlCommand = new SqlCommand(cmdText, sqlConnection))
                    {
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                        sqlCommand.Parameters.AddWithValue("@username", username);
                        var name = string.Empty;
                        var sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                        while (await sqlDataReader.ReadAsync())
                        {
                            name = sqlDataReader["UserName"].ToString();
                        }
                        return name;
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message + ex.StackTrace);
                    return null;
                }

            }
        }



    }
}
