using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.CodingChallenge.Requests
{
    public class AddCourse
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int SeasonTermId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TeacherId { get; set; }
    }
}
