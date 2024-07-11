using Microsoft.EntityFrameworkCore;

namespace ToDoList.Models
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { }
        public DbSet<Models.Task> Tasks { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<TaskStatus> TaskStatuses { get; set;  } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = "work", Name = "Работа" },
                new Category { CategoryId = "study", Name = "Обучение" },
                new Category { CategoryId = "home", Name = "Дом" },
                new Category { CategoryId = "shop", Name = "Покупки" },
                new Category { CategoryId = "family", Name = "Семья" }
            );

            modelBuilder.Entity<TaskStatus>().HasData(
                new TaskStatus { TaskStatusId = "open", Name = "Не завершено" },
                new TaskStatus { TaskStatusId = "close", Name = "Завершено" }
            );


        }
    }
}
