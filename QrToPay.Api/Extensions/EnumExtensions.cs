namespace QrToPay.Api.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// Konwertuje wartość int na odpowiadający typ enum.
    /// </summary>
    /// <typeparam name="TEnum">Typ enum, na który ma być zamieniona wartość int.</typeparam>
    /// <param name="value">Wartość int do zamiany.</param>
    /// <returns>Odpowiadająca wartość enum.</returns>
    /// <exception cref="ArgumentException">Jeśli wartość nie jest zdefiniowana w enum.</exception>
    public static TEnum ToEnum<TEnum>(this int value) where TEnum : struct, Enum
    {
        if (!Enum.IsDefined(typeof(TEnum), value))
            throw new ArgumentException($"{value} is not defined in enum {typeof(TEnum)}");

        return (TEnum)Enum.ToObject(typeof(TEnum), value);
    }

    /// <summary>
    /// Konwertuje wartość enum na odpowiadającą wartość int.
    /// </summary>
    /// <typeparam name="TEnum">Typ enum do zamiany na int.</typeparam>
    /// <param name="enumValue">Wartość enum do zamiany.</param>
    /// <returns>Odpowiadająca wartość int.</returns>
    public static int ToInt<TEnum>(this TEnum enumValue) where TEnum : struct, Enum
    {
        return Convert.ToInt32(enumValue);
    }
}