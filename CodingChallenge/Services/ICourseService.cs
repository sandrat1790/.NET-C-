using Sabio.Models;
using Sabio.Models.CodingChallenge.Domain;
using Sabio.Models.CodingChallenge.Requests;
using System.Collections.Generic;


namespace Sabio.Services.CodingChallenge
{
    public interface ICourseService
    {
        int AddCourse(AddCourse model);
        Course GetCourseById(int id);
        void Update(UpdateCourse model);
        void Delete(int id);
        Paged<Course> GetCoursesByPage(int pageIndex, int pageSize);
    }
}