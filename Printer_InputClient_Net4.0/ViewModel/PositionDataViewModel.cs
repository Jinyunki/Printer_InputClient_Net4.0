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
    public class PositionDataViewModel : ProductDataModel
    {
        public PositionDataViewModel()
        {
            FileName = "DataList.xlsx";
            
            TestPrint = new Command(BtnTestCommand); 
            BtnPrintCommand = new Command(InputDataSend);
            BtnInkPlusCommand = new Command(PlusInkValue);
            BtnInkMinusCommand = new Command(MinusInkValue);
            PrinterName = "TEC B-SX8T (305 dpi)";

            //GetModelData("99240-K3100"); // 기본값 TEST
        }

        private void PlusInkValue(object obj)
        {
            int plus = int.Parse(InkLevel);
            ++plus;
            if (plus > 10)
            {
                return;
            }
            InkLevel = plus.ToString();
        }

        private void MinusInkValue(object obj)
        {
            int minus = int.Parse(InkLevel);
            --minus;
            if (minus < 0)
            {
                return;
            }
            InkLevel = minus.ToString();
        }

        public void GetModelData(string inputData)
        {
            GetReadModelRecipe(FileName); // 모델 레시피 호출 (File명 + sheetNumber)
            for (int i = 0; i < productList.Count; i++) // i = CELL 가로 data
            {
                if (productList[i] is ProductDataModel product)
                {
                    if (inputData == product.ProductNumber) // 읽어온 데이터를 ProductNumber와 비교
                    {
                        GetReadLabelRecipe(FileName, product.LabelType);
                        GetLabelData(); // 라벨 데이터 불러오기

                        ProductNumber = product.ProductNumber;
                        ModelName = product.ModelName;
                        ProductName = product.ProductName;
                        LotCount = product.LotCount;
                        Ground = product.Ground;
                        Factory = product.Factory;
                        Company = product.Company;
                        //PrintCount = product.PrintCount;
                        CommandTPCL(product.Delivery, product.ModelName, product.LotCount, product.ProductNumber, product.ProductName, product.Company, product.Ground, product.Factory, product.SerialNumber, product.PrintCount);
                    } 
                }
            }
        }
        public void CommandTPCL(string delivery, string modelName, string lotCount, string productNumber, string productName, string company, string ground, string factory, string serialNumber, string printCount)
        {

            int groupNumber = 1;
            StringBuilder builder = new StringBuilder();
            builder.Append(SetSizeAndPrintDensity("라벨", "인쇄 영역", int.Parse(InkLevel)) + "\n"); // 라벨사이즈, 인쇄영역, 잉크 농도
            for (int i = 1; i <= int.Parse(PrintCount); i++)
            {
                Barcode = serialNumber + GenerateOutput(i+ (int.Parse(printCount)));

                builder.Append(tpclCommand._SetClearImageBuffer()); //클리어
                builder.Append("\n");

                builder.Append(SetPrintDataTrueFont(groupNumber, "납품장소", FONT_SMALL, delivery)); // 납품 장소
                builder.Append(SetPrintDataTrueFont(++groupNumber, "모델명", FONT_SMALL, modelName)); // ModelName
                builder.Append(SetPrintDataTrueFont(++groupNumber, "수량", FONT_MEDIUM, lotCount)); // 수량 pv
                builder.Append(SetPrintDataTrueFont(++groupNumber, "품번", FONT_MEDIUM, productNumber)); // 품번
                builder.Append(SetPrintDataTrueFont(++groupNumber, "품명1", FONT_MEDIUM, productName)); // 품명 11
                builder.Append(SetPrintDataTrueFont(++groupNumber, "품명2", FONT_MEDIUM, productName)); // 품명 22
                builder.Append(SetBarcode(++groupNumber, "바코드", Barcode)); //barcodeData
                builder.Append(SetPrintDataTrueFont(++groupNumber, "업체명", FONT_SMALL, company)); // 업체명
                builder.Append(SetPrintDataTrueFont(++groupNumber, "LotDate", FONT_SMALL, FormatDate)); // LotDate
                builder.Append(SetPrintDataTrueFont(++groupNumber, "발행 번호", FONT_SMALL, serialNumber)); // 발행번호Text
                builder.Append(SetPrintDataTrueFont(++groupNumber, "품명3", FONT_LARGE, productName)); // 품명 33
                builder.Append(SetPrintDataTrueFont(++groupNumber, "지역", FONT_SMALL, ground)); // 지역
                builder.Append(SetPrintDataTrueFont(++groupNumber, "공장", FONT_SMALL, factory) + "\n"); // 공장

                builder.Append(SetPrintDataTrueFontBelow(++groupNumber, "납품장소", FONT_SMALL, delivery)); // 납품 장소
                builder.Append(SetPrintDataTrueFontBelow(++groupNumber, "모델명", FONT_SMALL, modelName)); // ModelName
                builder.Append(SetPrintDataTrueFontBelow(++groupNumber, "수량", FONT_MEDIUM, lotCount)); // 수량 pv
                builder.Append(SetPrintDataTrueFontBelow(++groupNumber, "품번", FONT_MEDIUM, productNumber)); // 품번
                builder.Append(SetPrintDataTrueFontBelow(++groupNumber, "품명1", FONT_MEDIUM, productName)); // 품명 11
                builder.Append(SetPrintDataTrueFontBelow(++groupNumber, "품명2", FONT_MEDIUM, productName)); // 품명 22
                builder.Append(SetBarcodeBelow(++groupNumber, "바코드", Barcode)); //barcodeData
                builder.Append(SetPrintDataTrueFontBelow(++groupNumber, "업체명", FONT_SMALL, company)); // 업체명
                builder.Append(SetPrintDataTrueFontBelow(++groupNumber, "LotDate", FONT_SMALL, FormatDate)); // LotDate
                builder.Append(SetPrintDataTrueFontBelow(++groupNumber, "발행 번호", FONT_SMALL, serialNumber)); // 발행번호Text
                builder.Append(SetPrintDataTrueFontBelow(++groupNumber, "품명3", FONT_LARGE, productName)); // 품명 33
                builder.Append(SetPrintDataTrueFontBelow(++groupNumber, "지역", FONT_SMALL, ground)); // 지역
                builder.Append(SetPrintDataTrueFontBelow(++groupNumber, "공장", FONT_SMALL, factory) + "\n"); // 공장

                groupNumber = 1;
                
            }

            builder.Append(tpclCommand._SetStartPrinting(double.Parse(PrintCount), 0, 1, 0, 1, 2, 0, 1));

            InputPrinterCommand = builder.ToString();

            printCount = UpdateExcelData(FileName, productNumber, int.Parse(PrintCount).ToString() , int.Parse(printCount).ToString());

            

        }

        public void GetLabelData()
        {
            keyValuePositionX.Clear();
            keyValuePositionY.Clear();
            for (int j = 0; j < recipeList.Count; j++)
            {
                if (recipeList[j] is PositionDataModel labelData)
                {
                    string key = labelData.Category;
                    string valueX = labelData.XPosition;
                    string valueY = labelData.YPosition;

                    if (j > 0)
                    {
                        keyValuePositionX[key] = double.Parse(valueX);
                        keyValuePositionY[key] = double.Parse(valueY);
                    }
                }
            }
        }

        // 실제 프린터 출력 메서드
        private void InputDataSend(object obj)
        {
            PrinterSendTest();
            GetPrint(PrinterName); // 프린터 시작명령
        }

        public void PrinterSendTest()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(InputDataValue);
            InputPrinterCommand = builder.ToString();
        }

        
        // TPCL TEST TextView 호출 메서드
        private void BtnTestCommand(object obj)
        {
            GetModelData(ProductNumber);
        }
        

        private void GetPrint(string printerName)
        {
            Trace.WriteLine("Start::::::::::::" + (MethodBase.GetCurrentMethod().Name));
            try
            {
                RawPrinterHelper.SendStringToPrinter(printerName, InputPrinterCommand);
                //MessageBox.Show("연결 성공");
            } catch (Exception e)
            {
                Trace.WriteLine("Catch::::::::::" + (MethodBase.GetCurrentMethod().Name) + e);
                MessageBox.Show("연결 실패");
            }
        }


    }

}
