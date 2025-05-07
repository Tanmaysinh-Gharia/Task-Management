using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Data.Entities
{
    public class TaskDetail
    {
        [Key]
        public int Id { get; set; }

        public int TaskId { get; set; }

        [ForeignKey(nameof(TaskId))]
        public TaskEntity Task { get; set; }

        public int UpdatedById { get; set; }

        [ForeignKey(nameof(UpdatedById))]
        public User UpdatedBy { get; set; }

        [Required]
        public string FieldName { get; set; }

        public string? OldValue { get; set; }

        public string? NewValue { get; set; }

        public DateTime ChangeTime { get; set; } = DateTime.Now;
    }

}
