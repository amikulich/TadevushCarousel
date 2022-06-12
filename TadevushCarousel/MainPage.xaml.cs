using CommunityToolkit.Mvvm.ComponentModel;
using TadevushCarousel.Domain;

namespace TadevushCarousel;

public partial class MainPage : ContentPage
{
    private readonly ICarStore _carStore;
    private readonly IRandomizer _randomizer;

    public MainPage(MainViewModel viewModel, ICarStore carStore, IRandomizer randomizer)
    {
        InitializeComponent();

        _carStore = carStore;
        _randomizer = randomizer;

        //var nextPair = _randomizer.NextPair();

        //var leftCar = _carStore.GetCar(nextPair.left);
        //viewModel.LeftImageName = leftCar.Id;
        //viewModel.LeftImageUrl = leftCar.ImageUrl;

        //var rightCar = _carStore.GetCar(nextPair.right);
        //viewModel.RightImageName = rightCar.Id;
        //viewModel.RightImageUrl = rightCar.ImageUrl;

        BindingContext = viewModel;

        RefreshImages();
    }

    private void LikeLeftClick(object sender, EventArgs args)
    {
        var viewModel = (MainViewModel)BindingContext;

        UpdateStats(viewModel.LeftImageName, viewModel.RightImageName);

        OnButtonClick(LeftImage);
    }

    private void LikeRightClick(object sender, EventArgs args)
    {
        var viewModel = (MainViewModel)BindingContext;

        UpdateStats(viewModel.RightImageName, viewModel.LeftImageName);

        OnButtonClick(RightImage);
    }

    private void UpdateStats(string winnerId, string loserId)
    {
        var winnerCar = _carStore.GetCar(winnerId);
        winnerCar.Wins++;
        winnerCar.NumOfGames++;

        _carStore.SaveCar(winnerCar);

        var loserCar = _carStore.GetCar(loserId);
        loserCar.NumOfGames++;

        _carStore.SaveCar(loserCar);
    }

    private void OnButtonClick(ImageButton button)
    {
        var initcolor = button.BorderColor;
        button.BorderWidth = 10;
        button.BorderColor = Color.Parse("Blue");

        RefreshImages();

        button.BorderWidth = 0;
        button.BorderColor = initcolor;
    }

    private void RefreshImages()
    {
        var nextPair = _randomizer.NextPair();

        var viewModel = (MainViewModel)BindingContext;

        var leftCar = _carStore.GetCar(nextPair.left);
        viewModel.LeftImageName = leftCar.Id;
        viewModel.LeftImageUrl = leftCar.ImageUrl;

        var rightCar = _carStore.GetCar(nextPair.right);
        viewModel.RightImageName = rightCar.Id;
        viewModel.RightImageUrl = rightCar.ImageUrl;
    }
}

public partial class MainViewModel : ObservableObject
{
    IConnectivity _connectivity;
    public MainViewModel(IConnectivity connectivity)
    {
        _connectivity = connectivity;
    }

    [ObservableProperty]
    string leftImageUrl;

    public string LeftImageName { get; set; }

    [ObservableProperty]
    string rightImageUrl;

    public string RightImageName { get; set; }
}
