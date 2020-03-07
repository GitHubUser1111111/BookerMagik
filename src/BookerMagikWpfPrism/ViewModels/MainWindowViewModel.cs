using BetfairBookmaker.Contracts;
using BookerMagikCore.Bookmaker;
using BookerMagikCore.Common.EventArguments;
using BookerMagikWpfPrism.Services;
using PinnacleBookmaker.Contracts;
using Prism.Commands;
using Prism.Mvvm;

namespace BookerMagikWpfPrism.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IBookmakerService _bookmakerService;
        private readonly IBetfairBookmaker _betfairBookmaker;
        private readonly IPinnacleBookmaker _pinnacleBookmaker;
        private readonly IGlobalBookmaker _globalBookmaker;

        private string _title = "MoneyMaker";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public DelegateCommand LoginCommand { get; private set; }


        public MainWindowViewModel(IBookmakerService bookmakerService, IBetfairBookmaker betfairBookmaker, IPinnacleBookmaker pinnacleBookmaker, IGlobalBookmaker globalBookmaker)
        {
            _bookmakerService = bookmakerService;
            _betfairBookmaker = betfairBookmaker;
            _pinnacleBookmaker = pinnacleBookmaker;
            _globalBookmaker = globalBookmaker;

            LoginCommand = new DelegateCommand(Login, CanLogin);
        }

        private bool CanLogin()
        {
            return true;
        }

        private async void Login()
        {
            // Betfair
            var betfairConfig = _bookmakerService.LoadBookmakerConfiguration("betfair");
            var betfairLogin = await _bookmakerService.LoginBookmaker(_betfairBookmaker, betfairConfig);
            if (betfairLogin)
                _globalBookmaker.RegisterBookmaker(_betfairBookmaker);

            // Pinnacle
            var pinnacleConfig = _bookmakerService.LoadBookmakerConfiguration("pinnacle");
            var pinnacleLogin = await _bookmakerService.LoginBookmaker(_pinnacleBookmaker, pinnacleConfig);
            if (pinnacleLogin)
                _globalBookmaker.RegisterBookmaker(_pinnacleBookmaker);
        }
    }
}
