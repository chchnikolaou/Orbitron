namespace Orbitron.Models
{
    public class Package
    {

        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required int TotalMinutes { get; set; }
        public required int TotalMinutesAbroad { get; set; }
        public required int SMS { get; set; }
        public required int MB { get; set; }
        public required double Cost { get; set; }



    }
}
