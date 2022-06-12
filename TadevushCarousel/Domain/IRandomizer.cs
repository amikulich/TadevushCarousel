namespace TadevushCarousel.Domain
{
    public interface IRandomizer
    {
        (string left, string right) NextPair();
    }
}
