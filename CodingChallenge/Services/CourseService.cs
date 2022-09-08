using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.CodingChallenge.Domain;
using Sabio.Models.CodingChallenge.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services.CodingChallenge
{
    public class CourseService : ICourseService
    {
        IDataProvider _data = null;

        public CourseService(IDataProvider data)
        {
            _data = data;
        }

        public int AddCourse(AddCourse model)
        {
            int id = 0;

            _data.ExecuteNonQuery("[dbo].[Courses_Insert]", inputParamMapper: delegate (SqlParameterCollection collection)
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

        public Course GetCourseById(int id)
        {
            Course course = null;

            _data.ExecuteCmd("dbo.Courses_SelectById", delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            },
            delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                course = MapSingleCourse(reader, ref startingIndex);
            });
            return course;
        }

        public void Update(UpdateCourse model) 
        {
            _data.ExecuteNonQuery("dbo.Courses_Update", inputParamMapper: delegate (SqlParameterCollection collection)
            {
                AddCommonParams(model, collection);

                collection.AddWithValue("@Id", model.Id);
            },
            returnParameters: null);
        
        
        }

        public void Delete(int id) 
        {
            _data.ExecuteNonQuery("dbo.Students_Delete", inputParamMapper: delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@Id", id);
            },
            returnParameters: null);
        
        }

        public Paged<Course> GetCoursesByPage(int pageIndex, int pageSize) 
        {
            Paged<Course> pagedResult = null;
            List<Course> result = null;
            int totalCount = 0;

            _data.ExecuteCmd("dbo.Courses_Pagination", inputParamMapper: delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@PageIndex", pageIndex);
                collection.AddWithValue("@PageSize", pageSize);
            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                Course model = MapSingleCourse(reader, ref index);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(index++);
                }
                if (result == null)
                {
                    result = new List<Course>();
                }
                result.Add(model);
            });
            if(result != null)
            {
                pagedResult = new Paged<Course>(result, pageIndex, pageSize, totalCount);
            }
            return pagedResult;
        }

        private static Course MapSingleCourse(IDataReader reader, ref int startingIndex)
        {
            Course aCourse = new Course();


            aCourse.Id = reader.GetSafeInt32(startingIndex++);
            aCourse.Name = reader.GetSafeString(startingIndex++);
            aCourse.Description = reader.GetSafeString(startingIndex++);
            aCourse.SeasonTerm = reader.GetSafeString(startingIndex++);
            aCourse.Teacher = reader.GetSafeString(startingIndex++);
            
            string studentsAsString = reader.GetString(startingIndex++);
            if (!string.IsNullOrEmpty(studentsAsString)) 
            {
                aCourse.Students = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Student>>(studentsAsString);
            }
            
            

            return aCourse;
        }

        private static void AddCommonParams(AddCourse model, SqlParameterCollection collection)
        {
            collection.AddWithValue("@Name", model.Name);
            collection.AddWithValue("@Description", model.Description);
            collection.AddWithValue("@SeasonTermId", model.SeasonTermId);
            collection.AddWithValue("@TeacherId", model.TeacherId);
        }
    }
}
