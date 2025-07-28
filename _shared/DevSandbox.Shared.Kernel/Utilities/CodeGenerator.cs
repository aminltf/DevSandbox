using System.Security.Cryptography;
using System.Text;

namespace DevSandbox.Shared.Kernel.Utilities;

public static class CodeGenerator
{
    public static string GenerateNumeric(int length)
    {
        var code = new StringBuilder(length);
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[4];

        for (int i = 0; i < length; i++)
        {
            rng.GetBytes(bytes);
            uint digit = BitConverter.ToUInt32(bytes, 0) % 10;
            code.Append(digit);
        }

        return code.ToString();
    }

    public static string GenerateToken(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var token = new StringBuilder(length);
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[4];

        for (int i = 0; i < length; i++)
        {
            rng.GetBytes(bytes);
            uint index = BitConverter.ToUInt32(bytes, 0) % (uint)chars.Length;
            token.Append(chars[(int)index]);
        }

        return token.ToString();
    }
}
