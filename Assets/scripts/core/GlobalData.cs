using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public static class GlobalData
{
    private static int baseMoney = 1000;
    private static int baseBet = 100;
    private static int baseScore = 0;
    
    //AbilityValues
    public static int DealerWinCond { get; set; } = 21;
    public static int PlayerWinCond { get; set; } = 21;
    public static int BetLossRate { get; set; } = 100;
    public static int BetPayoutRate { get; set; } = 100;
    public static int DuplicateAmt { get; set; } = 0;

    public static void ClearAll()
    {
        PlayerPrefs.SetInt("Money", baseMoney);
        PlayerPrefs.SetInt("Bet", baseBet);
        PlayerPrefs.SetInt("Score", baseScore);
        PlayerPrefs.SetString("SPCs", "");
    }

    public static bool LoadableCheck() 
    {
        return !((PlayerPrefs.GetInt("Money") == baseMoney) && (PlayerPrefs.GetInt("Bet") == baseBet) && (PlayerPrefs.GetInt("Score") == baseScore));
    }
    public static bool FirstLoad()
    {
        return !((PlayerPrefs.GetInt("Money") == 0) && (PlayerPrefs.GetInt("Bet") == 0) && (PlayerPrefs.GetInt("Score") == 0));
    }

    public static void SaveSPC(List<string> list)
    {
        string[] SPCs = list.ToArray();
        PlayerPrefs.SetString("SPCs", string.Join(",", SPCs));
        PlayerPrefs.Save();
    }
    public static List<string> LoadSPC()
    {
        string savedData = PlayerPrefs.GetString("SPCs", "");
        string[] loadedArray = savedData.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
        return loadedArray.ToList();
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