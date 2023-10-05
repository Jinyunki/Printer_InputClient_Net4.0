using Printer_InputClient_Net4._0.Model;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Printer_InputClient_Net4._0.ViewModel
{
    public class PositionDataViewModel : LabelModel
    {
        public PositionDataViewModel()
        {

            ReadExcelDataRecive(1);
            TestPrint = new Command(BtnSendCommand);
            BtnPrintCommand = new Command(InputDataSend);
            PrinterName = FileName;
        }
        private void InputDataSend(object obj)
        {
            ValueUpdateTrigger();
        }

        public void ValueUpdateTrigger()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(SetPrintDataTrueFont(1, 180, 300, PrintCount)); // 수량 pv
            builder.Append(SetPrintDataTrueFont(2, 180, 1200, ProductNumber)); // 품번
            builder.Append(SetPrintDataTrueFont(3, 250, 1200, ProductName)); // 품명11
            builder.Append(SetPrintDataTrueFont(4, 340, 1200, ProductName)); // 품명22
            builder.Append(SetBarcode(5,400,1200, LotCount)); // 바코드데이터 {23.10.04 커맨드라이브러리 수정필요 }
            InputDataValue = builder.ToString();

            PrinterSendTest();
        }

        public void PrinterSendTest()
        {
            
            StringBuilder builder = new StringBuilder();
            StringBuilder builderInputValue = new StringBuilder();
            
            builder.Append(tpclCommand._SetLabelSize(Double.Parse(LabelSizeY), Double.Parse(LabelSizeX), Double.Parse(PrintY), Double.Parse(PrintX))); // 라벨사이즈 지정
            builder.Append(tpclCommand._SetClearImageBuffer()); //클리어
            
            builder.Append(InputDataValue);
            builder.Append(tpclCommand._SetStartPrinting(1, 0, 1, 0, 1, 2, 0, 1));
            InputPrinterCommand = builder.ToString();
        }

        

        private void BtnSendCommand(object obj)
        {
            GetPrint(PrinterName);
        }
        

        private void GetPrint(string printerName)
        {
            Trace.WriteLine("Start::::::::::::" + (MethodBase.GetCurrentMethod().Name));
            try
            {
                RawPrinterHelper.SendStringToPrinter(printerName, InputPrinterCommand);
                MessageBox.Show("연결 성공");
            } catch (Exception e)
            {
                Trace.WriteLine("Catch::::::::::" + (MethodBase.GetCurrentMethod().Name) + e);
                MessageBox.Show("연결 실패");
            }
        }


    }

}
