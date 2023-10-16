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
            
            Console.WriteLine(TestExcelData.Count);
            TestPrint = new Command(BtnSendCommand); 
            BtnPrintCommand = new Command(InputDataSend);
            PrinterName = "TEC B-SX8T (305 dpi)";

            testtest("99240-K3100");
        }
        public void testtest(string inputData)
        {
            GetReadModelRecipe(FileName); // 모델 레시피 호출 (File명 + sheetNumber)
            for (int i = 0; i < productList.Count; i++) // i = CELL 가로 data
            {
                if (productList[i] is ProductDataModel product)
                {
                    if (inputData == product.ProductNumber) // 읽어온 데이터를 ProductNumber와 비교
                    {
                        GetReadLabelRecipe(FileName, product.LabelType);
                        GetLabelData("라벨");
                        //Console.WriteLine(product.LabelType);
                        //Console.WriteLine(product.ModelName);
                    }
                    
                }
            }
        }

        // 23.10.16~ 데이터값을 참조하여 TPCL커맨드 구현 해야함. 담아서 처리할것인가 바로 처리할것인가 ?에 대한 고찰
        // 1. 한번에 값을 참조하여 바로명령하는 방법
        // 2. 각각 파라메터를 통하여 들어오는 값에 대한 Data만 수신하는 방법
        public void GetLabelData(string inputLabelData)
        {
            for (int i = 1; i < productList.Count; i++) // i = CELL 가로 data
            {
                if (recipeList[i] is PositionDataModel labelData)
                {
                    if (labelData.Category == inputLabelData) // 읽어온 데이터를 ProductNumber와 비교
                    {
                        Console.WriteLine(inputLabelData + " 의 X포지션 값은 : " + labelData.XPosition);
                        Console.WriteLine(inputLabelData + " 의 Y포지션 값은 : " +labelData.YPosition);
                    }

                }
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
            builder.Append(tpclCommand._SetPrintDensity(true,4,true));
            
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
            CallingBackData(FileName, 2); // Middle 포지션 레시피 호출
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
