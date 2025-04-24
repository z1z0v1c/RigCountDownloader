namespace RigCountProcessor.Domain.Models;

using CommandLine;

public class Options
{
    public Options()
    {
    }

    public Options(int startYear, int yearCount)
    {
        StartYear = startYear;
        YearCount = yearCount;
    }

    [Option('s', "start-year", Default = 2023, HelpText = "Data processing starting year.")]
    public int StartYear { get; set; }
    
    [Option('y', "year-count", Default = 2, HelpText = "Data processing year count.")]
    public int YearCount { get; set; }
}