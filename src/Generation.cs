using System.Collections.Generic;
namespace GreenVsRed
{
    public class Generation
    {
        public Generation()
        {
            Points = new List<GridPoint>();
        }

        public List<GridPoint> Points { get; set; }
    }
}