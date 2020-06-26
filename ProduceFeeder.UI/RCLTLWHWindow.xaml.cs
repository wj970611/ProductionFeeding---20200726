using DevExpress.Mvvm;
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
    /// RCLTLWHWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RCLTLWHWindow : Window
    {
        public static readonly RCLTLWHWindow Instance = new RCLTLWHWindow();
        private RCLTLWHWindow()
        {
            InitializeComponent();

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<NotificationMessageAction<MessageBoxResult>>(this, (msg) =>
            {
                var ret = MessageBox.Show("您即将删除选择的物料，是否继续？", "删除", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                //msg.Notification内容是“主页面测试”
                if (msg.Notification != null)
                {
                    msg.Execute(ret);
                }
            }
           );
            this.Closing += RCLTLWHWindow_Closing;
        }

        private void RCLTLWHWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
