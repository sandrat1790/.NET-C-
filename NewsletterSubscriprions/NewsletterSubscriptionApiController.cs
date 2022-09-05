using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.NewsletterSubscriptions;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/newsletter/subscriptions")]
    [ApiController]
    public class NewsletterSubscriptionApiController : BaseApiController
    {
        private INewsletterSubscriptionService _service = null;
        private IAuthenticationService<int> _authService = null;

        public NewsletterSubscriptionApiController(INewsletterSubscriptionService service
            , ILogger<NewsletterSubscriptionApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpPost]
        public ActionResult<ItemResponse<string>> Add(NewsletterSubscriptionAddRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.Add(model);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }
      

        [HttpPut("{email}")]
        public ActionResult<SuccessResponse> Update(NewsletterSubscriptionUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.Update(model);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }
 

        [HttpGet("subscribers")]
        public ActionResult<ItemsResponse<NewsletterSubscription>> GetAllSubscribed()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<NewsletterSubscription> list = _service.GetAllSubscribed();

                if(list.Count == 0)
                {
                    code = 404;
                    response = new ErrorResponse("Record not found");
                }
                else
                {
                    response = new ItemsResponse<NewsletterSubscription> { Items = list };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }


        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<NewsletterSubscription>>> GetByPage(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<NewsletterSubscription> page = _service.GetByPage(pageIndex, pageSize);
                pageIndex = 0;
                pageSize = 5;

                if(page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Records Not Found");
                }
                else
                {
                    response = new ItemResponse<Paged<NewsletterSubscription>> { Item = page };
                }
            }
            catch(Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("paginate_search")]
        public ActionResult<ItemResponse<Paged<NewsletterSubscription>>> GetByCreatedBy(int pageIndex, int pageSize, string createdBy)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<NewsletterSubscription> page = _service.GetByCreatedBy(pageIndex, pageSize, createdBy);

                if(page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Records Not Found");
                }
                else
                {
                    response = new ItemResponse<Paged<NewsletterSubscription>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

    }
}
