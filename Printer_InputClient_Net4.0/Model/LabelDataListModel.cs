using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Printer_InputClient_Net4._0.Model
{
    public class LabelDataListModel : ViewModelBase
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

        private string _productNumber;
        public string ProductNumber
        {
            get { return _productNumber; }
            set {
                _productNumber = value;
                RaisePropertyChanged("ProductNumber");
            }
        }

        private string _productName;
        public string PorductName
        {
            get { return _productName; }
            set {
                _productName = value;
                RaisePropertyChanged("PorductName");
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
    }
}
