using GalaSoft.MvvmLight;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Input;
using System.Windows;
using System;
using PrintCommand;
using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;

namespace Printer_InputClient_Net4._0.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        public TPCLCommand tpclCommand = new TPCLCommand();
        public ReadExcelData readExcelData = new ReadExcelData();
        private ObservableCollection<ObservableCollection<string>> testItem = new ObservableCollection<ObservableCollection<string>>();
        public ObservableCollection<ObservableCollection<string>> TestItems
        {
            get { return testItem; }
            set { testItem = value;
                RaisePropertyChanged(nameof(TestItems));
            }
        }

        public string filePath = System.IO.Path.Combine(@"D:\0.DefaultFile\JinYunki\Printer_InputClient_Net4.0\Printer_InputClient_Net4.0\bin\Data", "PrintPointRecipie.xlsx");
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            WinBtnEvent();
            //BtnSend = new Command(BtnSendCommand);
            ExcelDataRecive();
            PrinterSendTest("TestItemFirstResult");
            SaveCommand = new Command(BtnSaveCommand);

        }
        public void ExcelDataRecive()
        {
            readExcelData.ReadExcelDataList(filePath, 1);
            testItem = readExcelData.excelTotalData;
            for (int i = 0; i < testItem.Count; i++)
            {
                PositionCategorise.Add(testItem[i][0]);
                PositionData.Add(testItem[i][1]);
            }
            WorkSheetName = readExcelData.wrokSheetName;

        }

        public void PrinterSendTest(string testText)
        {
            InputPrinterCommand = tpclCommand._MiddleLabelCommand(Double.Parse(PositionData[0]), Double.Parse(PositionData[1]),
                                  Double.Parse(PositionData[2]), Double.Parse(PositionData[3]), Double.Parse(PositionData[4]), Double.Parse(PositionData[5]), testText);
        }

        private string _inputPrinterCommand;
        public string InputPrinterCommand
        {
            get { return _inputPrinterCommand; }
            set {
                _inputPrinterCommand = value;
                RaisePropertyChanged(nameof(InputPrinterCommand));
            }
        }

        private string _workSheetName;
        public string WorkSheetName
        {
            get { return _workSheetName; }
            set {
                _workSheetName = value;
                RaisePropertyChanged(nameof(WorkSheetName));
            }
        }

        public ObservableCollection<string> ConvertObservableCollection(ObservableCollection<string> valueList)
        {
            ObservableCollection<string> observableCollection = new ObservableCollection<string>(valueList);

            return observableCollection;
        }
        private ObservableCollection<string> _positionCategorise = new ObservableCollection<string>();
        private ObservableCollection<string> _positionData = new ObservableCollection<string>();
        public ObservableCollection<string> PositionCategorise
        {
            get { return _positionCategorise; }
            set {
                _positionCategorise = value;
                RaisePropertyChanged(nameof(PositionCategorise));
            }
        }


        public ObservableCollection<string> PositionData
        {
            get { return _positionData; }
            set {
                _positionData = value;
                RaisePropertyChanged(nameof(PositionData));
            }
        }

        private void BtnSendCommand(object obj)
        {

            GetPrint(PrinterName);

        }
        private void BtnSaveCommand(object obj)
        {
            readExcelData.SaveExcelData(filePath, TestItems);
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

        
        private string _printerName = string.Empty;
        public string PrinterName
        {
            get { return _printerName; }
            set {
                _printerName = value;
                RaisePropertyChanged("PrinterName");
            }
        }



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

        public void WinBtnEvent()
        {
            BtnMinmize = new RelayCommand(WinMinmize);
            BtnMaxsize = new RelayCommand(WinMaxSize);
            BtnClose = new RelayCommand(WindowClose);
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