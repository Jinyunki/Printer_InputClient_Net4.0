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
        /// Signal�� �޾Ƽ� ȭ���� ��ȯ �մϴ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleSignalFromSecondViewModelChanged(object sender, SignalEventArgs e)
        {
            Trace.WriteLine("==========   Start   ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\n");
            try
            {
                // �̺�Ʈ���� �ñ׳� ���� ó���մϴ�.
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