using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Models
{
    public class Task
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите описание задачи")]
        public string Description = string.Empty;

        [Required(ErrorMessage = "Введите дату")]
        public DateTime? DueDate { get; set; }

        [Required(ErrorMessage = "Выберите категорию")]
        public string CategoryId = string.Empty;

        [ValidateNever]
        public Category Category { get; set; } = null!;

        [Required(ErrorMessage = "Выберите статус")]
        public string TaskStatusId { get; set; } = string.Empty;

        [ValidateNever]
        public TaskStatus Status { get; set; } = null!;

        public bool Overdue => TaskStatusId == "open" && DueDate == DateTime.Today;
    }
}
