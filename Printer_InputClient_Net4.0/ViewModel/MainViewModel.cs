using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;
using Printer_InputClient_Net4._0.Model;
using System;
using System.Reflection;
using System.Diagnostics;

namespace Printer_InputClient_Net4._0.ViewModel
{
    public class MainViewModel : MainModel
    {
        public MainViewModel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            WinBtnEvent();
            CurrentViewModel = _locator.PositionDataViewModel;
            ProductDataModel.SignalFromSecondViewModelChanged += HandleSignalFromSecondViewModelChanged;
        }

        /// <summary>
        /// Signal을 받아서 화면을 전환 합니다
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleSignalFromSecondViewModelChanged(object sender, SignalEventArgs e)
        {
            Trace.WriteLine("==========   Start   ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\n");
            try
            {
                // 이벤트에서 시그널 값을 처리합니다.
                SignalNotRecipe = e.Signal;

                if (SignalNotRecipe)
                {
                    CurrentViewModel = _locator.PositionDataViewModel;
                }
                //else
                //{
                //    CurrentViewModel = _locator.AddRecipeViewModel;
                //}

            } catch (Exception ex)
            {
                Trace.WriteLine("========== Exception ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\nException : " + ex);
                throw;
            }

        }
    }
}