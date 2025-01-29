public static class GlobalData
{
    public static long money { get; set; } = 1000; //playermoney
    public static long bet { get; set; } = 100; //betAmount

    public static int DealerWinCond { get; set; } = 21;
    public static int BetLossRate { get; set; } = 100;
    public static int BetPayoutRate { get; set; } = 100;

    public static void ClearAll()
    {
        money = 1000;
        bet = 100;
    }

    public static void ResetAbilityValues()
    {
        DealerWinCond = 21;
        BetLossRate = 100;
        BetPayoutRate = 100;
    }

}