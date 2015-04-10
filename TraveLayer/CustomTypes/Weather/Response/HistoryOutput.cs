﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Weather
{
    public class Features
    {
        public int planner { get; set; }
    }

    public class Response
    {
        public string version { get; set; }
        public string termsofService { get; set; }
        public Features features { get; set; }
    }

    public class Date
    {
        public string epoch { get; set; }
        public string pretty { get; set; }
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public int yday { get; set; }
        public int hour { get; set; }
        public string min { get; set; }
        public int sec { get; set; }
        public string isdst { get; set; }
        public string monthname { get; set; }
        public string monthname_short { get; set; }
        public string weekday_short { get; set; }
        public string weekday { get; set; }
        public string ampm { get; set; }
        public string tz_short { get; set; }
        public string tz_long { get; set; }
    }

    public class DateStart
    {
        public Date date { get; set; }
    }

    public class Date2
    {
        public string epoch { get; set; }
        public string pretty { get; set; }
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public int yday { get; set; }
        public int hour { get; set; }
        public string min { get; set; }
        public int sec { get; set; }
        public string isdst { get; set; }
        public string monthname { get; set; }
        public string monthname_short { get; set; }
        public string weekday_short { get; set; }
        public string weekday { get; set; }
        public string ampm { get; set; }
        public string tz_short { get; set; }
        public string tz_long { get; set; }
    }

    public class DateEnd
    {
        public Date2 date { get; set; }
    }

    public class PeriodOfRecord
    {
        public DateStart date_start { get; set; }
        public DateEnd date_end { get; set; }
    }

    public class Min
    {
        public string F { get; set; }
        public string C { get; set; }
    }

    public class Avg
    {
        public string F { get; set; }
        public string C { get; set; }
    }

    public class Max
    {
        public string F { get; set; }
        public string C { get; set; }
    }

    public class TempHigh
    {
        public Min min { get; set; }
        public Avg avg { get; set; }
        public Max max { get; set; }
    }

    public class TempLow
    {
        public Min min { get; set; }
        public Avg avg { get; set; }
        public Max max { get; set; }
    }

    public class Min3
    {
        public string @in { get; set; }
        public string cm { get; set; }
    }

    public class Avg3
    {
        public string @in { get; set; }
        public string cm { get; set; }
    }

    public class Max3
    {
        public string @in { get; set; }
        public string cm { get; set; }
    }

    public class Precip
    {
        public Min3 min { get; set; }
        public Avg3 avg { get; set; }
        public Max3 max { get; set; }
    }

    public class DewpointHigh
    {
        public Min min { get; set; }
        public Avg avg { get; set; }
        public Max max { get; set; }
    }

    public class DewpointLow
    {
        public Min min { get; set; }
        public Avg avg { get; set; }
        public Max max { get; set; }
    }

    public class CloudCover
    {
        public string cond { get; set; }
    }

    public class WeatherChance
    {
        public string name { get; set; }
        public string description { get; set; }
        public string percentage { get; set; }
    }
    public class ChanceOf
    {
        public WeatherChance tempoversixty { get; set; }
        public WeatherChance chanceofwindyday { get; set; }
        public WeatherChance chanceofpartlycloudyday { get; set; }
        public WeatherChance chanceofsunnycloudyday { get; set; }
        public WeatherChance chanceoffogday { get; set; }
        public WeatherChance chanceofcloudyday { get; set; }
        public WeatherChance chanceofhumidday { get; set; }
        public WeatherChance tempoverninety { get; set; }
        public WeatherChance chanceofsnowonground { get; set; }
        public WeatherChance chanceoftornadoday { get; set; }
        public WeatherChance chanceofprecip { get; set; }
        public WeatherChance chanceofsultryday { get; set; }
        public WeatherChance tempbelowfreezing { get; set; }
        public WeatherChance tempoverfreezing { get; set; }
        public WeatherChance chanceofthunderday { get; set; }
        public WeatherChance chanceofhailday { get; set; }
        public WeatherChance chanceofsnowday { get; set; }
        public WeatherChance chanceofrainday { get; set; }
    }

    public class Trip
    {
        public string title { get; set; }
        public string airport_code { get; set; }
        public string error { get; set; }
        public PeriodOfRecord period_of_record { get; set; }
        public TempHigh temp_high { get; set; }
        public TempLow temp_low { get; set; }
        public Precip precip { get; set; }
        public DewpointHigh dewpoint_high { get; set; }
        public DewpointLow dewpoint_low { get; set; }
        public CloudCover cloud_cover { get; set; }
        public ChanceOf chance_of { get; set; }
    }

    public class HistoryOutput
    {
        public Response response { get; set; }
        public Trip trip { get; set; }
    }
}
