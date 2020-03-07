using Prism.Ioc;
using BookerMagikWpfPrism.Views;
using System.Windows;
using BetfairBookmaker;
using BetfairBookmaker.Contracts;
using BookerMagikWpfPrism.Services;
using PinnacleBookmaker;
using PinnacleBookmaker.Contracts;

namespace BookerMagikWpfPrism
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IBookmakerService, BookmakerService>();
            containerRegistry.Register<IBetfairBookmaker, BetfairBookmakerClass>();
            containerRegistry.Register<IPinnacleBookmaker, PinnacleBookmakerClass>();
        }
    }
}
