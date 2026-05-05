using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Models.Rating;

namespace Backend.Application.Interfaces;

public interface IRatingService
{
    Task<StudentRatingResponse> GetMyRatingAsync(Guid userId, Guid? subjectId);
}
