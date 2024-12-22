namespace Game.SaveSystem
{
	using System;
	using System.IO;
	using System.Security.Cryptography;

    public static class AESEncryptor
    {
        public static string Encrypt(string txt, byte[] salt, string password)
        {
            using Aes aesAlg = Aes.Create();
            using (var keyDerivationFunction = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                aesAlg.Key = keyDerivationFunction.GetBytes(16);
                aesAlg.IV = keyDerivationFunction.GetBytes(16);
            }

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                msEncrypt.Write(salt, 0, salt.Length);

                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(txt);
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText, byte[] salt, string password)
        {
            byte[] fullCipher = Convert.FromBase64String(cipherText);
            
            Array.Copy(fullCipher, 0, salt, 0, salt.Length);

            using (Aes aesAlg = Aes.Create())
            {
                using (var keyDerivationFunction = new Rfc2898DeriveBytes(password, salt, 10000))
                {
                    aesAlg.Key = keyDerivationFunction.GetBytes(16); // AES-128
                    aesAlg.IV = keyDerivationFunction.GetBytes(16);
                }

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt =
                       new MemoryStream(fullCipher, salt.Length, fullCipher.Length - salt.Length))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}