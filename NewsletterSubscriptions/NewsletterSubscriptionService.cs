using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.NewsletterSubscriptions;
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
    public class NewsletterSubscriptionService : INewsletterSubscriptionService
    {
        IDataProvider _data = null;

        public NewsletterSubscriptionService(IDataProvider data)
        {
            _data = data;
        }

        public void Update(NewsletterSubscriptionUpdateRequest model)
        {
            _data.ExecuteNonQuery("dbo.NewsletterSubscriptions_Update", inputParamMapper: delegate (SqlParameterCollection collection)
            {

                collection.AddWithValue("@Email", model.Email);
                collection.AddWithValue("@IsSubscribed", model.IsSubscribed);

            },
            returnParameters: null);
        }

        public void Add(NewsletterSubscriptionAddRequest model)
        {
            
            _data.ExecuteNonQuery("dbo.NewsletterSubscriptions_Insert", inputParamMapper: delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@Email", model.Email);
                collection.AddWithValue("@IsSubscribed", model.IsSubscribed);
            },
            returnParameters: null);  
        }

        public Paged<NewsletterSubscription> GetByPage(int pageIndex, int pageSize)
        {
            Paged<NewsletterSubscription> pagedResult = null;
            List<NewsletterSubscription> result = null;
            int totalCount = 0;

            _data.ExecuteCmd("[dbo].[NewsletterSubscriptions_SelectAll]", inputParamMapper: delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@PageIndex", pageIndex);
                collection.AddWithValue("@PageSize", pageSize);
                
            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                NewsletterSubscription model = MapSingleEmail(reader, ref index);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(index++);
                }
                if (result == null)
                {
                    result = new List<NewsletterSubscription>();
                }
                result.Add(model);
            });
            if (result != null)
            {
                pagedResult = new Paged<NewsletterSubscription>(result, pageIndex, pageSize, totalCount);
            }
            return pagedResult;

        }

        public List<NewsletterSubscription> GetAllSubscribed()
        {
            List<NewsletterSubscription> list = null;

            _data.ExecuteCmd("[dbo].[NewsletterSubscriptions_SelectAll_Subscribed]", inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                NewsletterSubscription email = MapSingleEmail(reader, ref index);

                if (list == null)
                {
                    list = new List<NewsletterSubscription>();
                }
                list.Add(email);
            });
            return list;
        }

        public Paged<NewsletterSubscription> GetByCreatedBy(int pageIndex, int pageSize, string createdBy)
        {
            Paged<NewsletterSubscription> pagedResult = null;
            List<NewsletterSubscription> result = null;
            int totalCount = 0;

            _data.ExecuteCmd("[dbo].[NewsletterSubscriptions_Select_ByCreatedBy]", inputParamMapper: delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@PageIndex", pageIndex);
                collection.AddWithValue("@PageSize", pageSize);
                collection.AddWithValue("@CreatedBy", createdBy);
            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                NewsletterSubscription model = MapSingleEmail(reader, ref index);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(index++);
                }
                if (result == null)
                {
                    result = new List<NewsletterSubscription>();
                }
                result.Add(model);
            });
            if (result != null)
            {
                pagedResult = new Paged<NewsletterSubscription>(result, pageIndex, pageSize, totalCount);
            }
            return pagedResult;
        }





        private static NewsletterSubscription MapSingleEmail(IDataReader reader, ref int startingIndex)
        {
            NewsletterSubscription aRecord = new NewsletterSubscription();



            aRecord.Email = reader.GetSafeString(startingIndex++);
            aRecord.IsSubscribed = reader.GetSafeBool(startingIndex++);
            aRecord.DateCreated = reader.GetSafeDateTime(startingIndex++);
            aRecord.DateModified = reader.GetSafeDateTime(startingIndex++);

            return aRecord;
        }










    }
}
