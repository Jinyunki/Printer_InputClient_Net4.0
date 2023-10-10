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
            TestPrint = new Command(BtnSendCommand); // 23.10.10 주석처리
            BtnPrintCommand = new Command(InputDataSend);
            PrinterName = "TEC B-SX8T (305 dpi)";
            //PrinterName = FileName; // 23.10.10 주석처리
        }
        private void InputDataSend(object obj)
        {
            ValueUpdateTrigger(); // View업데이트
            GetPrint(PrinterName); // 프린터 시작명령
        }

        public void ValueUpdateTrigger()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(tpclCommand._SetPrintDensity(true,5,true));
            builder.Append(SetPrintDataTrueFont(1, 90, 300, 30, Delivery)); // 납품 장소(minitext)
            builder.Append(SetPrintDataTrueFont(2, 90, 750, 30, ModelName)); // ModelName
            builder.Append(SetPrintDataTrueFont(3, 180, 300, 50, PrintCount)); // 수량 pv
            builder.Append(SetPrintDataTrueFont(4, 180, 1200, 50, ProductNumber)); // 품번
            builder.Append(SetPrintDataTrueFont(5, 250, 1200, 50, ProductName)); // 품명11
            builder.Append(SetPrintDataTrueFont(6, 340, 1200, 50, ProductName)); // 품명22
            builder.Append(SetBarcode(7,370,1200, LotCount)); //barcode
            builder.Append(SetPrintDataTrueFont(8, 490, 680, 30, "HyundaiMobis Co.,Ltd")); // 업체명(minitext)
            builder.Append(SetPrintDataTrueFont(9, 590, 910, 30, FormatDate)); // LotDate
            builder.Append(SetPrintDataTrueFont(10, 640, 1200, 30, BarcodeData)); // 발행번호Text
            builder.Append(SetPrintDataTrueFont(11, 700, 1200, 60, ProductName)); // Big 품명 33
            builder.Append(SetPrintDataTrueFont(12, 50, 750, 30, Aground)); // 지역
            //builder.Append(SetPrintDataBitamp_Kor(8, 540, 680, "한글 테스트")); // 업체명(minitext)
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
            ValueUpdateTrigger();
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
