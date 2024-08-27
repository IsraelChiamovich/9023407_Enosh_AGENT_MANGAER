namespace TargetsMvc.ViewModels
{
    public class GridVM
    {
        public int Size { get; set; }
        public string[,] Grid { get; set; }

        public GridVM(int size)
        {
            Size = size;
            Grid = new string[size, size];
        }
    }
}
