using RAWPhotosSorter.Global;
using RAWPhotosSorter.Model;
using RAWPhotosSorter.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RAWPhotosSorter
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Photo selectedphoto = null;
        public Photo SelectedPhoto {
            get { return selectedphoto; }
            set {
                selectedphoto = value;
                this.OnPropertyChanged("SelectedPhoto");
            }
        }

        private List<Photo> Photos { get; set; } = new List<Photo>();
        private DirectoryInfo SourcePath { get; set; } = null;

        public MainWindow(string path)
        {
            Config.Load();
            this.SourcePath = new DirectoryInfo(path);

            foreach(string file in Directory.GetFiles(this.SourcePath.FullName).ToList().Where(x => x.ToLower().EndsWith($".{Config.JpgExtension.ToLower()}"))) {
                this.Photos.Add(new Photo(file));
            }
            this.ChangePhoto(0);

            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right)
                this.ChangePhoto(e.Key == Key.Left ? -1 : 1);
            else if (e.Key == Key.P) {
                (new SettingsWindow()).ShowDialog();
            }
            else if (e.Key == Key.H) {
                (new HelpWindow()).ShowDialog();
            }
            else if(e.Key == Key.Escape) {
                this.Close();
            }
            else if (e.Key == Key.D) {
                if (CheckDestination()) {
                    Directory.CreateDirectory(Path.Combine(Config.DestinationPath, this.SourcePath.Name));
                    Process.Start("explorer.exe", Path.Combine(Config.DestinationPath, this.SourcePath.Name));
                }
            }
            else if (e.Key == Key.S) {
                if (CheckDestination()) {
                    if (this.SelectedPhoto == null) {
                        System.Windows.MessageBox.Show("Aucune photo n'est sélectionnée, merci de le faire !", "Problème pour l'envoi", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    FileInfo fi = new FileInfo(this.SelectedPhoto.Path);
                    string rawfile = fi.Name.Substring(0, fi.Name.IndexOf(".")) + $".{Config.RawExtension}";
                    string rawpath = Path.Combine(this.SourcePath.FullName, rawfile);
                    if (!File.Exists(rawpath)) {
                        System.Windows.MessageBox.Show("Le fichier RAW n'a pas l'air d'exister ?...", "Problème pour l'envoi", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else {
                        Directory.CreateDirectory(Path.Combine(Config.DestinationPath, this.SourcePath.Name));
                        File.Copy(rawpath, Path.Combine(Config.DestinationPath, this.SourcePath.Name, rawfile));
                    }
                }
            }
        }

        public void ChangePhoto(int direction)
        {
            if (this.Photos.Count > 0) {
                if (direction == 0) {
                    this.SelectedPhoto = this.Photos[0];
                }
                else {
                    if ((direction == -1 && this.Photos.IndexOf(this.SelectedPhoto) == 0) ||
                        (direction == 1 && this.Photos.IndexOf(this.SelectedPhoto) == this.Photos.Count - 1))
                        return;

                    if (direction == -1 && this.Photos.IndexOf(this.SelectedPhoto) + 5 <= this.Photos.Count - 1 && this.Photos[this.Photos.IndexOf(this.SelectedPhoto) + 5].Source != null) {
                        string path = this.Photos[this.Photos.IndexOf(this.SelectedPhoto) + 5].Path;
                        this.Photos.RemoveAt(this.Photos.IndexOf(this.SelectedPhoto) + 5);
                        this.Photos.Insert(this.Photos.IndexOf(this.SelectedPhoto) + 5, new Photo(path));
                        GC.Collect();
                    }
                    else if (direction == 1 && this.Photos.IndexOf(this.SelectedPhoto) - 5 >= 0 && this.Photos[this.Photos.IndexOf(this.SelectedPhoto) - 5].Source != null) {
                        string path = this.Photos[this.Photos.IndexOf(this.SelectedPhoto) - 5].Path;
                        this.Photos.RemoveAt(this.Photos.IndexOf(this.SelectedPhoto) - 5);
                        this.Photos.Insert(this.Photos.IndexOf(this.SelectedPhoto) - 4, new Photo(path));
                        GC.Collect();
                    }

                    this.SelectedPhoto = this.Photos[this.Photos.IndexOf(this.SelectedPhoto) + direction];
                }

                this.Title = new FileInfo(this.SelectedPhoto.Path).Name;
            }
        }

        private bool CheckDestination()
        {
            if (string.IsNullOrEmpty(Config.DestinationPath)) {
                System.Windows.MessageBox.Show("Le dossier de destination n'est pas sélectionné. Merci de le faire", "Problème de destination", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            return true;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Veux-tu vraiment fermer l'application ?", "Demande de confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) {
                e.Cancel = true;
            }
        }
    }
}
