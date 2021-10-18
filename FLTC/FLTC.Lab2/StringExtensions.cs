namespace FLTC.Lab2
{
    public static class StringExtensions
    {
        public static bool Is(this string token, TokenType type)
        {
            return type.Matches(token);
        }
    }
}
