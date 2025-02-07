

namespace SSO_Backend.Result;

public class ValidationError
{
    public string? Key { get; set; }
    public string? ErrorMessage { get; set; }
    public ValidationSeverity Severity { get; set; } = ValidationSeverity.Error;
}

public enum ResultStatus
{
    Ok,
    Error,
    Forbidden,
    Unauthorized,
    Invalid,
    NotFound,
    Other
}


public enum ValidationSeverity
{
    Error = 0,
    Warning = 1,
    Info = 2
}