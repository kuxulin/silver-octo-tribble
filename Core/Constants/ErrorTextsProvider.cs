using Core.Entities;

namespace Core.Constants;
public static class ErrorTextsProvider
{
    public static string ThereIsNoElementInDatabase(string element)
    {
        return $"There is no such {element} in database.";
    }

    public static string PasswordsAreNotEqual()
    {
        return "Passwords are not equal";
    }

    public static string ThereisAlreadySuchEntityInDatabase(string element)
    {
        return $"There is already such {element} in database.";
    }

    public static string InvalidElement(string element)
    {
        return $"Invalid {element}";
    }
}
