namespace ViewModels.PostingModels;

public class TicketTemplate
{
    public decimal Price { get; set; }

    /// <summary>
    /// Describes detailed ticket info.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Count of tickets with this template
    /// </summary>
    public int Count { get; set; }
}