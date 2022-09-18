namespace Server.Data;

public static class ServerConstants
{
    public static class ServerResponses
    {

        public static readonly ServerResponse ServiceNotAvailable = new()
        {
            StatusCode = 503,
            Description = "Service not available"
        };

    }

    public static class ExceptionTexts
    {
        public const string UserAlreadyExist = "There is user with same username";
        public const string AuthFailed = "The username/password is invalid";
    }

    public static class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}