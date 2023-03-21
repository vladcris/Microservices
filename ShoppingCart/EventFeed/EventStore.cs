using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.EventFeed;

public interface IEventStore
{
    IEnumerable<Event> GetEvents(long firstSequenceNumber, long lastSequenceNumber);
    void Raise(string eventName, object Content);
}

public class EventStore : IEventStore
{
    private static List<Event> events = new List<Event>
    {
        new Event(1, DateTimeOffset.UtcNow, "ShoppingCartItemAdded", new{ UserId = "123", ProductId = 11}),
        new Event(2, DateTimeOffset.UtcNow, "ShoppingCartItemAdded", new{ UserId = "123", ProductId = 22}),
        new Event(3, DateTimeOffset.UtcNow, "ShoppingCartItemRemoved", new{ UserId = "123", ProductId = 11})

    };
    public IEnumerable<Event> GetEvents(long firstSequenceNumber, long lastSequenceNumber)
    {
        var filteredEvents = events.Where(e => e.SequenceNumber >= firstSequenceNumber && e.SequenceNumber <= lastSequenceNumber)
            .OrderBy(e => e.OccuredAt);
        
        return filteredEvents;
    }

    public void Raise(string eventName, object Content)
    {
        System.Console.WriteLine($"Event: {eventName} was fired at {DateTimeOffset.UtcNow}; Content: {Content.ToString()}");
    }
}
