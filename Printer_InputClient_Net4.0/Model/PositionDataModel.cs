using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Printer_InputClient_Net4._0.Model
{
    public class PositionDataModel : MainModel
    {
        
        private string _category;
        public string Category
        {
            get { return _category; }
            set {
                _category = value;
                RaisePropertyChanged("Category");
            }
        }

        private string _xPosition;
        public string XPosition
        {
            get { return _xPosition; }
            set {
                _xPosition = value;
                RaisePropertyChanged("XPosition");
            }
        }

        private string _yPosition;


        public string YPosition
        {
            get { return _yPosition; }
            set {
                _yPosition = value;
                RaisePropertyChanged("YPosition");
            }
        }
    }
}
