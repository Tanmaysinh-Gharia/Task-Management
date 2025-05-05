using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Enums;
using TaskStatus = TaskManagement.Core.Enums.TaskStatus;

namespace TaskManagement.Core.ViewModels.TaskManagement
{
    public class TaskFilterModel
    {
        public string? SearchTerm { get; set; } = null;
        public TaskStatus? Status { get; set; } = null;
        public TaskPriority? Priority { get; set; } = null;
        public DateTime? DueBefore { get; set; } = null;
        public string? SortColumn { get; set; } = null;
        public string? SortOrder { get; set; } = "asc"; // or "desc"
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
