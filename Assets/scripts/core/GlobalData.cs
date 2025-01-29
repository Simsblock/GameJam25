public static class GlobalData
{
    public static int money { get; set; } = 1000; //playermoney
    public static int bet { get; set; } = 100; //betAmount

    public static int Score { get; set; } = 0;

    //AbilityValues
    public static int DealerWinCond { get; set; } = 21;
    public static int PlayerWinCond { get; set; } = 21;
    public static int BetLossRate { get; set; } = 100;
    public static int BetPayoutRate { get; set; } = 100;
    public static int DuplicateAmt { get; set; } = 0;

    public static void ClearAll()
    {
        money = 1000;
        bet = 100;
        Score = 0;
    }

    public static void ResetAbilityValues()
    {
        DealerWinCond = 21;
        PlayerWinCond = 21;
        BetLossRate = 100;
        BetPayoutRate = 100;
        DuplicateAmt = 0;
    }

}