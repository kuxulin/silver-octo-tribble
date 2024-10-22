using Core.ResultPattern;

namespace Core.Constants;

public static class DefinedError
{
    public static readonly Error DuplicateEntity = new Error(400, "There is already such entity in the database.");
    public static readonly Error AbsentElement = new Error(404, "There is no such entity in the database.");
    public static readonly Error InvalidElement = new Error(400, "Provided entity is invalid.");
    public static readonly Error NonEqualPasswords = new Error(400, "Passwords are not equal.");
    public static readonly Error ForbiddenAction = new Error(403, "Action was forbidden.");
}
