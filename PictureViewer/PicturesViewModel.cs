using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;

namespace PictureViewer
{
    public class PicturesViewModel : INotifyPropertyChanged
    {
        private readonly List<PictureViewModel> mPictures = new List<PictureViewModel>();
        private int mCurrentIndex = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public IList<PictureViewModel> Pictures { get { return mPictures; } }

        public PictureViewModel Current
        {
            get { return mPictures[mCurrentIndex]; }
        }

        public PicturesViewModel(IEnumerable<string> filePaths)
        {
            mPictures.AddRange(filePaths.Select(x => new PictureViewModel(x)));
        }

        public void ImportJson(string json)
        {
            var serialized = JsonConvert.DeserializeObject<List<PictureViewModel>>(json);

            foreach (var picture in mPictures)
            {
                var serializedPic = serialized.SingleOrDefault(x => x.Uri == picture.Uri);

                if (serializedPic != null)
                {
                    picture.Angle = serializedPic.Angle;
                }
            }
        }

        public void Next()
        {
            var current = Current;

            mCurrentIndex = (mCurrentIndex + 1) % mPictures.Count;

            OnPropertyChanged("Current");

            current.Cleanup();
        }

        public void Previous()
        {
            var current = Current;

            mCurrentIndex -= 1;

            if (mCurrentIndex < 0)
            {
                mCurrentIndex = mPictures.Count - 1;
            }

            OnPropertyChanged("Current");

            current.Cleanup();
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(mPictures.OrderBy(x => x.Uri.ToString()));
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
