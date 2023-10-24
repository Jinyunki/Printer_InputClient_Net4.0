using OfficeOpenXml;
using System;
using System.IO;

namespace Printer_InputClient_Net4._0.Model
{
    public class ProductDataModel : LabelModel
    {
        public void GetModelData(string inputData)
        {
            bool isRecipe = false; // 입력 값이 productList에 있는지 여부를 나타내는 플래그 변수

            GetReadModelRecipe(FileName); // 모델 레시피 호출 (File명 + sheetNumber)
            for (int i = 0; i < productList.Count; i++) // i = CELL 가로 data
            {
                if (productList[i] is ProductDataModel product)
                {
                    if (inputData == product.ProductNumber) // 읽어온 데이터를 ProductNumber와 비교
                    {
                        GetReadLabelRecipe(FileName, product.LabelType);
                        GetLabelData(); // 라벨 데이터 불러오기

                        Delivery = product.Delivery;
                        ModelName = product.ModelName;
                        LotCount = product.LotCount;
                        ProductNumber = product.ProductNumber;
                        ProductName = product.ProductName;
                        Company = product.Company;
                        Ground = product.Ground;
                        Factory = product.Factory;
                        SerialNumber = product.LotCount;
                        Today = product.Today;
                        ExcelDataCount = product.PrintCount;

                        isRecipe = true; // productList에 일치하는 항목이 있는 경우 플래그를 true로 설정
                        SendSignalToMainViewModel(isRecipe);
                        OpacityValue = 1.0;
                        NoneRecipe = false;
                        ExistRecipe = true;
                        break; // 일치하는 항목을 찾았으므로 루프를 종료합니다.
                    }
                }
            }

            // productList에 일치하는 항목이 없는 경우 메시지 상자를 표시합니다.
            if (!isRecipe)
            {
                Delivery = "";
                ModelName = "";
                LotCount = "";
                ProductNumber = inputData;
                ProductName = "";
                Company = "";
                Ground = "";
                Factory = "";
                SerialNumber = "";
                Today = "";
                ExcelDataCount = "";
                //TossValue = ProductNumber;
                isRecipe = false;
                SendSignalToMainViewModel(isRecipe);

                OpacityValue = 0.5;
                NoneRecipe = true;
                ExistRecipe = false;
                //MessageBox.Show("입력한 데이터는 productList에 존재하지 않습니다.");
            }
        }

        public string UpdateExcelData(string path, string inputProductNumber, string excelCount, string inputCount)
        {
            //string outputCount = "";
            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // 시트 선택
                int colCount = worksheet.Dimension.Columns; // 가로줄의 개수
                int rowCount = worksheet.Dimension.Rows; // 세로줄의 개수


                // 원하는 조건에 따라 특정 셀의 값을 업데이트합니다.
                for (int row = 1; row <= rowCount; row++)
                {
                    string productNumber = worksheet.Cells[row, 2].Text; // 예를 들어 ProductName을 기준으로 찾는다면 3번째 열에 해당합니다.
                    if (productNumber == inputProductNumber)
                    {
                        isProductNumberFound = true;
                        // 날짜가 오늘날짜이면 PrintCount를 증가 시키고,
                        if (worksheet.Cells[row, 10].Value.ToString() == FormatDate)
                        {
                            worksheet.Cells[row, 12].Value = (int.Parse(excelCount) + int.Parse(inputCount)).ToString(); // PrintCount 값 변경
                            ExcelDataCount = worksheet.Cells[row, 12].Value.ToString();
                        }

                        // 날짜가 프로그램 빌드 실행시 받는 날짜가 달라지면, PrintCount를 0으로 초기화 , 날짜를 오늘날짜로 변경
                        else
                        {
                            worksheet.Cells[row, 10].Value = FormatDate; // FormatDate 값 변경
                            worksheet.Cells[row, 12].Value = int.Parse(inputCount).ToString(); // PrintCount 값 변경
                            ExcelDataCount = worksheet.Cells[row, 12].Value.ToString();
                        }
                    }
                }

                if (!isProductNumberFound)
                {
                    // 마지막 행의 다음 행에 데이터를 추가합니다.
                    int newRow = rowCount + 1;
                    worksheet.Cells[rowCount + 1, 1].Value = ModelName; // 모델명
                    worksheet.Cells[rowCount + 1, 2].Value = ProductNumber; // 품번
                    worksheet.Cells[rowCount + 1, 3].Value = ProductName; // 품명
                    worksheet.Cells[rowCount + 1, 4].Value = LotCount; // 수량
                    worksheet.Cells[rowCount + 1, 5].Value = "KOREA"; // 지역
                    worksheet.Cells[rowCount + 1, 6].Value = "R7A8"; // 납품장소
                    worksheet.Cells[rowCount + 1, 7].Value = "HyundaiMobis Co.,Ltd"; // 업체명
                    worksheet.Cells[rowCount + 1, 8].Value = "Sekonix"; // 공장
                    worksheet.Cells[rowCount + 1, 9].Value = "M"; // 라벨타입
                    worksheet.Cells[rowCount + 1, 10].Value = "0"; // Today
                    worksheet.Cells[rowCount + 1, 11].Value = "0"; // S/N = 0
                    worksheet.Cells[rowCount + 1, 12].Value = "0"; // Count
                }

                package.Save(); // 변경된 내용을 원본 파일에 저장합니다.
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return ExcelDataCount;
        }


        #region Signal Item

        public static event EventHandler<SignalEventArgs> SignalFromSecondViewModelChanged;

        private void SendSignalToMainViewModel(bool signal)
        {
            OnSignalFromSecondViewModelChanged(new SignalEventArgs(signal));
        }

        protected virtual void OnSignalFromSecondViewModelChanged(SignalEventArgs e)
        {
            SignalFromSecondViewModelChanged?.Invoke(this, e);
        }

        #endregion

        /// <summary>
        /// 라벨의 각 위치를 Key,ValueX,Y로 반환 합니다.
        /// </summary>
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

        #region ModelDataList

        private string _modelName;
        public string ModelName
        {
            get { return _modelName; }
            set {
                _modelName = value;
                RaisePropertyChanged("ModelName");
            }
        }
        private string _productNumber = "";
        public string ProductNumber
        {
            get { return _productNumber; }
            set {
                // 문자열의 길이가 10 이상인지 확인하고, 10 이상이면 11번째 이후의 문자를 제거합니다.
                if (value.Length > 11)
                {
                    value = value.Substring(0, 10);
                }

                // 문자열에 "-"가 포함되어 있는지 확인합니다.
                if (value.Contains("-"))
                {
                    _productNumber = value;
                } else
                {
                    if (value.Length > 5)
                    {
                        _productNumber = value.Insert(5, "-");
                    } else
                    {
                        // 적절한 길이가 되지 않는 경우, 예외 처리를 수행하거나 기본값을 설정할 수 있습니다.
                        // 이 예시에서는 그대로 저장합니다.
                        _productNumber = value;
                    }
                }

                RaisePropertyChanged("ProductNumber");
            }
        }


        private string _productName;
        public string ProductName
        {
            get { return _productName; }
            set {
                _productName = value;
                RaisePropertyChanged("ProductName");
            }
        }
        //RemainderLotCount
        private string _lotCount;
        public string LotCount
        {
            get { return _lotCount; }
            set {
                _lotCount = value;
                RaisePropertyChanged("LotCount");
            }
        }

        private string _remainderLotCount = "0";
        public string RemainderLotCount
        {
            get { return _remainderLotCount; }
            set {
                _remainderLotCount = value;
                RaisePropertyChanged("RemainderLotCount");
            }
        }

        private string _ground;
        public string Ground
        {
            get { return _ground; }
            set {
                _ground = value;
                RaisePropertyChanged("Ground");
            }
        }

        private string _delivery;
        public string Delivery
        {
            get { return _delivery; }
            set {
                _delivery = value;
                RaisePropertyChanged("Delivery");
            }
        }

        private string _company;
        public string Company
        {
            get { return _company; }
            set {
                _company = value;
                RaisePropertyChanged("Company");
            }
        }

        private string _factory;
        public string Factory
        {
            get { return _factory; }
            set {
                _factory = value;
                RaisePropertyChanged("Factory");
            }
        }

        private string _labelType;
        public string LabelType
        {
            get { return _labelType; }
            set {
                _labelType = value;
                RaisePropertyChanged("LabelType");
            }
        }

        private string _today;
        public string Today
        {
            get { return _today; }
            set {
                _today = value;
                RaisePropertyChanged("Today");
            }
        }

        private string _serialNumber;
        public string SerialNumber
        {
            get { return _serialNumber; }
            set {
                _serialNumber = _productNumber.Replace("-", "") + "  " + value + FormatDate ;
                RaisePropertyChanged("SerialNumber");
            }
        }

        private string _printCount = "1";
        public string PrintCount
        {
            get { return _printCount; }
            set {
                _printCount = value;
                RaisePropertyChanged("PrintCount");
            }
        }

        private string _barcode ;
        public string Barcode
        {
            get { return _barcode; }
            set {
                _barcode = value;
                RaisePropertyChanged("Barcode");
            }
        }
        #endregion

        #region ViewDataList

        private double _opacityValue = 1.0;
        public double OpacityValue
        {
            get { return _opacityValue; }
            set {
                _opacityValue = value;
                RaisePropertyChanged("OpacityValue");
            }
        }


        private bool _noneRecipe = false;
        public bool NoneRecipe
        {
            get { return _noneRecipe; }
            set {
                _noneRecipe = value;
                RaisePropertyChanged("NoneRecipe");
            }
        }

        private bool _existRecipe = true;
        public bool ExistRecipe
        {
            get { return _existRecipe; }
            set {
                _existRecipe = value;
                RaisePropertyChanged("ExistRecipe");
            }
        }


        #endregion
    }
}
