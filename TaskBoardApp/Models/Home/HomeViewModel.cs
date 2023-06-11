namespace TaskBoardApp.Models.Home
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            this.BoardsWithTasksCount = new List<HomeBoardModel>();
        }
        public int AllTasksCount { get; init; }
        public List<HomeBoardModel> BoardsWithTasksCount { get; init; }
        public int UserTasksCount { get; init; }
    }
}
