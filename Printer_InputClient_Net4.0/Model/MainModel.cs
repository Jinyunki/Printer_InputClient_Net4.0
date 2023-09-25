using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Printer_InputClient_Net4._0.ViewModel;
using System;
using System.Windows;
using System.Windows.Input;

namespace Printer_InputClient_Net4._0.Model
{
    public class MainModel : ViewModelBase
    {

        #region Window State
        public void WinBtnEvent()
        {
            BtnMinmize = new RelayCommand(WinMinmize);
            BtnMaxsize = new RelayCommand(WinMaxSize);
            BtnClose = new RelayCommand(WindowClose);
            BtnCapture = new Command(ViewCaptureCommand);
            btMainHome = new Command(HomeCommand);
        }

        private void HomeCommand(object obj)
        {
            CurrentViewModel = _locator.PositionDataViewModel;
        }

        private WindowState _windowState;
        public WindowState WindowState
        {
            get { return _windowState; }
            set {
                if (_windowState != value)
                {
                    _windowState = value;
                    RaisePropertyChanged("WindowState");
                }
            }
        }
        public ICommand BtnMinmize { get; private set; }
        public ICommand BtnMaxsize { get; private set; }
        public ICommand BtnClose { get; private set; }
        public ICommand BtnCapture { get; set; }
        public Command btMainHome { get; set; }

        

        private void ViewCaptureCommand(object obj)
        {
            ViewCapture.Capture(obj, "CaptureFile");
        }

        // Window Minimize
        private void WinMinmize()
        {
            WindowState = WindowState.Minimized;
        }

        // Window Size
        private void WinMaxSize()
        {
            WindowState = (WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }
        private void WindowClose()
        {
            Application.Current.Shutdown();
        }

        #endregion

        public ViewModelLocator _locator = new ViewModelLocator();
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get {
                return _currentViewModel;
            }
            set {
                _currentViewModel = value;
                _currentViewModel.RaisePropertyChanged("CurrentViewModel");
            }
        }
        private ViewModelBase _sideTab;
        public ViewModelBase SideTab
        {
            get {
                return _sideTab;
            }
            set {
                if (_sideTab != value)
                {
                    _sideTab = value;
                    _sideTab.RaisePropertyChanged("SideTab");
                }
            }
        }
    }
}
