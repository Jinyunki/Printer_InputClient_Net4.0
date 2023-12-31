﻿using ExcelCommand;
using GalaSoft.MvvmLight;
using OfficeOpenXml;
using PrintCommand;
using Printer_InputClient_Net4._0.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Reflection;
using System.Text;
using System.Windows.Input;

namespace Printer_InputClient_Net4._0.Model
{
    public class LabelModel : ViewModelBase
    {
        public TPCLCommand tpclCommand = new TPCLCommand();
        public ReadExcelData readExcelData = new ReadExcelData();
        public ReadRecive ReadDataRecive = new ReadRecive();
        public ICommand TestPrint { get; set; }
        public ICommand BtnPortConnectCommand { get; set; }
        public ICommand BtnAddSaveCommand { get; set; }
        public ICommand BtnCancelCommand { get; set; }
        public ICommand EnterCommand { get; set; }
        public ViewModelLocator _locator = new ViewModelLocator();
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get {
                return _currentViewModel;
            }
            set {
                _currentViewModel = value;
                RaisePropertyChanged("CurrentViewModel");
            }
        }

        #region Serial I/O
        public delegate void SerialDataReceivedDelegate(object sender, SerialDataReceivedEventArgs e);
        private SerialPort serialPort;
        

        // 포트 연결 상태
        private string resultConnect = "포트 연결을 눌러 주세요";
        public string ResultConnect
        {
            get { return resultConnect; }
            set {
                resultConnect = value;
                RaisePropertyChanged("ResultConnect");
            }
        }
        /// <summary>
        /// PortNumber = 연결할 스캐너의 포트번호
        /// dataReceivedHandler = 기능 바인딩된 핸들러
        /// </summary>
        /// <param name="portNumber"></param>
        /// <param name="dataReceivedHandler"></param>
        public void OpenSerialPort(string portNumber, SerialDataReceivedDelegate dataReceivedHandler)
        {
            Trace.WriteLine("==========   Start   ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\n");
            try
            {
                if (serialPort != null && serialPort.IsOpen)
                {
                    serialPort.Close();
                    serialPort.Dispose();
                }
                serialPort = new SerialPort
                {
                    PortName = portNumber,
                    BaudRate = 9600,
                    DataBits = 8,
                    StopBits = StopBits.One,
                    Parity = Parity.None
                };

                serialPort.DataReceived += new SerialDataReceivedEventHandler(dataReceivedHandler);

                serialPort.Open();
                ResultConnect = "PORT ON";
            } catch (UnauthorizedAccessException ex)
            {
                ResultConnect = "액세스 거부: " + ex.Message;
                Trace.WriteLine("========== Exception ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\nException : " + ex);
                // 포트 액세스 거부 예외 처리
                // 포트를 닫고 다시 열어보세요.
                serialPort?.Close();
                serialPort?.Dispose();
                //OpenSerialPort(portNumber, dataReceivedHandler); // 재귀적으로 메서드 호출
            } catch (Exception ex)
            {
                ResultConnect = "연결 오류: " + ex.Message;
                Trace.WriteLine("========== Exception ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\nException : " + ex);
            }
        }



        #endregion


        #region ExcelReadTest

        public ObservableCollection<object> productList = new ObservableCollection<object>();
        public ObservableCollection<object> recipeList = new ObservableCollection<object>();

        public Dictionary<string, double> keyValuePositionX = new Dictionary<string, double>();
        public Dictionary<string, double> keyValuePositionY = new Dictionary<string, double>();

        public List<string> WorkSheetNameList = new List<string>();
        public bool isProductNumberFound = false;
        
        
        /// <summary>
        /// 모델 데이터를 읽어옵니다
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ObservableCollection<object> GetReadModelRecipe(string path)
        {
            Trace.WriteLine("==========   Start   ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\n");
            try
            {
                productList.Clear();
                string wrokSheetName;
                using (var package = new ExcelPackage(new FileInfo(path)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // 시트 선택

                    ExcelWorksheets excelWorksheets = package.Workbook.Worksheets;
                    WorkSheetNameList.Clear();
                    for (int i = 0; i < excelWorksheets.Count; i++)
                    {
                        WorkSheetNameList.Add(excelWorksheets[i].Name);
                    }
                    wrokSheetName = worksheet.Name;
                    int colCount = worksheet.Dimension.Columns; // 가로줄의 개수
                    int rowCount = worksheet.Dimension.Rows; // 세로줄의 개수

                    for (int row = 1; row <= rowCount; row++)
                    {
                        ObservableCollection<string> columnData = new ObservableCollection<string>();
                        for (int col = 1; col <= colCount; col++) // 열 제목도 데이터로 포함시키기 위해 1부터 시작
                        {
                            string cellValue = worksheet.Cells[row, col].Text;
                            columnData.Add(cellValue);
                        }

                        productList.Add(new ProductDataModel
                        {
                            ModelName = columnData[0],
                            ProductNumber = columnData[1],
                            ProductName = columnData[2],
                            LotCount = columnData[3],
                            Ground = columnData[4],
                            Delivery = columnData[5],
                            Company = columnData[6],
                            Factory = columnData[7],
                            LabelType = columnData[8],
                            Today = columnData[9],
                            SerialNumber = columnData[10],
                            PrintCount = columnData[11]
                        });
                    }
                    package.Dispose();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                return productList;
            } catch (Exception ex)
            {
                Trace.WriteLine("========== Exception ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\nException : " + ex);
                throw;
            }

        }

        /// <summary>
        /// 레시피 데이터를 읽어옵니다
        /// </summary>
        /// <param name="path"></param>
        /// <param name="labelType"></param>
        /// <returns></returns>
        public ObservableCollection<object> GetReadLabelRecipe(string path,string labelType)
        {

            Trace.WriteLine("==========   Start   ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\n");
            try
            {

                int intLabelType = 0;
                recipeList.Clear();
                switch (labelType)
                {
                    case "S":
                        intLabelType = 1;
                        break;
                    case "M":
                        intLabelType = 2;
                        break;
                    case "L":
                        intLabelType = 3;
                        break;
                    default:
                        Console.WriteLine("레시피의 라벨 데이터를 확인하세요");
                        break;
                }
                string wrokSheetName;
                using (var package = new ExcelPackage(new FileInfo(path)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[intLabelType]; // 시트 선택

                    ExcelWorksheets excelWorksheets = package.Workbook.Worksheets;
                    WorkSheetNameList.Clear();
                    for (int i = 0; i < excelWorksheets.Count; i++)
                    {
                        WorkSheetNameList.Add(excelWorksheets[i].Name);
                    }

                    wrokSheetName = worksheet.Name;
                    int colCount = worksheet.Dimension.Columns; // 가로줄의 개수
                    int rowCount = worksheet.Dimension.Rows; // 세로줄의 개수

                    for (int row = 1; row <= rowCount; row++)
                    {
                        ObservableCollection<string> columnData = new ObservableCollection<string>();
                        for (int col = 1; col <= colCount; col++) // 열 제목도 데이터로 포함시키기 위해 1부터 시작
                        {
                            string cellValue = worksheet.Cells[row, col].Text;
                            columnData.Add(cellValue);
                        }

                        recipeList.Add(new PositionDataModel
                        {
                            Category = columnData[0],
                            XPosition = columnData[1],
                            YPosition = columnData[2]
                        });
                    }
                    package.Dispose();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                return recipeList;
            } catch (Exception ex)
            {
                Trace.WriteLine("========== Exception ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\nException : " + ex);
                throw;
            }

        }

        #endregion

        #region DataList  
        private string _preViewFontSize = "20";
        public string PreViewFontSize
        {
            get { return _preViewFontSize; }
            set {
                _preViewFontSize = value;
                RaisePropertyChanged("PreViewFontSize");
            }
        }

        private string _dataListViewFontSize = "12";
        public string DataListViewFontSize
        {
            get { return _dataListViewFontSize; }
            set {
                _dataListViewFontSize = value;
                RaisePropertyChanged("DataListViewFontSize");
            }
        }

        private string _FileName = "DataList.xlsx";
        public string FileName
        {
            get { return readExcelData.GetRecipeFile(_FileName); }
            set {
                _FileName = readExcelData.GetRecipeFile(value);
                RaisePropertyChanged("FileName");
            }
        }
        private string _excelDataCount = "0";
        public string ExcelDataCount
        {
            get { return _excelDataCount; }
            set {
                _excelDataCount = value;
                RaisePropertyChanged("ExcelDataCount");
            }
        }
        private string _formatDate = $"{DateTime.Now:yy}{(char)('A' + DateTime.Now.Month - 1)}{DateTime.Now:dd}";
        public string FormatDate
        {
            get { return _formatDate; }
            set {
                _formatDate = value;
                RaisePropertyChanged("FormatDate");
            }
        }

        public ICommand BtnPrintCommand { get; set; }
        public ICommand BtnInkPlusCommand { get; set; }
        public ICommand BtnInkMinusCommand { get; set; }
        public ICommand BtnInkReturnCommand { get; set; }


        #endregion


        #region PositionData

        private string _printerName = string.Empty;
        public string PrinterName
        {
            get { return _printerName; }
            set {
                _printerName = value;
                RaisePropertyChanged("PrinterName");
            }
        }
        private string _inputPrinterCommand;
        public string InputPrinterCommand
        {
            get { return _inputPrinterCommand; }
            set {
                _inputPrinterCommand = value;
                RaisePropertyChanged("InputPrinterCommand");
            }
        }

        private string _inputDataValue;
        public string InputDataValue
        {
            get { return _inputDataValue; }
            set {
                _inputDataValue = value;
                RaisePropertyChanged("InputDataValue");
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

        private string _inkLevel = "+02";
        public string InkLevel
        {
            get { return _inkLevel; }
            set {
                _inkLevel = value;
                RaisePropertyChanged(nameof(InkLevel));
            }
        }

        public static string FONT_SMALL = "SmallFontSize";
        public static string FONT_MEDIUM = "MediumFontSize";
        public static string FONT_LARGE = "LargeFontSize";

        #endregion

        #region defaultListUpdate

        public string SetSizeAndPrintDensity(string labelSize, string printArea)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(tpclCommand._SetLabelSize(keyValuePositionX[labelSize], keyValuePositionY[labelSize], keyValuePositionX[printArea], keyValuePositionY[printArea])); // 라벨사이즈 지정

            return builder.ToString();
        }
        public string SetPrintDataTrueFont(double groupNum, string groupPositionName, string fontSize,string inputData)
        {
            
            StringBuilder builder = new StringBuilder();
            builder.Append(tpclCommand._SetTrueFont(groupNum, (keyValuePositionY[groupPositionName]), (keyValuePositionX[groupPositionName]), keyValuePositionY[fontSize], keyValuePositionX[fontSize], "B", 270, "B")); // 폰트셋팅
            builder.Append(tpclCommand._SetTrueValueInput(groupNum, inputData)); // 폰트 데이터 인풋

            return builder.ToString();
        }
        

        public string SetBarcode(double groupNum, string groupPositionName, string barcodeData)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(tpclCommand._SetBarcode(groupNum, (keyValuePositionY[groupPositionName]), (keyValuePositionX[groupPositionName]), "9",1,3,270,60));
            builder.Append(tpclCommand._SetBarcodeValueInput(groupNum, barcodeData));

            return builder.ToString();
        }

        public string SetPrintDataTrueFontBelow(double groupNum, string groupPositionName, string fontSize, string inputData)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(tpclCommand._SetTrueFont(groupNum, (keyValuePositionY[groupPositionName])+800, (keyValuePositionX[groupPositionName]), keyValuePositionY[fontSize], keyValuePositionY[fontSize], "B", 270, "B")); // 폰트셋팅
            builder.Append(tpclCommand._SetTrueValueInput(groupNum, inputData)); // 폰트 데이터 인풋

            return builder.ToString();
        }


        public string SetBarcodeBelow(double groupNum, string groupPositionName, string barcodeData)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(tpclCommand._SetBarcode(groupNum, (keyValuePositionY[groupPositionName])+800, (keyValuePositionX[groupPositionName]), "9", 1, 3, 270, 60));
            builder.Append(tpclCommand._SetBarcodeValueInput(groupNum, barcodeData));

            return builder.ToString();
        }


        public string ConvertOutput(int printCount)
        {
            Trace.WriteLine("==========   Start   ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\n");
            try
            {

                string output;
                if (printCount > 0 && printCount <= 10) // 1~10
                {
                    output = $"{printCount - 1}";
                } else // 11 이상
                {
                    if (printCount < 37) // 11~36
                    {
                        char customChar = (char)('A' + (printCount - 11));
                        output = $"{customChar}";
                    } else // 37 이상
                    {
                        int remainder = printCount % 36;
                        if (remainder > 0 && remainder <= 10)
                        {
                            output = $"{remainder - 1}";
                        } else
                        {
                            char remainderChar;
                            if (remainder == 0)
                            {
                                remainderChar = 'Z';
                            } else
                            {
                                remainderChar = (char)('A' + remainder - 11);
                            }
                            output = $"{remainderChar}";
                        }
                    }
                }

                return output;
            } catch (Exception ex)
            {
                Trace.WriteLine("========== Exception ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\nException : " + ex);
                throw;
            }

        }
        /// <summary>
        /// 36진법 Converter
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ConvertToBase36(int value)
        {
            const string base36Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string result = string.Empty;

            while (value > 0)
            {
                int remainder = value % 36;
                result = base36Chars[remainder] + result;
                value /= 36;
            }
            
            return "00" + result.PadLeft(2, '0');
        }
        #endregion
    }
}
