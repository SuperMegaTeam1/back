using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Models.Journal
{
    public class UpdateGradeRequest
    {
        public bool? Attended { get; set; }
        public int? Grade { get; set; }
    }
}
