namespace TargetsRest.Utils
{
    public static class PossitionUtils
    {
        public static double CalculateDistance(int xA, int yA, int xT, int yT)
        {
            return Math.Sqrt(Math.Pow(xT - xA, 2) + Math.Pow(yT - yA, 2));
        }
    }
}
