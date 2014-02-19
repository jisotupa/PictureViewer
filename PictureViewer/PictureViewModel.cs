using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace PictureViewer
{
    public class PictureViewModel : INotifyPropertyChanged
    {
        private ImageSource mSource;
        private int mAngle;
        private Uri mUri;

        public event PropertyChangedEventHandler PropertyChanged;

        public Uri Uri
        {
            get { return mUri; }
            set
            {
                if (mUri == value) return;

                mUri = value;

                OnPropertyChanged("Uri");
                OnPropertyChanged("Source");
            }
        }

        [JsonIgnore]
        public ImageSource Source
        {
            get
            {
                if (mSource == null)
                {
                    mSource = new BitmapImage(mUri);
                }

                return mSource;
            }
        }

        public int Angle
        {
            get { return mAngle; }
            set
            {
                if (mAngle == value) return;

                if (value != 0 && value != 90 && value != 180 && value != 270)
                {
                    throw new ArgumentOutOfRangeException("Allowed values for Angle are 0, 90, 180 and 270.");
                }

                mAngle = value;

                OnPropertyChanged("Angle");
            }
        }


        public PictureViewModel()
        {
        }

        public PictureViewModel(string fullpath)
        {
            Uri = new Uri(fullpath);
            Angle = 180;
        }

        public void Cleanup()
        {
            mSource = null;
        }

        public void RotateClockwise()
        {
            Angle = (Angle + 90) % 360;
        }

        public void RotateCounterClockwise()
        {
            Angle = (Angle + 270) % 360;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
