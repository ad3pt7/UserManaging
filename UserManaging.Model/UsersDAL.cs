using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UserManaging.Model.Regexes;

namespace UserManaging.Model
{
    public class UsersDAL
    {
        private readonly MySqlConnection _connection;

        public UsersDAL()
        {
            _connection = new MySqlConnection("server=localhost;port=3307;user=root;database=usersmanaging;ssl mode=none");
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<User> GetUsers()
        {
            OpenConnection();
            var table = new DataTable();
            var selectRecordsCommand = new MySqlCommand($"SELECT *" +
                                                        $"FROM Users",
                                                        _connection);
            var reader = selectRecordsCommand.ExecuteReader();
            table.Load(reader);
            reader.Close();
            CloseConnection();
            
            var users = MapRecordsFromDataTable(table);
            return users;
        }
        /// <summary>
        /// Преобразовывает данные из DataTable в User
        /// </summary>
        private IEnumerable<User> MapRecordsFromDataTable(DataTable table)
        {
            OpenConnection();
            var users = new List<User>();

            foreach (DataRow row in table.Rows)
            {
                var user = new User
                {
                    Id = int.Parse(row["Id"].ToString()),
                    FirstName = row["FirstName"].ToString(),
                    LastName = row["LastName"].ToString(),
                    MiddleName = row["MiddleName"].ToString(),
                    Phone = row["Phone"].ToString(),
                    Email = row["Email"].ToString()
                };
                users.Add(user);
            }
            CloseConnection();
            return users;
        }
        /// <summary>
        /// Добавляет запись в базу данных
        /// </summary>
        public void Insert(User user)
        {
            var users = GetUsers();
            if (IsValidUser(user)
                && !users.Any(u => u.FirstName == user.FirstName
                               && u.LastName == user.LastName
                               && u.MiddleName == user.MiddleName
                               && u.Phone == user.Phone
                               && u.Email == user.Email))
            {
                var insertQuery = $"INSERT INTO Users (FirstName, LastName, MiddleName, Phone, Email)" +
                                  $"VALUES ('{user.FirstName}', '{user.LastName}', '{user.MiddleName}', '{user.Phone}', '{user.Email}')";
                var command = new MySqlCommand(insertQuery, _connection);
                OpenConnection();
                command.ExecuteNonQuery();
            }
            CloseConnection();
        }
        /// <summary>
        /// Изменяет запись в базе данных
        /// </summary>
        public void Update(User user)
        {
            var users = GetUsers();
            if (IsValidUser(user))
            {
                var updateQuery = $"UPDATE Users SET FirstName = '{user.FirstName}', " +
                        $"LastName = '{user.LastName}', " +
                        $"MiddleName = '{user.MiddleName}', " +
                        $"Phone = '{user.Phone}', " +
                        $"Email = '{user.Email}' " +
                        $"WHERE Id = {user.Id}";
                var command = new MySqlCommand(updateQuery, _connection);
                OpenConnection();
                command.ExecuteNonQuery();
            }
            CloseConnection();
        }
        /// <summary>
        /// Проверяет пользователя на правильно введённые данные
        /// </summary>
        private bool IsValidUser(User user)
        {
            if (!InitialsValidator.IsValidInitials(user.FirstName))
            {
                throw new ArgumentNullException(nameof(user.FirstName), "First name should consist of latin latters and not be a null");
            }
            if (!InitialsValidator.IsValidInitials(user.LastName))
            {
                throw new ArgumentNullException(nameof(user.LastName), "Last name should consist of latin latters and not be a null");
            }
            if (!InitialsValidator.IsValidInitials(user.MiddleName))
            {
                throw new ArgumentNullException(nameof(user.MiddleName), "Middle name should consist of latin latters and not be a null");
            }
            if (PhoneValidator.IsValidPhone(user.Phone))
            {
                throw new ArgumentException("Phone should be in format \"+79xxxxxxxxx\"");
            }
            if (!EmailValidator.IsValidEmail(user.Email))
            {
                throw new ArgumentException("Mail should be in format \"*@*.*\"");
            }

            return true;
        }
        /// <summary>
        /// Удаляет запись в базе данных
        /// </summary>
        public void Delete(int id)
        {
            var deleteQuery = $"DELETE FROM Users " +
                              $"WHERE `Id` = {id}";
            var command = new MySqlCommand(deleteQuery, _connection);

            OpenConnection();
            command.ExecuteNonQuery();
            CloseConnection();
        }
        /// <summary>
        /// Открывает соединение
        /// </summary>
        private void OpenConnection() => _connection.Open();
        /// <summary>
        /// Закрывает соединение
        /// </summary>
        private void CloseConnection() => _connection.Close();
    }
}
