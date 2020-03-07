using Prism.Ioc;
using BookerMagikWpfPrism.Views;
using System.Windows;
using BetfairBookmaker;
using BetfairBookmaker.Contracts;
using BookerMagikCore.Bookmaker;
using BookerMagikCore.Infrastructure;
using BookerMagikCore.Services.Arbitrage;
using BookerMagikCore.Sport;
using BookerMagikCore.Strategies;
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

            containerRegistry.Register<ISportArbitrageService, SportArbitrageService>();

            containerRegistry.Register<ILongestCommonSubsequence, LongestCommonSubsequence>();
            containerRegistry.Register<ISimilarStringsCalculator, SimilarStringsCalculator>();
            containerRegistry.Register<ISameTimeEventsSearch, SameTimeEventsSearch>();

            containerRegistry.Register<IGlobalBookmaker, GlobalBookmaker>();
            containerRegistry.Register<IBetfairBookmaker, BetfairBookmakerClass>();
            containerRegistry.Register<IPinnacleBookmaker, PinnacleBookmakerClass>();
        }
    }
}
