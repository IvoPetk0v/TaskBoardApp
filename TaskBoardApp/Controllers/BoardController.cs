using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TaskBoardApp.Data;
using TaskBoardApp.Models.Board;
using TaskBoardApp.Models.Task;

namespace TaskBoardApp.Controllers
{
    [Authorize]
    public class BoardController : Controller
    {
        private readonly TaskBoardAppDbContext dbContext;

        public BoardController(TaskBoardAppDbContext dbContext)
        {
                this.dbContext=dbContext;
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<BoardViewModel> allBoards = await this.dbContext
                .Boards
                .Select(b => new BoardViewModel()
                {
                    Name = b.Name,
                    Tasks = b.Tasks
                        .Select(t => new TaskViewModel()
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Description = t.Description,
                            Owner = t.Owner.UserName
                        })
                        .ToArray()
                }).ToArrayAsync();

            return View(allBoards);
        }
    }
}
