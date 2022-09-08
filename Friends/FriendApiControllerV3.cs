using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Friends;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/v3/friends")]
    [ApiController]
    public class FriendApiControllerV3 : BaseApiController
    {
        private IFriendService _service = null;
        private IAuthenticationService<int> _authSerivce = null;

        public FriendApiControllerV3(IFriendService service
                  , ILogger<FriendApiControllerV3>logger
                  , IAuthenticationService<int> authSerivce) : base(logger)
        {
            _service = service;
            _authSerivce = authSerivce;
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<FriendV3>> SelectById(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                FriendV3 friendV3 = _service.GetV3(id);

                if( friendV3 == null)
                {
                    code = 404;
                    response = new ErrorResponse("Record not found");
                }
                else
                {
                    response = new ItemResponse<FriendV3> { Item = friendV3 };
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
        public ActionResult<ItemsResponse<FriendV3>> GetAll()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<FriendV3> list = _service.GetAllV3();

                if(list.Count == 0)
                {
                    code = 404;
                    response = new ErrorResponse("Records Not Found");
                }
                else
                {
                    response = new ItemsResponse<FriendV3> { Items = list };
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


        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<FriendV3>>> GetPage(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<FriendV3> page = _service.PaginationV3(pageIndex, pageSize);

                if(page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Records Not Found");
                }
                else
                {
                    response = new ItemResponse<Paged<FriendV3>> { Item = page };
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


        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<FriendV3>>> SearchPage(int pageIndex, int pageSize, string query)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<FriendV3> page = _service.Search_PaginationV3(pageIndex, pageSize, query);

                if(page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Records Not Found");
                }
                else
                {
                    response = new ItemResponse<Paged<FriendV3>> { Item = page };
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
