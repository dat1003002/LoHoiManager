using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LoHoiManager.Data;
using LoHoiManager.model;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using System.IO;

namespace LoHoiManager
{
    public partial class XuatKho : Form
    {
        private int currentPage = 1;
        private readonly int pageSize = 20;
        private List<ExportProduct> allExportProducts;
        private List<ExportProduct> filteredExportProducts;
        private double totalNetWeight = 0;
        private Label lblPageInfo;
        private Button btnPrevious, btnNext;
        private bool isFormLoaded = false;
        private bool isExporting = false; // Biến cờ để kiểm soát trạng thái xuất file

        public XuatKho()
        {
            InitializeComponent();
            InitializeFormProperties();
            InitializeControls();
            InitializeEvents();
            LoadDataAsync(); // Tải dữ liệu tuần tự
        }

        private void InitializeFormProperties()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.MinimumSize = new Size(800, 600);
        }

        private void InitializeControls()
        {
            // Cấu hình DataGridView
            dataExport.ScrollBars = ScrollBars.Vertical;
            dataExport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataExport.RowHeadersVisible = true;
            dataExport.RowHeadersWidth = 60;

            // Cấu hình ComboBox cbFactory
            cbFactory.DropDownStyle = ComboBoxStyle.DropDownList;

            // Tạo các nút và nhãn phân trang
            btnPrevious = new Button { Text = "Previous", Size = new Size(100, 30) };
            btnNext = new Button { Text = "Next", Size = new Size(100, 30) };
            lblPageInfo = new Label { Text = "Trang 1", AutoSize = true };

            this.Controls.Add(btnPrevious);
            this.Controls.Add(btnNext);
            this.Controls.Add(lblPageInfo);
        }

        private void InitializeEvents()
        {
            // Bỏ gắn sự kiện trước đó để tránh trùng lặp
            btnPrevious.Click -= (s, e) => ChangePage(-1);
            btnNext.Click -= (s, e) => ChangePage(1);
            this.Load -= (s, e) => CenterDataGridView();
            this.Resize -= (s, e) => CenterDataGridView();
            dataExport.CellContentClick -= dataExport_CellContentClick;
            paneTong.Paint -= paneTong_Paint;
            panelTinhTong.Paint -= panelTinhTong_Paint;
            dateStart.ValueChanged -= dateStart_ValueChanged;
            dateStop.ValueChanged -= dateStop_ValueChanged;
            cbFactory.SelectedIndexChanged -= cbFactory_SelectedIndexChanged;
            btnClose.Click -= btnClose_Click;
            btnLoc.Click -= btnLoc_Click;
            btnExecl.Click -= btnExecl_Click; // Bỏ gắn sự kiện Click trước đó

            // Gắn lại các sự kiện
            btnPrevious.Click += (s, e) => ChangePage(-1);
            btnNext.Click += (s, e) => ChangePage(1);
            this.Load += (s, e) => CenterDataGridView();
            this.Resize += (s, e) => CenterDataGridView();
            dataExport.CellContentClick += dataExport_CellContentClick;
            paneTong.Paint += paneTong_Paint;
            panelTinhTong.Paint += panelTinhTong_Paint;
            dateStart.ValueChanged += dateStart_ValueChanged;
            dateStop.ValueChanged += dateStop_ValueChanged;
            cbFactory.SelectedIndexChanged += cbFactory_SelectedIndexChanged;
            btnClose.Click += btnClose_Click;
            btnLoc.Click += btnLoc_Click;
            btnExecl.Click += btnExecl_Click; // Gắn lại sự kiện Click
        }

        private async Task LoadDataAsync()
        {
            try
            {
                await LoadExportProductDataAsync();

                await LoadFactoryDataAsync();

                isFormLoaded = true;

                if (cbFactory.SelectedIndex != -1)
                {
                    cbFactory_SelectedIndexChanged(cbFactory, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo form: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadFactoryDataAsync()
        {
            try
            {
                using (var dbContext = new LoHoiDbContext())
                {
                    var factories = await dbContext.Factories
                        .AsNoTracking()
                        .Select(f => new { f.Id, f.Name })
                        .OrderBy(f => f.Name)
                        .ToListAsync();

                    // Add "All Factories" option
                    var factoryList = new List<object> { new { Id = 0, Name = "Tất cả nhà máy" } };
                    factoryList.AddRange(factories);

                    cbFactory.DataSource = factoryList;
                    cbFactory.DisplayMember = "Name";
                    cbFactory.ValueMember = "Id";
                    cbFactory.SelectedValue = 0; // Chọn "Tất cả nhà máy" mặc định
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu nhà máy: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadExportProductDataAsync()
        {
            try
            {
                using (var dbContext = new LoHoiDbContext())
                {
                    allExportProducts = await dbContext.ExportProducts
                        .AsNoTracking()
                        .Include(p => p.Supplier)
                        .Include(p => p.Pallet)
                        .Include(p => p.Factory)
                        .Where(p => p.Status == ProductStatus.Exported)
                        .OrderByDescending(p => p.ExportTime)
                        .ToListAsync();

                    filteredExportProducts = new List<ExportProduct>(); // Khởi tạo rỗng ban đầu
                    totalNetWeight = 0; // Tổng ban đầu là 0
                    currentPage = 1;
                    DisplayPage(currentPage);
                    panelTinhTong.Invalidate(); // Vẽ lại panelTinhTong
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayPage(int page)
        {
            if (filteredExportProducts == null || !filteredExportProducts.Any())
            {
                dataExport.DataSource = null;
                lblPageInfo.Text = "Không tìm thấy kết quả";
                currentPage = 1;
                return;
            }

            int totalPages = (int)Math.Ceiling((double)filteredExportProducts.Count / pageSize);
            currentPage = Math.Clamp(page, 1, totalPages);

            var pageData = filteredExportProducts
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new
                {
                    Id = p.Id,
                    ProductName = p.Name,
                    Weight = p.Weight.ToString(),
                    NetWeight = p.NetWeight,
                    PalletName = p.Pallet?.Name ?? "N/A",
                    SupplierName = p.Supplier?.Name ?? "N/A",
                    FactoryName = p.Factory?.Name ?? "N/A",
                    Status = p.Status == ProductStatus.Exported ? "Đã xuất kho" : "Đã nhập kho",
                    ExportTime = p.ExportTime.ToString("dd/MM/yyyy HH:mm:ss")
                })
                .ToList();

            dataExport.DataSource = pageData;

            // Cập nhật số thứ tự (STT) cho hàng
            for (int i = 0; i < pageData.Count; i++)
            {
                int stt = (currentPage - 1) * pageSize + i + 1;
                dataExport.Rows[i].HeaderCell.Value = stt.ToString();
            }

            // Cấu hình các cột hiển thị theo thứ tự yêu cầu
            dataExport.Columns["Id"].Visible = false;
            dataExport.Columns["ProductName"].HeaderText = "Tên sản phẩm";
            dataExport.Columns["ProductName"].DisplayIndex = 0;
            dataExport.Columns["Weight"].HeaderText = "Trọng Lượng";
            dataExport.Columns["Weight"].DisplayIndex = 1;
            dataExport.Columns["NetWeight"].HeaderText = "TL Thực Tế";
            dataExport.Columns["NetWeight"].DisplayIndex = 2;
            dataExport.Columns["PalletName"].HeaderText = "Tên Pallet";
            dataExport.Columns["PalletName"].DisplayIndex = 3;
            dataExport.Columns["SupplierName"].HeaderText = "Nhà cung cấp";
            dataExport.Columns["SupplierName"].DisplayIndex = 4;
            dataExport.Columns["FactoryName"].HeaderText = "Nhà máy";
            dataExport.Columns["FactoryName"].DisplayIndex = 5;
            dataExport.Columns["Status"].HeaderText = "Trạng thái";
            dataExport.Columns["Status"].DisplayIndex = 6;
            dataExport.Columns["ExportTime"].HeaderText = "Thời gian xuất kho";
            dataExport.Columns["ExportTime"].DisplayIndex = 7;

            // Định dạng font cho tiêu đề cột
            Font headerFont = new Font(dataExport.Font.FontFamily, 12, FontStyle.Bold);
            foreach (DataGridViewColumn column in dataExport.Columns)
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
            dataExport.Location = new Point(
                (this.ClientSize.Width - dataExport.Width) / 2,
                this.ClientSize.Height - dataExport.Height - verticalSpace
            );

            int controlY = dataExport.Bottom + 10;
            btnPrevious.Location = new Point(dataExport.Left, controlY);
            btnNext.Location = new Point(dataExport.Right - btnNext.Width, controlY);
            lblPageInfo.Location = new Point(
                (dataExport.Left + dataExport.Right - lblPageInfo.Width) / 2,
                controlY
            );
        }

        private void ChangePage(int direction)
        {
            int totalPages = (int)Math.Ceiling((double)filteredExportProducts.Count / pageSize);
            currentPage = Math.Clamp(currentPage + direction, 1, totalPages > 0 ? totalPages : 1);
            DisplayPage(currentPage);
        }

        private void UpdatePaginationInfo()
        {
            int totalPages = (int)Math.Ceiling((double)filteredExportProducts.Count / pageSize);
            lblPageInfo.Text = filteredExportProducts.Any() ? $"Trang {currentPage}/{totalPages}" : "Không tìm thấy kết quả";
        }

        private void dataExport_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dataExport.Rows[e.RowIndex];
                var productName = selectedRow.Cells["ProductName"].Value?.ToString();
                var status = selectedRow.Cells["Status"].Value?.ToString();

                MessageBox.Show($"Sản phẩm: {productName}\nTrạng thái: {status}",
                    "Thông tin sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void paneTong_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            string text = "Tổng";
            Font font = new Font("Times New Roman", 16, FontStyle.Bold);
            Brush brush = new SolidBrush(Color.White);
            SizeF textSize = g.MeasureString(text, font);
            float x = (paneTong.Width - textSize.Width) / 2;
            float y = (paneTong.Height - textSize.Height) / 2;
            g.DrawString(text, font, brush, x, y);
            font.Dispose();
            brush.Dispose();
        }

        private void panelTinhTong_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            string text = $"{totalNetWeight:F1} kg";
            Font font = new Font("Times New Roman", 16, FontStyle.Bold);
            Brush brush = new SolidBrush(Color.White);
            SizeF textSize = g.MeasureString(text, font);
            float x = (panelTinhTong.Width - textSize.Width) / 2;
            float y = (panelTinhTong.Height - textSize.Height) / 2;
            g.DrawString(text, font, brush, x, y);
            font.Dispose();
            brush.Dispose();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dateStart_ValueChanged(object sender, EventArgs e)
        {
            // Không gọi FilterExportProducts ở đây để đợi nhấn btnLoc
        }

        private void dateStop_ValueChanged(object sender, EventArgs e)
        {
            // Không gọi FilterExportProducts ở đây để đợi nhấn btnLoc
        }

        private void cbFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Chỉ xử lý sự kiện sau khi form đã tải xong
            if (!isFormLoaded || cbFactory.SelectedIndex == -1) return;

            try
            {
                string selectedFactoryName = cbFactory.SelectedItem != null
                    ? ((dynamic)cbFactory.SelectedItem).Name
                    : "Tất cả nhà máy";

                if (selectedFactoryName == "Tất cả nhà máy")
                {
                    filteredExportProducts = allExportProducts;
                }
                else
                {
                    filteredExportProducts = allExportProducts
                        .Where(p => p.Factory != null && p.Factory.Name == selectedFactoryName)
                        .ToList();
                }

                totalNetWeight = filteredExportProducts.Sum(p => p.NetWeight); // Tính tổng NetWeight
                currentPage = 1;
                DisplayPage(currentPage);
                panelTinhTong.Invalidate(); // Vẽ lại panelTinhTong
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lọc theo nhà máy: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterExportProducts()
        {
            try
            {
                DateTime startDate = dateStart.Value.Date;
                DateTime stopDate = dateStop.Value.Date.AddDays(1).AddSeconds(-1); // Bao gồm toàn bộ ngày kết thúc

                if (startDate > stopDate)
                {
                    MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy ID nhà máy được chọn
                int selectedFactoryId = cbFactory.SelectedValue != null ? Convert.ToInt32(cbFactory.SelectedValue) : 0;

                // Lọc theo khoảng thời gian và nhà máy
                filteredExportProducts = allExportProducts
                    .Where(p => p.ExportTime >= startDate && p.ExportTime <= stopDate)
                    .Where(p => selectedFactoryId == 0 || p.FactoryId == selectedFactoryId)
                    .OrderByDescending(p => p.ExportTime)
                    .ToList();

                // Tính tổng NetWeight
                totalNetWeight = filteredExportProducts.Sum(p => p.NetWeight);

                currentPage = 1;
                DisplayPage(currentPage);
                panelTinhTong.Invalidate(); // Vẽ lại panelTinhTong
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lọc dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            FilterExportProducts();
        }

        private void btnExecl_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu đang xử lý thì không cho phép gọi lại
            if (isExporting)
            {
                MessageBox.Show("Đang xử lý xuất file, vui lòng chờ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                isExporting = true; // Đặt cờ đang xử lý

                if (filteredExportProducts == null || !filteredExportProducts.Any())
                {
                    MessageBox.Show("Không có dữ liệu để xuất Excel!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Danh sách xuất kho");

                    // Sắp xếp tiêu đề cột theo thứ tự yêu cầu
                    var headers = new[] { "STT", "Tên sản phẩm", "Trọng Lượng", "TL Thực Tế",
                        "Tên Pallet", "Nhà cung cấp", "Nhà máy", "Trạng thái", "Thời gian xuất kho" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cell(1, i + 1).Value = headers[i];
                        worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                        worksheet.Cell(1, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                    }

                    var sortedData = filteredExportProducts
                        .OrderByDescending(p => p.ExportTime)
                        .Select((product, index) => new
                        {
                            STT = index + 1,
                            product.Name,
                            Weight = product.Weight.ToString(),
                            product.NetWeight,
                            PalletName = product.Pallet?.Name ?? "N/A",
                            SupplierName = product.Supplier?.Name ?? "N/A",
                            FactoryName = product.Factory?.Name ?? "N/A",
                            StatusDisplay = product.Status == ProductStatus.Exported ? "Đã xuất kho" : "Đã nhập kho",
                            ExportTime = product.ExportTime.ToString("dd/MM/yyyy HH:mm:ss")
                        })
                        .ToList();

                    for (int i = 0; i < sortedData.Count; i++)
                    {
                        var row = sortedData[i];
                        worksheet.Cell(i + 2, 1).Value = row.STT;
                        worksheet.Cell(i + 2, 2).Value = row.Name;
                        worksheet.Cell(i + 2, 3).Value = row.Weight;
                        worksheet.Cell(i + 2, 4).Value = row.NetWeight;
                        worksheet.Cell(i + 2, 4).Style.NumberFormat.Format = "0.##";
                        worksheet.Cell(i + 2, 5).Value = row.PalletName;
                        worksheet.Cell(i + 2, 6).Value = row.SupplierName;
                        worksheet.Cell(i + 2, 7).Value = row.FactoryName;
                        worksheet.Cell(i + 2, 8).Value = row.StatusDisplay;
                        worksheet.Cell(i + 2, 9).Value = row.ExportTime;
                    }

                    int totalRow = sortedData.Count + 2;
                    worksheet.Cell(totalRow, 3).Value = "Tổng TL Thực Tế";
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
                        saveFileDialog.FileName = $"DanhSachXuatKho_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                        saveFileDialog.Title = "Lưu tệp Excel";

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            string filePath = saveFileDialog.FileName;

                            // Kiểm tra xem file có đang bị khóa không
                            try
                            {
                                if (File.Exists(filePath))
                                {
                                    using (File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { }
                                }
                            }
                            catch (IOException)
                            {
                                MessageBox.Show("File đã xuất trước đó đang được mở. Vui lòng đóng file Excel trước khi xuất file mới!", "Cảnh báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                            {
                                workbook.SaveAs(stream);
                            }
                            MessageBox.Show("Xuất file Excel thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất Excel: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isExporting = false; // Đặt lại cờ sau khi xử lý xong
            }
        }
    }
}