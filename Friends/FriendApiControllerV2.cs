using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Friends;
using Sabio.Models.Requests.Friends;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/v2/friends")]
    [ApiController]
    public class FriendApiControllerV2 : BaseApiController
    {
        private IFriendService _service = null;
        private IAuthenticationService<int> _authService = null;

        public FriendApiControllerV2(IFriendService serivce
                    , ILogger<FriendApiControllerV2> logger
                    , IAuthenticationService<int> authService) : base(logger)
        {
            _service = serivce;
            _authService = authService;
        }


       


        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<FriendV2>> SelectById(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                FriendV2 friendV2 = _service.GetV2(id);

                if( friendV2 == null)
                {
                    code = 404;
                    response = new ErrorResponse("Friend not found");
                }
                else
                {
                    response = new ItemResponse<FriendV2> { Item =friendV2 };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                Logger.LogError(ex.ToString()); 
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }


        [HttpGet]
        public ActionResult<ItemsResponse<FriendV2>> GetAll()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<FriendV2> list = _service.GetAllV2();

                if(list.Count == 0)
                {
                    code = 404;
                    response = new ErrorResponse("Friend not found");
                }
                else
                {
                    response = new ItemsResponse<FriendV2> { Items = list };
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


        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<FriendV2>>> GetPage(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<FriendV2> page = _service.PaginationV2(pageIndex, pageSize);

                if(page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Friend Resource not found");
                }
                else
                {
                    response = new ItemResponse<Paged<FriendV2>> { Item = page };
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


        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<FriendV2>>> SearchPage(int pageIndex, int pageSize, string query)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<FriendV2> page = _service.Search_PaginationV2(pageIndex, pageSize, query);

                if(page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Records not found");
                }
                else
                {
                    response = new ItemResponse<Paged<FriendV2>> { Item = page };
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
    }
}
