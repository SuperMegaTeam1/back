using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Models.Rating
{
    public class TopStudentDto
    {
        public Guid StudentId { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? FatherName { get; set; }

        public double TotalGrade { get; set; }
        public int RatingPosition { get; set; }
    }
}
