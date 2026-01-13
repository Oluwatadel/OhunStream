

/// <summary>Thrown when a value‐object or domain type fails validation.</summary>
public class DomainValidationException(string message) : DomainException(message);