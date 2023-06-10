using System.ComponentModel.DataAnnotations;

namespace TaskBoardApp.Data.Models
{
    public class Board
    {
        public Board()
        {
            this.Tasks = new HashSet<Task>();
        }
      
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(DataConstants.Board.MaxName)]
        public string Name { get; set; } = null!;

        public ICollection<Task> Tasks { get; set; }
    }
}
