using GalaSoft.MvvmLight.Command;
using Printer_InputClient_Net4._0.Model;
using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Reflection;
using System.Text;
using System.Windows;

namespace Printer_InputClient_Net4._0.ViewModel
{
    public class PositionDataViewModel : ProductDataModel
    {
        public PositionDataViewModel()
        {
            OpenSerialPort(SelectedPort, SerialPort_DataReceived);
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

            Trace.WriteLine("==========   Start   ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\n");
            try
            {
                TestPrint = new Command(BtnTestCommand);

                BtnPrintCommand = new Command(InputDataSend);

                BtnInkPlusCommand = new Command(PlusInkValue);
                BtnInkMinusCommand = new Command(MinusInkValue);
                BtnInkReturnCommand = new Command(InkReturnCommand);

                BtnAddSaveCommand = new Command(AddDataSaveCommand);
                BtnCancelCommand = new Command(AddDataCancelCommand);
                
                BtnPortConnectCommand = new Command(RetryOpenSerial);
                EnterCommand = new RelayCommand(GetEnterCommand);
                
            } catch (Exception ex)
            {
                Trace.WriteLine("========== Exception ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\nException : " + ex);
                throw;
            }

        }

        private void GetEnterCommand()
        {
            AddProductNumber = ProductNumber;
            GetModelData(AddProductNumber);
            //Console.WriteLine(ProductNumber);
        }

        private void RetryOpenSerial(object obj)
        {
            OpenSerialPort(SelectedPort, SerialPort_DataReceived);
        }

        private void InkReturnCommand(object obj)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(tpclCommand._SetPrintDensity(InkLevel, true) + "{C|}"); // 인쇄 농도
            GetPrint(PrinterName, builder.ToString());
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
                if (ProductNumber != "" && ModelName !="" && ProductName != "" && LotCount != "")
                {
                    UpdateExcelData(FileName, ProductNumber, "", "");
                } else
                {
                    MessageBox.Show("모두 작성 부탁 드립니다.");
                    return;
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

            if (plus >= 0)
            {
                InkLevel = "+" + plus.ToString("00");
            } else
            {
                InkLevel = plus.ToString("00");
            }
            
        }

        private void MinusInkValue(object obj)
        {
            int minus = int.Parse(InkLevel);
            --minus;
            if (minus < -10)
            {
                return;
            }

            if (minus < 0)
            {
                InkLevel = minus.ToString("00");
            } else
            {
                InkLevel = "+" + minus.ToString("00");
            }
            
        }

        
        public void CommandTPCL(string delivery, string modelName, string lotCount, string productNumber, string productName, string company, string ground, string factory, string serialNumber, string printCount)
        {
            Trace.WriteLine("==========   Start   ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\n");
            try
            {
                int groupNumber = 1;
                StringBuilder builder = new StringBuilder();
                builder.Append(SetSizeAndPrintDensity("라벨", "인쇄 영역" )+ "\n"); // 라벨사이즈, 인쇄영역
                builder.Append(tpclCommand._SetPrintDensity(InkLevel, true)); // 인쇄 농도
                if (printCount == "0")
                {
                    printCount = "1";
                    
                }

                for (int i = 1; i <= int.Parse(PrintCount); i++)
                {

                    Barcode = serialNumber + GenerateOutput(i - 1 + (int.Parse(printCount)));

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

                    builder.Append(tpclCommand._SetStartPrinting(1, 0, 1, 1, 1, 2, 0, 1));
                }

                //builder.Append(tpclCommand._SetStartPrinting(double.Parse(PrintCount), 1, 1, 0, 1, 2, 0, 1));
                builder.Append(tpclCommand._SetPrintDensity("+02", true) + "{C|}"); // 인쇄 농도 롤백

                InputPrinterCommand = builder.ToString();
                Console.WriteLine(InputPrinterCommand);

            } catch (Exception ex)
            {
                Trace.WriteLine("========== Exception ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\nException : " + ex);
                throw;
            }

        }
        // 실제 프린터 출력 메서드
        private void InputDataSend(object obj)
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
                    GetModelData(ProductNumber);
                    UpdateExcelData(FileName, ProductNumber, int.Parse(ExcelDataCount).ToString(), int.Parse(PrintCount).ToString());
                    CommandTPCL(Delivery, ModelName, LotCount, ProductNumber, ProductName, Company, Ground, Factory, SerialNumber, ExcelDataCount);
                    GetModelData(ProductNumber);

                    GetPrint(PrinterName, InputPrinterCommand);
                    Console.WriteLine(InputPrinterCommand);
                } else
                {
                    GetModelData(ProductNumber);
                    PrintCount = "1";
                    SerialNumber = double.Parse(RemainderLotCount).ToString("00");
                    UpdateExcelData(FileName, ProductNumber, int.Parse(ExcelDataCount).ToString(), int.Parse(PrintCount).ToString());
                    CommandTPCL(Delivery, ModelName, RemainderLotCount, ProductNumber, ProductName, Company, Ground, Factory, SerialNumber, ExcelDataCount);
                    GetModelData(ProductNumber);

                    GetPrint(PrinterName,InputPrinterCommand);
                    Console.WriteLine(InputPrinterCommand);
                }
            } catch (Exception ex)
            {
                Trace.WriteLine("========== Exception ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\nException : " + ex);
                throw;
            }

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
                    UpdateExcelData(FileName, ProductNumber, int.Parse(ExcelDataCount).ToString(), int.Parse(PrintCount).ToString());
                    CommandTPCL(Delivery, ModelName, LotCount, ProductNumber, ProductName, Company, Ground, Factory, SerialNumber, ExcelDataCount);
                    GetModelData(ProductNumber);

                    //GetPrint(PrinterName);
                    Console.WriteLine(InputPrinterCommand);
                } else
                {
                    SerialNumber = double.Parse(RemainderLotCount).ToString("00");
                    PrintCount = "1";
                    UpdateExcelData(FileName, ProductNumber, int.Parse(ExcelDataCount).ToString(), int.Parse(PrintCount).ToString());
                    CommandTPCL(Delivery, ModelName, RemainderLotCount, ProductNumber, ProductName, Company, Ground, Factory, SerialNumber, ExcelDataCount);
                    GetModelData(ProductNumber);

                    //GetPrint(PrinterName);
                    Console.WriteLine(InputPrinterCommand);
                }
            } catch (Exception ex)
            {
                Trace.WriteLine("========== Exception ==========\nMethodName : " + (MethodBase.GetCurrentMethod().Name) + "\nException : " + ex);
                throw;
            }


        }


        private void GetPrint(string printerName, string sendCommand)
        {
            Trace.WriteLine("Start::::::::::::" + (MethodBase.GetCurrentMethod().Name));
            try
            {
                RawPrinterHelper.SendStringToPrinter(printerName, sendCommand);
            } catch (Exception e)
            {
                Trace.WriteLine("Catch::::::::::" + (MethodBase.GetCurrentMethod().Name) + e);
                MessageBox.Show("연결 실패");
            }
        }


    }

}
