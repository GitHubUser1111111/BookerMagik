using BetfairBookmaker.Contracts;
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

        private string _title = "MoneyMaker";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public DelegateCommand LoginCommand { get; private set; }


        public MainWindowViewModel(IBookmakerService bookmakerService, IBetfairBookmaker betfairBookmaker, IPinnacleBookmaker pinnacleBookmaker)
        {
            _bookmakerService = bookmakerService;
            _betfairBookmaker = betfairBookmaker;
            _pinnacleBookmaker = pinnacleBookmaker;

            LoginCommand = new DelegateCommand(Login, CanLogin);

            _betfairBookmaker.LineUpdated += _betfairBookmaker_LineUpdated;
            _pinnacleBookmaker.LineUpdated += _pinnacleBookmaker_LineUpdated;
        }

        private void _pinnacleBookmaker_LineUpdated(object sender, LineUpdatedEventArgs e)
        {
        }

        private void _betfairBookmaker_LineUpdated(object sender, LineUpdatedEventArgs e)
        {
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
            if(betfairLogin)
                _betfairBookmaker.StartReadLineThread();

            // Pinnacle
            var pinnacleConfig = _bookmakerService.LoadBookmakerConfiguration("pinnacle");
            var pinnacleLogin = await _bookmakerService.LoginBookmaker(_pinnacleBookmaker, pinnacleConfig);
            if (pinnacleLogin)
                _pinnacleBookmaker.StartReadLineThread();
        }
    }
}
