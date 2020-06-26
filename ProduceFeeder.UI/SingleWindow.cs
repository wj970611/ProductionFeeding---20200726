using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProduceFeeder.UI
{
  public  class SingleWindow:Window
    {
        public static readonly SingleWindow singleWindow = new SingleWindow();
        private SingleWindow():base()
        { 
        }

        

        

        
        protected override void OnClosing(CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            base.OnClosing(e);
        }
    }
}
