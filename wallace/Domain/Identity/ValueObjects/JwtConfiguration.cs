using Microsoft.Extensions.Configuration;
using Wallace.Domain.ValueObjects;

namespace Wallace.Domain.Identity.Entities
{
    public class JwtConfiguration
    {
        private readonly Minutes _defaultTokenLifetime = 15;
        private readonly IConfiguration _configuration;

        public JwtConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        /// <summary>
        /// JWT key from the settings.
        /// </summary>
        public string Key => _configuration["Jwt:Key"];

        /// <summary>
        /// Returns either the lifetime from the configuration in minutes or the
        /// default one (15 minutes).
        /// </summary>
        public Minutes TokenLifetime
        {
            get
            {
                if(int.TryParse(
                    _configuration["Jwt:TokenLifetime"], 
                    out int lifetime
                ))
                {
                    return lifetime;
                }
                
                return _defaultTokenLifetime;
            }
        }
        
        public string Issuer => _configuration["Jwt:Issuer"];
        public string Audience => _configuration["Jwt:Audience"];
    }
}