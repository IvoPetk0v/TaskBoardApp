using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using TaskBoardApp.Data;
using TaskBoardApp.Models.Home;

namespace TaskBoardApp.Controllers
{
    public class HomeController : Controller
    {
        // Normally this should be in Service layer - the Controller should not have access to dbContext at all !!! 
        private readonly TaskBoardAppDbContext dbContext;

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        public HomeController(TaskBoardAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
#pragma warning disable CS1998
        public async Task<IActionResult> Index()
#pragma warning restore CS1998
        {
            var taskBoards = dbContext.Boards
                .Select(b => b.Name)
                .Distinct();
            var tasksCounts = new List<HomeBoardModel>();
            foreach (var boardName in taskBoards)
            {
                var taskInBoard = dbContext.Tasks.Where(t => t.Board!.Name == boardName).Count();

                tasksCounts.Add(new HomeBoardModel()
                {
                    BoardName = boardName,
                    TasksCount = taskInBoard
                });
            }

            var userTasksCount = -1;
            if (User.Identity!.IsAuthenticated)
            {
                var currentUserId = GetUserId();
                userTasksCount = dbContext.Tasks.Where(t => t.OwnerId == currentUserId).Count();
            }

            var homeModel = new HomeViewModel()
            {
                AllTasksCount = dbContext.Tasks.Count(),
                BoardsWithTasksCount = tasksCounts,
                UserTasksCount = userTasksCount
            };
            return View(homeModel);
        }

    }
}