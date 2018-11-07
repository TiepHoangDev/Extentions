using System;
using System.Security.Cryptography;
using System.Text;

namespace LibraryCommon
{
    public class MD5Easy : IDisposable
    {
        private MD5 md5Hash;

        public MD5Easy()
        {
            md5Hash = MD5.Create();
        }

        public void Dispose()
        {
            if (md5Hash != null) md5Hash.Dispose();
        }

        public string GetMd5Hash(string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public bool Verify(string input, string hash)
        {
            string hashOfInput = GetMd5Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashOfInput, hash) == 0;
        }
    }

    public class SHA1Easy : IDisposable
    {
        private SHA1Managed sha1;

        public SHA1Easy()
        {
            sha1 = new SHA1Managed();
        }

        public void Dispose()
        {
            if (sha1 != null) sha1.Dispose();
        }

        public string GetSHA1Hash(string input)
        {
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }

        public bool Verify(string input, string hash)
        {
            string hashOfInput = GetSHA1Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashOfInput, hash) == 0;
        }
    }
}
