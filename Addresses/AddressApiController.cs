using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Addresses;
using Sabio.Models.Requests.Addresses;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/addresses")]
    [ApiController]
    public class AddressApiController : BaseApiController
    {
        private IAddressService _service = null;
        private IAuthenticationService<int> _authService = null;

        public AddressApiController(IAddressService service
            , ILogger<AddressApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        // GET      api/addresses/ (endpoint)
        [HttpGet]
        public ActionResult<ItemsResponse<Address>> GetAll()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<Address> list = _service.GetRandomAddresses();

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Address not found");

                }
                else
                {
                    response = new ItemsResponse<Address> { Items = list };

                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }
            
     //----------------------------------------------------------------------------------------------------------

        // GET      api/addresses/{id}
        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Address>> Get(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Address address = _service.Get(id);

                if (address == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Address not found");
                }
                else
                {
                    response = new ItemResponse<Address> { Item = address };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }
            return StatusCode(iCode, response);
        }

     //----------------------------------------------------------------------------------------------------------

        // POST     api/addresses
        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(AddressAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                // id of the new address
                int id = _service.Add(model, userId);

                ItemResponse<int> response = new ItemResponse<int>() { Item = id};

                result = Created201(response);
            }

            catch (Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, response);
            }

            return result;
        }

     //----------------------------------------------------------------------------------------------------------

        // PUT      api/addresses/{id}
        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(AddressUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                _service.Update(model, userId);

                response = new SuccessResponse();
            }

            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

     //----------------------------------------------------------------------------------------------------------

        // DELETE   api/addresses/delete/{id}
        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.Delete(id);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            
            return StatusCode(code, response);
        }

    }
}
