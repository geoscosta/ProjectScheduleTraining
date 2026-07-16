namespace ProjectScheduleTraining.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public bool IsDeleted { get; private set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        protected void SetUpdated() => UpdatedAt = DateTime.UtcNow;

        public void SoftDelete()
        {
            IsDeleted = true;
            SetUpdated();
        }
    }
}
