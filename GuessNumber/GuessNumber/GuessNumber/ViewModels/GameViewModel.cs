using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;
using Xamarin.Forms;

namespace GuessNumber
{
    public class GameViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        //List of human guesses with results
        private ObservableCollection<ListViewItem> _Guesses;
        public ObservableCollection<ListViewItem> Guesses
        {
            get { return _Guesses; }
            set
            {
                if (_Guesses != value)
                {
                    ObservableCollection<ListViewItem> newValue = value; 
                    _Guesses = newValue;
                    RaisePropertyChanged(nameof(Guesses));
                }
            }
        }

        public GameViewModel()
        {
            _Guesses = new ObservableCollection<ListViewItem>();
        }

    }

    public class ListViewItem
    {
        public string Choice { get; set; }
        public string Result { get; set; }
    }
}
