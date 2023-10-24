using System;
using System.IO;
using System.Linq;
using GeoCoordinatePortable;
namespace LoggingKata
{
    class Program
    {
        static readonly ILog logger = new TacoLogger();
        const string csvPath = "TacoBell-US-AL.csv";
        static void Main(string[] args)
        {
            logger.LogInfo("Log initialized");
            var lines = File.ReadAllLines(csvPath);
            if (lines.Length == 0)
            {
                logger.LogError("No data found in the CSV file.");
                return;
            }
            else if (lines.Length == 1)
            {
                logger.LogWarning("Only one line of data found in the CSV file.");
                return;
            }
            var parser = new TacoParser();
            var locations = lines.Select(parser.Parse).ToArray();
            var maxDistanceQuery = from locA in locations
                                   from locB in locations
                                   where locA != locB
                                   let corA = new GeoCoordinate(locA.Location.Latitude, locA.Location.Longitude)
                                   let corB = new GeoCoordinate(locB.Location.Latitude, locB.Location.Longitude)
                                   let distance = corA.GetDistanceTo(corB) / 1609.34 
                                   orderby distance descending
                                   select new
                                   {
                                       FirstTacoBell = locA,
                                       SecondTacoBell = locB,
                                       Distance = distance
                                   };
            var result = maxDistanceQuery.FirstOrDefault();
            if (result != null)
            {
                var firstTacoBell = result.FirstTacoBell;
                var secondTacoBell = result.SecondTacoBell;
                var maxDistance = result.Distance;
                logger.LogInfo($"The two Taco Bells furthest from each other are {firstTacoBell.Name} and {secondTacoBell.Name} with a distance of {maxDistance:F2} miles.");
            }
        }
    }
}

