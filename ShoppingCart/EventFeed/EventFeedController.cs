using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.EventFeed;

[ApiController]
[Route("/events")]
public class EventFeedController : ControllerBase
{
    private readonly IEventStore _eventStore;

    public EventFeedController(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    [HttpGet]
    public Event[] Get(
        [FromQuery] long start,
        [FromQuery] long end = long.MaxValue)
    {
        return _eventStore.GetEvents(start, end).ToArray();
    }
}
