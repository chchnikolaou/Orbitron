namespace Orbitron.Models.ViewModels
{
    public class CallClientViewModel
    {

        public Client Client { get; set; }
        public List<Call> Calls { get; set; } = new List<Call>();
        public Package Package { get; set; }

    }
}
