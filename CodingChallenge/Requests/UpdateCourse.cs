using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.CodingChallenge.Requests
{
    public class UpdateCourse : AddCourse
    {
        public int Id { get; set; }
    }
}
