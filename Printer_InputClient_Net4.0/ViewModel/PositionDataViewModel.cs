using Printer_InputClient_Net4._0.Model;
using System;
using System.Collections.Generic;
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
            FileName = "DataList.xlsx";
            CallingBackData(FileName, (int)RecipeSerial.MODEL_DATA); // 모델 레시피 호출
            //CallingBackData(FileName, (int)RecipeSerial.M_LABEL_POSITION); // Middle 포지션 레시피 호출
            //ReadDataRecive.CallingBackData(FileName,1);
            Console.WriteLine(TestExcelData.Count);
            Console.WriteLine(WorkSheetNameList.Count);
            //Console.WriteLine(ReadDataRecive.WorkSheetNameList.Count);
            TestPrint = new Command(BtnSendCommand); 
            BtnPrintCommand = new Command(InputDataSend);
            PrinterName = "TEC B-SX8T (305 dpi)";
        }

        public Dictionary<int, object> selectedDataSheet = new Dictionary<int, object>();
        public List<object> DataList = new List<object>();
        public void SelectedData(int sheetNum)
        {
            for (int i = 1; i < WorkSheetNameList.Count; i++)
            {
                PositionDataModel position = new PositionDataModel();
                DataList.Add(position);
            }
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
            
            builder.Append(SetPrintDataTrueFont(1, "납품장소", 30, Delivery)); // 납품 장소
            builder.Append(SetPrintDataTrueFont(2, "모델명", 30, ModelName)); // ModelName
            builder.Append(SetPrintDataTrueFont(3, "수량", 50, LotCount)); // 수량 pv
            builder.Append(SetPrintDataTrueFont(4, "품번", 50, ProductNumber)); // 품번
            builder.Append(SetPrintDataTrueFont(5, "품명1", 50, ProductName)); // 품명 11
            builder.Append(SetPrintDataTrueFont(6, "품명2", 50, ProductName)); // 품명 22
            builder.Append(SetBarcode(7, "바코드", PrintCount)); //barcode
            builder.Append(SetPrintDataTrueFont(8, "업체명", 30, Company)); // 업체명
            builder.Append(SetPrintDataTrueFont(9, "LotDate", 30, FormatDate)); // LotDate
            builder.Append(SetPrintDataTrueFont(10, "발행 번호", 30, BarcodeData)); // 발행번호Text
            builder.Append(SetPrintDataTrueFont(11, "품명3", 60, ProductName)); // 품명 33
            builder.Append(SetPrintDataTrueFont(12, "지역", 30, Aground)); // 지역
            builder.Append(SetPrintDataTrueFont(13, "공장", 30, Factory)); // 공장


            builder.Append(SetPrintDataTrueFontBelow(14, "납품장소", 30, Delivery)); // 납품 장소
            builder.Append(SetPrintDataTrueFontBelow(15, "모델명", 30, ModelName)); // ModelName
            builder.Append(SetPrintDataTrueFontBelow(16, "수량", 50, LotCount)); // 수량 pv
            builder.Append(SetPrintDataTrueFontBelow(17, "품번", 50, ProductNumber)); // 품번
            builder.Append(SetPrintDataTrueFontBelow(18, "품명1", 50, ProductName)); // 품명 11
            builder.Append(SetPrintDataTrueFontBelow(19, "품명2", 50, ProductName)); // 품명 22
            builder.Append(SetBarcodeBelow(20, "바코드", PrintCount)); //barcode
            builder.Append(SetPrintDataTrueFontBelow(21, "업체명", 30, Company)); // 업체명
            builder.Append(SetPrintDataTrueFontBelow(22, "LotDate", 30, FormatDate)); // LotDate
            builder.Append(SetPrintDataTrueFontBelow(23, "발행 번호", 30, BarcodeData)); // 발행번호Text
            builder.Append(SetPrintDataTrueFontBelow(24, "품명3", 60, ProductName)); // 품명 33
            builder.Append(SetPrintDataTrueFontBelow(25, "지역", 30, Aground)); // 지역
            builder.Append(SetPrintDataTrueFontBelow(26, "공장", 30, Factory)); // 공장

            InputDataValue = builder.ToString();

            PrinterSendTest();
        }

        public void PrinterSendTest()
        {
            
            StringBuilder builder = new StringBuilder();
            
            builder.Append(tpclCommand._SetLabelSize(Double.Parse(keyValuePairsX["라벨"]), Double.Parse(keyValuePairsY["라벨"]), Double.Parse(keyValuePairsX["인쇄 영역"]), Double.Parse(keyValuePairsY["인쇄 영역"]))); // 라벨사이즈 지정
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
