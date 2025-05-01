using System;
using System.IO;
using System.Data.SQLite;
using System.Runtime.Intrinsics.Arm;
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
                VALUES (@Username, @Password, @Role);
                SELECT last_insert_rowid();";

            return connection.ExecuteScalar<long>(query, user);
        }
        public void UpdateReceiver(Receiver receiver)
        {
            using var connection = CreateConnection();
            const string query = @"
            UPDATE Receiver SET
                login = @Login,
                password = @Password,
                name = @Name,
                surname = @Surname,
                middle_name = @MiddleName,
                nationality = @Nationality,
                date_of_birth = @DateOfBirth,
                address_of_birth = @AddressOfBirth,
                gender = @Gender,
                documental_address = @DocumentalAddress,
                current_address = @CurrentAddress,
                phone_number = @PhoneNumber,
                card_number = @CardNumber,
                twoPhotos_of_passport = @TwoPhotosOfPassport
                WHERE id = @Id";
            connection.Execute(query, receiver);
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
        public void InsertUserData(Receiver data)
        {
            using var connection = CreateConnection();
            const string query = @"
            INSERT INTO USERSDATA (
                 id_user, firstname, secondname, middlename,
                 nationality, date_of_birth, address_of_birth,
                 gender, addres_offical, addres_current,
                 phone_number, card_number
                ) VALUES (
                @Id_user, '', '', '', '', @DateOfBirth, '', '', '', '', '', ''
        );";

            connection.Execute(query, data);
        }
        public Receiver? GetUserDataByUserId(long userId)
        {
            using var connection = CreateConnection();
            const string query = @"
            SELECT 
                id,
                id_user AS IdUser,
                firstname AS FirstName,
                secondname AS SecondName,
                middlename AS MiddleName,
                nationality AS Nationality,
                date_of_birth AS DateOfBirth,
                address_of_birth AS AddresOfBirth,
                gender AS Gender,
                addres_offical AS AddresOffical,
                addres_current AS AddresCurrent,
                phone_number AS PhoneNumber,
                 card_number AS CardNumber
            FROM USERSDATA
            WHERE id_user = @UserId;";

            return connection.QueryFirstOrDefault<Receiver>(query, new { UserId = userId });
        }
        public void InsertAdminData(Admin data)
        {
            using var connection = CreateConnection();
            const string query = @"
            INSERT INTO ADMINDATA (
                id_user, firstname, secondname, middlename, id_help_point
            ) VALUES (
                @IdUser, @FirstName, @SecondName, @MiddleName, @IdHelpPoint
            );";

            connection.Execute(query, data);
        }
        public Admin? GetAdminDataByUserId(long userId)
        {
            using var connection = CreateConnection();
            const string query = @"
            SELECT 
                 id,
                 id_user AS IdUser,
                 firstname AS FirstName,
                 secondname AS SecondName,
                 middlename AS MiddleName,
                 id_help_point AS IdHelpPoint
            FROM ADMINDATA
            WHERE id_user = @UserId;";

            return connection.QueryFirstOrDefault<Admin>(query, new { UserId = userId });
        }
    }
}
