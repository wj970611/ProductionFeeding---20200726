/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:ProduceFeeder.UI"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace ProduceFeeder.UI.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MpsSelecteViewModel>();
            SimpleIoc.Default.Register<MPSTLViewModel>();
            SimpleIoc.Default.Register<MPSYPFeedingViewModel>();
            SimpleIoc.Default.Register<YPToScheduleViewModel>();
            SimpleIoc.Default.Register<CPComponetInventoryViewModel>();
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<RCLTLWHViewModel>();
            SimpleIoc.Default.Register<ERKeViewModel>();
            SimpleIoc.Default.Register<MPSMainUserControlViewModel>();
        }

        public MPSMainUserControlViewModel MPSMainUserControl 
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MPSMainUserControlViewModel>();
            }
        }
        public ERKeViewModel ERKe
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ERKeViewModel>();
            }
        }
        public RCLTLWHViewModel RCLTLWH
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RCLTLWHViewModel>();
            }
        }
        public MainWindowViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            }
        }
        public MpsSelecteViewModel MpsSelecteWindow
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MpsSelecteViewModel>();
            }
        }
        public MPSTLViewModel MPSTLItemWindow
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MPSTLViewModel>();
            }
        }

        public MPSYPFeedingViewModel MPSYPFeedingWindow
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MPSYPFeedingViewModel>();
            }
        }
        public YPToScheduleViewModel YPToSchedule
        {
            get
            {
                return ServiceLocator.Current.GetInstance<YPToScheduleViewModel>();
            }
        }
        public CPComponetInventoryViewModel CPComponetInventoryWindow
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CPComponetInventoryViewModel>();
            }
        }
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}