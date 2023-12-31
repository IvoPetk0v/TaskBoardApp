﻿using TaskBoardApp.Models.Task;

namespace TaskBoardApp.Models.Board
{
    public class BoardViewModel
    {
        public BoardViewModel()
        {
            this.Tasks = new HashSet<TaskViewModel>();
        }
 
        public string Name { get; set; } = null!;

        public ICollection<TaskViewModel>Tasks { get; set; }
    }
}
