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
            StringBuilder builder = new StringBuilder();
            builder.Append(tpclCommand._SetLabelSize(Double.Parse(LabelSizeY), Double.Parse(LabelSizeX), Double.Parse(PrintY), Double.Parse(PrintX))); // 라벨사이즈 지정
            builder.Append(tpclCommand._SetClearImageBuffer()); //클리어
            builder.Append(tpclCommand._SetTrueFont(1, 500, 600, 80, 80, "E", 90, "B")); // 폰트셋팅
            builder.Append(tpclCommand._SetTrueValueInput(1, testText)); // 폰트 데이터 인풋
            builder.Append(tpclCommand._SetStartPrinting(1, 0, 1, 0, 1, 2, 0, 1));
            InputPrinterCommand = builder.ToString();

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
