using System;
using System.Text;
using MySql.Data.MySqlClient;

namespace NinjaTrader.Custom.AddOns.HistoricalTickDataCollectionTool
{
    public class DBUitl
    {
        private static MySqlConnection mySqlConnection;
        // update it to connect to your mysql database
        private static string connectionString = "Server=<server_address>;User ID=<username>;Password=<password>;Database=<db_name>;Port=<port_num>;Pooling=false";
        
        private static Boolean ValidateConnectionString()
        {
            if (connectionString == "Server=<server_address>;User ID=<username>;Password=<password>;Database=<db_name>;Port=<port_num>;Pooling=false")
            {
                Logger.LogMessage("Make sure you have updated the connectionString variable in DBUtils.cs file.");
                return false;
            }

            return true;
        }

        private static Boolean Init()
        {
            if (mySqlConnection != null) return true;
            if(!ValidateConnectionString()) return false;

            try
            {
                mySqlConnection = new MySqlConnection(connectionString);
                mySqlConnection.Open();
            }
            catch(Exception ex)
            {
                Logger.LogException("Failed to create mysql connection. Make sure the connectionString variable in DBUtils.cs class is correct.", ex);
                return false;
            }

            return true;
        }

        private static string BacktickQuoteVar(string var)
        {
            return "`" + var + "`";
        }

        private static Boolean DeleteTableIfExsits(string tableName)
        {
            if(!Init()) return false;

            StringBuilder commandStr = new StringBuilder();
            commandStr.Append("DROP TABLE IF EXISTS ").Append(BacktickQuoteVar(tableName)).Append(";");

            try
            {
                MySqlCommand command = new MySqlCommand(commandStr.ToString(), mySqlConnection);
                command.ExecuteNonQuery();
            } 
            catch(Exception ex)
            {
                Logger.LogMessage("Failed query: " + commandStr.ToString());
                Logger.LogException("Failed to drop table: " + tableName, ex);
                return false;
            }

            return true;
        }

        public static Boolean CreateTable(string tableName)
        {
            if(!Init()) return false;
            if(!DeleteTableIfExsits(tableName)) return false;

            StringBuilder commandStr = new StringBuilder();
            commandStr.Append("CREATE TABLE ").Append(BacktickQuoteVar(tableName)).Append(" (");
            commandStr.Append("id BIGINT NOT NULL AUTO_INCREMENT, ");
            commandStr.Append("time BIGINT NOT NULL, ");
            commandStr.Append("bid DOUBLE(15,2) NOT NULL, ");
            commandStr.Append("ask DOUBLE(15,2) NOT NULL, ");
            commandStr.Append("price DOUBLE(15,2) NOT NULL, ");
            commandStr.Append("volume BIGINT NOT NULL, ");
            commandStr.Append("PRIMARY KEY (`id`));");

            try
            {
                MySqlCommand command = new MySqlCommand(commandStr.ToString(), mySqlConnection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.LogMessage("Failed query: " + commandStr.ToString());
                Logger.LogException("Failed to create table: " + tableName, ex);
                return false;
            }

            return true;
        }

        public static Boolean AddRecord(string tableName, long time, Double bid, Double ask, Double price, long volume)
        {
            if(!Init()) return false;

            StringBuilder commandStr = new StringBuilder();
            commandStr.Append("INSERT INTO ").Append(BacktickQuoteVar(tableName)).Append(" (");
            commandStr.Append("time, bid, ask, price, volume) VALUES(");
            commandStr.Append(@"'").Append(time.ToString()).Append(@"', ");
            commandStr.Append(@"'").Append(bid.ToString()).Append(@"', ");
            commandStr.Append(@"'").Append(ask.ToString()).Append(@"', ");
            commandStr.Append(@"'").Append(price.ToString()).Append(@"', ");
            commandStr.Append(@"'").Append(volume.ToString()).Append(@"');");

            try
            {
                MySqlCommand command = new MySqlCommand(commandStr.ToString(), mySqlConnection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.LogMessage("Failed query: " + commandStr.ToString());
                Logger.LogException("Failed to create table: " + tableName, ex);
                return false;
            }

            return true;
        }
    }
}
