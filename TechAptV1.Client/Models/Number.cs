// Copyright © 2025 Always Active Technologies PTY Ltd

namespace TechAptV1.Client.Models;

public class Number
{
    public int Value { get; set; }
    // Update: Change this to be a boolean, as it is more natural to represent a prime number as a boolean.
    public bool IsPrime { get; set; }
}
