using Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Entities;

public sealed class StudentRating : IEntityWithId, IEntityWithChangeInfo
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; } 
    public Student Student { get; set; } = null!;

    public Guid? SubjectId { get; set; }
    public SubjectEntity? Subject { get; set; }

    public double TotalGrade { get; set; }

    public int RatingPosition { get; set; }

    public Guid GroupId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
