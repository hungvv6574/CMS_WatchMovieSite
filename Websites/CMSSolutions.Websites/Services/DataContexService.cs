using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CMSSolutions.Configuration;
using CMSSolutions.Extensions;
using CMSSolutions.Security.Cryptography;

namespace CMSSolutions.Websites.Services
{
    public class DataContexService
    {
        public string SqlConnectionString { get; set; }
        public const string ErrorDbConnection = "Can not found connection string.";

        public DataContexService()
        {
            SqlConnectionString = ConfigurationManager.ConnectionStrings[CMSConfigurationSection.Instance.Data.SettingConnectionString].ConnectionString;
            if (KeyConfiguration.IsEncrypt)
            {
                SqlConnectionString = EncryptionExtensions.Decrypt(CMSConfigurationSection.Instance.Data.SettingConnectionString, SqlConnectionString);
            }
        }

        public virtual SqlParameter AddInputParameter<T>(
           string name,
           T value)
        {
            var parameter = new SqlParameter
            {
                ParameterName = name,
                Value = value,
                Direction = ParameterDirection.Input
            };

            return parameter;
        }

        public virtual SqlParameter AddOutputParameter(
            string name,
            int size,
            SqlDbType type)
        {
            var parameter = new SqlParameter
            {
                ParameterName = name,
                SqlDbType = type,
                Size = size,
                Direction = ParameterDirection.Output
            };

            return parameter;
        }

        public virtual SqlParameter AddInputParameter(
            string name,
            SqlDbType type,
            int size,
            string columnName)
        {
            var parameter = new SqlParameter
            {
                ParameterName = name,
                SqlDbType = type,
                Size = size,
                SourceColumn = columnName,
                Direction = ParameterDirection.Input
            };

            return parameter;
        }

        public virtual List<T> ExecuteSqlCommand<T>(string commandText)
        {
            if (string.IsNullOrEmpty(SqlConnectionString))
            {
                throw new ArgumentNullException(ErrorDbConnection);
            }

            using (var connection = new SqlConnection(SqlConnectionString))
            {
                connection.Open();
                var list = new List<T>();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = commandText;
                    command.CommandType = CommandType.Text;
                    var reader = command.ExecuteReader();

                    var properties = typeof(T).GetProperties();
                    while (reader.Read())
                    {
                        var local = Activator.CreateInstance<T>();
                        foreach (var info in properties)
                        {
                            try
                            {
                                var name = ReflectionExtensions.GetAttributeDisplayName(info);
                                if (name == Constants.NotMapped)
                                {
                                    continue;
                                }

                                var data = reader[name];
                                if (data.GetType() != typeof(DBNull))
                                {
                                    info.SetValue(local, data, null);
                                }
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }

                        list.Add(local);
                    }

                    reader.Close();
                }

                connection.Close();
                return list;
            }
        }

        public virtual List<T> ExecuteReader<T>(
            string storedProcedureName)
        {
            if (string.IsNullOrEmpty(SqlConnectionString))
            {
                throw new ArgumentNullException(ErrorDbConnection);
            }

            using (var connection = new SqlConnection(SqlConnectionString))
            {
                connection.Open();
                var list = new List<T>();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = storedProcedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    var reader = command.ExecuteReader();

                    var properties = typeof(T).GetProperties();
                    while (reader.Read())
                    {
                        var local = Activator.CreateInstance<T>();
                        foreach (var info in properties)
                        {
                            try
                            {
                                var name = ReflectionExtensions.GetAttributeDisplayName(info);
                                if (name == Constants.NotMapped)
                                {
                                    continue;
                                }

                                var data = reader[name];
                                if (data.GetType() != typeof(DBNull))
                                {
                                    info.SetValue(local, data, null);
                                }
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }

                        list.Add(local);
                    }

                    reader.Close();
                }

                connection.Close();
                return list;
            }
        }

        public virtual List<T> ExecuteReader<T>(
            string storedProcedureName,
            params SqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(SqlConnectionString))
            {
                throw new ArgumentNullException(ErrorDbConnection);
            }

            using (var connection = new SqlConnection(SqlConnectionString))
            {
                connection.Open();
                var list = new List<T>();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = storedProcedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    var reader = command.ExecuteReader();

                    var properties = typeof(T).GetProperties();
                    while (reader.Read())
                    {
                        var local = Activator.CreateInstance<T>();
                        foreach (var info in properties)
                        {
                            try
                            {
                                var name = ReflectionExtensions.GetAttributeDisplayName(info);
                                if (name == Constants.NotMapped)
                                {
                                    continue;
                                }

                                var data = reader[name];
                                if (data.GetType() != typeof(DBNull))
                                {
                                    info.SetValue(local, data, null);
                                }
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }

                        list.Add(local);
                    }

                    reader.Close();
                }

                connection.Close();
                return list;
            }
        }

        public virtual List<T> ExecuteReader<T>(
            string storedProcedureName,
            string columnNameOut,
            out int totalRecords,
            params SqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(SqlConnectionString))
            {
                throw new ArgumentNullException(ErrorDbConnection);
            }

            totalRecords = 0;
            using (var connection = new SqlConnection(SqlConnectionString))
            {
                connection.Open();
                var list = new List<T>();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = storedProcedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    command.Parameters.Add(AddOutputParameter(columnNameOut, 10, SqlDbType.Int));
                    var reader = command.ExecuteReader();
                    var properties = typeof(T).GetProperties();
                    while (reader.Read())
                    {
                        var local = Activator.CreateInstance<T>();
                        foreach (var info in properties)
                        {
                            try
                            {
                                var name = ReflectionExtensions.GetAttributeDisplayName(info);
                                if (name == Constants.NotMapped)
                                {
                                    continue;
                                }

                                var data = reader[name];
                                if (data.GetType() != typeof(DBNull))
                                {
                                    info.SetValue(local, data, null);
                                }
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }
                        list.Add(local);
                    }
                    reader.Close();

                    if (command.Parameters[columnNameOut].Value != null)
                    {
                        totalRecords = (int)command.Parameters[columnNameOut].Value;
                    }
                }
                connection.Close();

                return list;
            }
        }

        public virtual int ExecuteNonQuery(
            string storedProcedureName,
            string columnNameOut,
            out string errorMessages,
            params SqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(SqlConnectionString))
            {
                throw new ArgumentNullException(ErrorDbConnection);
            }

            using (var connection = new SqlConnection(SqlConnectionString))
            {
                var result = 0;
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = storedProcedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    command.Parameters.Add(AddOutputParameter(columnNameOut, 250, SqlDbType.NVarChar));
                    result = command.ExecuteNonQuery();
                    if (command.Parameters[columnNameOut].Value != null)
                    {
                        errorMessages = command.Parameters[columnNameOut].Value.ToString();
                    }
                    else
                    {
                        errorMessages = string.Empty;
                    }
                }

                connection.Close();
                return result;
            }
        }

        public virtual List<T> ExecuteReader<T>(
            string storedProcedureName,
            string columnNameOut,
            out int totalRecords)
        {
            if (string.IsNullOrEmpty(SqlConnectionString))
            {
                throw new ArgumentNullException(ErrorDbConnection);
            }

            totalRecords = 0;
            using (var connection = new SqlConnection(SqlConnectionString))
            {
                connection.Open();
                var list = new List<T>();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = storedProcedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(AddOutputParameter(columnNameOut, 50, SqlDbType.Int));
                    var reader = command.ExecuteReader();
                    var properties = typeof(T).GetProperties();
                    while (reader.Read())
                    {
                        var local = Activator.CreateInstance<T>();
                        foreach (var info in properties)
                        {
                            try
                            {
                                var name = ReflectionExtensions.GetAttributeDisplayName(info);
                                if (name == Constants.NotMapped)
                                {
                                    continue;
                                }

                                var data = reader[name];
                                if (data.GetType() != typeof(DBNull))
                                {
                                    info.SetValue(local, data, null);
                                }
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }

                        list.Add(local);
                    }

                    reader.Close();

                    if (command.Parameters[columnNameOut].Value != null)
                    {
                        totalRecords = (int)command.Parameters[columnNameOut].Value;
                    }
                }

                connection.Close();
                return list;
            }
        }

        public virtual DataSet ExecuteReader(
           string storedProcedureName,
           params SqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(SqlConnectionString))
            {
                throw new ArgumentNullException(ErrorDbConnection);
            }

            var ds = new DataSet();
            using (var connection = new SqlConnection(SqlConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = storedProcedureName;
                    command.Connection = connection;
                    command.Parameters.AddRange(parameters);
                    var da = new SqlDataAdapter(command);
                    da.Fill(ds);
                }

                connection.Close();
            }

            return ds;
        }

        public virtual DataSet ExecuteReader(
          string storedProcedureName)
        {
            if (string.IsNullOrEmpty(SqlConnectionString))
            {
                throw new ArgumentNullException(ErrorDbConnection);
            }

            var ds = new DataSet();
            using (var connection = new SqlConnection(SqlConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = storedProcedureName;
                    command.Connection = connection;
                    var da = new SqlDataAdapter(command);
                    da.Fill(ds);
                }

                connection.Close();
            }

            return ds;
        }

        public virtual T ExecuteReaderRecord<T>(
            string storedProcedureName,
            params SqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(SqlConnectionString))
            {
                throw new ArgumentNullException(ErrorDbConnection);
            }

            using (var connection = new SqlConnection(SqlConnectionString))
            {
                connection.Open();
                T local = default(T);
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = storedProcedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    var reader = command.ExecuteReader();

                    var properties = typeof(T).GetProperties();
                    while (reader.Read())
                    {
                        local = Activator.CreateInstance<T>();
                        foreach (var info in properties)
                        {
                            try
                            {
                                var name = ReflectionExtensions.GetAttributeDisplayName(info);
                                if (name == Constants.NotMapped)
                                {
                                    continue;
                                }

                                var data = reader[name];
                                if (data.GetType() != typeof(DBNull))
                                {
                                    info.SetValue(local, data, null);
                                }
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }
                        break;
                    }

                    reader.Close();
                }

                connection.Close();
                return local;
            }
        }

        public virtual object ExecuteReaderResult(
            string storedProcedureName,
            params SqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(SqlConnectionString))
            {
                throw new ArgumentNullException(ErrorDbConnection);
            }

            using (var connection = new SqlConnection(SqlConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = storedProcedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        return reader[0];

                    }

                    reader.Close();
                }

                connection.Close();

                return null;
            }
        }

        public int ExecuteNonQuery(
           string storedProcedureName,
           DataTable listDetails,
           params SqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(SqlConnectionString))
            {
                throw new ArgumentNullException(ErrorDbConnection);
            }

            var data = 0;
            using (var connection = new SqlConnection(SqlConnectionString))
            {
                connection.Open();
                var adapter = new SqlDataAdapter();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = storedProcedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);

                    adapter.InsertCommand = command;
                    data = adapter.Update(listDetails);
                }

                connection.Close();
            }

            return data;
        }

        public int ExecuteNonQuery(
           string storedProcedureName,
           params SqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(SqlConnectionString))
            {
                throw new ArgumentNullException(ErrorDbConnection);
            }

            var data = 0;
            using (var connection = new SqlConnection(SqlConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = storedProcedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    data = command.ExecuteNonQuery();
                }

                connection.Close();
            }

            return data;
        }
    }
}