namespace NsgLogViewer.Models
{
    public record Nsg
    {
        public Guid SubscriptionId { get; set; }
        public string ResourceGroupName { get; set; } = "";
        public string Name { get; set; } = "";
    }
}