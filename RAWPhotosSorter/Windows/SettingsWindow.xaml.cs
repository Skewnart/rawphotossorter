using RAWPhotosSorter.Global;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RAWPhotosSorter.Windows
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            this.JPG.Text = Config.JpgExtension;
            this.RAW.Text = Config.RawExtension;
            this.DestPath.Text = Config.DestinationPath;

            this.JPG.Focus();
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((FrameworkElement)sender).Name.Equals("JPG")) {
                Config.JpgExtension = ((TextBox)sender).Text;
            }
            else if (((FrameworkElement)sender).Name.Equals("RAW")) {
                Config.RawExtension = ((TextBox)sender).Text;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog()) {
                fbd.ShowDialog();
                if (!string.IsNullOrEmpty(fbd.SelectedPath)) {
                    this.DestPath.Text = Config.DestinationPath = fbd.SelectedPath;
                }
            }
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Escape) {
                this.Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Config.Save();
        }
    }
}
