namespace SE.Application.Common.Response;

public enum ErrorType
{
    Validation,     // bad input
    NotFound,       // item doesn't exist
    Unauthorized,   // not logged in / wrong password
    Forbidden,      // logged in but not allowed
    Conflict,       // duplicate, already exists
    Failure         // anything else
}