using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Users;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class UserServiceV1 : IUserServiceV1
    {
        IDataProvider _data = null;

        public UserServiceV1(IDataProvider data)
        {
            _data = data;
        }

        public User Get(int id)
        {
            string procName = "[dbo].[Users_SelectById]";

            User user = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            },
            delegate (IDataReader reader, short set)
            {
                user = MapSingleUser(reader);
            }
            );
            return user;

        }

        public List<User> GetAll()
        {
            List<User> userList = null;
            string procName = "[dbo].[Users_SelectAll]";

            _data.ExecuteCmd(procName
                       , inputParamMapper: null
                       , singleRecordMapper: delegate (IDataReader reader, short set)
                       {
                           User aUser = MapSingleUser(reader);

                           if (userList == null)
                           {
                               userList = new List<User>();
                           }
                           userList.Add(aUser);
                       }
                    );
            return userList;

        }

        public int Add(UserAddRequest model, int userId)
        {
            int id = 0;

            string procName = "[dbo].[Users_Insert]";
            _data.ExecuteNonQuery(procName,
                     inputParamMapper: delegate (SqlParameterCollection collection)
                     {
                         AddCommonParams(model, collection);

                         SqlParameter idOutput = new SqlParameter("@Id", SqlDbType.Int);
                         idOutput.Direction = ParameterDirection.Output;

                         collection.Add(idOutput);
                     },
                     returnParameters: delegate (SqlParameterCollection returnCollection)
                     {
                         object objectId = returnCollection["@Id"].Value;

                         int.TryParse(objectId.ToString(), out id);
                     });
            return id;
        }

        public void Update(UserUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Users_Update]";
            _data.ExecuteNonQuery(procName,
                       inputParamMapper: delegate (SqlParameterCollection collection)
                       {
                           AddCommonParams(model, collection);
                           collection.AddWithValue("@Id", model.Id);
                       },
                       returnParameters: null);
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Users_Delete]";
            _data.ExecuteNonQuery(procName,
                     inputParamMapper: delegate (SqlParameterCollection collection)
                     {
                         collection.AddWithValue("Id", id);
                     },
                     returnParameters: null);
        }


        private static User MapSingleUser(IDataReader reader)
        {
            User aUser = new User();

            int startingIdex = 0;

            aUser.Id = reader.GetSafeInt32(startingIdex++);
            aUser.FirstName = reader.GetSafeString(startingIdex++);
            aUser.LastName = reader.GetSafeString(startingIdex++);
            aUser.Email = reader.GetSafeString(startingIdex++);
            aUser.AvatarUrl = reader.GetSafeString(startingIdex++);
            aUser.TenantId = reader.GetSafeString(startingIdex++);
            aUser.DateCreated = reader.GetSafeDateTime(startingIdex++);
            aUser.DateModified = reader.GetSafeDateTime(startingIdex++);

            return aUser;

        }

        private static void AddCommonParams(UserAddRequest model, SqlParameterCollection collection)
        {
            collection.AddWithValue("@FirstName", model.FirstName);
            collection.AddWithValue("@LastName", model.LastName);
            collection.AddWithValue("@Email", model.Email);
            collection.AddWithValue("@AvatarUrl", model.AvatarUrl);
            collection.AddWithValue("@TenantId", model.TenantId);
            collection.AddWithValue("@Password", model.Password);
            collection.AddWithValue("@PasswordConfirm", model.PasswordConfirm);

        }


    }
}
