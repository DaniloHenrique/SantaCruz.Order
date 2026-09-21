namespace SantaCruz.Application.DI
{
    public class ApiConfig
    {
        public string ConnectionStrings { get; set; } = "";
        public bool ServiceOk { get; init; }
        public int MaxAttempts { get; init; }
    }
}
