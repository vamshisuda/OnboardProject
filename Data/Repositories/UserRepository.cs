using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.DTOs;
using UserAPI.Models;
using UserAPI.Interfaces;
using System;
using Dapper;
using System.Data;
using UserAPI.Utility;

namespace UserAPI.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;
        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> Create(CreateUserDTO createUserDTO)
        {
            int result;
            string sqlQuery = "INSERT into Usersv (UserName, FirstName, LastName, Email) values (@User_Name, @First_Name, @Last_Name, @Email)";
            var parameters = new DynamicParameters();
            parameters.Add("User_Name", createUserDTO.UserName, DbType.String);
            parameters.Add("First_Name", createUserDTO.FirstName, DbType.String);
            parameters.Add("Last_Name", createUserDTO.LastName, DbType.String);
            parameters.Add("Email", createUserDTO.Email, DbType.String);
            Console.WriteLine("create user");
            using (var connection = _context.CreateConnection())
            {
                result = await connection.ExecuteAsync(sqlQuery, parameters);
            }
            if (result == 0)
                return result;
            else
            {
                User createdUser = await GetCreatedUserAsync(createUserDTO);
                return createdUser.UserId;
            }
        }
        public async Task<User> GetCreatedUserAsync(CreateUserDTO createdUserDTO) {
            string sqlQuery = "SELECT UserId,UserName,FirstName,LastName,Email FROM Usersv WHERE UserName = @User_Name and LastName = @Last_Name and FirstName = @First_Name and Email = @Email";
            var parameters = new DynamicParameters();
            parameters.Add("User_Name", createdUserDTO.UserName, DbType.String);
            parameters.Add("First_Name", createdUserDTO.FirstName, DbType.String);
            parameters.Add("Last_Name", createdUserDTO.LastName, DbType.String);
            parameters.Add("Email", createdUserDTO.Email, DbType.String);
            Console.WriteLine("created user");
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<User>(sqlQuery, parameters);
            }
             
        }
        public async Task<User> Delete(int id)
        {
            string query = "DELETE FROM Usersv WHERE UserId = @Id";
            string sqlQuery = "SELECT UserId,UserName,FirstName,LastName,Email FROM Usersv WHERE UserId = @Id";

            using (var connection = _context.CreateConnection())
            {
                var duser = await connection.QueryFirstOrDefaultAsync<User>(sqlQuery, new { Id = id });
                await connection.ExecuteAsync(query, new { Id = id });
                return duser;
            }
            
        }

        public async Task<PagedList<User>> GetAll(PagingParams pagingParams)
        {
            
            string sqlQuery = "SELECT  UserId,UserName,FirstName,LastName,Email FROM Usersv ORDER BY UserId" ;
            Console.WriteLine(sqlQuery);
            List<User> totalUsers = null;
            using (var connection = _context.CreateConnection())
            {
                var Usersv = await connection.QueryAsync<User>(sqlQuery);
                totalUsers = Usersv.ToList();
            }
            var pagedData = new PagedList<User>(
                totalUsers, pagingParams.PageNumber, pagingParams.PageSize);
            return pagedData;
        }

        public async Task<User> GetById(int id)
        {
            string sqlQuery = "SELECT * FROM Usersv WHERE UserId = @Id";
            Console.WriteLine(sqlQuery);
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<User>(sqlQuery, new { Id = id });
            }
        }

        public async Task<User> Update(GeneralUserDTO updateUserDTO, int id)
        {
            string u1 = updateUserDTO.UserName != null ? "UserName = @User_Name ," : "";
            string u2 = updateUserDTO.LastName != null ? "LastName = @Last_Name ," : "";
            string u3 = updateUserDTO.FirstName != null ? "FirstName = @First_Name ," : "";
            string u4 = updateUserDTO.Email != null ? "Email = @Email" : "";
            string ucon = u1 + u2 + u3 + u4;
            if (ucon.EndsWith(","))
                ucon= ucon.Substring(0, ucon.Length - 1);
            
            string sqlQuery = "UPDATE Usersv SET "+ucon+" WHERE UserId = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("User_Name", updateUserDTO.UserName, DbType.String);
            parameters.Add("First_Name", updateUserDTO.FirstName, DbType.String);
            parameters.Add("Last_Name", updateUserDTO.LastName, DbType.String);
            parameters.Add("Email", updateUserDTO.Email, DbType.String);
            parameters.Add("Id", id, DbType.Int32);
            Console.WriteLine(sqlQuery);
            int  result;
            using (var connection = _context.CreateConnection())
            {
                result = await connection.ExecuteAsync(sqlQuery, parameters);
                
            }
            var updatedUser = await GetById(id);
            return updatedUser;
        }

        public async Task<IEnumerable<User>> Search(GeneralUserDTO searchDTO)
        {
            string consql = QueryFormat(searchDTO);
            string sqlQuery = "SELECT  UserId,UserName,FirstName,LastName,Email FROM Usersv "+consql;
            Console.WriteLine(sqlQuery);
            using (var connection = _context.CreateConnection())
            {
                var Usersv = await connection.QueryAsync<User>(sqlQuery);
                return Usersv.ToList();
            }
        }

        public string QueryFormat(GeneralUserDTO generalDTO)
        {
            string c1 = generalDTO.UserName != String.Empty && generalDTO.UserName != null ? " UserName like '%" + generalDTO.UserName + "%' or" : "";
            string c2 = generalDTO.FirstName != String.Empty && generalDTO.FirstName != null ? " FirstName like '%" + generalDTO.FirstName + "%' or" : "";
            string c3 = generalDTO.LastName != String.Empty && generalDTO.LastName != null ? " LastName like '%" + generalDTO.LastName + "%' or" : "";
            string c4 = generalDTO.Email != String.Empty && generalDTO.Email != null ? " Email like '%" + generalDTO.Email +"%' " : "";
            string condition = "where " + c1 + c2 + c3 + c4;
            if (condition.Contains('%'))
                if(condition.EndsWith("or"))
                    return condition.Substring(0, condition.Length - 2);
                else
                    return condition;
            else 
                return "";
        }
    }
}