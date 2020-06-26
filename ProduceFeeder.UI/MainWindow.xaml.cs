using GalaSoft.MvvmLight.Messaging;
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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<string>(this, "MpsSelectedWindowShow", MpsSelecteWinShow);
            Messenger.Default.Register<string>(this, "MpsYPWindowShow", MpsYPWinShow);
            Messenger.Default.Register<string>(this, "MpsTLWindowShow", MpsTLWinShow);
            Messenger.Default.Register<string>(this, "RCLTLWHWindowShow", TLWHWinShow);
            Messenger.Default.Register<string>(this, "ERKeWindowShow", ERKEWinShow);
            Messenger.Default.Register<string>(this, "MPSQueryWindowShow", MPSQueryWinShow);
            Messenger.Default.Register<string>(this, "TLQueryWindowShow", TLQueryWinShow);
            this.Unloaded += (sender, e) => { Messenger.Default.Unregister(this); };
        }

        private void TLQueryWinShow(string obj)
        { 
            TLQueryWindow win = TLQueryWindow.Instance;
            win.Owner = this;
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.Show();
            win.Activate();
        }

        private void MPSQueryWinShow(string obj)
        { 
            MPSQueryWindow win = MPSQueryWindow.Instance;
            win.Owner = this;
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.Show();
            win.Activate();
        }

        private void ERKEWinShow(string obj)
        { 
            ERKeWindow win = ERKeWindow.Instance;
            win.Owner = this;
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.Show();
            win.Activate();
        }

        private void TLWHWinShow(string obj)
        {

            RCLTLWHWindow win = RCLTLWHWindow.Instance;
            win.Owner = this;
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.Show();
            win.Activate();
        }

        private void MpsTLWinShow(string obj)
        { 
            MPSTLWindow win =  MPSTLWindow.Instance;
            win.Owner = this;
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.Show();
            win.Activate();
        }

        private void MpsYPWinShow(string obj)
        { 
            MPSYPFeedingWindow win =  MPSYPFeedingWindow.Instanc;
            win.Owner = this;
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.Show();
            win.Activate();
        }

        private void MpsSelecteWinShow(string obj)
        {
            MpsSelecteWindow win = new MpsSelecteWindow();
            win.Owner = this;
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.Show();
        }

         
    }
}
