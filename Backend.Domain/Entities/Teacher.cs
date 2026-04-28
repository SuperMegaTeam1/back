using Backend.Domain.Interfaces;

namespace Backend.Domain.Entities
{
    public class Teacher : IEntityWithId, IEntityWithChangeInfo
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid ParentUserId { get; set; }

        public string FirstName { get; set; } = null;
        public string LastName { get; set; } = null;

        public string? FatherName { get; set; } = null;
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    }
}
