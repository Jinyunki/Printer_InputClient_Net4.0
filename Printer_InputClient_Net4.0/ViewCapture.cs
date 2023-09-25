using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Printer_InputClient_Net4._0
{
    public static class ViewCapture
    {
        public static void Capture(object obj, string saveFileName)
        {
            try
            {
                // 화면 캡처를 위한 렌더링 타겟 비트맵 생성
                RenderTargetBitmap renderTarget = new RenderTargetBitmap((int)SystemParameters.PrimaryScreenWidth,
                                                                        (int)SystemParameters.PrimaryScreenHeight,
                                                                        96, 96, PixelFormats.Pbgra32);

                // 렌더링 타겟에 화면 캡처
                DrawingVisual visual = new DrawingVisual();
                using (DrawingContext context = visual.RenderOpen())
                {
                    VisualBrush brush = new VisualBrush(Application.Current.MainWindow);
                    context.DrawRectangle(brush, null, new Rect(new Point(0, 0),
                                                                new Point(SystemParameters.PrimaryScreenWidth,
                                                                          SystemParameters.PrimaryScreenHeight)));
                }
                renderTarget.Render(visual);

                // 캡처한 이미지를 파일로 저장 (예: PNG 형식)
                string fileName = saveFileName + $"_{DateTime.Now:yyyyMMddHHmmss}.png";
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderTarget));

                using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    encoder.Save(stream);
                }

                MessageBox.Show($"화면 캡처가 완료되었습니다.\n파일 경로: {filePath}", "완료", MessageBoxButton.OK, MessageBoxImage.Information);
            } catch (Exception ex)
            {
                MessageBox.Show($"화면 캡처 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
