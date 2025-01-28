public static class GlobalData
{
    public static long money { get; set; } = 1000; //playermoney
    public static long bet { get; set; } = 100; //betAmount

    public static void ClearAll()
    {
        money = 0;
        bet = 0;
    }

}