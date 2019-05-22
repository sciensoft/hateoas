using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Sciensoft.Samples.Products.Api.Application.Services
{
    public class EntityTagHashProvider : IEntityTagHashProvider
    {
        public string CreateHash(string payload)
        {
            AssurePayloadIsNotNull(payload);

            var hashEncryptor = new SHA1Managed();
            var hash = hashEncryptor.ComputeHash(Encoding.UTF8.GetBytes(payload));

            return string.Join(string.Empty, hash.Select(h => h.ToString("X2")));
        }

        public string CreateHash(int payload)
            => CreateHash(payload.ToString());

        public bool ValidatePayloadHash(string payload, string eTag)
        {
            AssurePayloadIsNotNull(payload);

            return CreateHash(payload).Equals(eTag);
        }

        public bool ValidatePayloadHash(int payload, string eTag)
            => ValidatePayloadHash(payload.ToString(), eTag);

        private void AssurePayloadIsNotNull(string payload)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }
        }
    }

    public interface IEntityTagHashProvider
    {
        string CreateHash(string payload);

        string CreateHash(int payload);

        bool ValidatePayloadHash(string payload, string eTag);

        bool ValidatePayloadHash(int payload, string eTag);
    }
}
