using System.Windows;
using System.Windows.Input;

namespace RAWPhotosSorter.Windows
{
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();

            this.HelpText.Text =
@"Flèches : Naviguer dans les photos
Touche S : Envoyer la photo actuelle dans le dossier de destination ( + sous dossier des photos )
Touche D : Ouvrir le dossier de destination
Touche P : Ouvrir les paramètres
Touche H : Ouvrir cette aide";
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Escape) {
                this.Close();
            }
        }
    }
}
