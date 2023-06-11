using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static TaskBoardApp.Data.DataConstants.Task;

namespace TaskBoardApp.Models.Task
{
    public class TaskFormModel
    {
        [Required]
        [StringLength(MaxTitle, MinimumLength = MinTitle,
            ErrorMessage = "Title should be at least {2} characters long.")]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(MaxDescription, MinimumLength = MinDescription, ErrorMessage = "Description should be at least {2} characters long.")]
        public string Description { get; set; } = null!;

        [Display(Name = "Board")]
        public int BoardId { get; set; }

        [ForeignKey(nameof(BoardId))]
        public IEnumerable<TaskBoardModel> Boards { get; set; } = new HashSet<TaskBoardModel>();

    }
}
