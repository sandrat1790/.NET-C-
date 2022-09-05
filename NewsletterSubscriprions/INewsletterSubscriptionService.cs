using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.NewsletterSubscriptions;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface INewsletterSubscriptionService
    {
        void Add(NewsletterSubscriptionAddRequest model);
        List<NewsletterSubscription> GetAllSubscribed();
        Paged<NewsletterSubscription> GetByCreatedBy(int pageIndex, int pageSize, string createdBy);
        Paged<NewsletterSubscription> GetByPage(int pageIndex, int pageSize);
        void Update(NewsletterSubscriptionUpdateRequest model);
    }
}