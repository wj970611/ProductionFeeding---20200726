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
    /// MPSQuery.xaml 的交互逻辑
    /// </summary>
    public partial class MPSQueryWindow : Window
    {
        public static readonly MPSQueryWindow Instance = new MPSQueryWindow();
        private MPSQueryWindow()
        {
            InitializeComponent();

            this.Unloaded += (sender, e) => { this.Hide(); e.Handled = true; };
        }
    }
}
