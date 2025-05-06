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
        public UserData? GetUserDataById(long userDataId)
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
WHERE id = @UserDataId;";

            return connection.QueryFirstOrDefault<UserData>(query, new { UserDataId = userDataId });
        }

        public void InsertAdminData(Admin data)
        {
            using var connection = CreateConnection();
            const string query = @"
        INSERT INTO ADMINDATA (
        id_user, firstname, secondname, middlename, id_help_point
        ) VALUES (
        @id_user, '', '', '', @id_help_point
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

        // Новий метод для оновлення даних адміністратора
        public void UpdateAdminData(Admin adminData)
        {
            using var connection = CreateConnection();
            const string query = @"
        UPDATE ADMINDATA SET
            firstname = @firstname,
            secondname = @secondname,
            middlename = @middlename
        WHERE id_user = @id_user;";

            int rowsAffected = connection.Execute(query, adminData);
            Debug.WriteLine($"Admin data update - rows affected: {rowsAffected}");
        }

        // Новий метод для отримання адреси пункту допомоги за його ID
        public string GetHelpPointAddressById(long helpPointId)
        {
            using var connection = CreateConnection();
            const string query = @"
        SELECT addres
        FROM HELPPOINT 
        WHERE id = @HelpPointId;";

            return connection.QueryFirstOrDefault<string>(query, new { HelpPointId = helpPointId })
                ?? "Адресу не знайдено";
        }

        // Метод для отримання інформації про пункт допомоги за його ID
        //public HelpPoint? GetHelpPointById(long helpPointId)
        //{
        //    using var connection = CreateConnection();
        //    const string query = @"
        //SELECT 
        //    id,
        //    name,
        //    address,
        //    phone_number,
        //    email,
        //    working_hours
        //FROM HELPPOINT 
        //WHERE id = @HelpPointId;";

        //    return connection.QueryFirstOrDefault<HelpPoint>(query, new { HelpPointId = helpPointId });
        //}

        // Методи для роботи з заявами на допомогу
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
        WHERE id_user_data = @ApplicationId;";

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

        // Додатковий метод для отримання розширеної інформації про заявки
        public List<ApplicationViewModel> GetExtendedApplicationsForHelpPoint(long helpPointId, string? state = null)
        {
            using var connection = CreateConnection();
            string whereClause = "a.id_help_point = @HelpPointId";

            if (!string.IsNullOrEmpty(state))
            {
                whereClause += " AND a.state = @State";
            }

            string query = $@"
SELECT 
    a.id AS Id,
    a.id_user_data AS UserDataId,
    a.state AS Status,
    a.created_at AS SubmitDate,
    a.updated_at AS UpdatedDate,
    ud.firstname || ' ' || ud.secondname || ' ' || ud.middlename AS FullName,
    ud.phone_number AS Phone,
    a.message_kind_of_help AS RequestedHelp
FROM APPLICATION a
JOIN USERSDATA ud ON a.id_user_data = ud.id
WHERE {whereClause}
ORDER BY a.created_at DESC;";

            return connection.Query<ApplicationViewModel>(query, new { HelpPointId = helpPointId, State = state }).ToList();
        }

        // Метод для підрахунку статистики заявок за статусами
        public Dictionary<string, int> GetApplicationStatisticsByHelpPoint(long helpPointId)
        {
            using var connection = CreateConnection();
            const string query = @"
        SELECT 
            state AS Status,
            COUNT(*) AS Count
        FROM APPLICATION
        WHERE id_help_point = @HelpPointId
        GROUP BY state;";

            var result = connection.Query<ApplicationStatusCount>(query, new { HelpPointId = helpPointId }).ToList();

            // Підготовка словника з результатами
            var statistics = new Dictionary<string, int>
        {
            { "Total", 0 },
            { "У розгляді", 0 },
            { "Одобрені", 0 },
            { "Скасовані", 0 }
        };

            foreach (var item in result)
            {
                if (statistics.ContainsKey(item.Status))
                {
                    statistics[item.Status] = item.Count;
                }
                statistics["Total"] += item.Count;
            }

            return statistics;
        }
    }

    // Допоміжний клас для відображення заявок у DataGrid
    public class ApplicationViewModel
    {
        public long Id { get; set; }
        public long UserDataId { get; set; }  // Додано поле для id_user_data
        public string Status { get; set; } = string.Empty;
        public string SubmitDate { get; set; } = string.Empty;
        public string UpdatedDate { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string RequestedHelp { get; set; } = string.Empty;
    }

    // Допоміжний клас для статистики заявок
    public class ApplicationStatusCount
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    // Клас для зберігання інформації про пункт допомоги
    //public class HelpPoint
    //{
    //    public long Id { get; set; }
    //    public string Name { get; set; } = string.Empty;
    //    public string Address { get; set; } = string.Empty;
    //    public string PhoneNumber { get; set; } = string.Empty;
    //    public string Email { get; set; } = string.Empty;
    //    public string WorkingHours { get; set; } = string.Empty;
    //}
}

