using System.ComponentModel;
using System.Runtime.CompilerServices;
using HSC.Mobile.Enums;
using HSC.Mobile.Pages;
using HSC.Mobile.Pages.ChessPage.ChessBoardPage;
using PonzianiComponents;

namespace HSC.Mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel viewModel)
        {
            BindingContext = viewModel;
            InitializeComponent();
        }
    }

    public class MainViewModel: INotifyPropertyChanged
    {
        private int _moveCount = 0;
        private string _lastMove;

        public MainViewModel()
        {
            MessagingCenter.Subscribe<ChessBoardPage, MovePlayedInfo>(this, MessageTypes.MoveMade, (_, arg) => MoveMade(arg));
        }

        private void MoveMade(MovePlayedInfo e)
        {
            MoveCount++;
            LastMove = e.San;
        }

        public int MoveCount
        {
            get => _moveCount;
            set => SetField(ref _moveCount, value);
        }

        public string LastMove
        {
            get => _lastMove;
            set => SetField(ref _lastMove, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

}