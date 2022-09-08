using Sabio.Models;
using Sabio.Models.Domain.Friends;
using Sabio.Models.Requests.Friends;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IFriendService
    {
        int Add(FriendAddRequest model, int UserId);
        void Delete(int id);
        Friend Get(int id);
        List<Friend> GetAll();
        void Update(FriendUpdateRequest model, int UserId);
        Paged<Friend> Pagination(int pageIndex, int pageSize);
        //public Paged<Friend> Search_Pagination(int pageIndex, int pageSize, string query);



       // void AddImages(FriendV2 model);
        //void UpdateFriendV2(FriendV2UpdateRequest model, int userId);
        FriendV2 GetV2(int id);
        List<FriendV2> GetAllV2();
        Paged<FriendV2> PaginationV2(int pageIndex, int pageSize);
        Paged<FriendV2> Search_PaginationV2(int pageIndex, int pageSize, string query);

        FriendV3 GetV3(int id);
        List<FriendV3> GetAllV3();
        Paged<FriendV3> PaginationV3(int pageIndex, int pageSize);
        Paged<FriendV3> Search_PaginationV3(int pageIndex, int pageSize, string query);


    }
}