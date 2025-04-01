using System;

namespace TechAptV1.Client.Services;

public class ThreadingServiceOptions
{
    public int MaxEntries { get; set; }
    public int EvenThreadTriggerThreshold { get; set; }
}

