namespace Wallace.Domain.ValueObjects
{
    /// <summary>
    /// Encapsulates an integer and allows conversion between it and this class
    /// to easily represent days.
    /// </summary>
    public class Days
    {
        private int _value;

        public Days(int value)
        {
            _value = value;
        }

        public Minutes Minutes => _value * 24;
        public int Seconds => _value * 24 * 60;

        public static implicit operator int(Days d) {
            return d._value;
        }
        
        public static implicit operator Days(int i) {
            return new Days(i);
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}