using System;
using System.Windows.Media.Imaging;

namespace RAWPhotosSorter.Model
{
    public class Photo
    {
        public string Path { get; set; }

        private BitmapImage source = null;
        public BitmapImage Source {
            get { 
                if (source == null) {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new System.Uri(this.Path);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    source = bitmap;
                }
                return source;
            }
        }

        public Photo(string path)
        {
            this.Path = path;
        }
    }
}
