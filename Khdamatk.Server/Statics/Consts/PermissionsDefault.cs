namespace Khdamatk.Server.Statics.Consts;


public class PermissionsDefault
{
    public const string Type = "permissions";

    public class WeatherForecast
    {
        private const string _name = nameof(WeatherForecast);

        public const string View = _name + ":View";
        public const string Modify = _name + ":Modify";
    }
    
    public class Authentications
    {
        private const string _name = nameof(Authentications);

        public const string View = _name + ":View";
        public const string Modify = _name + ":Modify";

    }

    public class Users
    {
        private const string _name = nameof(Users);

        public const string View = _name + ":View";
        public const string Modify = _name + ":Modify";
    }

    

    public static List<string> GetAllPermissions()
    {
        return new List<string>()
        {
            WeatherForecast.View,
            WeatherForecast.Modify,
            Authentications.View,
            Authentications.Modify,
            Users.View,
            Users.Modify
        };
    }
}