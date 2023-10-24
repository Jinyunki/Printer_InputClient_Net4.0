using Printer_InputClient_Net4._0.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
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
            OpenSerialPort(3, SerialPort_DataReceived);
            ButtonEvent();
            PrinterName = "TEC B-SX8T (305 dpi)";
        }

        

        public void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Trace.WriteLine("==========   Start   ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\n");
            try
            {
                SerialPort sp = (SerialPort)sender;
                string indata = sp.ReadExisting();

                ProductNumber = indata;
                GetModelData(ProductNumber);
            } catch (Exception ex)
            {
                Trace.WriteLine("========== Exception ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\nException : " + ex);
                throw;
            }

        }

        private void ButtonEvent()
        {
            TestPrint = new Command(BtnTestCommand);

            BtnPrintCommand = new Command(InputDataSend);

            BtnInkPlusCommand = new Command(PlusInkValue);
            BtnInkMinusCommand = new Command(MinusInkValue);

            BtnAddSaveCommand = new Command(AddDataSaveCommand);
            BtnCancelCommand = new Command(AddDataCancelCommand);
        }

        private void AddDataCancelCommand(object obj)
        {

            Trace.WriteLine("==========   Start   ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\n");
            try
            {

                ProductNumber = "";
                ModelName = "";
                ProductName = "";
                LotCount = "";

                OpacityValue = 1.0;
                NoneRecipe = false;
                ExistRecipe = true;
            } catch (Exception ex)
            {
                Trace.WriteLine("========== Exception ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\nException : " + ex);
                throw;
            }

        }

        private void AddDataSaveCommand(object obj)
        {

            Trace.WriteLine("==========   Start   ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\n");
            try
            {
                if (ProductNumber != "")
                {
                    UpdateExcelData(FileName, ProductNumber, "", "");
                }

                ProductNumber = "";
                ModelName = "";
                ProductName = "";
                LotCount = "";

                OpacityValue = 1.0;
                NoneRecipe = false;
                ExistRecipe = true;
            } catch (Exception ex)
            {
                Trace.WriteLine("========== Exception ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\nException : " + ex);
                throw;
            }

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

        
        public void CommandTPCL(string delivery, string modelName, string lotCount, string productNumber, string productName, string company, string ground, string factory, string serialNumber, string printCount)
        {
            Trace.WriteLine("==========   Start   ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\n");
            try
            {
                int groupNumber = 1;
                StringBuilder builder = new StringBuilder();
                builder.Append(SetSizeAndPrintDensity("라벨", "인쇄 영역", int.Parse(InkLevel)) + "\n"); // 라벨사이즈, 인쇄영역, 잉크 농도
                for (int i = 0; i <= int.Parse(PrintCount); i++)
                {
                    Barcode = serialNumber + GenerateOutput(i + (int.Parse(printCount)));

                    builder.Append(tpclCommand._SetClearImageBuffer() + "\n"); //클리어

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

                    builder.Append(tpclCommand._SetStartPrinting(1, 0, 1, 0, 1, 2, 0, 1));
                }

                //builder.Append(tpclCommand._SetStartPrinting(double.Parse(PrintCount), 1, 1, 0, 1, 2, 0, 1));

                InputPrinterCommand = builder.ToString();

            } catch (Exception ex)
            {
                Trace.WriteLine("========== Exception ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\nException : " + ex);
                throw;
            }

        }

       
        // 실제 프린터 출력 메서드
        private void InputDataSend(object obj)
        {
            if (ProductNumber == "")
            {
                ProductNumber = "99240-K3100";
                GetModelData(ProductNumber);
            }
            if (Today != FormatDate)
            {
                ExcelDataCount = "0";
            }
            CommandTPCL(Delivery, ModelName, LotCount, ProductNumber, ProductName, Company, Ground, Factory, SerialNumber, ExcelDataCount);
            UpdateExcelData(FileName, ProductNumber, int.Parse(ExcelDataCount).ToString(), int.Parse(PrintCount).ToString());  
            GetModelData(ProductNumber);
            GetPrint(PrinterName);
        }
        
        // TPCL TEST TextView 호출 메서드
        private void BtnTestCommand(object obj)
        {

            Trace.WriteLine("==========   Start   ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\n");
            try
            {
                if (ProductNumber == "")
                {
                    ProductNumber = "99240-K3100";
                    GetModelData(ProductNumber);
                }
                if (Today != FormatDate)
                {
                    ExcelDataCount = "0";
                }

                if (RemainderLotCount == "0")
                {
                    CommandTPCL(Delivery, ModelName, LotCount, ProductNumber, ProductName, Company, Ground, Factory, SerialNumber, ExcelDataCount);
                    //Console.WriteLine(InputPrinterCommand);
                    UpdateExcelData(FileName, ProductNumber, int.Parse(ExcelDataCount).ToString(), int.Parse(PrintCount).ToString());
                    GetModelData(ProductNumber);
                } else
                {
                    SerialNumber = double.Parse(RemainderLotCount).ToString("00");
                    CommandTPCL(Delivery, ModelName, RemainderLotCount, ProductNumber, ProductName, Company, Ground, Factory, SerialNumber, ExcelDataCount);
                    UpdateExcelData(FileName, ProductNumber, int.Parse(ExcelDataCount).ToString(), int.Parse(PrintCount).ToString());
                    GetModelData(ProductNumber);
                    Console.WriteLine(InputPrinterCommand);
                }
            } catch (Exception ex)
            {
                Trace.WriteLine("========== Exception ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\nException : " + ex);
                throw;
            }

            
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
