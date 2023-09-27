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
    public class PositionDataViewModel : LabelPositionModel
    {
        public PositionDataViewModel()
        {
            ReadExcelDataRecive(1);
            //PrinterSendTest("TestItemFirstResult");
            PrinterSendTest22("testText");
            TestPrint = new Command(BtnSendCommand);
            
            PrinterName = FileName;
        }
        


        /// <summary>
        /// 라이브러리를 통해 호출된 엑셀데이터를 받아온다
        /// </summary>
        /// <param name="selectSheet">엑셀 파일의 </param>
        public void ReadExcelDataRecive(int selectSheet)
        {
            FileName = "PrintPointRecipie.xlsx";
            readExcelData.ReadExcelDataList(FileName, selectSheet);

            for (int i = 0; i < ExcelTotalData.Count; i++)
            {
                PositionCategorise.Add(ExcelTotalData[i][0]);
                PositionData.Add(ExcelTotalData[i][1]);
            }
            WorkSheetName = readExcelData.wrokSheetName;

            

        }
        public void PrinterSendTest22(string testText)
        {
            byte[] utf8Bytes = Encoding.UTF8.GetBytes("한글 테스트");

            StringBuilder builder = new StringBuilder();
            builder.Append(tpclCommand._SetLabelSize(Double.Parse(LabelSizeY), Double.Parse(LabelSizeX), Double.Parse(PrintY), Double.Parse(PrintX))); // 라벨사이즈 지정
            builder.Append(tpclCommand._SetClearImageBuffer()); //클리어
            //builder.Append(tpclCommand._SetTrueFont(1, 500, 600, 50, 50, "B", 270, "B")); // 폰트셋팅
            //builder.Append(tpclCommand._SetTrueValueInput(1, testText)); // 폰트 데이터 인풋
            builder.Append(SetPrintData(1, 180, 300, "24")); // 수량
            builder.Append(SetPrintData(2, 180, 1200, "99240-AA010")); // 품번
            builder.Append(SetPrintData(3, 250, 1200, "UNIT ASSY-RR VIEW CAMERA")); // 품명
            builder.Append(SetPrintData(4, 340, 1200, "UNIT ASSY-RR VIEW CAMERA")); // 품명
            builder.Append(SetPrintDataKorea(5, 440, 1200, "한글 테스트")); // 한글 테스트
            builder.Append(SetPrintDataKoreaPC(6, 540, 1200, "한글 테스트")); // 한글 테스트
            builder.Append(tpclCommand._SetStartPrinting(1, 0, 1, 0, 1, 2, 0, 1));
            InputPrinterCommand = builder.ToString();
        }
        public string SetPrintData(double groupNum, double groupPositionX, double groupPositionY, string inputData)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(tpclCommand._SetTrueFont(groupNum, groupPositionX, groupPositionY, 50, 50, "B", 270, "B")); // 폰트셋팅
            builder.Append(tpclCommand._SetTrueValueInput(groupNum, inputData)); // 폰트 데이터 인풋

            return builder.ToString();
        }
        public string SetPrintDataKorea(double groupNum, double groupPositionX, double groupPositionY, string inputData)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(tpclCommand._SetTrueFont(groupNum, groupPositionX, groupPositionY, 50, 50, "21", 270, 1)); // 폰트셋팅
            builder.Append(tpclCommand._SetTrueValueInput(groupNum, inputData)); // 폰트 데이터 인풋

            return builder.ToString();
        }
        public string SetPrintDataKoreaPC(double groupNum, double groupPositionX, double groupPositionY, string inputData)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(tpclCommand._SetBitmapFont(groupNum, groupPositionX, groupPositionY, 50, 50, "51", 270)); // 폰트셋팅
            builder.Append(tpclCommand._SetBitmapValueInput(groupNum, inputData)); // 폰트 데이터 인풋

            return builder.ToString();
        }


        public void PrinterSendTest(string testText)
        {
            InputPrinterCommand = tpclCommand._MiddleLabelCommand(Double.Parse(PositionData[0]), Double.Parse(PositionData[1]),
                                  Double.Parse(PositionData[2]), Double.Parse(PositionData[3]), Double.Parse(PositionData[4]), Double.Parse(PositionData[5]), testText);
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


        public ICommand BtnSend { get; set; }
        public ICommand TestPrint { get; set; }

    }

}
