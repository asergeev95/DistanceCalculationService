namespace DCS.Core
{
    public class Result<T> where T: class
    {
        public T Value { get; set; }
        public bool IsSuccess { get; set; }
        public string FaultMessage { get; set; }
    }
}