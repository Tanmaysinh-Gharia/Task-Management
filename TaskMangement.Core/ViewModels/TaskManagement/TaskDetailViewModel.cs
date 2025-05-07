using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Core.ViewModels.TaskManagement
{
    public class TaskDetailViewModel
    {
        public string FieldName { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime ChangeTime { get; set; }
    }
}
