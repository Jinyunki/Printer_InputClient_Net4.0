using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;
using Printer_InputClient_Net4._0.Model;

namespace Printer_InputClient_Net4._0.ViewModel
{
    public class MainViewModel : MainModel
    {
        public MainViewModel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            WinBtnEvent();
            CurrentViewModel = _locator.PositionDataViewModel;

        }
       
    }
}