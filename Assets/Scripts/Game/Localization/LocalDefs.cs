using UnityEngine;

public static class LangAbbr
{
    public const string ZHCN = "zhcn";
    public const string EN = "en";

    public static string GetAbbr(Lang lang)
    {
        if (lang == Lang.ZHCN) return ZHCN;
        else if (lang == Lang.EN) return EN;
        Debug.LogError($"cant get lang abbr > {lang}");
        return EN;
    }
}

public enum Lang
{
    ZHCN = 0,
    EN = 1,
}

// public enum LocalType
// {
//     UI = 0,
//     TOWER = 1,
// }