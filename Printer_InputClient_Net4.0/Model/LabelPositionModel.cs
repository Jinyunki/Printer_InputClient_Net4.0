using GalaSoft.MvvmLight;
using PrintCommand;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Printer_InputClient_Net4._0.Model
{
    public class LabelPositionModel : ViewModelBase
    {

        public static string FILEPATH = Path.Combine(@"D:\0.DefaultFile\JinYunki\Printer_InputClient_Net4.0\Printer_InputClient_Net4.0\bin\Data", "PrintPointRecipie.xlsx");
        public TPCLCommand tpclCommand = new TPCLCommand();
        public ReadExcelData readExcelData = new ReadExcelData();

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
            get { return ExcelTotalData[0][1]; }
            set {
                ExcelTotalData[0][1] = value;
                RaisePropertyChanged("LabelSizeX");
            }
        }

        private string _labelSizeY = string.Empty;
        public string LabelSizeY
        {
            get { return ExcelTotalData[1][1]; }
            set {
                ExcelTotalData[1][1] = value;
                RaisePropertyChanged("LabelSizeY");
            }
        }

        private string _printX = string.Empty;
        public string PrintX
        {
            get { return ExcelTotalData[2][1]; }
            set {
                ExcelTotalData[2][1] = value;
                RaisePropertyChanged("PrintX");
            }
        }

        private string _printY = string.Empty;
        public string PrintY
        {
            get { return ExcelTotalData[3][1]; }
            set {
                ExcelTotalData[3][1] = value;
                RaisePropertyChanged("PrintY");
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

    }
}
