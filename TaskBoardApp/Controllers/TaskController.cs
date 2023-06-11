using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TaskBoardApp.Data;
using TaskBoardApp.Models.Task;

using Task = TaskBoardApp.Data.Models.Task;

namespace TaskBoardApp.Controllers
{
    public class TaskController : Controller
    {
        // Normally this should be in Service layer - the Controller should not have access to dbContext at all !!! 
        private readonly TaskBoardAppDbContext dbContext;

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        public TaskController(TaskBoardAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        private IEnumerable<TaskBoardModel> GetBoards()
            => dbContext
                .Boards
                .Select(b => new TaskBoardModel()
                {
                    Id = b.Id,
                    Name = b.Name
                });

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IActionResult> Create()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            TaskFormModel model = new TaskFormModel()
            {
                Boards = GetBoards()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskFormModel taskModel)
        {
            if (!GetBoards().Any(b => b.Id == taskModel.BoardId))
            {
                ModelState.AddModelError(nameof(taskModel.BoardId), "Board does not exist.");
            }

            string currentUserId = GetUserId();
            if (!ModelState.IsValid)
            {
                taskModel.Boards = GetBoards();
                return View(taskModel);
            }

            Task task = new Task()
            {
                Title = taskModel.Title,
                Description = taskModel.Description,
                CreatedOn = DateTime.Now,
                BoardId = taskModel.BoardId,
                OwnerId = currentUserId
            };
            await dbContext.Tasks.AddAsync(task);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("All", "Board");
        }

        public async Task<IActionResult> Details(int id)
        {
            var task = await dbContext
                .Tasks
                .Where(t => t.Id == id)
                .Select(t => new TaskDetailsViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    CreatedOn = t.CreatedOn.ToString("dd/MM/yyyy HH:mm"),
                    Board = t.Board.Name,
                    Owner = t.Owner.UserName
                }).FirstOrDefaultAsync();

            if (task == null)
            {
                return BadRequest();
            }
            return View(task);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var task = await dbContext
                .Tasks
                .FindAsync(id);

            if (task == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();
            if (currentUserId != task.OwnerId)
            {
                return Unauthorized();

            }
            TaskFormModel taskModel = new TaskFormModel()
            {
                Title = task.Title,
                Description = task.Description,
                BoardId = task.BoardId,
                Boards = GetBoards()
            };
            return View(taskModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TaskFormModel taskForm)
        {
            var task = await dbContext
                .Tasks
                .FindAsync(id);

            if (task == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (userId != task.OwnerId)
            {
                return Unauthorized();
            }

            if (!GetBoards().Any(b => b.Id == taskForm.BoardId))
            {
                ModelState.AddModelError(nameof(taskForm.BoardId), "Board does not exist.");
            }

            if (!ModelState.IsValid)
            {
                taskForm.Boards = GetBoards();
                return View(taskForm);
            }

            task.Title = taskForm.Title;
            task.Description = taskForm.Description;
            task.BoardId = taskForm.BoardId;
            await dbContext.SaveChangesAsync();
            return RedirectToAction("All", "Board");
        }


        public async Task<IActionResult> Delete(int id)
        {
            var task = await dbContext.Tasks.FindAsync(id);

            if (task == null)
            {
                return BadRequest();
            }
            string userId = GetUserId();
            if (userId != task.OwnerId)
            {
                return Unauthorized();
            }

            TaskViewModel taskViewModel = new TaskViewModel()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
            };
            return View(taskViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(TaskViewModel taskModel)
        {
            var task = await dbContext.Tasks.FindAsync(taskModel.Id);

            if (task == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();
            if (userId != task.OwnerId)
            {
                return Unauthorized();
            }

            dbContext.Remove(task);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("All", "Board");
        }
    }

}
