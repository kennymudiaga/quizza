namespace Quizza.Common.Constants
{
    public static class Milliseconds
    {
        public const int OneSecond = 1000;
        public const int OneMinute = OneSecond * 60;
        public const int OneHour = OneMinute * 60;
        public const int OneDay = OneHour * 24;
    }

    public static class Seconds
    {
        public const int OneMinute = 60;
        public const int OneHour = OneMinute * 60;
        public const int OneDay = OneHour * 24;
        public const int HalfDay = OneHour * 12;
    }

    public static class Minutes
    {
        public const int OneHour = 60;
        public const int OneDay = OneHour * 24;
        public const int SevenDays = OneDay * 7;
    }

    public static class Hours
    {
        public const int OneDay = 24;
        public const int HalfDay = 12;
        public const int SevenDays = OneDay * 7;
    }
}
