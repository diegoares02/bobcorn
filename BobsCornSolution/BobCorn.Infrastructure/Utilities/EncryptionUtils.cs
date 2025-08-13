namespace BobsCorn.Infrastructure.Utilities
{
    public class EncryptionUtils
    {
        private static readonly byte[] Key = Convert.FromBase64String(
            Environment.GetEnvironmentVariable("ENCRYPTION_KEY")
            ?? throw new InvalidOperationException("ENCRYPTION_KEY environment variable is not set.")
        );
        private static readonly byte[] IV = Convert.FromBase64String(
            Environment.GetEnvironmentVariable("ENCRYPTION_IV")
            ?? throw new InvalidOperationException("ENCRYPTION_IV environment variable is not set.")
        );

        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            using var aes = System.Security.Cryptography.Aes.Create();

            byte[] validKey = new byte[32];
            byte[] validIV = new byte[16];

            Array.Copy(Key, validKey, Math.Min(Key.Length, validKey.Length));
            Array.Copy(IV, validIV, Math.Min(IV.Length, validIV.Length));

            aes.Key = validKey;
            aes.IV = validIV;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream();
            using (var cs = new System.Security.Cryptography.CryptoStream(ms, encryptor, System.Security.Cryptography.CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}
