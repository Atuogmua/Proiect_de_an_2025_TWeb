﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Helpers
{
     public static class CookieGenerator
     {
          private const string SaltData = "zsOfXQRNwo*WbNpu_t)qVfIU.C?2Q8";

          private static readonly byte[] Salt = Encoding.ASCII.GetBytes(SaltData);

          public static string Create(string value)
          {
               return EncryptStringAes(value, "@l2LJltsI*Qc:l4+Y0Q=)#dbOiDzZpnO8MccWXgd!0XHW-gx@zhDy+nv0zTRw-+0tHAKclFtHx6Y:4pv/55./OLjuwy*O/G?tKb?");
          }
          public static string Validate(string value)
          {
               return DecryptStringAes(value, "@l2LJltsI*Qc:l4+Y0Q=)#dbOiDzZpnO8MccWXgd!0XHW-gx@zhDy+nv0zTRw-+0tHAKclFtHx6Y:4pv/55./OLjuwy*O/G?tKb?");
          }

          private static string DecryptStringAes(string cipherText, string sharedSecret)
          {
               if (string.IsNullOrEmpty(cipherText))
                    throw new ArgumentNullException(nameof(cipherText));
               if (string.IsNullOrEmpty(sharedSecret))
                    throw new ArgumentNullException(nameof(sharedSecret));

               // Declare the RijndaelManaged object
               // used to decrypt the data.
               RijndaelManaged aesAlg = null;

               // Declare the string used to hold
               // the decrypted text.
               string plaintext;

               try
               {
                    // generate the key from the shared secret and the salt
                    var key = new Rfc2898DeriveBytes(sharedSecret, Salt);

                    // Create the streams used for decryption.                
                    var bytes = Convert.FromBase64String(cipherText);
                    using (var msDecrypt = new MemoryStream(bytes))
                    {
                         // Create a RijndaelManaged object
                         // with the specified key and IV.
                         aesAlg = new RijndaelManaged();
                         aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                         // Get the initialization vector from the encrypted stream
                         aesAlg.IV = ReadByteArray(msDecrypt);
                         // Create a decrytor to perform the stream transform.
                         ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                         using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                         {
                              using (var srDecrypt = new StreamReader(csDecrypt))

                                   // Read the decrypted bytes from the decrypting stream
                                   // and place them in a string.
                                   plaintext = srDecrypt.ReadToEnd();
                         }
                    }
               }
               finally
               {
                    // Clear the RijndaelManaged object.
                    if (aesAlg != null)
                         aesAlg.Clear();
               }

               return plaintext;
          }

          private static byte[] ReadByteArray(Stream s)
          {
               var rawLength = new byte[sizeof(int)];
               if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
               {
                    throw new SystemException("Stream did not contain properly formatted byte array");
               }

               var buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
               if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
               {
                    throw new SystemException("Did not read byte array properly");
               }

               return buffer;
          }

          private static string EncryptStringAes(string plainText, string sharedSecret)
          {
               if (string.IsNullOrEmpty(plainText))
                    throw new ArgumentNullException(nameof(plainText));
               if (string.IsNullOrEmpty(sharedSecret))
                    throw new ArgumentNullException(nameof(sharedSecret));

               string outStr;                       // Encrypted string to return
               RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.

               try
               {
                    // generate the key from the shared secret and the salt
                    var key = new Rfc2898DeriveBytes(sharedSecret, Salt);

                    // Create a RijndaelManaged object
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                    // Create a decryptor to perform the stream transform.
                    var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for encryption.
                    using (var msEncrypt = new MemoryStream())
                    {
                         // prepend the IV
                         msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                         msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                         using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                         {
                              using (var swEncrypt = new StreamWriter(csEncrypt))
                              {
                                   //Write all data to the stream.
                                   swEncrypt.Write(plainText);
                              }
                         }
                         outStr = Convert.ToBase64String(msEncrypt.ToArray());
                    }
               }
               finally
               {
                    aesAlg?.Clear();
               }

               // Return the encrypted bytes from the memory stream.
               return outStr;
          }
     }
}
