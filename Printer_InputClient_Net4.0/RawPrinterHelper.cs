using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Printer_InputClient_Net4._0
{
    // RawPrinterHelper 클래스 선언
    public class RawPrinterHelper
    {
        #region DLLImport ItemList
        // OpenPrinterA 함수를 호출하기 위한 P/Invoke 선언
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        // ClosePrinter 함수를 호출하기 위한 P/Invoke 선언
        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        // StartDocPrinterA 함수를 호출하기 위한 P/Invoke 선언
        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        // EndDocPrinter 함수를 호출하기 위한 P/Invoke 선언
        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        // StartPagePrinter 함수를 호출하기 위한 P/Invoke 선언
        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        // EndPagePrinter 함수를 호출하기 위한 P/Invoke 선언
        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        // WritePrinter 함수를 호출하기 위한 P/Invoke 선언
        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);
        #endregion

        // 프린터로 바이트 데이터를 전송하는 메서드
        public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount) {

            Trace.WriteLine("START :::: " + (MethodBase.GetCurrentMethod().Name));
            try
            {
                IntPtr hPrinter;
                DOCINFOA di = new DOCINFOA();
                Int32 dwError = 0, dwWritten = 0;
                bool success = false;

                // 문서 정보 설정
                di.pDocName = "ZPL Document";
                di.pDataType = "RAW";

                // 프린터 열기
                if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
                {
                    // 문서 시작
                    if (StartDocPrinter(hPrinter, 1, di))
                    {
                        // 페이지 시작
                        if (StartPagePrinter(hPrinter))
                        {
                            // 프린터에 데이터 쓰기
                            success = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                            // 페이지 종료
                            EndPagePrinter(hPrinter);
                        }
                        // 문서 종료
                        EndDocPrinter(hPrinter);
                    }
                    // 프린터 닫기
                    ClosePrinter(hPrinter);
                }

                // 전송 실패 시 예외 발생
                if (!success)
                {
                    dwError = Marshal.GetLastWin32Error();
                    throw new System.ComponentModel.Win32Exception(dwError);
                }

                return success;

            } catch (Exception e)
            {
                Trace.WriteLine("Exception ::::: " + (MethodBase.GetCurrentMethod().Name) + e);
                throw;
            }

        }

        // 문자열을 프린터로 전송하는 메서드
        public static bool SendStringToPrinter(string szPrinterName, string szString) {
            IntPtr pBytes;
            Int32 dwCount;
            byte[] rawData = Encoding.UTF8.GetBytes(szString);
            pBytes = Marshal.AllocCoTaskMem(rawData.Length);
            Marshal.Copy(rawData, 0, pBytes, rawData.Length);
            dwCount = rawData.Length;

            try {
                // 바이트 데이터를 프린터로 전송
                SendBytesToPrinter(szPrinterName, pBytes, dwCount);
            } finally {
                // 할당된 메모리 해제
                Marshal.FreeCoTaskMem(pBytes);
            }

            return true;
        }

        // 문서 정보를 나타내는 클래스 선언
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)] public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)] public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)] public string pDataType;
        }
    }
}