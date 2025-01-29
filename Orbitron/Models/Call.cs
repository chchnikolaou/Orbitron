namespace Orbitron.Models
{
    public class Call
    {
        public required int Id { get; set; }
        public required string PhoneSender { get; set; }
        public required string PhoneReceiver { get; set; }
        public required DateTime Started { get; set; }
        public required DateTime Ended { get; set; }


        public string ParseDuration()
        {
            TimeSpan timespan = Ended.Subtract(Started);
            return timespan.TotalHours + " ώρες, " + timespan.TotalMinutes + " λεπτά και " + timespan.TotalSeconds + " δευτερόλεπτα";
        }

    }
}
