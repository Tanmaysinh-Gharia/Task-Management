using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using TaskManagement.Core.Enums;
using TaskStatus = TaskManagement.Core.Enums.TaskStatus;
namespace TaskManagement.Data.Entities
{
    public class TaskEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public TaskStatus Status { get; set; }

        [Required]
        public TaskPriority Priority { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; } = false;

        public int CreatorId { get; set; }
        public User Creator { get; set; }

        public int AssigneeId { get; set; }
        public User Assignee { get; set; }

        public ICollection<TaskDetail> TaskDetails { get; set; }
    }

}
