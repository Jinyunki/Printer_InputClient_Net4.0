using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Printer_InputClient_Net4._0.Model
{
    public class LabelModel : MainModel
    {

        public ICommand BtnSend { get; set; }
        public ICommand TestPrint { get; set; }

        #region ExcelReadTest
        

        public ObservableCollection<ObservableCollection<string>> TestExcelData = new ObservableCollection<ObservableCollection<string>>();
        public Dictionary<string, string> keyValuePairsX = new Dictionary<string, string>();
        public Dictionary<string, string> keyValuePairsY = new Dictionary<string, string>();

        public List<string> WorkSheetNameList = new List<string>();



        public ObservableCollection<ObservableCollection<string>> CallingBackData(string path, int selectedSheet)
        {
            string wrokSheetName;
            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                
                ExcelWorksheet worksheet = package.Workbook.Worksheets[selectedSheet]; // 시트 선택

                ExcelWorksheets excelWorksheets = package.Workbook.Worksheets;
                WorkSheetNameList.Clear();
                for (int i = 0; i < excelWorksheets.Count; i++)
                {
                    WorkSheetNameList.Add(excelWorksheets[i].Name);
                }

                wrokSheetName = worksheet.Name;
                int colCount = worksheet.Dimension.Columns; // 가로줄의 개수
                int rowCount = worksheet.Dimension.Rows; // 세로줄의 개수
                
                for (int col = 1; col <= colCount; col++)
                {
                    ObservableCollection<string> columnData = new ObservableCollection<string>();

                    for (int row = 1; row <= rowCount; row++) // 열 제목도 데이터로 포함시키기 위해 1부터 시작
                    {
                        string cellValue = worksheet.Cells[row, col].Text;
                        columnData.Add(cellValue);
                    }

                    TestExcelData.Add(columnData);
                }

                for (int i = 1; i < TestExcelData[0].Count; i++)
                {
                    string key = TestExcelData[0][i];
                    string valueX = TestExcelData[1][i];
                    string valueY = TestExcelData[2][i];
                    keyValuePairsX[key] = valueX;
                    keyValuePairsY[key] = valueY;
                }
            }
            return TestExcelData;
        }
        
        #endregion

        #region DataList        

        private string _FileName;
        public string FileName
        {
            get { return readExcelData.GetRecipeFile(_FileName); }
            set {
                _FileName = readExcelData.GetRecipeFile(value);
                RaisePropertyChanged("FilePath");
            }
        }
        private string _modelFileName;
        public string ModelFileName
        {
            get { return readExcelData.GetRecipeFile(_modelFileName); }
            set {
                _modelFileName = readExcelData.GetRecipeFile(value);
                RaisePropertyChanged("ModelFileName");
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
        private string _modelName = "modelName";
        public string ModelName
        {
            get { return _modelName; }
            set {
                _modelName = value;
                RaisePropertyChanged("ModelName");
            }
        }
        private string _barcodeData ;
        public string BarcodeData
        {
            get { return _barcodeData; }
            set {
                _barcodeData = value;
                RaisePropertyChanged("BarcodeData");
            }
        }
        private string _company = "HyundaiMobis Co.,Ltd";
        public string Company
        {
            get { return _company; }
            set {
                _company = value;
                RaisePropertyChanged("Company");
            }
        }
        private string _printCount = "10";
        public string PrintCount
        {
            get { return _printCount; }
            set {
                _printCount = value;
                RaisePropertyChanged("PrintCount");
            }
        }

        private string _productNumber = "99240-AA010";
        public string ProductNumber
        {
            get { return _productNumber; }
            set {
                _productNumber = value;
                RaisePropertyChanged("ProductNumber");
            }
        }

        private string _productName = "UNIT ASSY-RR VIEW CAMERA";
        public string ProductName
        {
            get { return _productName; }
            set {
                _productName = value;
                RaisePropertyChanged("ProductName");
            }
        }

        private string _lotCount = "24";
        public string LotCount
        {
            get { return _lotCount; }
            set {
                _lotCount = value;
                RaisePropertyChanged("LotCount");
            }
        }

        private string _aground = "Korea";
        public string Aground
        {
            get { return _aground; }
            set {
                _aground = value;
                RaisePropertyChanged("Aground");
            }
        }

        private string _delivery = "R7A8";
        public string Delivery
        {
            get { return _delivery; }
            set {
                _delivery = value;
                RaisePropertyChanged("Delivery");
            }
        }

        private string _codeName;
        public string CodeName
        {
            get { return _codeName; }
            set {
                _codeName = value;
                RaisePropertyChanged("CodeName");
            }
        }

        private string _issueNumber;
        public string IssueNumber
        {
            get { return _issueNumber; }
            set {
                _issueNumber = value;
                RaisePropertyChanged("IssueNumber");
            }
        }

        private string _labelType;
        public string LabelType
        {
            get { return _labelType; }
            set {
                _labelType = value;
                RaisePropertyChanged("LabelType");
            }
        }
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

        private string _factory = "GV";
        public string Factory
        {
            get { return _factory; }
            set {
                _factory = value;
                RaisePropertyChanged("Factory");
            }
        }
        

        #endregion

        #region defaultListUpdate
        public string SetPrintDataTrueFont(double groupNum, string groupPositionName, double fontSize,string inputData)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(tpclCommand._SetTrueFont(groupNum, double.Parse(keyValuePairsY[groupPositionName]), double.Parse(keyValuePairsX[groupPositionName]), fontSize, fontSize, "B", 270, "B")); // 폰트셋팅
            builder.Append(tpclCommand._SetTrueValueInput(groupNum, inputData)); // 폰트 데이터 인풋

            return builder.ToString();
        }
        

        public string SetBarcode(double groupNum, string groupPositionName, string countInput)
        {
            string barcodeProductNumber = ProductNumber.Replace("-", "");
            BarcodeData = barcodeProductNumber + "  " + LotCount + FormatDate + GenerateOutput(int.Parse(countInput));
            StringBuilder builder = new StringBuilder();
            builder.Append(tpclCommand._SetBarcode(groupNum, double.Parse(keyValuePairsY[groupPositionName]), double.Parse(keyValuePairsX[groupPositionName]), "9",1,3,270,60));
            builder.Append(tpclCommand._SetBarcodeValueInput(groupNum, BarcodeData));

            return builder.ToString();
        }

        public string SetPrintDataTrueFontBelow(double groupNum, string groupPositionName, double fontSize, string inputData)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(tpclCommand._SetTrueFont(groupNum, double.Parse(keyValuePairsY[groupPositionName])+800, double.Parse(keyValuePairsX[groupPositionName]), fontSize, fontSize, "B", 270, "B")); // 폰트셋팅
            builder.Append(tpclCommand._SetTrueValueInput(groupNum, inputData)); // 폰트 데이터 인풋

            return builder.ToString();
        }


        public string SetBarcodeBelow(double groupNum, string groupPositionName, string countInput)
        {
            string barcodeProductNumber = ProductNumber.Replace("-", "");
            BarcodeData = barcodeProductNumber + "  " + LotCount + FormatDate + GenerateOutput(int.Parse(countInput));
            StringBuilder builder = new StringBuilder();
            builder.Append(tpclCommand._SetBarcode(groupNum, double.Parse(keyValuePairsY[groupPositionName])+800, double.Parse(keyValuePairsX[groupPositionName]), "9", 1, 3, 270, 60));
            builder.Append(tpclCommand._SetBarcodeValueInput(groupNum, BarcodeData));

            return builder.ToString();
        }

        public string ConvertOutput(int printCount)
        {
            string output;
            if (printCount > 0 && printCount <= 10) // 1~10
            {
                output = $"{printCount-1}";
            } 
            else // 11 이상
            {
                if (printCount < 37) // 11~36
                {
                    char customChar = (char)('A' + (printCount - 11));
                    output = $"{customChar}";
                } 
                else // 37 이상
                {
                    int remainder = printCount % 36;
                    if (remainder > 0 && remainder <= 10)
                    {
                        output = $"{remainder-1}";
                    }
                    
                    else
                    {
                        char remainderChar;
                        if (remainder == 0)
                        {
                            remainderChar = 'Z';
                        }
                        
                        else
                        {
                            remainderChar = (char)('A' + remainder - 11);
                        }
                        output = $"{remainderChar}";
                    }
                }
            }

            return output;
        }
        public string GenerateOutput(int printCount)
        {
            string returnValue = "범위 초과";
            double temp = printCount / 36.01;
            int a = (int)temp; // temp의 정수 값 (0~35.999)
            double fractionalPart = temp - a; // 정수값을 뺸 소수점 값
            int b = (int)fractionalPart * 10; // 소수점 0번째 값
            string convertA = "";
            // ( a = 0 )
            if (a < 1)
            {
                returnValue = ConvertOutput(printCount); // 반환 할 값
                return "00" + a + returnValue;
            }

            // ( 1 =< a < 36 )
            else if (a >= 1 && a < 36) 
            {
                if (a > 9) // a, 즉 두번째 자리의 정수치가 10이 넘을때
                {
                    convertA = ConvertOutput(a + 1);
                    for (int i = 1; i <= 36; i++) // 이곳에서의 36은 한사이클 36의 제곱을 의미.
                    {
                        if (i < temp && temp <= i + 1)
                        {
                            returnValue = ConvertOutput(printCount); // 반환 할 값
                            return "00" + convertA + returnValue;
                        }
                    }
                } 
                else
                {
                    for (int i = 1; i <= 36; i++) // 이곳에서의 36은 한사이클 36의 제곱을 의미.
                    {
                        if (i < temp && temp <= i + 1)
                        {
                            returnValue = ConvertOutput(printCount); // 반환 할 값
                            return "00" + a + returnValue;
                        }
                    }
                }
                
            }

            // a > 36
            else
            {
                returnValue = "출력 범위 초과";
                return returnValue;
            }

            return returnValue;
  
        }
        #endregion
    }
}
