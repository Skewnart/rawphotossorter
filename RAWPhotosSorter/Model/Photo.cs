using RAWPhotosSorter.Extensions;
using System.Drawing;
using System.IO;
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
                    var image = Image.FromFile(this.Path);
                    image.NormalizeOrientation();
                    var bitmap = new BitmapImage();

                    using (MemoryStream stream = new MemoryStream()) {
                        image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        stream.Seek(0, SeekOrigin.Begin);

                        // Tell the WPF BitmapImage to use this stream
                        bitmap.BeginInit();
                        bitmap.StreamSource = stream;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                    }
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
