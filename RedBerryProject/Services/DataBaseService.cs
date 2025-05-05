using System;
using System.Data.SQLite;
using System.IO;
using System.Diagnostics;
using Dapper;
using RedBerryProject.Models;

namespace RedBerryProject.Services
{
    public class DataBaseService
    {
        private readonly string _connectionString;

        public DataBaseService(string dbPath)
        {
            if (!File.Exists(dbPath))
            {
                throw new FileNotFoundException("Базу даних не знайдено.", dbPath);
            }
            _connectionString = $"Data Source={dbPath};Version=3;";
        }

        private SQLiteConnection CreateConnection()
        {
            var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public long InsertUser(User user)
        {
            using var connection = CreateConnection();
            const string query = @"
            INSERT INTO USERS (username, password, role)
            VALUES (@username, @password, @role);
            SELECT last_insert_rowid();";

            return connection.ExecuteScalar<long>(query, user);
        }

        public bool UserExists(string username)
        {
            using var connection = CreateConnection();
            const string query = "SELECT COUNT(1) FROM USERS WHERE username = @Username";
            return connection.ExecuteScalar<int>(query, new { Username = username }) > 0;
        }

        public User? GetUserByUsername(string username)
        {
            using var connection = CreateConnection();
            const string query = @"
        SELECT 
            id AS Id,
            username AS Username,
            password AS Password,
            role AS Role
        FROM USERS
        WHERE username = @Username";

            return connection.QueryFirstOrDefault<User>(query, new { Username = username });
        }

        public void InsertUserData(UserData data)
        {
            using var connection = CreateConnection();
            const string query = @"
        INSERT INTO USERSDATA (
            id_user, firstname, secondname, middlename,
            nationality, date_of_birth, addres_of_birth,
            gender, addres_offical, addres_current,
            phone_number, card_number
            ) VALUES (
            @id_user, '', '', '', '', @date_of_birth, '', '', '', '', '', ''
    );";

            connection.Execute(query, data);
        }

        public void UpdateUserData(UserData user_data)
        {
            using var connection = CreateConnection();

            const string query = @"
        UPDATE USERSDATA SET
            firstname = @firstname,
            secondname = @secondname,
            middlename = @middlename,
            nationality = @nationality,
            date_of_birth = @date_of_birth,
            addres_of_birth = @addres_of_birth,
            gender = @gender,
            addres_offical = @addres_offical,
            addres_current = @addres_current,
            phone_number = @phone_number,
            card_number = @card_number
        WHERE id_user = @id_user;
        ";

            int rowsAffected = connection.Execute(query, user_data);
            Debug.WriteLine($"Rows affected: {rowsAffected}");
        }

        public UserData? GetUserDataByUserId(long userId)
        {
            using var connection = CreateConnection();
            const string query = @"
        SELECT 
            id,
            id_user,
            firstname,
            secondname,
            middlename,
            nationality,
            date_of_birth,
            addres_of_birth,
            gender,
            addres_offical,
            addres_current,
            phone_number,
            card_number
        FROM USERSDATA
        WHERE id_user = @UserId;";

            return connection.QueryFirstOrDefault<UserData>(query, new { UserId = userId });
        }

        public void InsertAdminData(Admin data)
        {
            using var connection = CreateConnection();
            const string query = @"
        INSERT INTO ADMINDATA (
            id_user, firstname, secondname, middlename, id_help_point
        ) VALUES (
            @id_user, @firstname, @secondname, @middlename, @id_help_point
        );";

            connection.Execute(query, data);
        }

        public Admin? GetAdminDataByUserId(long userId)
        {
            using var connection = CreateConnection();
            const string query = @"
        SELECT 
            id,
            id_user,
            firstname, 
            secondname, 
            middlename,
            id_help_point
        FROM ADMINDATA
        WHERE id_user = @UserId;";

            return connection.QueryFirstOrDefault<Admin>(query, new { UserId = userId });
        }

        // Нові методи для роботи з заявами на допомогу

        public long InsertApplication(HelpApplicationData application)
        {
            using var connection = CreateConnection();
            const string query = @"
            INSERT INTO APPLICATION (
                id_user_data, 
                id_help_point, 
                message_why_ran_away, 
                message_kind_of_help, 
                state, 
                created_at, 
                updated_at
            ) VALUES (
                @id_user_data, 
                @id_help_point, 
                @message_why_ran_away, 
                @message_kind_of_help, 
                @state, 
                @created_at, 
                @updated_at
            );
            SELECT last_insert_rowid();";

            return connection.ExecuteScalar<long>(query, application);
        }

        public void UpdateApplicationState(long applicationId, string newState)
        {
            using var connection = CreateConnection();
            const string query = @"
            UPDATE APPLICATION 
            SET 
                state = @State,
                updated_at = @UpdatedAt
            WHERE id = @Id;";

            connection.Execute(query, new
            {
                Id = applicationId,
                State = newState,
                UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }

        public HelpApplicationData? GetApplicationById(long applicationId)
        {
            using var connection = CreateConnection();
            const string query = @"
            SELECT 
                id,
                id_user_data,
                id_help_point,
                message_why_ran_away,
                message_kind_of_help,
                state,
                created_at,
                updated_at
            FROM APPLICATION
            WHERE id = @ApplicationId;";

            return connection.QueryFirstOrDefault<HelpApplicationData>(query, new { ApplicationId = applicationId });
        }

        public List<HelpApplicationData> GetApplicationsByUserDataId(long userDataId)
        {
            using var connection = CreateConnection();
            const string query = @"
            SELECT 
                id,
                id_user_data,
                id_help_point,
                message_why_ran_away,
                message_kind_of_help,
                state,
                created_at,
                updated_at
            FROM APPLICATION
            WHERE id_user_data = @UserDataId
            ORDER BY created_at DESC;";

            return connection.Query<HelpApplicationData>(query, new { UserDataId = userDataId }).ToList();
        }

        public List<HelpApplicationData> GetApplicationsByHelpPointId(long helpPointId)
        {
            using var connection = CreateConnection();
            const string query = @"
            SELECT 
                a.id,
                a.id_user_data,
                a.id_help_point,
                a.message_why_ran_away,
                a.message_kind_of_help,
                a.state,
                a.created_at,
                a.updated_at
            FROM APPLICATION a
            WHERE a.id_help_point = @HelpPointId
            ORDER BY a.created_at DESC;";

            return connection.Query<HelpApplicationData>(query, new { HelpPointId = helpPointId }).ToList();
        }
    }
}

