using GalaSoft.MvvmLight.Messaging;
using ProduceFeeder.UI.Models;
using ProduceFeeder.UI.Models.YuPai;
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
using System.Windows.Shapes;

namespace ProduceFeeder.UI
{
    /// <summary>
    /// Window3.xaml 的交互逻辑
    /// </summary>
    public partial class MPSYPFeedingWindow : Window
    {
        public static readonly MPSYPFeedingWindow Instanc = new MPSYPFeedingWindow();
        private MPSYPFeedingWindow()
        {
            InitializeComponent();

            Messenger.Default.Register<NotificationMessageAction<MessageBoxResult>>(this, (msg) =>
            {
                var ret = MessageBox.Show("您即将删除选择的物料，是否继续？","删除",  MessageBoxButton.OKCancel,MessageBoxImage.Warning);
                //msg.Notification内容是“主页面测试”
                if (msg.Notification != null)
                {
                    msg.Execute(ret);
                }
            }
           );
             
            Messenger.Default.Register<MPSPlanItem>(this, "SelectedYPToTLView", ShowTLWindow);
            this.Unloaded += (sender, e) => { Messenger.Default.Unregister(this); };

            this.Closing += MPSYPFeedingWindow_Closing;
        }

        private void MPSYPFeedingWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void ShowTLWindow(MPSPlanItem obj)
        {
          
            YPToScheduleWindow _win = YPToScheduleWindow.Instance;
            ((YPToScheduleViewModel)_win.DataContext).YpItemImport = obj.ToTLItem();
            _win.Show();
        }
    }
}
