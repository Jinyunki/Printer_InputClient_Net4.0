using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Printer_InputClient_Net4._0.Model
{
    public class ProductDataModel : LabelModel
    {
        private string _modelName;
        public string ModelName
        {
            get { return _modelName; }
            set {
                _modelName = value;
                RaisePropertyChanged("ModelName");
            }
        }

        private string _productNumber = "";
        public string ProductNumber
        {
            get { return _productNumber; }
            set {
                _productNumber = value;
                RaisePropertyChanged("ProductNumber");
            }
        }

        private string _productName;
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

        private string _ground;
        public string Ground
        {
            get { return _ground; }
            set {
                _ground = value;
                RaisePropertyChanged("Ground");
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

        private string _company;
        public string Company
        {
            get { return _company; }
            set {
                _company = value;
                RaisePropertyChanged("Company");
            }
        }

        private string _factory;
        public string Factory
        {
            get { return _factory; }
            set {
                _factory = value;
                RaisePropertyChanged("Factory");
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

        private string _today;
        public string Today
        {
            get { return _today; }
            set {
                _today = value;
                RaisePropertyChanged("Today");
            }
        }

        private string _serialNumber;
        public string SerialNumber
        {
            get { return _serialNumber; }
            set {
                _serialNumber = _productNumber.Replace("-", "") + "  " + LotCount + FormatDate ;
                RaisePropertyChanged("SerialNumber");
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

        private string _barcode ;
        public string Barcode
        {
            get { return _barcode; }
            set {
                _barcode = value;
                RaisePropertyChanged("Barcode");
            }
        }

    }
}
