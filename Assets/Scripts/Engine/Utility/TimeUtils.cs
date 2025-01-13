using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Utils
{
    public static DateTime TimeStampStartTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
    public static double GetTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - TimeStampStartTime;
        return ts.TotalSeconds;
    }
}
