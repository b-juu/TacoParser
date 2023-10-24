namespace LoggingKata
{
    public class TacoParser
    {
        readonly ILog logger = new TacoLogger();
        public ITrackable Parse(string line)
        {
            var cells = line.Split(',');
            //logger.LogInfo("Begin parsing");
            if (cells.Length < 3)
            {
                logger.LogWarning("Invalid input line format");
                return null;
            }
            if (double.TryParse(cells[0], out double latitude) == false ||
                double.TryParse(cells[1], out double longitude) == false)
            {
                logger.LogError("Invalid latitude or longitude format. Skipping parsing.");
                return null;
            }
            var name = cells[2];
            return new TacoBell
            {
                Name = name,
                Location = new Point { Latitude = latitude, Longitude = longitude }
            };
        }
    }
}
