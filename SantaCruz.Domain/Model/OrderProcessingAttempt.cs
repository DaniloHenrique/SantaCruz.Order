using SantaCruz.Domain.Contract;

namespace SantaCruz.Domain.Model
{
    public class OrderProcessingAttempt : IEntity<int>
    {
        public int Id { get; set; }
        public Order Order { get; set; } = new Order();
        public int AttemptNumber { get; set;} = 0;
        public DateTime StartedAt { get; set; } = DateTime.Now;
        public DateTime? FinishedAt { get; set; } = null;
        public bool Success { get; set; } = false;
        public string? ErrorMessage { get; set; } = "";
    }
}
