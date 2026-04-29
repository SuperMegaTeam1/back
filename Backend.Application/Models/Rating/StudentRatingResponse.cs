using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Models.Rating
{
    public sealed record StudentRatingResponse
    {
        public Guid GroupId { get; set; }
        public string GroupName { get; set; } = null!;

        public Guid? SubjectId { get; set; }
        public string? SubjectName { get; set; }

        public int RatingPosition { get; set; }
        public double TotalGrade { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<TopStudentDto> TopStudents { get; set; } = new();
    }
}
