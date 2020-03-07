using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Prism.Mvvm;

namespace BookerMagikWpfPrism.ViewModels
{
    public class BookmakerViewModel : BindableBase
    {
        public string Name { get; set; }

        private bool isLogged;
        public bool IsLogged
        {
            get => isLogged;
            set => SetProperty(ref isLogged, value);
        }

        public ObservableCollection<string> Sports { get; set; }

        public ObservableCollection<string> Leagues { get; set; }
    }
}
