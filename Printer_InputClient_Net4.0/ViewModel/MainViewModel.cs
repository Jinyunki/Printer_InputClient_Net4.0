using System.Diagnostics;
using System.Reflection;
using System.Windows.Input;
using System.Windows;
using System;
using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using System.Text;
using Printer_InputClient_Net4._0.Model;
using CaptureCommand;
using System.Collections.Generic;

namespace Printer_InputClient_Net4._0.ViewModel
{
    public class MainViewModel : LabelPositionModel
    {
        public MainViewModel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            WinBtnEvent();
            //BtnSend = new Command(BtnSendCommand);
            ReadExcelDataRecive(1);
            PrinterSendTest("TestItemFirstResult");
            //SaveCommand = new Command(BtnSaveCommand);

        }
        /// <summary>
        /// 라이브러리를 통해 호출된 엑셀데이터를 받아온다
        /// </summary>
        /// <param name="selectSheet">엑셀 파일의 </param>
        public void ReadExcelDataRecive(int selectSheet)
        {
            readExcelData.ReadExcelDataList(FILEPATH, selectSheet);
            
            for (int i = 0; i < ExcelTotalData.Count; i++)
            {
                PositionCategorise.Add(ExcelTotalData[i][0]);
                PositionData.Add(ExcelTotalData[i][1]);
            }
            WorkSheetName = readExcelData.wrokSheetName;

        }
        public void PrinterSendTest22(string testText)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(tpclCommand._SetClearImageBuffer()); //클리어
            builder.Append(tpclCommand._SetLabelSize(Double.Parse(LabelSizeY), Double.Parse(LabelSizeX), Double.Parse(PrintY), Double.Parse(PrintX))); // 라벨사이즈 지정
        }


        public void PrinterSendTest(string testText)
        {
            InputPrinterCommand = tpclCommand._MiddleLabelCommand(Double.Parse(PositionData[0]), Double.Parse(PositionData[1]),
                                  Double.Parse(PositionData[2]), Double.Parse(PositionData[3]), Double.Parse(PositionData[4]), Double.Parse(PositionData[5]), testText);
        }


        public ObservableCollection<string> ConvertObservableCollection(List<string> valueList)
        {
            ObservableCollection<string> observableCollection = new ObservableCollection<string>(valueList);

            return observableCollection;
        }


        private void BtnSendCommand(object obj)
        {

            GetPrint(PrinterName);

        }
        private void BtnSaveCommand(object obj)
        {
            readExcelData.SaveExcelData(FILEPATH, ExcelTotalData);
        }

        private void GetPrint(string printerName) {

            Trace.WriteLine("Start::::::::::::" + (MethodBase.GetCurrentMethod().Name));
            try {
                RawPrinterHelper.SendStringToPrinter(printerName, InputPrinterCommand);
                MessageBox.Show("연결 성공");
                Console.WriteLine("THIS IS SUCCESS!!!!");
            } catch (Exception e) {
                Trace.WriteLine("Catch::::::::::" + (MethodBase.GetCurrentMethod().Name) + e);
                MessageBox.Show("연결 실패");
                Console.WriteLine($"HOLY FUCK FUCK FUCK : {e.Message}");
            }
        }


        public ICommand BtnSend { get; set; }
        public ICommand SaveCommand { get; set; }
        

        #region Window State

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
        public ICommand BtnCapture { get; private set; }

        public void WinBtnEvent()
        {
            BtnMinmize = new RelayCommand(WinMinmize);
            BtnMaxsize = new RelayCommand(WinMaxSize);
            BtnClose = new RelayCommand(WindowClose);
            BtnCapture = new Command(ViewCaptureCommand);
        }

        private void ViewCaptureCommand(object obj)
        {
            ViewCapture.Capture(obj,"CaptureFiel");
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
    }
}