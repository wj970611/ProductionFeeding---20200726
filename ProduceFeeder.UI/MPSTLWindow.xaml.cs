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

namespace ProduceFeeder.UI
{
    /// <summary>
    /// Window2.xaml 的交互逻辑
    /// </summary>
    public partial class MPSTLWindow : Window
    {
         public static readonly MPSTLWindow Instance = new MPSTLWindow();
        private MPSTLWindow()
        {
            InitializeComponent();
         
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
