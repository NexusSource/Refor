using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;

namespace Refor.Services
{
    public interface IRandomStringService
    {
        string GetRandomString();
    }

    public class RandomStringService : IRandomStringService
    {
        private readonly ILogger _logger;
        private readonly RandomNumberGenerator _randomNumberGenerator;

        public RandomStringService(ILogger<RandomStringService> logger, RandomNumberGenerator randomNumberGenerator)
        {
            _logger = logger;
            _randomNumberGenerator = randomNumberGenerator;
        }

        public string GetRandomString()
        {
            // We declare the bytes used to generate the string that is 8 characters.
            // We want to keep the string size small so the URL looks clean.
            byte[] bytes = new byte[8];

            // Fill our array with securely random bytes.
            _logger.LogDebug("Generating a new random string via RNG.");
            _randomNumberGenerator.GetBytes(bytes);

            // Convert it to a base64 string.
            return Convert.ToBase64String(bytes);
        }
    }
}