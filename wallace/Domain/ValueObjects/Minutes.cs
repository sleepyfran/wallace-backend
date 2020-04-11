namespace Wallace.Domain.ValueObjects
{
    /// <summary>
    /// Encapsulates an integer and allows conversion between it and this class
    /// to represent minutes.
    /// </summary>
    public class Minutes
    {
        private int _value;

        public Minutes(int value)
        {
            _value = value;
        }

        public static implicit operator int(Minutes m) {
            return m._value;
        }
        
        public static implicit operator Minutes(int i) {
            return new Minutes(i);
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}