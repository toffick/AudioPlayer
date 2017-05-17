using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AudioPlayer_v1._0
{
    /// <summary>
    /// Логика взаимодействия для PlaylistName.xaml
    /// </summary>
    public partial class PlaylistName : Window
    {
        public string Plname { get; private set; }

        public PlaylistName()
        {
            InitializeComponent();
            textBox.MaxLength = 99;
            textBox.Focus();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!textBox.Text.Equals(""))
                {
                    Plname = textBox.Text;
                    this.Close();
                }
            }
            catch
            {
                MessageBox.Show("Попробуйте другое имя");
            }
        }



        private void button1_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    try
                    {
                        if (!textBox.Text.Equals(""))
                        {
                            Plname = textBox.Text;
                            this.Close();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Попробуйте другое имя");
                    }
                    break;
                case Key.Escape:
                    this.Close();
                    break;
            }
        }
    }
}
