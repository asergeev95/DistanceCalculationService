using JetBrains.Annotations;

namespace DCS.ApplicationService.Contracts
{
    [PublicAPI]
    public class AirportsDistanceState
    {
        public string FaultMessage { get; set; }
        public bool IsSuccess { get; set; }
        public double Distance { get; set; }
    }
}