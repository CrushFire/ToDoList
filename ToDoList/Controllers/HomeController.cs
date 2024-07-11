using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private DataBaseContext context;

        public HomeController(DataBaseContext ctx) => context = ctx;


        public IActionResult Index(string id)
        {
            var filters = new Filters(id);
            ViewBag.Filters = filters;

            ViewBag.Categories = context.Categories.ToList();
            ViewBag.TaskStatuses = context.TaskStatuses.ToList();
            ViewBag.DueFilters = Filters.DueFilterValues;

            IQueryable<Models.Task> query = context.Tasks
                .Include(t => t.Category)
                .Include(t => t.Status);
                
            if (filters.HasCategory)
            {
                query = query.Where(t => t.CategoryId == filters.CategoryId);
            }

            if (filters.HasTaskStatus)
            {
                query = query.Where(t => t.TaskStatusId == filters.TaskStatusId);
            }

            if (filters.HasDue)
            {
                var today = DateTime.Today;
                if (filters.IsPast)
                {
                    query = query.Where(t => t.DueDate < today);
                }
                else if (filters.IsFuture)
                {
                    query = query.Where(t => t.DueDate > today);
                }
                else if (filters.IsToday)
                {
                    query = query.Where(t => t.DueDate == today);
                }
            }
            var tasks = query.OrderBy(t => t.DueDate).ToList();

            return View(tasks);
        }

        [HttpGet]

        public IActionResult Add()
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.TaskStatuses = context.TaskStatuses.ToList();
            var task = new Models.Task { TaskStatusId = "open" };
            return View(task);
        }

        [HttpPost]

        public IActionResult Add(Models.Task task)
        {
            if (ModelState.IsValid)
            {
                context.Tasks.Add(task);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Categories = context.Categories.ToList();
                ViewBag.TaskStatuses = context.TaskStatuses.ToList();
                return View(task);
            }
        }

        [HttpPost]
        public IActionResult Filter(string[] filter)
        {
            string id = string.Join('-', filter);
            return RedirectToAction("Index", new { ID = id });
        }

        [HttpPost]
        public IActionResult MarkComplete([FromRoute] string id, Models.Task selected)
        {
            selected = context.Tasks.Find(selected.Id)!;

            if (selected != null)
            {
                selected.TaskStatusId = "close";
                context.SaveChanges();
            }
            return RedirectToAction("Index", new { ID = id });
        }

        [HttpPost]

        public IActionResult DeleteComplete(string id)
        {
            var toDelete = context.Tasks.Where(t => t.TaskStatusId == "close").ToList();

            foreach (var task in toDelete)
            {
                context.Tasks.Remove(task);
            }
            context.SaveChanges();

            return RedirectToAction("Index", new { ID = id });
        }

    }
}