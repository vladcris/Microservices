using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.EventFeed;

public record Event(long SequenceNumber,
    DateTimeOffset OccuredAt,
    string Name,
    object Content);

