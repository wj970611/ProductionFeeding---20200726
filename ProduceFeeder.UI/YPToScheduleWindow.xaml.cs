using GalaSoft.MvvmLight.Messaging;
using ProduceFeeder.UI.Models.YuPai;
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
    /// YPToScheduleWindow.xaml 的交互逻辑
    /// </summary>
    public partial class YPToScheduleWindow : Window
    {
        public static readonly YPToScheduleWindow  Instance=new YPToScheduleWindow();
        private YPToScheduleWindow()
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
