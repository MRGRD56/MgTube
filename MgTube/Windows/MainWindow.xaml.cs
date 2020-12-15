using System.Windows;

namespace MgTube.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MessageBox.Show(MySecrets.Properties.YtKey);
        }
    }
}
