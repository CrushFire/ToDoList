namespace ToDoList.Models
{
    public class Filters
    {

        public Filters(string filterstring)
        {
            FilterString = filterstring ?? "all-all-all";
            string[] filters = FilterString.Split("-");
            CategoryId = filters[0];
            Due = filters[1];
            TaskStatusId = filters[2];
        }

        public string FilterString { get; }
        public string CategoryId { get; }
        public string Due { get; }

        public string TaskStatusId { get; }

        public bool HasCategory => CategoryId.ToLower() != "all";
        public bool HasDue => Due.ToLower() != "all";
        public bool HasTaskStatus => TaskStatusId.ToLower() != "all";

        public static Dictionary<string, string> DueFilterValues =>
            new Dictionary<string, string>
            {
                {"future", "Не наступило" },
                {"past", "Прошло" },
                {"today", "Сегодня" }
            };

        public bool IsPast => Due.ToLower() == "past";
        public bool IsFuture => Due.ToLower() == "future";
        public bool IsToday => Due.ToLower() == "today";
    }
}
