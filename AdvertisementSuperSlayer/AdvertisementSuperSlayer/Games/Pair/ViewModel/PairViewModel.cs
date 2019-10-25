using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using static Xamarin.Forms.AbsoluteLayout;

namespace AdvertisementSuperSlayer.Games.Pair.ViewModel
{
    partial class PairViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public PairViewModel()
        {

        }

        public StackOrientation StOrientation
        {
            get
            {
                return this._WindowModel.Orientation;
            }
            set
            {
                this._WindowModel.Orientation = value;
                OnPropertyChanged("StOrientation");
            }
        }



        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
