using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Servise.Dto;

namespace Servise.DataProviders
{
    public class DataBaseManager : IDisposable
    {

        private readonly SqlConnection _connection;

        public DataBaseManager()
        {

            _connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DemoSite"].ConnectionString);
            _connection.Open();

        }

        public string SetPeople(PeopleDto people)
        {

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "dbo.[spSetPeople]";
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 300;
                command.Parameters.AddWithValue("@FirstName", people.FirstName);
                command.Parameters.AddWithValue("@LastName", people.LastName);
                command.Parameters.AddWithValue("@Gender", people.Gender);
                command.Parameters.AddWithValue("@Qoute", people.Qoute);
                command.Parameters.AddWithValue("@City", people.City);
                command.Parameters.AddWithValue("@Street", people.Street);
                command.Parameters.AddWithValue("@Email", people.Email);
                command.Parameters.AddWithValue("@PictureMedium", people.PictureMedium);

                return command.ExecuteScalar().ToString();
            }

        }

        public void SetPeom(PoemDto poem)
        {

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "dbo.[spSetPeom]";
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 300;
                command.Parameters.AddWithValue("@UserId", poem.UserKey);
                command.Parameters.AddWithValue("@Title", poem.Title);
                command.Parameters.AddWithValue("@Content", poem.Content);
                command.Parameters.AddWithValue("@Distance", poem.Distance);

                command.ExecuteReader();
            }

        }

        public IDataReader GetReport()
        {

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "dbo.[spGetReport]";
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 300;

                return command.ExecuteReader();
            }

        }

#region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_connection == null)
                return;

            if (_connection.State != ConnectionState.Closed)
                _connection.Close();
        }

#endregion
    }
}
