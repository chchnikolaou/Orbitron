namespace Orbitron.Models
{
    public class Phone
    {

        public required string Number { get; set; }
        public required string Package { get; set; }
        public required DateTime issuedDate { get; set; }
        public required DateTime renewDate { get; set; }
           
    }
}
