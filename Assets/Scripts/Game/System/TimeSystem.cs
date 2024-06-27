using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    public static PageHUD hud => GameMgr.Inst.hud;

    public float timeSpd; // mins / per real second
    [Range(0.1f, 7f)]
    public float timeScale = 1f;
    public float startHour;
    public int startDays;

    [Header("RUNTIME")]
    public int dayCount;
    public float dayMins;
    public float deltaMins;
    public float totalMins;

    public float dayHours => dayMins / 60f;

    public System.Func<bool> onDayEnd;

    public void Init()
    {
        Reset();
    }

    public void Reset()
    {
        dayMins = startHour * Defs.ONE_HOUR_MINS;
        totalMins = startHour * Defs.ONE_HOUR_MINS;
        dayCount = startDays;
    }

    public void Tick(float dt)
    {
        deltaMins = timeSpd * dt * timeScale;
        dayMins += deltaMins;
        totalMins += deltaMins;

        if (dayMins >= Defs.ONE_DAY_MINS)
        {
            dayCount += 1;
            dayMins -= Defs.ONE_DAY_MINS;

            onDayEnd?.Invoke();
        }
    }

    public float GetDayTimeNOR()
    {
        return dayMins / Defs.ONE_DAY_MINS;
    }
}
