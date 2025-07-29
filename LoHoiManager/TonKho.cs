using ClosedXML.Excel;
using LoHoiManager.Data;
using LoHoiManager.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;
using System.IO;
using Zen.Barcode;
using System.Diagnostics;

namespace LoHoiManager
{
    public partial class TonKho : Form
    {
        private int currentPage = 1;
        private readonly int pageSize = 20;
        private List<Product> allProducts;
        private List<Product> filteredProducts;
        private List<ProductViewModel> _allProductViewModels;
        private List<ProductViewModel> _filteredProductViewModels;
        private Label lblPageInfo;
        private Button btnPrevious, btnNext;
        private double totalNetWeight = 0;
        private bool isProcessing = false;
        private ProductViewModel selectedProduct;

        public TonKho()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;

            allProducts = new List<Product>();
            filteredProducts = new List<Product>();
            _allProductViewModels = new List<ProductViewModel>();
            _filteredProductViewModels = new List<ProductViewModel>();

            cbFactory.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFactory.MouseDown += (s, e) => cbFactory.DroppedDown = true;

            dataTonKho.ScrollBars = ScrollBars.Vertical;
            dataTonKho.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataTonKho.RowHeadersVisible = false;
            dataTonKho.AllowUserToAddRows = false;
            dataTonKho.AllowUserToDeleteRows = false;
            dataTonKho.ReadOnly = true;
            dataTonKho.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataTonKho.MultiSelect = false;
            dataTonKho.CellFormatting += dataTonKho_CellFormatting;
            dataTonKho.CellContentClick += dataTonKho_CellContentClick;

            btnPrevious = new Button { Text = "Previous", Size = new Size(100, 30) };
            btnNext = new Button { Text = "Next", Size = new Size(100, 30) };
            lblPageInfo = new Label { Text = "Trang 1", AutoSize = true };

            btnPrevious.Click += (s, e) => ChangePage(-1);
            btnNext.Click += (s, e) => ChangePage(1);

            this.Controls.Add(btnPrevious);
            this.Controls.Add(btnNext);
            this.Controls.Add(lblPageInfo);

            this.Load += (s, e) => CenterDataGridView();
            this.Resize += (s, e) => CenterDataGridView();

            LoadProductDataAsync();
        }

        private void dataTonKho_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataTonKho.Columns[e.ColumnIndex].Name == "StatusDisplay")
            {
                string status = e.Value?.ToString();
                if (status == "Đã nhập kho")
                {
                    e.CellStyle.ForeColor = Color.Black;
                    e.CellStyle.SelectionForeColor = Color.Black;
                }
            }
        }

        private async void LoadProductDataAsync()
        {
            try
            {
                using (var dbContext = new LoHoiDbContext())
                {
                    var factories = await dbContext.Factories
                        .AsNoTracking()
                        .Select(f => f.Name)
                        .ToListAsync();
                    cbFactory.Items.Clear();
                    cbFactory.Items.Add("Tất cả");
                    cbFactory.Items.AddRange(factories.ToArray());
                    cbFactory.SelectedIndex = 0;

                    allProducts = await dbContext.Products
                        .AsNoTracking()
                        .Include(p => p.Factory)
                        .Where(p => p.Status == ProductStatus.Imported)
                        .ToListAsync();

                    _allProductViewModels = await dbContext.Products
                        .AsNoTracking()
                        .Include(p => p.Pallet)
                        .Include(p => p.Supplier)
                        .Include(p => p.Factory)
                        .Where(p => p.Status == ProductStatus.Imported)
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
                    _filteredProductViewModels = _allProductViewModels;

                    if (!allProducts.Any())
                    {
                        MessageBox.Show("Không có sản phẩm nào ở trạng thái Đã nhập kho!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    totalNetWeight = filteredProducts.Sum(p => p.NetWeight);
                    currentPage = 1;
                    DisplayPage(currentPage);
                    panelTinhTong.Invalidate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                allProducts = new List<Product>();
                _allProductViewModels = new List<ProductViewModel>();
                filteredProducts = new List<Product>();
                _filteredProductViewModels = new List<ProductViewModel>();
                totalNetWeight = 0;
                DisplayPage(currentPage);
                panelTinhTong.Invalidate();
            }
        }

        private void DisplayPage(int page)
        {
            if (_filteredProductViewModels == null || !_filteredProductViewModels.Any())
            {
                dataTonKho.DataSource = null;
                lblPageInfo.Text = "Không tìm thấy kết quả";
                totalNetWeight = 0;
                currentPage = 1;
                panelTinhTong.Invalidate();
                return;
            }

            int totalPages = (int)Math.Ceiling((double)_filteredProductViewModels.Count / pageSize);
            currentPage = Math.Clamp(page, 1, totalPages);

            var sortedData = _filteredProductViewModels
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

            dataTonKho.DataSource = sortedData;

            foreach (DataGridViewColumn column in dataTonKho.Columns)
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

            dataTonKho.Columns["STT"].HeaderText = "STT";
            dataTonKho.Columns["STT"].DisplayIndex = 0;
            dataTonKho.Columns["STT"].Width = 50;
            dataTonKho.Columns["STT"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            dataTonKho.Columns["ProductName"].HeaderText = "Tên sản phẩm";
            dataTonKho.Columns["ProductName"].DisplayIndex = 1;

            dataTonKho.Columns["Weight"].HeaderText = "Trọng lượng";
            dataTonKho.Columns["Weight"].DisplayIndex = 2;

            dataTonKho.Columns["NetWeight"].HeaderText = "TL thực tế";
            dataTonKho.Columns["NetWeight"].DisplayIndex = 3;

            dataTonKho.Columns["PalletName"].HeaderText = "Tên Pallet";
            dataTonKho.Columns["PalletName"].DisplayIndex = 4;

            dataTonKho.Columns["DefaultWeight"].HeaderText = "TL Pallet";
            dataTonKho.Columns["DefaultWeight"].DisplayIndex = 5;

            dataTonKho.Columns["SupplierName"].HeaderText = "Nhà cung cấp";
            dataTonKho.Columns["SupplierName"].DisplayIndex = 6;

            dataTonKho.Columns["FactoryName"].HeaderText = "Nhà máy";
            dataTonKho.Columns["FactoryName"].DisplayIndex = 7;

            dataTonKho.Columns["CreateTime"].HeaderText = "Ngày tạo";
            dataTonKho.Columns["CreateTime"].DisplayIndex = 8;

            dataTonKho.Columns["StatusDisplay"].HeaderText = "Trạng thái";
            dataTonKho.Columns["StatusDisplay"].DisplayIndex = 9;
            dataTonKho.Columns["StatusDisplay"].Width = 100;

            Font headerFont = new Font(dataTonKho.Font.FontFamily, 12, FontStyle.Bold);
            foreach (DataGridViewColumn column in dataTonKho.Columns)
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
            dataTonKho.Location = new Point(
                (this.ClientSize.Width - dataTonKho.Width) / 2,
                this.ClientSize.Height - dataTonKho.Height - verticalSpace
            );

            int controlY = dataTonKho.Bottom + 10;

            btnPrevious.Location = new Point(dataTonKho.Left, controlY);
            btnNext.Location = new Point(dataTonKho.Right - btnNext.Width, controlY);
            lblPageInfo.Location = new Point(
                (dataTonKho.Left + dataTonKho.Right - lblPageInfo.Width) / 2,
                controlY
            );
        }

        private void ChangePage(int direction)
        {
            int totalPages = (int)Math.Ceiling((double)(_filteredProductViewModels?.Count ?? 0) / pageSize);
            currentPage = Math.Clamp(currentPage + direction, 1, totalPages > 0 ? totalPages : 1);
            DisplayPage(currentPage);
        }

        private void UpdatePaginationInfo()
        {
            int totalPages = (int)Math.Ceiling((double)(_filteredProductViewModels?.Count ?? 0) / pageSize);
            lblPageInfo.Text = (_filteredProductViewModels?.Any() ?? false) ? $"Trang {currentPage}/{totalPages}" : "Không tìm thấy kết quả";
        }

        private void dataTonKho_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            try
            {
                DataGridViewRow row = dataTonKho.Rows[e.RowIndex];
                if (!int.TryParse(row.Cells["Id"].Value?.ToString(), out int productId))
                {
                    MessageBox.Show("Không thể lấy Id sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                selectedProduct = _filteredProductViewModels
                    .FirstOrDefault(p => p.Id == productId);

                if (selectedProduct == null)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm trong danh sách!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var productEntity = allProducts.FirstOrDefault(p => p.Id == selectedProduct.Id);
                double netWeight = productEntity?.NetWeight ?? 0;

                MessageBox.Show($"Đã chọn sản phẩm: {selectedProduct.ProductName}\nTL thực tế: {netWeight:0.##} kg", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panelTong_Paint(object sender, PaintEventArgs e)
        {
            using (Graphics g = e.Graphics)
            {
                using (Font font = new Font("Times New Roman", 16, FontStyle.Bold))
                {
                    using (SolidBrush brush = new SolidBrush(Color.White))
                    {
                        string text = "Tổng";
                        SizeF textSize = g.MeasureString(text, font);
                        float x = (panelTong.Width - textSize.Width) / 2;
                        float y = (panelTong.Height - textSize.Height) / 2;
                        g.DrawString(text, font, brush, x, y);
                    }
                }
            }
        }

        private void panelTinhTong_Paint(object sender, PaintEventArgs e)
        {
            using (Graphics g = e.Graphics)
            {
                using (Font font = new Font("Times New Roman", 16, FontStyle.Bold))
                {
                    using (SolidBrush brush = new SolidBrush(Color.White))
                    {
                        string text = $"{totalNetWeight:0.##} kg";
                        SizeF textSize = g.MeasureString(text, font);
                        float x = (panelTinhTong.Width - textSize.Width) / 2;
                        float y = (panelTinhTong.Height - textSize.Height) / 2;
                        g.DrawString(text, font, brush, x, y);
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFactory.SelectedIndex == -1) return;

            string selectedFactory = cbFactory.SelectedItem?.ToString();

            try
            {
                if (selectedFactory == "Tất cả")
                {
                    filteredProducts = allProducts ?? new List<Product>();
                    _filteredProductViewModels = _allProductViewModels ?? new List<ProductViewModel>();
                }
                else
                {
                    filteredProducts = allProducts
                        ?.Where(p => p.Factory != null && p.Factory.Name == selectedFactory && p.Status == ProductStatus.Imported)
                        .ToList() ?? new List<Product>();

                    _filteredProductViewModels = _allProductViewModels
                        ?.Where(p => p.FactoryName == selectedFactory && p.Status == ProductStatus.Imported)
                        .ToList() ?? new List<ProductViewModel>();
                }

                totalNetWeight = filteredProducts.Sum(p => p.NetWeight);
                currentPage = 1;
                DisplayPage(currentPage);
                panelTinhTong.Invalidate();

                if (!filteredProducts.Any() && selectedFactory != "Tất cả")
                {
                    MessageBox.Show($"Không có sản phẩm nào thuộc nhà máy '{selectedFactory}' ở trạng thái Đã nhập kho!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lọc theo nhà máy: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dateStart_ValueChanged(object sender, EventArgs e)
        {
            // Không gọi lọc ngay, đợi nhấn btnLoc
        }

        private void dateStop_ValueChanged(object sender, EventArgs e)
        {
            // Không gọi lọc ngay, đợi nhấn btnLoc
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startDate = dateStart.Value.Date;
                DateTime stopDate = dateStop.Value.Date.AddDays(1).AddSeconds(-1);

                if (startDate > stopDate)
                {
                    MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string selectedFactory = cbFactory.SelectedItem?.ToString() ?? "Tất cả";

                _filteredProductViewModels = _allProductViewModels
                    .Where(p => p.CreateTime >= startDate && p.CreateTime <= stopDate)
                    .Where(p => selectedFactory == "Tất cả" || p.FactoryName == selectedFactory)
                    .Where(p => p.Status == ProductStatus.Imported)
                    .OrderByDescending(p => p.CreateTime)
                    .ToList();

                filteredProducts = allProducts
                    .Where(p => p.CreateTime >= startDate && p.CreateTime <= stopDate)
                    .Where(p => selectedFactory == "Tất cả" || (p.Factory != null && p.Factory.Name == selectedFactory))
                    .Where(p => p.Status == ProductStatus.Imported)
                    .ToList();

                totalNetWeight = filteredProducts.Sum(p => p.NetWeight);

                if (!_filteredProductViewModels.Any())
                {
                    MessageBox.Show("Không Có Danh Sách Tồn Kho!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                currentPage = 1;
                DisplayPage(currentPage);
                panelTinhTong.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lọc dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExecl_Click(object sender, EventArgs e)
        {
            try
            {
                if (_filteredProductViewModels == null || !_filteredProductViewModels.Any())
                {
                    MessageBox.Show("Không có dữ liệu để xuất Excel!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Tồn Kho");

                    var headers = new[] { "STT", "Tên sản phẩm", "Trọng lượng", "TL thực tế", "Tên Pallet", "TL Pallet", "Nhà cung cấp", "Nhà máy", "Ngày tạo", "Trạng thái" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cell(1, i + 1).Value = headers[i];
                        worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                        worksheet.Cell(1, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                    }

                    var sortedData = _filteredProductViewModels
                        .OrderByDescending(p => p.CreateTime)
                        .Select((pvm, index) => new
                        {
                            STT = index + 1,
                            pvm.ProductName,
                            pvm.Weight,
                            NetWeight = allProducts.FirstOrDefault(p => p.Id == pvm.Id)?.NetWeight ?? 0,
                            pvm.PalletName,
                            pvm.DefaultWeight,
                            pvm.SupplierName,
                            pvm.FactoryName,
                            CreateTime = pvm.CreateTime.ToString("dd/MM/yyyy HH:mm:ss"),
                            pvm.StatusDisplay
                        })
                        .ToList();

                    for (int i = 0; i < sortedData.Count; i++)
                    {
                        var row = sortedData[i];
                        worksheet.Cell(i + 2, 1).Value = row.STT;
                        worksheet.Cell(i + 2, 2).Value = row.ProductName;
                        worksheet.Cell(i + 2, 3).Value = row.Weight;
                        worksheet.Cell(i + 2, 4).Value = row.NetWeight;
                        worksheet.Cell(i + 2, 4).Style.NumberFormat.Format = "0.##";
                        worksheet.Cell(i + 2, 5).Value = row.PalletName;
                        worksheet.Cell(i + 2, 6).Value = row.DefaultWeight;
                        worksheet.Cell(i + 2, 6).Style.NumberFormat.Format = "0.##";
                        worksheet.Cell(i + 2, 7).Value = row.SupplierName;
                        worksheet.Cell(i + 2, 8).Value = row.FactoryName;
                        worksheet.Cell(i + 2, 9).Value = row.CreateTime;
                        worksheet.Cell(i + 2, 10).Value = row.StatusDisplay;
                    }

                    int totalRow = sortedData.Count + 2;
                    worksheet.Cell(totalRow, 3).Value = "Tổng TL thực tế";
                    worksheet.Cell(totalRow, 3).Style.Font.Bold = true;
                    worksheet.Cell(totalRow, 4).Value = totalNetWeight;
                    worksheet.Cell(totalRow, 4).Style.Font.Bold = true;
                    worksheet.Cell(totalRow, 4).Style.NumberFormat.Format = "0.##";
                    worksheet.Cell(totalRow, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                    var range = worksheet.Range(1, 1, totalRow, headers.Length);
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Columns().AdjustToContents();

                    using (var saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                        saveFileDialog.FileName = $"TonKho_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                        saveFileDialog.Title = "Lưu tệp Excel";

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            using (var stream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write))
                            {
                                workbook.SaveAs(stream);
                            }
                            MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất Excel: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnIn_Click(object sender, EventArgs e)
        {
            if (isProcessing)
            {
                MessageBox.Show("Đang xử lý, vui lòng chờ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (selectedProduct == null)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm từ bảng trước khi in!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isProcessing = true;
            Button btn = sender as Button;
            btn.Enabled = false;

            string tempFilePath = null;

            try
            {
                var productEntity = allProducts.FirstOrDefault(p => p.Id == selectedProduct.Id);
                if (productEntity == null)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm trong danh sách!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string productName = selectedProduct.ProductName;
                double netWeight = productEntity.NetWeight;
                string supplierName = selectedProduct.SupplierName;
                string palletName = selectedProduct.PalletName;
                double defaultWeight = selectedProduct.DefaultWeight;
                string createTime = selectedProduct.CreateTime.ToString("dd/MM/yyyy HH:mm:ss");
                string barcode = productEntity.Barcode;

                if (string.IsNullOrEmpty(palletName))
                {
                    MessageBox.Show("Tên pallet không hợp lệ hoặc rỗng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine($"Lỗi: palletName rỗng hoặc null");
                    return;
                }
                Console.WriteLine($"PalletName được lấy: {palletName}");

                string resourceName = "LoHoiManager.Templates.PhieuNhapKhoTemplate.xlsx";
                using (Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        MessageBox.Show("Không tìm thấy file template trong tài nguyên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    tempFilePath = Path.Combine(Path.GetTempPath(), $"PhieuNhapKhoTemp_{Guid.NewGuid()}.xlsx");
                    using (FileStream fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
                    {
                        await stream.CopyToAsync(fileStream);
                    }

                    using (var package = new ExcelPackage(new FileInfo(tempFilePath)))
                    {
                        var worksheet = package.Workbook.Worksheets[0];

                        worksheet.Cells["A3"].Value = productName;
                        worksheet.Cells["A3"].Style.Font.Name = "Times New Roman";
                        worksheet.Cells["A3"].Style.Font.Size = 14;
                        worksheet.Cells["A3"].Style.Font.Bold = true;
                        worksheet.Cells["A3"].Style.WrapText = true;
                        worksheet.Cells["A3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        worksheet.Cells["B3"].Value = netWeight.ToString("F1") + " kg";
                        worksheet.Cells["B3"].Style.Font.Name = "Times New Roman";
                        worksheet.Cells["B3"].Style.Font.Size = 16;
                        worksheet.Cells["B3"].Style.Font.Bold = true;
                        worksheet.Cells["B3"].Style.WrapText = true;
                        worksheet.Cells["B3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["B3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        worksheet.Cells["C3"].Value = supplierName ?? "Chưa chọn nhà cung cấp";
                        worksheet.Cells["C3"].Style.Font.Name = "Times New Roman";
                        worksheet.Cells["C3"].Style.Font.Size = 14;
                        worksheet.Cells["C3"].Style.Font.Bold = true;
                        worksheet.Cells["C3"].Style.WrapText = true;
                        worksheet.Cells["C3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["C3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        worksheet.Cells["A4:B4"].Merge = true;
                        worksheet.Cells["A4"].Value = palletName;
                        worksheet.Cells["A4"].Style.Font.Name = "Times New Roman";
                        worksheet.Cells["A4"].Style.Font.Size = 16;
                        worksheet.Cells["A4"].Style.Font.Bold = true;
                        worksheet.Cells["A4"].Style.WrapText = true;
                        worksheet.Cells["A4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells["A4"].Style.Hidden = false;
                        worksheet.Cells["A4"].Style.Locked = false;
                        Console.WriteLine($"Đã gộp A4:B4 và gán palletName '{palletName}' vào ô A4");

                        worksheet.Cells["C4"].Value = defaultWeight.ToString("F1") + " kg";
                        worksheet.Cells["C4"].Style.Font.Name = "Times New Roman";
                        worksheet.Cells["C4"].Style.Font.Size = 16;
                        worksheet.Cells["C4"].Style.Font.Bold = true;
                        worksheet.Cells["C4"].Style.WrapText = true;
                        worksheet.Cells["C4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["C4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        worksheet.Cells["B5"].Value = createTime;
                        worksheet.Cells["B5"].Style.Font.Name = "Times New Roman";
                        worksheet.Cells["B5"].Style.Font.Size = 16;
                        worksheet.Cells["B5"].Style.Font.Bold = true;
                        worksheet.Cells["B5"].Style.WrapText = true;
                        worksheet.Cells["B5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["B5"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        try
                        {
                            Console.WriteLine($"Creating barcode for: {barcode}");
                            var barcodeFactory = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                            var barcodeImage = barcodeFactory.Draw(barcode, 40);

                            var finalImage = new Bitmap(barcodeImage.Width, 50);
                            using (var graphics = Graphics.FromImage(finalImage))
                            {
                                graphics.Clear(Color.White);
                                using (var font = new Font("Times New Roman", 8))
                                {
                                    var textSize = graphics.MeasureString(barcode, font);
                                    graphics.DrawString(barcode, font, Brushes.Black, (barcodeImage.Width - textSize.Width) / 2, 0);
                                }
                                graphics.DrawImage(barcodeImage, 0, 12);
                            }

                            using (var ms = new MemoryStream())
                            {
                                finalImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                ms.Position = 0;

                                var picture = worksheet.Drawings.AddPicture("Barcode", ms);
                                int barcodeWidth = 250;
                                int barcodeHeight = 50;
                                picture.SetSize(barcodeWidth, barcodeHeight);

                                double colAWidth = worksheet.Column(1).Width * 7;
                                double colBWidth = worksheet.Column(2).Width * 7;
                                double colCWidth = worksheet.Column(3).Width * 7;
                                double totalWidth = colAWidth + colBWidth + colCWidth;
                                double offsetX = (totalWidth - barcodeWidth) / 2 - colAWidth;

                                picture.SetPosition(5, 5, 1, (int)offsetX);
                                Console.WriteLine($"Barcode image added to cell B6, centered across A6:C6 with offsetX: {offsetX}, offsetY: 5");
                            }
                        }
                        catch (Exception barcodeEx)
                        {
                            MessageBox.Show($"Lỗi khi tạo mã vạch: {barcodeEx.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Console.WriteLine($"Lỗi tạo mã vạch: {barcodeEx.Message}");
                            return;
                        }

                        await package.SaveAsync();
                    }
                }

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = tempFilePath,
                    Verb = "print",
                    UseShellExecute = true,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                Process process = Process.Start(psi);
                if (process == null)
                {
                    MessageBox.Show("Không thể khởi động quá trình in. Vui lòng kiểm tra cài đặt máy in hoặc ứng dụng Excel!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                await Task.Delay(1000);
                try
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi kết thúc process: {ex.Message}");
                }

                MessageBox.Show("Đã in Phiếu Nhập Kho thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi in phiếu nhập kho: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!string.IsNullOrEmpty(tempFilePath) && File.Exists(tempFilePath))
                {
                    try
                    {
                        File.Delete(tempFilePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi khi xóa file tạm: {ex.Message}");
                    }
                }
                isProcessing = false;
                btn.Enabled = true;
            }
        }
    }
}