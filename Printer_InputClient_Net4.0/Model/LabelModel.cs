using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Printer_InputClient_Net4._0.Model
{
    public class LabelModel : MainModel
    {

        public ICommand BtnSend { get; set; }
        public ICommand TestPrint { get; set; }



        #region DataList

        /// <summary>
        /// 라이브러리를 통해 호출된 엑셀데이터를 받아온다
        /// </summary>
        /// <param name="selectSheet">엑셀 파일의 </param>
        public void ReadExcelDataRecive(int selectSheet)
        {
            FileName = "PrintPointRecipie.xlsx";
            readExcelData.ReadExcelDataList(FileName, selectSheet);

            for (int i = 0; i < ExcelTotalData.Count; i++)
            {
                PositionCategorise.Add(ExcelTotalData[i][0]);
                PositionData.Add(ExcelTotalData[i][1]);
            }
            WorkSheetName = readExcelData.wrokSheetName;
        }


        private string _fileName;
        public string FileName
        {
            get { return readExcelData.GetRecipeFile(_fileName); }
            set {
                _fileName = readExcelData.GetRecipeFile(value);
                RaisePropertyChanged("FilePath");
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
        private string _modelName;
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
        private string _printCount = "24";
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

        private string _lotCount;
        public string LotCount
        {
            get { return _lotCount; }
            set {
                _lotCount = value;
                RaisePropertyChanged("LotCount");
            }
        }

        private string _aground;
        public string Aground
        {
            get { return _aground; }
            set {
                _aground = value;
                RaisePropertyChanged("Aground");
            }
        }

        private string _delivery;
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
        //private ObservableCollection<ObservableCollection<string>> _excelTotalData = new ObservableCollection<ObservableCollection<string>>();
        public ObservableCollection<ObservableCollection<string>> ExcelTotalData
        {
            get { return readExcelData.excelTotalData; }
            set {
                readExcelData.excelTotalData = value;
                RaisePropertyChanged(nameof(ExcelTotalData));
            }
        }
        private ObservableCollection<string> _positionCategorise = new ObservableCollection<string>();
        public ObservableCollection<string> PositionCategorise
        {
            get { return _positionCategorise; }
            set {
                _positionCategorise = value;
                RaisePropertyChanged(nameof(PositionCategorise));
            }
        }
        private ObservableCollection<string> _positionData = new ObservableCollection<string>();
        public ObservableCollection<string> PositionData
        {
            get { return _positionData; }
            set {
                _positionData = value;
                RaisePropertyChanged(nameof(PositionData));
            }
        }
        private string _labelSizeX = string.Empty;
        public string LabelSizeX
        {
            get {
                if (!string.IsNullOrEmpty(_labelSizeX))
                {
                    return _labelSizeX;
                } else
                {
                    // 변경된 값이 없으면 원래 데이터를 반환
                    return ExcelTotalData[0][1];
                }
            }
            set {
                if (_labelSizeX != value)
                {
                    _labelSizeX = value;
                    RaisePropertyChanged("LabelSizeX");
                }
            }
        }
        private string _labelSizeY = string.Empty;
        public string LabelSizeY
        {
            get {
                if (!string.IsNullOrEmpty(_labelSizeY))
                {
                    return _labelSizeY;
                } else
                {
                    // 변경된 값이 없으면 원래 데이터를 반환
                    return ExcelTotalData[1][1];
                }
            }
            set {
                if (_labelSizeY != value)
                {
                    _labelSizeY = value;
                    RaisePropertyChanged("LabelSizeY");
                }
            }
        }

        private string _printX = string.Empty;
        public string PrintX
        {
            get {
                if (!string.IsNullOrEmpty(_printX))
                {
                    return _printX;
                } else
                {
                    // 변경된 값이 없으면 원래 데이터를 반환
                    return ExcelTotalData[2][1];
                }
            }
            set {
                if (_printX != value)
                {
                    _printX = value;
                    RaisePropertyChanged("PrintX");
                }
            }
        }

        private string _printY = string.Empty;
        public string PrintY
        {
            get {
                if (!string.IsNullOrEmpty(_printY))
                {
                    return _printY;
                } else
                {
                    // 변경된 값이 없으면 원래 데이터를 반환
                    return ExcelTotalData[3][1];
                }
            }
            set {
                if (_printY != value)
                {
                    _printY = value;
                    RaisePropertyChanged("PrintY");
                }
            }
        }

        private string _groundX = string.Empty;
        public string GroundX
        {
            get { return _groundX; }
            set {
                _groundX = value;
                RaisePropertyChanged("GroundX");
            }
        }

        private string _groundY = string.Empty;
        public string GroundY
        {
            get { return _groundY; }
            set {
                _groundY = value;
                RaisePropertyChanged("GroundY");
            }
        }

        private string _factoryX = string.Empty;
        public string FactoryX
        {
            get { return _factoryX; }
            set {
                _factoryX = value;
                RaisePropertyChanged("FactoryX");
            }
        }

        private string _factoryY = string.Empty;
        public string FactoryY
        {
            get { return _factoryY; }
            set {
                _factoryY = value;
                RaisePropertyChanged("FactoryY");
            }
        }

        private string _carNameX = string.Empty;
        public string CarNameX
        {
            get { return _carNameX; }
            set {
                _carNameX = value;
                RaisePropertyChanged("CarNameX");
            }
        }

        private string _carNameY = string.Empty;
        public string CarNameY
        {
            get { return _carNameY; }
            set {
                _carNameY = value;
                RaisePropertyChanged("CarNameY");
            }
        }

        private string _deliveryX = string.Empty;
        public string DeliveryX
        {
            get { return _deliveryX; }
            set {
                _deliveryX = value;
                RaisePropertyChanged("DeliveryX");
            }
        }

        private string _deliveryY = string.Empty;
        public string DeliveryY
        {
            get { return _deliveryY; }
            set {
                _deliveryY = value;
                RaisePropertyChanged("DeliveryY");
            }
        }

        private string _productNumX = string.Empty;
        public string ProductNumX
        {
            get { return _productNumX; }
            set {
                _productNumX = value;
                RaisePropertyChanged("ProductNumX");
            }
        }

        private string _productNumY = string.Empty;
        public string ProductNumY
        {
            get { return _productNumY; }
            set {
                _productNumY = value;
                RaisePropertyChanged("ProductNumY");
            }
        }

        private string _countX = string.Empty;
        public string CountX
        {
            get { return _countX; }
            set {
                _countX = value;
                RaisePropertyChanged("CountX");
            }
        }

        private string _countY = string.Empty;
        public string CountY
        {
            get { return _countY; }
            set {
                _countY = value;
                RaisePropertyChanged("CountY");
            }
        }

        private string _productNameX = string.Empty;
        public string ProductNameX
        {
            get { return _productNameX; }
            set {
                _productNameX = value;
                RaisePropertyChanged("ProductNameX");
            }
        }

        private string _productNameY = string.Empty;
        public string ProductNameY
        {
            get { return _productNameY; }
            set {
                _productNameY = value;
                RaisePropertyChanged("ProductNameY");
            }
        }

        private string _productColorX = string.Empty;
        public string ProductColorX
        {
            get { return _productColorX; }
            set {
                _productColorX = value;
                RaisePropertyChanged("ProductColorX");
            }
        }

        private string _productColorY = string.Empty;
        public string ProductColorY
        {
            get { return _productColorY; }
            set {
                _productColorY = value;
                RaisePropertyChanged("ProductColorY");
            }
        }

        private string _barcodeX = string.Empty;
        public string BarcodeX
        {
            get { return _barcodeX; }
            set {
                _barcodeX = value;
                RaisePropertyChanged("BarcodeX");
            }
        }

        private string _barcodeY = string.Empty;
        public string BarcodeY
        {
            get { return _barcodeY; }
            set {
                _barcodeY = value;
                RaisePropertyChanged("BarcodeY");
            }
        }

        private string _deliveryDateX = string.Empty;
        public string DeliveryDateX
        {
            get { return _deliveryDateX; }
            set {
                _deliveryDateX = value;
                RaisePropertyChanged("DeliveryDateX");
            }
        }

        private string _deliveryDateY = string.Empty;
        public string DeliveryDateY
        {
            get { return _deliveryDateY; }
            set {
                _deliveryDateY = value;
                RaisePropertyChanged("DeliveryDateY");
            }
        }

        private string _companyX = string.Empty;
        public string CompanyX
        {
            get { return _companyX; }
            set {
                _companyX = value;
                RaisePropertyChanged("CompanyX");
            }
        }

        private string _companyY = string.Empty;
        public string CompanyY
        {
            get { return _companyY; }
            set {
                _companyY = value;
                RaisePropertyChanged("CompanyY");
            }
        }

        private string _factorySerialX = string.Empty;
        public string FactorySerialX
        {
            get { return _factorySerialX; }
            set {
                _factorySerialX = value;
                RaisePropertyChanged("FactorySerialX");
            }
        }

        private string _factorySerialY = string.Empty;
        public string FactorySerialY
        {
            get { return _factorySerialY; }
            set {
                _factorySerialY = value;
                RaisePropertyChanged("FactorySerialY");
            }
        }

        private string _lotNoX = string.Empty;
        public string LotNoX
        {
            get { return _lotNoX; }
            set {
                _lotNoX = value;
                RaisePropertyChanged("LotNoX");
            }
        }

        private string _lotNoY = string.Empty;
        public string LotNoY
        {
            get { return _lotNoY; }
            set {
                _lotNoY = value;
                RaisePropertyChanged("LotNoY");
            }
        }

        private string _HPCX = string.Empty;
        public string HPCX
        {
            get { return _HPCX; }
            set {
                _HPCX = value;
                RaisePropertyChanged("HPCX");
            }
        }
        private string _HPCY = string.Empty;
        public string HPCY
        {
            get { return _HPCY; }
            set {
                _HPCY = value;
                RaisePropertyChanged("HPCY");
            }
        }

        private string _issueNumX = string.Empty;
        public string IssueNumX
        {
            get { return _issueNumX; }
            set {
                _issueNumX = value;
                RaisePropertyChanged("IssueNumX");
            }
        }

        private string _issueNumY = string.Empty;
        public string IssueNumY
        {
            get { return _issueNumY; }
            set {
                _issueNumY = value;
                RaisePropertyChanged("IssueNumY");
            }
        }

        private string _containerX = string.Empty;
        public string ContainerX
        {
            get { return _containerX; }
            set {
                _containerX = value;
                RaisePropertyChanged("ContainerX");
            }
        }

        private string _containerY = string.Empty;
        public string ContainerY
        {
            get { return _containerY; }
            set {
                _containerY = value;
                RaisePropertyChanged("ContainerY");
            }
        }

        private string _bigProductNameX = string.Empty;
        public string BigProductNameX
        {
            get { return _bigProductNameX; }
            set {
                _bigProductNameX = value;
                RaisePropertyChanged("BigProductNameX");
            }
        }

        private string _bigProductNameY = string.Empty;
        public string BigProductNameY
        {
            get { return _bigProductNameY; }
            set {
                _bigProductNameY = value;
                RaisePropertyChanged("BigProductNameY");
            }
        }

        #endregion

        #region defaultListUpdate
        public string SetPrintDataTrueFont(double groupNum, double groupPositionX, double groupPositionY, string inputData)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(tpclCommand._SetTrueFont(groupNum, groupPositionX, groupPositionY, 50, 50, "B", 270, "B")); // 폰트셋팅
            builder.Append(tpclCommand._SetTrueValueInput(groupNum, inputData)); // 폰트 데이터 인풋

            return builder.ToString();
        }

        public string SetBarcode(double groupNum, double groupPositionX, double groupPositionY, string countInput)
        {
            BarcodeData = ProductNumber + "  " + PrintCount + FormatDate + ConvertToCustomString(Int32.Parse(countInput));
            StringBuilder builder = new StringBuilder();
            builder.Append(tpclCommand._SetBarcode(groupNum, groupPositionX, groupPositionY,"9",1,5,270,700));
            builder.Append(tpclCommand._SetBarcodeValueInput(groupNum, BarcodeData));

            return builder.ToString();
        }

        public string ConvertToCustomString(int number)
        {
            if (number >= 0 && number <= 10)
            {
                return $"000{number-1}";
            } else if (number >= 11 && number <= 36)
            {
                char customChar = (char)('A' + (number - 11));
                return $"000{customChar}";
            } else
            {
                int baseNumber = (number - 36) / 26;
                int remainder = (number - 36) % 26;
                char customChar = (char)('0' + baseNumber);
                char remainderChar = (char)('A' + remainder);
                return $"{customChar}{remainderChar}";
            }
            //if (number >= 0 && number <= 9)
            //{
            //    return $"000{number}";
            //} else if (number >= 10 && number <= 35)
            //{
            //    char customChar = (char)('A' + (number - 10));
            //    return $"000{customChar}";
            //} else
            //{
            //    int baseNumber = (number - 36) / 26;
            //    int remainder = (number - 36) % 26;
            //    char customChar = (char)('0' + baseNumber);
            //    char remainderChar = (char)('A' + remainder);
            //    return $"{customChar}{remainderChar}";
            //}
        }



        #endregion
    }
}
