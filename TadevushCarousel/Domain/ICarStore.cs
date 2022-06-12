namespace TadevushCarousel.Domain
{
    public interface ICarStore
    {
        Car GetCar(string id);

        void SaveCar(Car car);
    }
}
