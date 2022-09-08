using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.CodingChallenge.Domain;
using Sabio.Models.CodingChallenge.Requests;
using Sabio.Services;
using Sabio.Services.CodingChallenge;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;

namespace Sabio.Web.Api.CodingChallenge
{
    [Route("api/courses")]
    [ApiController]
    public class CourseApiController : BaseApiController
    {
        private ICourseService _service = null;
        private IAuthenticationService<int> _authService = null;

        public CourseApiController(ICourseService service
                , ILogger<CourseApiController> logger
                , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(AddCourse model)
        {
            ObjectResult result = null;

            try
            {
                int id = _service.AddCourse(model);

                ItemResponse<int> response = new ItemResponse<int>() { Item = id };

                result = Created201(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, response);
            }
            return result;

        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(UpdateCourse model) 
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.Update(model);

                response = new SuccessResponse();
            }
            catch(Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Course>> GetById(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Course course = _service.GetCourseById(id);
                
                if( course == null)
                {
                    code = 404;
                    response = new ErrorResponse("Record not found");
                }
                else
                {
                    response = new ItemResponse<Course> { Item = course };
                }
            }
            catch(Exception ex)
            {
                code = 500;
                Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Course>>> GetByPage(int pageIndex, int pageSize) 
        {

            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Course> page = _service.GetCoursesByPage(pageIndex, pageSize);
                pageIndex = 0;
                pageSize = 5;

                if(page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Records Not Found");
                }
                else
                {
                    response = new ItemResponse<Paged<Course>> { Item = page };
                }
            }
            catch(Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                Logger.LogError(ex.ToString()); 
            }
            return StatusCode(code,response);
        }


        [HttpDelete("students/{id:int}")]
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



