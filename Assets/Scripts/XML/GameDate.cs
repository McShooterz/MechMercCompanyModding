using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameDate
{
    public int Day = 1;
    public int Month = 1;
    public int Year = 1;

    public string Display { get => System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.MonthNames[Month - 1] + " " + StaticHelper.GetOrdinal(Day) + " " + Year.ToString(); }

    public GameDate()
    {

    }

    public GameDate(GameDate gameDate)
    {
        if (gameDate != null)
        {
            Day = gameDate.Day;
            Month = gameDate.Month;
            Year = gameDate.Year;

            ValidateDate();
        }
    }

    public GameDate(int day, int month, int year)
    {
        Day = day;
        Month = month;
        Year = year;

        ValidateDate();
    }

    public void AddDays(int days)
    {
        Day += days;

        while (Day > System.DateTime.DaysInMonth(Year, Month))
        {
            Day -= System.DateTime.DaysInMonth(Year, Month);
            Month++;

            if (Month > 12)
            {
                Year++;
                Month = 1;
            }
        }
    }

    public bool GreaterThan(GameDate gameDate)
    {
        if (gameDate.Year < Year)
            return false;

        if (gameDate.Year == Year)
        {
            if (gameDate.Month < Month || gameDate.Month == Month && gameDate.Day <= Day)
            {
                return false;
            }
        }

        return true;
    }

    public int WeeksGreater(GameDate targetGameDate)
    {
        GameDate activeGameDate = new GameDate(this);
        int weeks = 0;

        while (activeGameDate.GreaterThan(targetGameDate))
        {
            weeks++;
            activeGameDate.AddDays(7);
        }

        return weeks;
    }

    void ValidateDate()
    {
        Day = Mathf.Max(Day, 1);
        Month = Mathf.Max(Month, 1);

        while (Month > 12)
        {
            Year++;
            Month -= 12;
        }

        while (Day > System.DateTime.DaysInMonth(Year, Month))
        {
            Day -= System.DateTime.DaysInMonth(Year, Month);
            Month++;

            if (Month > 12)
            {
                Year++;
                Month = 1;
            }
        }
    }
}
