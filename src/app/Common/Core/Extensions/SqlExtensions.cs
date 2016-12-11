using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using Dapper;

namespace Clinic.Common.Core.Extensions
{
    public static class SqlExtensions
    {
        public static bool IsRetryable(this SqlException that)
        {
            //for error descriptions go to:
            //http://social.technet.microsoft.com/wiki/contents/articles/sql-azure-connection-management-in-sql-azure.aspx
            //http://msdn.microsoft.com/en-us/library/4cff491e-9359-4454-bd7c-fb72c4c452ca
            var sqlErrorCodesToRetry = new[]
			{
				-2 /*Timeout expired. The timeout period elapsed prior to completion of the operation or the server is not responding.*/
				, 20 /*The instance of SQL Server you attempted to connect to does not support encryption. (PMcE: amazingly, this is transient)*/
				, 64 /*A connection was successfully established with the server, but then an error occurred during the login process.*/
				, 233 /*The client was unable to establish a connection because of an error during connection initialization process before login*/
				, 10053 /*A transport-level error has occurred when receiving results from the server.*/
				, 10054 /*A transport-level error has occurred when sending the request to the server.*/
				, 10060 /*A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible.*/
				, 40143 /*The service has encountered an error processing your request. Please try again.*/
				, 40197 /*The service has encountered an error processing your request. Please try again.*/
				, 40501 /*The service is currently busy. Retry the request after 10 seconds.*/
				, 40613 /*Database '%.*ls' on server '%.*ls' is not currently available. Please retry the connection later.*/
			};

            return that.Errors.Cast<SqlError>().Any(sqlError => sqlErrorCodesToRetry.Contains(sqlError.Number));
        }


        public static void OpenWithRetry(this SqlConnection connection)
        {
            var retries = 0;
            Exception failedToRetryException = null;
            while (connection.State != ConnectionState.Open && retries < 10)
            {
                try
                {
                    connection.Open();
                    break;
                }
                catch (SqlException exc)
                {
                    if (!exc.IsRetryable())
                        throw;

                    failedToRetryException = exc;
                    retries += 1;
                    Thread.Sleep(1000); // 1 second
                }
            }
            if (connection.State != ConnectionState.Open)
                throw new Exception("Failed to open sql connection after 10 retries", failedToRetryException);
        }

        public static int ExecuteWithRetry(this IDbConnection cnn, string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var retries = 0;
            var result = 0;
            while (retries < 10)
            {
                try
                {
                    result = cnn.Execute(sql, (object)param, transaction, commandTimeout, commandType);
                    break;
                }
                catch (SqlException exc)
                {
                    if (!exc.IsRetryable())
                        throw;

                    retries += 1;
                    Thread.Sleep(1000); // 1 second
                }
            }
            return result;
        }
    }
}
