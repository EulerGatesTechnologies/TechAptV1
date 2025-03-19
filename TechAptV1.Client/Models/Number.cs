// Copyright © 2025 Always Active Technologies PTY Ltd

namespace TechAptV1.Client.Models;

public class Number
{
    public int Value { get; set; }

    public bool IsPrime { get; set; } // Defaults to false, set to true if the number is prime. More natural to name it as bool rather than int.
}
