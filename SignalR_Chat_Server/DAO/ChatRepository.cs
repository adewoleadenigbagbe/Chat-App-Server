using Microsoft.Extensions.Options;
using SignalR_Chat_Server.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using NLog;

namespace SignalR_Chat_Server.DAO
{
    public class ChatRepository
    {
        private readonly DataBaseSettings _databaseSettings = new DataBaseSettings();
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ChatRepository(IOptions<DataBaseSettings> options)
        {
            _databaseSettings = options.Value;
        }
        public async Task<bool> CreateConnectionDetails(UserDetails userDetails)
        {
            using (var sqlConnection = new SqlConnection(_databaseSettings.ConnectionString))
            {
                await sqlConnection.OpenAsync();
                try
                {
                    var cmdText = "[dbo].[CreateOrUpdateConnectionDetails]";
                    using (var sqlCommand = new SqlCommand(cmdText, sqlConnection))
                    {
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                        sqlCommand.Parameters.AddWithValue("@username", userDetails.UserName);
                        sqlCommand.Parameters.AddWithValue("@groupid", userDetails.GroupId);
                        sqlCommand.Parameters.AddWithValue("@connectionId", userDetails.ConnectionId);
                        sqlCommand.Parameters.AddWithValue("@onlineStatus", userDetails.OnlineStatus);

                        var rowsAffected = await sqlCommand.ExecuteNonQueryAsync();
                        return rowsAffected > 0 ? true : false;
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message + ex.StackTrace);
                    return false;
                }

            }
        }
        public async void GetConnectionIds()
        {
            using (var sqlConnection = new SqlConnection(_databaseSettings.ConnectionString))
            {
                try
                {
                    var cmdText = "[dbo].[GetConnectionIds]";
                    using (var sqlCommand = new SqlCommand(cmdText, sqlConnection))
                    {
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                        var sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                        while(await sqlDataReader.ReadAsync())
                        {
                            var username = sqlDataReader["UserName"].ToString();
                            ConnectedClientRepo.Add(username, new UserDetails {
                                UserName = username,
                                GroupId = sqlDataReader["GroupId"].ToString(),
                                ConnectionId = sqlDataReader["ConnectionId"].ToString(),
                                OnlineStatus = bool.Parse(sqlDataReader["OnlineStatus"].ToString())
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message + ex.StackTrace);
                }

            }
        }
        public async Task<bool> UpdateOnlineStatus(string username)
        {
            using (var sqlConnection = new SqlConnection(_databaseSettings.ConnectionString))
            {
                await sqlConnection.OpenAsync();
                try
                {
                    var cmdText = "[dbo].[UpdateOnlineStatus]";
                    using (var sqlCommand = new SqlCommand(cmdText, sqlConnection))
                    {
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@username", username);

                        var rowsAffected = await sqlCommand.ExecuteNonQueryAsync();
                        return rowsAffected > 0 ? true : false;
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message + ex.StackTrace);
                    return false;
                }

            }
        }

    }
}
