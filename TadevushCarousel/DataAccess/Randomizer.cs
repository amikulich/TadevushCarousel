using TadevushCarousel.Domain;

namespace TadevushCarousel.DataAccess
{
    public class Randomizer : IRandomizer
    {
        private readonly Random _rand;
        private readonly string[] _items;

        public Randomizer(ICollection<string> items)
        {
            if (items == null || items.Count == 0)
            {
                throw new ArgumentNullException("No items to choose from");
            }

            _items = items.ToArray();
            _rand = new Random(DateTime.Now.Millisecond);
            _rand.Next(items.Count);
        }

        public (string left, string right) NextPair()
        {
            var left = _rand.Next(_items.Length);
            var right = left;

            while(right == left)
            {
                right = _rand.Next(_items.Length);
            }

            //return ("1258952", "1337643");
            return (_items[left], _items[right]);
        }
    }
}
