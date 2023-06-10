namespace TaskBoardApp.Data
{
    public static class DataConstants
    {
        public static class Task
        {
            public const int MaxTitle = 70;
            public const int MinTitle = 5;

            public const int MaxDescription = 1000;
            public const int MinDescription = 10;

        }

        public static class Board
        {
            public const int MaxName = 30;
            public const int MinName = 3;
        }
    }
}
