using System.Security.Cryptography;

namespace MarcketPlace.Core.Utils;

public class PasswordUtils
{
    private static readonly char[] Punctuations = "!@#$%^&*()_-+[{]}:>|/?".ToCharArray();
    public static string Generate(int length, int numberOfNonAlphanumericCharacters)
    {
        if (length is < 1 or > 128)
        {
            throw new ArgumentException(null, nameof(length));
        }

        if (numberOfNonAlphanumericCharacters > length || numberOfNonAlphanumericCharacters < 0)
        {
            throw new ArgumentException(null, nameof(numberOfNonAlphanumericCharacters));
        }

        using var rng = RandomNumberGenerator.Create();
        var byteBuffer = new byte[length];
           
        rng.GetBytes(byteBuffer);

        var count = 0;
        var characterBuffer = new char[length];

        for (var iter = 0; iter < length; iter++)
        {
            var i = byteBuffer[iter] % 87;

            if (i < 10)
            {
                characterBuffer[iter] = (char)('0' + i);
            }
            else if (i < 36)
            {
                characterBuffer[iter] = (char)('A' + i - 10);
            }
            else if (i < 62)
            {
                characterBuffer[iter] = (char)('a' + i - 36);
            }
            else
            {
                characterBuffer[iter] = Punctuations[GetRandomInt(rng, Punctuations.Length)];
                count++;
            }
        }

        if (count >= numberOfNonAlphanumericCharacters)
        {
            return new string(characterBuffer);
        }

        int j;
            
        for (j = 0; j < numberOfNonAlphanumericCharacters - count; j++)
        {
            int k;
            do
            {
                k = GetRandomInt(rng, length);
            }
            while (!char.IsLetterOrDigit(characterBuffer[k]));

            characterBuffer[k] = Punctuations[GetRandomInt(rng, Punctuations.Length)];
        }

        return new string(characterBuffer);
    }

    private static int GetRandomInt(RandomNumberGenerator randomGenerator)
    {
        var buffer = new byte[4];
        randomGenerator.GetBytes(buffer);

        return BitConverter.ToInt32(buffer);
    }
    
    private static int GetRandomInt(RandomNumberGenerator randomGenerator, int maxInput)
    {
        return Math.Abs(GetRandomInt(randomGenerator) % maxInput);
    }

}