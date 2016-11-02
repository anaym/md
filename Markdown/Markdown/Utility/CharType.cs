namespace Markdown.Utility
{
    //потому что запрещены regexp-ы
    public enum CharType
    {
        Space,
        Digit,
        Letter,
        SpaceOrDigit,
        DigitOrLetter,
        LetterOrSpace,
        Any
    }
}