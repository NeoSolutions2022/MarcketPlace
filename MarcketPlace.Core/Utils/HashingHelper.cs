using System.Security.Cryptography;
using System.Text;

namespace MarcketPlace.Core.Utils;

public class HashingHelper
{
    public static string ToSha256(string text)
    {
        return ToShaHash(text);
    }

    private static string ToShaHash(string text, EShaType type = EShaType.Sha256)
    {
        // Create a SHA256, SHA384 or SHA512 
        var algorithm = DecideHashAlgorithm(type);
        
        // ComputeHash - returns byte array  
        var bytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(text));  
  
        // Convert byte array to a string   
        var builder = new StringBuilder();  
        foreach (var b in bytes)
        {
            builder.Append(b.ToString("x2"));
        }  
        
        return builder.ToString();
    }

    private static HashAlgorithm DecideHashAlgorithm(EShaType type)
    {
        return type switch
        {
            EShaType.Sha384 => SHA384.Create(),
            EShaType.Sha512 => SHA512.Create(),
            EShaType.Sha256 or _ => SHA256.Create()
        };
    }

}

public enum EShaType
{
    Sha256 = 1,
    Sha384,
    Sha512,
}