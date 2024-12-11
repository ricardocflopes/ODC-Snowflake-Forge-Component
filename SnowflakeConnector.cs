﻿using OutSystems.ExternalLibraries.SDK;
using System.Data;
using OutSystems.SnowflakeConnector;
using Newtonsoft.Json;
using Snowflake.Data.Client;

namespace OutSystems.SnowflakeConnector
{
    public class SnowflakeConnector : ISnowflakeConnector
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ssUsername"></param>
        /// <param name="ssPassword"></param>
        /// <param name="ssIsSuccessful"></param>
        /// <param name="ssQuery"></param>
        /// <param name="ssScheme"></param>
        /// <param name="ssAccount"></param>
        /// <param name="ssHost"></param>
        /// <param name="ssPort"></param>
        /// <param name="ssRole"></param>
        /// <param name="ssWarehouse"></param>
        /// <param name="ssExtraParametersForConnectionString"></param>
        /// <param name="ssResultInJSON"></param>
        public void RunQuery(string ssUsername, string ssPassword, string ssScheme, string ssAccount, string ssHost, string ssPort, string ssRole, string ssWarehouse, string ssExtraParametersForConnectionString, string ssQuery, out bool ssIsSuccessful, out string ssResultInJSON, string ssDatabase)
        {
            string connectionString = "SCHEMA=" + ssScheme + ";DB=" + ssDatabase + ";ACCOUNT=" + ssAccount + ";HOST=" + ssHost + ";WAREHOUSE=" + ssWarehouse + ";USER=" + ssUsername + ";PASSWORD=" + ssPassword + ";" + ssExtraParametersForConnectionString;

            if (ssPort != "")
            {
                connectionString = connectionString + ";PORT=" + ssPort;
            }

            if (ssRole != "")
            {
                connectionString = connectionString + ";ROLE=" + ssRole;
            }

            if (ssExtraParametersForConnectionString != "")
            {
                connectionString = connectionString + ssExtraParametersForConnectionString;
            }

            using (var conn = new SnowflakeDbConnection())
            {
                conn.ConnectionString = connectionString;
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = ssQuery;
                ssResultInJSON = "";

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    ssResultInJSON = JsonConvert.SerializeObject(dataTable);
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                conn.Close();
                ssIsSuccessful = true;
            }
        } // MssRunQuery
    }
}