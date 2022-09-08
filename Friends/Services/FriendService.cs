using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain.Friends;
using Sabio.Models.Requests.Friends;
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
    public class FriendService : IFriendService
    {
        IDataProvider _data = null;


        public FriendService(IDataProvider data)
        {
            _data = data;
        }

        #region --- FRIENDS V1 SERVICE METHODS ---
        public Friend Get(int id)
        {
            string procName = "[dbo].[Friends_SelectById]";

            Friend friend = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            },
            delegate (IDataReader reader, short set)
            {
                int startingIdex = 0;
                friend = MapSingleFriend(reader, ref startingIdex);


            });
            return friend;
        }

        public List<Friend> GetAll()
        {
            List<Friend> friendsList = null;
            string procName = "[dbo].[Friends_SelectAll]";

            _data.ExecuteCmd(procName, inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIdex = 0;
                Friend aFriend = MapSingleFriend(reader, ref startingIdex);

                if (friendsList == null)
                {
                    friendsList = new List<Friend>();
                }
                friendsList.Add(aFriend);
            });
            return friendsList;
        }

        public int Add(FriendAddRequest model, int userId)
        {

            int id = 0;

            string procName = "[dbo].[Friends_Insert]";

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

        public void Update(FriendUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Friends_Update]";

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
            string procName = "[dbo].[Friends_Delete]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@Id", id);
            },
            returnParameters: null);
        }

        public Paged<Friend> Pagination(int pageIndex, int pageSize)
        {
            Paged<Friend> pagedResult = null;

            List<Friend> result = null;

            int totalCount = 0;

            _data.ExecuteCmd("dbo.Friends_Pagination", inputParamMapper: delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@PageIndex", pageIndex);
                parameterCollection.AddWithValue("@PageSize", pageSize);
            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                Friend model = MapSingleFriend(reader, ref index);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(index++);
                }
                if (result == null)
                {
                    result = new List<Friend>();
                }
                result.Add(model);
            });
            if(result != null)
            {
                pagedResult = new Paged<Friend>(result, pageIndex, pageSize, totalCount);
            };
            return pagedResult;
        }



        private static Friend MapSingleFriend(IDataReader reader, ref int startingIndex)
        {
            Friend aFriend = new Friend();



            aFriend.Id = reader.GetSafeInt32(startingIndex++);
            aFriend.Title = reader.GetSafeString(startingIndex++);
            aFriend.Bio = reader.GetSafeString(startingIndex++);
            aFriend.Summary = reader.GetSafeString(startingIndex++);
            aFriend.Headline = reader.GetSafeString(startingIndex++);
            aFriend.Slug = reader.GetSafeString(startingIndex++);
            aFriend.StatusId = reader.GetSafeInt32(startingIndex++);
            aFriend.PrimaryImageUrl = reader.GetSafeString(startingIndex++);
            aFriend.DateCreated = reader.GetSafeDateTime(startingIndex++);
            aFriend.DateModified = reader.GetSafeDateTime(startingIndex++);
            aFriend.UserId = reader.GetSafeInt32(startingIndex++);

            return aFriend;

        }

        private static void AddCommonParams(FriendAddRequest model, SqlParameterCollection collection)
        {
            collection.AddWithValue("@Title", model.Title);
            collection.AddWithValue("@Bio", model.Bio);
            collection.AddWithValue("@Summary", model.Summary);
            collection.AddWithValue("@Headline", model.Headline);
            collection.AddWithValue("@Slug", model.Slug);
            collection.AddWithValue("@StatusId", model.StatusId);
            collection.AddWithValue("@PrimaryImageUrl", model.PrimaryImageUrl);
        }
        #endregion


        #region --- FRIENDS V2 SERVICE METHODS ---

        


        public FriendV2 GetV2(int id) 
        {
            string procName = "[dbo].[Friends_SelectByIdV2]";

            FriendV2 friend = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            },
            delegate (IDataReader reader, short set)
            {
                int startingIdex = 0;
                friend = MapSingleFriendV2(reader, ref startingIdex);


            });
            return friend;
        }


        public List<FriendV2> GetAllV2() 
        {
            List<FriendV2> friendsV2List = null;
            string procName = "[dbo].[Friends_SelectAllV2]";

            _data.ExecuteCmd(procName, inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIdex = 0;
                FriendV2 v2Friend = MapSingleFriendV2(reader, ref startingIdex);


                if (friendsV2List == null)
                {
                    friendsV2List = new List<FriendV2>();
                }
                friendsV2List.Add(v2Friend);
                });
                return friendsV2List;
            }


        public Paged<FriendV2> PaginationV2(int pageIndex, int pageSize)
        {
            Paged<FriendV2> pagedResult = null;

            List<FriendV2> result = null;

            int totalCount = 0;

            _data.ExecuteCmd(
                "dbo.Friends_PaginationV2",
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {

                    FriendV2 model = new FriendV2();
                    int index = 0;
                    int userId = 0;
                    model = MapSingleFriendV2(reader, ref userId);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(index++);
                    }


                    if (result == null)
                    {
                        result = new List<FriendV2>();
                    }

                    result.Add(model);
                }

            );
            if (result != null)
            {
                pagedResult = new Paged<FriendV2>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }


        public Paged<FriendV2> Search_PaginationV2(int pageIndex, int pageSize, string query)
        {
            Paged<FriendV2> pagedResult = null;
            List<FriendV2> result = null;
            int totalCount = 0;

            _data.ExecuteCmd("dbo.Friends_Search_PaginationV2", inputParamMapper: delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@PageIndex", pageIndex);
                parameterCollection.AddWithValue("@PageSize", pageSize);
                parameterCollection.AddWithValue("@Query", query);

            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                FriendV2 model = new FriendV2();
                int index = 0;
                int userId = 0;
                model = MapSingleFriendV2(reader, ref userId);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(index++);
                }
                if (result == null)
                {
                    result = new List<FriendV2>();
                }
                result.Add(model);

            }
            );
            if (result != null)
            {
                pagedResult = new Paged<FriendV2>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }




        private static FriendV2 MapSingleFriendV2(IDataReader reader, ref int startingIndex) 
        {
            FriendV2 v2Friend = new FriendV2();
            Image image = v2Friend.PrimaryImage;

            v2Friend.Id = reader.GetSafeInt32(startingIndex++);
            image.Url = reader.GetString(startingIndex++);
            image.ImageId = reader.GetSafeInt32(startingIndex++);
            image.TypeId = reader.GetSafeInt32(startingIndex++);
            v2Friend.Title = reader.GetString(startingIndex++);
            v2Friend.Bio = reader.GetString(startingIndex++);
            v2Friend.Summary = reader.GetString(startingIndex++);
            v2Friend.Headline = reader.GetString(startingIndex++);
            v2Friend.Slug = reader.GetString(startingIndex++);
            v2Friend.StatusId = reader.GetSafeInt32(startingIndex++);
            v2Friend.DateCreated = reader.GetSafeUtcDateTime(startingIndex++); 
            v2Friend.DateModified = reader.GetSafeUtcDateTime(startingIndex++);
            v2Friend.UserId = reader.GetSafeInt32(startingIndex++);

            return v2Friend;

        }




        #endregion


        #region --- FRIENDS V3 SERVICE METHODS ---

    




        public FriendV3 GetV3(int id)
        {
            string procName = "[dbo].[Friends_SelectByIdV3]";

            FriendV3 friend = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            },
            delegate (IDataReader reader, short set)
            {
                int startingIdex = 0;
                friend = MapSingleFriendV3(reader, ref startingIdex);


            });
            return friend;
        }


        public List<FriendV3> GetAllV3() 
        {
            List<FriendV3> list = null;
            string procName = "[dbo].[Friends_SelectAllV3]";

            _data.ExecuteCmd(procName, inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                FriendV3 v3Friend = MapSingleFriendV3(reader, ref startingIndex);

                if (list == null)
                {
                    list = new List<FriendV3>();
                }
                list.Add(v3Friend);
            });

            return list;
        }


        public Paged<FriendV3> PaginationV3(int pageIndex, int pageSize)
        {
            Paged<FriendV3> pagedResult = null;

            List<FriendV3> result = null;

            int totalCount = 0;

            _data.ExecuteCmd(
                "[dbo].[Friends_PaginationV3]", inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {

                    int index = 0;
                    FriendV3 model = MapSingleFriendV3(reader, ref index);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(index++);
                    }
                    if (result == null)
                    {
                        result = new List<FriendV3>();
                    }
                    result.Add(model);
                }
              );
              if(result != null)
               {
                pagedResult = new Paged<FriendV3>(result, pageIndex, pageSize, totalCount);
               }
               return pagedResult;
        }


        public Paged<FriendV3> Search_PaginationV3(int pageIndex, int pageSize, string query)
        {

            Paged<FriendV3> pagedResult = null;
            List<FriendV3> result = null;
            int totalCount = 0;

            _data.ExecuteCmd("dbo.Friends_Search_PaginationV3", inputParamMapper: delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@PageIndex", pageIndex);
                parameterCollection.AddWithValue("@PageSize", pageSize);
                parameterCollection.AddWithValue("@Query", query);
            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                FriendV3 model = MapSingleFriendV3(reader, ref index);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(index++);
                }
                if (result == null)
                {
                    result = new List<FriendV3>();
                }
                result.Add(model);
            }
            );
            if(result != null)
            {
                pagedResult = new Paged<FriendV3>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }



        private static FriendV3 MapSingleFriendV3(IDataReader reader, ref int startingIndex)
        {
            FriendV3 v3Friend = new FriendV3();
            Image image = v3Friend.PrimaryImage;

            v3Friend.Id = reader.GetSafeInt32(startingIndex++);
            image.Url = reader.GetString(startingIndex++);
            image.ImageId = reader.GetSafeInt32(startingIndex++);
            image.TypeId = reader.GetSafeInt32(startingIndex++);
            v3Friend.Title = reader.GetString(startingIndex++);
            v3Friend.Bio = reader.GetString(startingIndex++);
            v3Friend.Summary = reader.GetString(startingIndex++);
            v3Friend.Headline = reader.GetString(startingIndex++);
            v3Friend.Slug = reader.GetString(startingIndex++);
            v3Friend.StatusId = reader.GetSafeInt32(startingIndex++);
            v3Friend.DateCreated = reader.GetSafeUtcDateTime(startingIndex++);
            v3Friend.DateModified = reader.GetSafeUtcDateTime(startingIndex++);
            v3Friend.UserId = reader.GetSafeInt32(startingIndex++);

            //string skillsAsString = reader.GetString(startingIndex++);

            //if (!string.IsNullOrEmpty(skillsAsString))
            //{
            //    v3Friend.Skills = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Skill>>(skillsAsString);
            //}

            //v3Friend.Skills = reader.DeserializeObject<List<Skill>>(startingIndex++);

            return v3Friend;

        }



        #endregion



    }

}
