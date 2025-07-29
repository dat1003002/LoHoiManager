using LoHoiManager.Data;
using LoHoiManager.model;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LoHoiManager
{
    public partial class Listcui : Form
    {
        private int currentPage = 1;
        private readonly int pageSize = 20;
        private List<Product> allProducts;
        private List<Product> filteredProducts;
        private List<ProductViewModel> _allProductViewModels;
        private List<ProductViewModel> _filteredProductFactory;
        private Label lblPageInfo;
        private Button btnPrevious, btnNext;
        private ProductViewModel selectedProduct;
        private bool isProcessing = false;
        private string scannedBarcode = "";
        private double totalNetWeightForPeriod = 0.0;
        private bool isDateRangeSelected = false;
        private DateTime selectedStartDate;
        private DateTime selectedStopDate;
        private string selectedFactory;

        public Listcui()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            allProducts = new List<Product>();
            filteredProducts = new List<Product>();
            _allProductViewModels = new List<ProductViewModel>();
            _filteredProductFactory = new List<ProductViewModel>();

            cbtimkiem.DropDownStyle = ComboBoxStyle.DropDownList;
            cbtimkiem.MouseDown += (s, e) => cbtimkiem.DroppedDown = true;

            dataproduct.ScrollBars = ScrollBars.Vertical;
            dataproduct.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataproduct.RowHeadersVisible = false;
            dataproduct.CellFormatting += dataproduct_CellFormatting;

            this.KeyPreview = true;
            this.Activated += (s, e) => this.Focus();
            this.KeyPress += Listcui_KeyPress;

            this.Load += (s, e) => CenterDataGridView();
            this.Resize += (s, e) => CenterDataGridView();

            btnPrevious = new Button { Text = "Previous", Size = new Size(100, 30) };
            btnNext = new Button { Text = "Next", Size = new Size(100, 30) };
            lblPageInfo = new Label { Text = "Trang 1", AutoSize = true };

            btnPrevious.Click += (s, e) => ChangePage(-1);
            btnNext.Click += (s, e) => ChangePage(1);

            this.Controls.Add(btnPrevious);
            this.Controls.Add(btnNext);
            this.Controls.Add(lblPageInfo);

            LoadProductDataAsync();
        }

        private void dataproduct_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataproduct.Columns[e.ColumnIndex].Name == "StatusDisplay")
            {
                string status = e.Value?.ToString();
                if (status == "Đã xuất kho")
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.SelectionForeColor = Color.Red;
                }
                else
                {
                    e.CellStyle.ForeColor = Color.Black;
                    e.CellStyle.SelectionForeColor = Color.Black;
                }
            }
        }

        private async void Listcui_KeyPress(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine($"KeyPress: Char='{e.KeyChar}', ASCII={(int)e.KeyChar}");
            try
            {
                scannedBarcode += e.KeyChar;
                Console.WriteLine($"Current scannedBarcode: '{scannedBarcode}'");

                if (scannedBarcode.Length >= 12)
                {
                    if (!string.IsNullOrWhiteSpace(scannedBarcode))
                    {
                        Console.WriteLine($"Mã vạch hoàn tất: '{scannedBarcode}'");
                        await ProcessScannedBarcodeAsync(scannedBarcode.Trim());
                        scannedBarcode = "";
                    }
                    else
                    {
                        Console.WriteLine("Mã vạch rỗng!");
                        scannedBarcode = "";
                    }
                    e.Handled = true;
                }
                else
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi Listcui_KeyPress: {ex.Message}");
                MessageBox.Show($"Lỗi xử lý mã vạch: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                scannedBarcode = "";
            }
        }

        private async Task ProcessScannedBarcodeAsync(string barcode)
        {
            Console.WriteLine($"ProcessScannedBarcodeAsync: Barcode='{barcode}'");
            if (isProcessing)
            {
                Console.WriteLine("Đang xử lý, bỏ qua!");
                MessageBox.Show("Đang xử lý, vui lòng chờ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            isProcessing = true;

            try
            {
                barcode = barcode.Trim();
                Console.WriteLine($"Mã vạch sau trim: '{barcode}'");

                using (var dbContext = new LoHoiDbContext())
                {
                    var product = await dbContext.Products
                        .Include(p => p.Pallet)
                        .Include(p => p.Supplier)
                        .Include(p => p.Factory)
                        .FirstOrDefaultAsync(p => p.Barcode.ToLower() == barcode.ToLower());

                    if (product == null)
                    {
                        Console.WriteLine($"Không tìm thấy sản phẩm với Barcode: '{barcode}'");
                        MessageBox.Show("Mã Vạch Không Tồn Tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    Console.WriteLine($"Tìm thấy sản phẩm: ID={product.Id}, Name={product.Name}, FactoryId={product.FactoryId}");

                    if (product.FactoryId == 1)
                    {
                        Console.WriteLine($"Mã vạch thuộc nhà máy FactoryId=1, không được phép quét: Barcode={barcode}");
                        MessageBox.Show("Bạn không được phép quét mã vạch này!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else if (product.FactoryId != 2)
                    {
                        Console.WriteLine($"Mã vạch không thuộc nhà máy FactoryId=2, không xử lý: Barcode={barcode}, FactoryId={product.FactoryId}");
                        MessageBox.Show("Mã vạch không thuộc nhà máy được phép xử lý!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (product.Status == ProductStatus.Exported)
                    {
                        Console.WriteLine($"Sản phẩm đã xuất kho: Barcode={barcode}, Status={product.Status}");
                        MessageBox.Show("Mã Vạch Đã Xuất Kho!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var exportProduct = new ExportProduct
                    {
                        Name = product.Name,
                        Weight = product.Weight,
                        NetWeight = product.NetWeight,
                        CreateTime = product.CreateTime,
                        ExportTime = DateTime.Now,
                        Barcode = product.Barcode,
                        Status = ProductStatus.Exported,
                        SupplierId = product.SupplierId,
                        PalletId = product.PalletId,
                        FactoryId = product.FactoryId
                    };

                    dbContext.ExportProducts.Add(exportProduct);
                    product.Status = ProductStatus.Exported;
                    product.UpdateTime = DateTime.Now;

                    await dbContext.SaveChangesAsync();
                    Console.WriteLine($"Xuất kho thành công: ID={product.Id}, Barcode={barcode}, ExportTime={exportProduct.ExportTime}");

                    await LoadProductDataAsync();
                    MessageBox.Show($"Đã xuất kho thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi ProcessScannedBarcodeAsync: {ex.Message}\nStackTrace: {ex.StackTrace}");
                MessageBox.Show($"Lỗi khi xử lý xuất kho: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isProcessing = false;
            }
        }

        private async Task LoadProductDataAsync()
        {
            try
            {
                using (var dbContext = new LoHoiDbContext())
                {
                    var factories = await dbContext.Factories
                        .AsNoTracking()
                        .Select(f => f.Name)
                        .ToListAsync();
                    cbtimkiem.Items.Clear();
                    cbtimkiem.Items.Add("Tất cả");
                    cbtimkiem.Items.AddRange(factories.ToArray());
                    cbtimkiem.SelectedIndex = 0;
                    selectedFactory = "Tất cả";

                    allProducts = await dbContext.Products
                        .AsNoTracking()
                        .Include(p => p.Factory)
                        .ToListAsync();

                    _allProductViewModels = await dbContext.Products
                        .AsNoTracking()
                        .Include(p => p.Pallet)
                        .Include(p => p.Supplier)
                        .Include(p => p.Factory)
                        .Select(p => new ProductViewModel
                        {
                            Id = p.Id,
                            ProductName = p.Name,
                            Weight = p.Weight,
                            CreateTime = p.CreateTime,
                            PalletName = p.Pallet != null ? p.Pallet.Name : "N/A",
                            SupplierName = p.Supplier != null ? p.Supplier.Name : "N/A",
                            DefaultWeight = p.Pallet != null ? p.Pallet.DefaultWeight : 0,
                            FactoryName = p.Factory != null ? p.Factory.Name : "N/A",
                            Status = p.Status,
                            StatusDisplay = p.Status == ProductStatus.Imported ? "Đã nhập kho" : "Đã xuất kho"
                        })
                        .ToListAsync();

                    allProducts = allProducts ?? new List<Product>();
                    _allProductViewModels = _allProductViewModels ?? new List<ProductViewModel>();
                    filteredProducts = allProducts;
                    _filteredProductFactory = _allProductViewModels;

                    currentPage = 1;
                    DisplayPage(currentPage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                allProducts = new List<Product>();
                _allProductViewModels = new List<ProductViewModel>();
                filteredProducts = new List<Product>();
                _filteredProductFactory = new List<ProductViewModel>();
                DisplayPage(currentPage);
            }
        }

        private void DisplayPage(int page)
        {
            if (_filteredProductFactory == null || !_filteredProductFactory.Any())
            {
                dataproduct.DataSource = null;
                lblPageInfo.Text = "Không tìm thấy kết quả";
                currentPage = 1;
                return;
            }

            int totalPages = (int)Math.Ceiling((double)_filteredProductFactory.Count / pageSize);
            currentPage = Math.Clamp(page, 1, totalPages);

            var sortedData = _filteredProductFactory
                .OrderByDescending(p => p.CreateTime)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select((pvm, index) => new
                {
                    STT = (currentPage - 1) * pageSize + index + 1,
                    pvm.Id,
                    pvm.ProductName,
                    Weight = $"{pvm.Weight:0.##} Kg",
                    NetWeight = $"{allProducts.FirstOrDefault(p => p.Id == pvm.Id)?.NetWeight:0.##} Kg",
                    pvm.PalletName,
                    DefaultWeight = $"{pvm.DefaultWeight:0.##} Kg",
                    pvm.SupplierName,
                    pvm.FactoryName,
                    pvm.CreateTime,
                    pvm.StatusDisplay
                })
                .ToList();

            dataproduct.DataSource = sortedData;

            foreach (DataGridViewColumn column in dataproduct.Columns)
            {
                column.Visible = column.Name == "STT" ||
                                 column.Name == "ProductName" ||
                                 column.Name == "Weight" ||
                                 column.Name == "NetWeight" ||
                                 column.Name == "PalletName" ||
                                 column.Name == "DefaultWeight" ||
                                 column.Name == "SupplierName" ||
                                 column.Name == "FactoryName" ||
                                 column.Name == "CreateTime" ||
                                 column.Name == "StatusDisplay";
            }

            dataproduct.Columns["STT"].HeaderText = "STT";
            dataproduct.Columns["STT"].DisplayIndex = 0;
            dataproduct.Columns["STT"].Width = 50;
            dataproduct.Columns["STT"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            dataproduct.Columns["ProductName"].HeaderText = "Tên sản phẩm";
            dataproduct.Columns["ProductName"].DisplayIndex = 1;

            dataproduct.Columns["Weight"].HeaderText = "Trọng lượng";
            dataproduct.Columns["Weight"].DisplayIndex = 2;

            dataproduct.Columns["NetWeight"].HeaderText = "TL thực tế";
            dataproduct.Columns["NetWeight"].DisplayIndex = 3;

            dataproduct.Columns["PalletName"].HeaderText = "Tên Pallet";
            dataproduct.Columns["PalletName"].DisplayIndex = 4;

            dataproduct.Columns["DefaultWeight"].HeaderText = "TL Pallet";
            dataproduct.Columns["DefaultWeight"].DisplayIndex = 5;

            dataproduct.Columns["SupplierName"].HeaderText = "Nhà cung cấp";
            dataproduct.Columns["SupplierName"].DisplayIndex = 6;

            dataproduct.Columns["FactoryName"].HeaderText = "Nhà máy";
            dataproduct.Columns["FactoryName"].DisplayIndex = 7;

            dataproduct.Columns["CreateTime"].HeaderText = "Ngày tạo";
            dataproduct.Columns["CreateTime"].DisplayIndex = 8;

            dataproduct.Columns["StatusDisplay"].HeaderText = "Trạng thái";
            dataproduct.Columns["StatusDisplay"].DisplayIndex = 9;
            dataproduct.Columns["StatusDisplay"].Width = 100;

            Font headerFont = new Font(dataproduct.Font.FontFamily, 12, FontStyle.Bold);
            foreach (DataGridViewColumn column in dataproduct.Columns)
            {
                if (column.Visible)
                {
                    column.HeaderCell.Style.Font = headerFont;
                }
            }

            UpdatePaginationInfo();
        }

        private void CenterDataGridView()
        {
            int verticalSpace = 45;
            dataproduct.Location = new Point(
                (this.ClientSize.Width - dataproduct.Width) / 2,
                this.ClientSize.Height - dataproduct.Height - verticalSpace
            );

            int controlY = dataproduct.Bottom + 10;

            btnPrevious.Location = new Point(dataproduct.Left, controlY);
            btnNext.Location = new Point(dataproduct.Right - btnNext.Width, controlY);
            lblPageInfo.Location = new Point(
                (dataproduct.Left + dataproduct.Right - lblPageInfo.Width) / 2,
                controlY
            );
        }

        private void ChangePage(int direction)
        {
            int totalPages = (int)Math.Ceiling((double)(_filteredProductFactory?.Count ?? 0) / pageSize);
            currentPage = Math.Clamp(currentPage + direction, 1, totalPages > 0 ? totalPages : 1);
            DisplayPage(currentPage);
        }

        private void UpdatePaginationInfo()
        {
            int totalPages = (int)Math.Ceiling((double)(_filteredProductFactory?.Count ?? 0) / pageSize);
            lblPageInfo.Text = (_filteredProductFactory?.Any() ?? false) ? $"Trang {currentPage}/{totalPages}" : "Không tìm thấy kết quả";
        }

        private void dataproduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            try
            {
                DataGridViewRow row = dataproduct.Rows[e.RowIndex];
                if (!int.TryParse(row.Cells["Id"].Value?.ToString(), out int productId))
                {
                    MessageBox.Show("Không thể lấy Id sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                selectedProduct = _filteredProductFactory
                    .FirstOrDefault(p => p.Id == productId);

                if (selectedProduct == null)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm trong danh sách!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show($"Đã chọn sản phẩm: {selectedProduct.ProductName}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                selectedProduct = null;
            }
        }

        private void panelTong_Paint(object sender, PaintEventArgs e)
        {
            if (filteredProducts == null || !filteredProducts.Any())
            {
                string noDataText = "0.00";
                Font font = new Font("Arial", 14, FontStyle.Bold);
                SizeF textSize = e.Graphics.MeasureString(noDataText, font);
                float x = (panelTong.Width - textSize.Width) / 2;
                float y = (panelTong.Height - textSize.Height) / 2;
                e.Graphics.DrawString(noDataText, font, Brushes.White, new PointF(x, y));
                return;
            }

            double displayWeight = 0.0;
            if (isDateRangeSelected)
            {
                displayWeight = totalNetWeightForPeriod;
            }
            else
            {
                DateTime today = DateTime.Today;
                var todayProducts = filteredProducts
                    ?.Where(p => p.CreateTime.Date == today)
                    .ToList() ?? new List<Product>();
                displayWeight = todayProducts.Sum(p => p.NetWeight);
            }

            string displayText = $"{displayWeight:0.##} kg";
            Font displayFont = new Font("Arial", 16, FontStyle.Bold);
            SizeF textSizeDisplay = e.Graphics.MeasureString(displayText, displayFont);
            float centerX = (panelTong.Width - textSizeDisplay.Width) / 2;
            float centerY = (panelTong.Height - textSizeDisplay.Height) / 2;
            e.Graphics.DrawString(displayText, displayFont, Brushes.White, new PointF(centerX, centerY));
        }

        private void btnExecl_Click(object sender, EventArgs e)
        {
            DateTime defaultDateStart = datestart.MinDate;
            DateTime defaultDateStop = datestop.MinDate;

            if (datestart.Value == defaultDateStart || datestop.Value == defaultDateStop)
            {
                MessageBox.Show("Vui lòng chọn ngày bắt đầu và ngày kết thúc trước khi xuất file!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime startDate = datestart.Value.Date;
            DateTime stopDate = datestop.Value.Date.AddDays(1).AddTicks(-1);

            if (startDate > stopDate)
            {
                MessageBox.Show("Ngày bắt đầu không thể lớn hơn ngày kết thúc!",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var productsToExport = _filteredProductFactory
                ?.Where(p => p.CreateTime >= startDate && p.CreateTime <= stopDate)
                .OrderByDescending(p => p.CreateTime)
                .ToList() ?? new List<ProductViewModel>();

            if (productsToExport == null || !productsToExport.Any())
            {
                MessageBox.Show("Không có dữ liệu để xuất trong khoảng thời gian đã chọn!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Danh sách sản phẩm");

                worksheet.Cells[1, 1].Value = "STT";
                worksheet.Cells[1, 2].Value = "Tên sản phẩm";
                worksheet.Cells[1, 3].Value = "Trọng lượng";
                worksheet.Cells[1, 4].Value = "TL thực tế";
                worksheet.Cells[1, 5].Value = "Tên Pallet";
                worksheet.Cells[1, 6].Value = "TL Pallet";
                worksheet.Cells[1, 7].Value = "Nhà cung cấp";
                worksheet.Cells[1, 8].Value = "Nhà máy";
                worksheet.Cells[1, 9].Value = "Ngày tạo";

                using (var range = worksheet.Cells[1, 1, 1, 9])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                for (int i = 0; i < productsToExport.Count; i++)
                {
                    var product = productsToExport[i];
                    var productEntity = allProducts.FirstOrDefault(p => p.Id == product.Id);
                    worksheet.Cells[i + 2, 1].Value = i + 1;
                    worksheet.Cells[i + 2, 2].Value = product.ProductName;
                    worksheet.Cells[i + 2, 3].Value = product.Weight;
                    worksheet.Cells[i + 2, 4].Value = productEntity != null ? productEntity.NetWeight.ToString("0.##") : "0";
                    worksheet.Cells[i + 2, 5].Value = product.PalletName;
                    worksheet.Cells[i + 2, 6].Value = product.DefaultWeight.ToString("0.##");
                    worksheet.Cells[i + 2, 7].Value = product.SupplierName;
                    worksheet.Cells[i + 2, 8].Value = product.FactoryName;
                    worksheet.Cells[i + 2, 9].Value = product.CreateTime.ToString("dd/MM/yyyy");
                }

                double totalNetWeight = productsToExport
                    .Select(p => allProducts.FirstOrDefault(pr => pr.Id == p.Id)?.NetWeight ?? 0)
                    .Sum();

                int totalRow = productsToExport.Count + 2;
                worksheet.Cells[totalRow, 3].Value = "Tổng TL thực tế:";
                worksheet.Cells[totalRow, 3].Style.Font.Bold = true;
                worksheet.Cells[totalRow, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                worksheet.Cells[totalRow, 4].Value = totalNetWeight.ToString("0.##") + " kg";
                worksheet.Cells[totalRow, 4].Style.Font.Bold = true;
                worksheet.Cells[totalRow, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                worksheet.Cells.AutoFitColumns();

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    FileName = $"DanhSachSanPham_{startDate:yyyyMMdd}_{stopDate:yyyyMMdd}.xlsx"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(saveFileDialog.FileName, package.GetAsByteArray());
                    MessageBox.Show("Xuất file Excel thành công!",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            DateTime startDate = datestart.Value.Date;
            DateTime stopDate = datestop.Value.Date.AddDays(1).AddTicks(-1);

            if (startDate > stopDate)
            {
                MessageBox.Show("Ngày bắt đầu không thể lớn hơn ngày kết thúc!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            filteredProducts = allProducts
                ?.Where(p => p.CreateTime >= startDate && p.CreateTime <= stopDate)
                .Where(p => selectedFactory == "Tất cả" || (p.Factory != null && p.Factory.Name == selectedFactory))
                .ToList() ?? new List<Product>();

            _filteredProductFactory = _allProductViewModels
                ?.Where(p => p.CreateTime >= startDate && p.CreateTime <= stopDate)
                .Where(p => selectedFactory == "Tất cả" || p.FactoryName == selectedFactory)
                .ToList() ?? new List<ProductViewModel>();

            if (filteredProducts.Any())
            {
                totalNetWeightForPeriod = filteredProducts.Sum(p => p.NetWeight);
            }
            else
            {
                totalNetWeightForPeriod = 0;
                MessageBox.Show($"Không có sản phẩm nào thuộc nhà máy '{selectedFactory}' trong khoảng thời gian đã chọn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            isDateRangeSelected = true;
            selectedStartDate = startDate;
            selectedStopDate = stopDate;

            currentPage = 1;
            DisplayPage(currentPage);
            panelTong.Invalidate();
        }

        private void cbtimkiem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbtimkiem.SelectedIndex == -1) return;

            selectedFactory = cbtimkiem.SelectedItem?.ToString();
            if (selectedFactory == "Tất cả")
            {
                filteredProducts = allProducts ?? new List<Product>();
                _filteredProductFactory = _allProductViewModels ?? new List<ProductViewModel>();
            }
            else
            {
                filteredProducts = allProducts
                    ?.Where(p => p.Factory != null && p.Factory.Name == selectedFactory)
                    .ToList() ?? new List<Product>();

                _filteredProductFactory = _allProductViewModels
                    ?.Where(p => p.FactoryName == selectedFactory)
                    .ToList() ?? new List<ProductViewModel>();
            }

            if (!filteredProducts.Any() && selectedFactory != "Tất cả")
            {
                MessageBox.Show($"Không có sản phẩm nào thuộc nhà máy '{selectedFactory}'!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (isDateRangeSelected)
            {
                filteredProducts = allProducts
                    ?.Where(p => p.CreateTime >= selectedStartDate && p.CreateTime <= selectedStopDate)
                    .Where(p => selectedFactory == "Tất cả" || (p.Factory != null && p.Factory.Name == selectedFactory))
                    .ToList() ?? new List<Product>();

                _filteredProductFactory = _allProductViewModels
                    ?.Where(p => p.CreateTime >= selectedStartDate && p.CreateTime <= selectedStopDate)
                    .Where(p => selectedFactory == "Tất cả" || p.FactoryName == selectedFactory)
                    .ToList() ?? new List<ProductViewModel>();

                if (filteredProducts.Any())
                {
                    totalNetWeightForPeriod = filteredProducts.Sum(p => p.NetWeight);
                }
                else
                {
                    totalNetWeightForPeriod = 0;
                    MessageBox.Show($"Không có sản phẩm nào thuộc nhà máy '{selectedFactory}' trong khoảng thời gian đã chọn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                panelTong.Invalidate();
            }
            else
            {
                totalNetWeightForPeriod = 0;
                isDateRangeSelected = false;
                panelTong.Invalidate();
            }

            currentPage = 1;
            DisplayPage(currentPage);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            XuatKho xuatKhoForm = new XuatKho();
            xuatKhoForm.Show();
        }

        private void btnTonKho_Click(object sender, EventArgs e)
        {
            TonKho tonkhoForm = new TonKho();
            tonkhoForm.Show();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void datestop_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}