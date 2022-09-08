using Sabio.Data;
using Sabio.Models.Domain.Addresses;
using Sabio.Models.Domain.Friends;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Addresses;
using Sabio.Models.Requests.Friends;
using Sabio.Models.Requests.Users;
using Sabio.Services;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace Sabio.Db.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Here are two example connection strings. Please check with the wiki and video courses to help you pick an option

            //string connString = @"Data Source=ServerName_Or_IpAddress;Initial Catalog=DB_Name;User ID=SabioUser;Password=Sabiopass1!";
            string connString = @"Data Source=104.42.194.102
                                ;Initial Catalog=C118_sandrat1790_gmail
                                ;User ID=C118_sandrat1790_gmail_User
                                ;Password=C118_sandrat1790_gmail_User765AADB0";

            TestConnection(connString);
            TestAddressService(connString);
            TestUserService(connString);
            TestFriendService(connString);


            Console.ReadLine();//this waits for you to hit the enter key before closing window
        }

        #region --- ADDRESS CRUD ---
        private static void TestAddressService(string myconnection)
        {
            #region - constructor calls - 
            SqlDataProvider provider = new SqlDataProvider(myconnection);

            IAddressService addressService = new AddressService(provider);
            #endregion

            #region - gets/selects -
            Address anaddress = addressService.Get(9);

            List<Address> addresses = addressService.GetRandomAddresses();


            #endregion

            #region - updates -
            AddressAddRequest request = new AddressAddRequest();

            request.LineOne = "45 sabio street";
            request.SuiteNumber = 95;
            request.City = "los angeles";
            request.State = "ca";
            request.PostalCode = "42356";


            int newId = addressService.Add(request);

            //-----------------------------------------------------------------------

            AddressUpdateRequest updateRequest = new AddressUpdateRequest();

            updateRequest.LineOne = "444 s flower st";
            updateRequest.SuiteNumber = 5901;
            updateRequest.City = "denver";
            updateRequest.State = "colorado";
            updateRequest.PostalCode = "80238";


            updateRequest.Id = newId;

            addressService.Update(updateRequest);

            Address upAddress = addressService.Get(newId);

            //-----------------------------------------------------------------------

            AddressDeleteRequest deleteRequest = new AddressDeleteRequest();

            deleteRequest.Id = newId;

            //-----------------------------------------------------------------------

            #endregion
            Console.WriteLine(upAddress.Id.ToString());


        }
        #endregion


        #region --- USERV1 CRUD ---
        private static void TestUserService(string myConnection)
        {

            #region -- Constructor Calls --
            SqlDataProvider provider = new SqlDataProvider(myConnection);

            IUserServiceV1 userServiceV1 = new UserServiceV1(provider);
            #endregion

            #region -- Gets, Selects --
            User aUser = userServiceV1.Get(9);

            List<User> users = userServiceV1.GetAll();
            #endregion

            #region -- Add, Update, Delete --
            //----Add Request----------------------------------------------
            UserAddRequest request = new UserAddRequest();

            request.FirstName = "";
            request.LastName = "";
            request.Email = "";
            request.AvatarUrl = "";
            request.TenantId = "";
            request.Password = "";
            request.PasswordConfirm = "";

            int newId = userServiceV1.Add(request);
            //---------------------------------------------------------------
            //----Update Request---------------------------------------------
            UserUpdateRequest updateRequest = new UserUpdateRequest();

            updateRequest.FirstName = "";
            updateRequest.LastName = "";
            updateRequest.Email = "";
            updateRequest.AvatarUrl = "";
            updateRequest.TenantId = "";
            updateRequest.Password = "";
            updateRequest.PasswordConfirm = "";

            updateRequest.Id = newId;

            userServiceV1.Update(updateRequest);

            User upUser = userServiceV1.Get(newId);
            //---------------------------------------------------------------
            //----Delete Request---------------------------------------------
            UserDeleteRequest deleteRequest = new UserDeleteRequest();

            deleteRequest.Id = newId;
            #endregion

            Console.WriteLine(upUser.Id.ToString());

        }
        #endregion


        #region --- FRIEND CRUD ---
        private static void TestFriendService(string myConnection)
        {

            #region -- Constructor Calls --
            SqlDataProvider provider = new SqlDataProvider(myConnection);

            IFriendService friendService = new FriendService(provider);
            #endregion

            #region -- Gets, Selects --
            Friend aFriend = friendService.Get(9);

            List<Friend> friends = friendService.GetAll();

            //FriendPaginationRequest paginate = friendService.Pagination();

            //FriendSearchRequest search = friendService.Search();

            #endregion


            #region -- Add, Update, Delete --
            //----Add Request----------------------------------------------

            FriendAddRequest request = new FriendAddRequest();

            request.Title = "";
            request.Bio = "";
            request.Summary = "";
            request.Headline = "";
            request.Slug = "";
            request.StatusId = 1;
            request.PrimaryImageUrl = "";
            int UserId = 61790;

            int newId = friendService.Add(request, UserId);


            //---------------------------------------------------------------
            //----Update Request---------------------------------------------

            FriendUpdateRequest updateRequest = new FriendUpdateRequest();
            updateRequest.Title = "";
            updateRequest.Bio = "";
            updateRequest.Summary = "";
            updateRequest.Headline = "";
            updateRequest.Slug = "";
            updateRequest.StatusId = 1;
            updateRequest.PrimaryImageUrl = "";

            updateRequest.Id = newId;

            friendService.Update(updateRequest, UserId);

            Friend uFriend = friendService.Get(newId);

            //---------------------------------------------------------------
            //----Delete Request---------------------------------------------

            FriendDeleteRequest deleteRequest = new FriendDeleteRequest();





            #endregion

            Console.WriteLine();

        } 
        #endregion

        private static void TestConnection(string connString)
        {
            bool isConnected = IsServerConnected(connString);
            Console.WriteLine("DB isConnected = {0}", isConnected);
        }

        private static bool IsServerConnected(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }
    }
}
