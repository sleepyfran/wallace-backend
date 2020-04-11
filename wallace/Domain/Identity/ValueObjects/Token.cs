using Wallace.Domain.ValueObjects;

namespace Wallace.Domain.Identity.Entities
{
    /// <summary>
    /// Defines a JWT token with its value and its lifetime.
    /// </summary>
    public class Token
    {
        private readonly string _value;

        public Token(string val)
        {
            _value = val;
        }

        public string Jwt => _value;
        public Minutes Lifetime { get; set; }

        public static implicit operator string(Token t) {
            return t._value;
        }
        
        public static implicit operator Token(string s) {
            return new Token(s);
        }
    }
}