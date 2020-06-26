using GalaSoft.MvvmLight.Messaging;
using ProduceFeeder.UI.Models;
using ProduceFeeder.UI.Models.YuPai;
using ProduceFeeder.UI.Repository;
using ProduceFeeder.UI.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProduceFeeder.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MpsSelecteWindow : Window
    {
        public MpsSelecteWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<string>(this, "ShowYPWindow", ShowYPWindow);  
            Messenger.Default.Register<MPSPlanItem>(this, "SelectedYPToTLView", ShowTLWindow);
            this.Unloaded += (sender, e) => { Messenger.Default.Unregister(this); };
        }

        private void ShowYPWindow(string obj)
        {
            MPSYPFeedingWindow win = MPSYPFeedingWindow.Instanc;
            win.Show();
            win.Activate();
        }
        private void ShowTLWindow(MPSPlanItem obj)
        {
            YPToScheduleWindow _win =  YPToScheduleWindow.Instance;
            ((YPToScheduleViewModel)_win.DataContext).YpItemImport = obj.ToTLItem();
            _win.Show();
            _win.Activate();
        }
    }
}
