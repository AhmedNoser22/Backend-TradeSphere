namespace TradeSphere.Application.Common.Utilities;
public static class CodeGenerator
{
    public static string GenerateNumericCode(int length = 6)
    {
        Span<byte> bytes = stackalloc byte[length];
        RandomNumberGenerator.Fill(bytes);

        var code = new char[length];
        for (var i = 0; i < length; i++)
            code[i] = (char)('0' + bytes[i] % 10);

        return new string(code);
    }
}