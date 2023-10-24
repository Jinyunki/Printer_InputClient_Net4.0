

using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace Printer_InputClient_Net4._0.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<PositionDataViewModel>();
            
        }
        
        public PositionDataViewModel PositionDataViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PositionDataViewModel>();
            }
        }
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}