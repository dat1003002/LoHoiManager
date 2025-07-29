using System;
using System.Drawing;
using System.Windows.Forms;
using LoHoiManager.Data;
using LoHoiManager.model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace LoHoiManager
{
    public partial class Nhacungcap : Form
    {
        private int currentPage = 1; // Trang hiện tại
        private readonly int pageSize = 20; // Số bản ghi mỗi trang
        private List<Supplier> allSuppliers; // Danh sách tất cả nhà cung cấp
        private List<Supplier> filteredSuppliers; // Danh sách nhà cung cấp sau khi lọc
        private Label lblPageInfo; // Nhãn hiển thị thông tin phân trang
        private Button btnPrevious, btnNext; // Nút chuyển trang trước/sau

        public Nhacungcap()
        {
            InitializeComponent();
            InitializeFormProperties();
            InitializeControls();
            InitializeEvents();
            LoadSupplierDataAsync();
        }

        private void InitializeFormProperties()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MinimumSize = new Size(800, 600);
        }

        private void InitializeControls()
        {
            lbnhacc.AutoSize = false;
            lbnhacc.TextAlign = ContentAlignment.MiddleCenter;
            lbnhacc.Dock = DockStyle.Fill;
            header.Controls.Add(lbnhacc);

            datanhacc.ScrollBars = ScrollBars.Vertical;
            datanhacc.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            datanhacc.RowHeadersVisible = true;
            datanhacc.RowHeadersWidth = 60;

            btnPrevious = new Button { Text = "Previous", Size = new Size(100, 30) };
            btnNext = new Button { Text = "Next", Size = new Size(100, 30) };
            lblPageInfo = new Label { Text = "Trang 1", AutoSize = true };

            this.Controls.Add(btnPrevious);
            this.Controls.Add(btnNext);
            this.Controls.Add(lblPageInfo);
        }

        private void InitializeEvents()
        {
            btnPrevious.Click += (s, e) => ChangePage(-1);
            btnNext.Click += (s, e) => ChangePage(1);
            this.Load += (s, e) => CenterDataGridView();
            this.Resize += (s, e) => CenterDataGridView();
        }

        private async Task LoadSupplierDataAsync()
        {
            try
            {
                using (var dbContext = new LoHoiDbContext())
                {
                    allSuppliers = await dbContext.Suppliers
                        .AsNoTracking()
                        .ToListAsync();

                    filteredSuppliers = allSuppliers;
                    currentPage = 1;
                    DisplayPage(currentPage);
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
            if (filteredSuppliers == null || !filteredSuppliers.Any())
            {
                datanhacc.DataSource = null;
                lblPageInfo.Text = "Không tìm thấy kết quả";
                currentPage = 1;
                return;
            }

            int totalPages = (int)Math.Ceiling((double)filteredSuppliers.Count / pageSize);
            currentPage = Math.Clamp(page, 1, totalPages);

            var pageData = filteredSuppliers.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            datanhacc.DataSource = pageData;

            for (int i = 0; i < pageData.Count; i++)
            {
                int stt = (currentPage - 1) * pageSize + i + 1;
                datanhacc.Rows[i].HeaderCell.Value = stt.ToString();
            }

            foreach (DataGridViewColumn column in datanhacc.Columns)
            {
                column.Visible = column.Name == "Manhacungcap" || column.Name == "Name" || column.Name == "CreateTime";
            }

            datanhacc.Columns["Manhacungcap"].HeaderText = "Mã nhà cung cấp";
            datanhacc.Columns["Name"].HeaderText = "Tên nhà cung cấp";
            datanhacc.Columns["CreateTime"].HeaderText = "Ngày tạo";

            Font headerFont = new Font(datanhacc.Font.FontFamily, 12, FontStyle.Bold);
            foreach (DataGridViewColumn column in datanhacc.Columns)
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
            datanhacc.Location = new Point(
                (this.ClientSize.Width - datanhacc.Width) / 2,
                this.ClientSize.Height - datanhacc.Height - verticalSpace
            );

            int controlY = datanhacc.Bottom + 10;
            btnPrevious.Location = new Point(datanhacc.Left, controlY);
            btnNext.Location = new Point(datanhacc.Right - btnNext.Width, controlY);
            lblPageInfo.Location = new Point(
                (datanhacc.Left + datanhacc.Right - lblPageInfo.Width) / 2,
                controlY
            );
        }

        private void ChangePage(int direction)
        {
            int totalPages = (int)Math.Ceiling((double)filteredSuppliers.Count / pageSize);
            currentPage = Math.Clamp(currentPage + direction, 1, totalPages > 0 ? totalPages : 1);
            DisplayPage(currentPage);
        }

        private void UpdatePaginationInfo()
        {
            int totalPages = (int)Math.Ceiling((double)filteredSuppliers.Count / pageSize);
            lblPageInfo.Text = filteredSuppliers.Any() ? $"Trang {currentPage}/{totalPages}" : "Không tìm thấy kết quả";
        }

        private async void btnadd_Click(object sender, EventArgs e)
        {
            string maNhaCungCap = txtmanhacc.Text.Trim();
            string tenNhaCungCap = texttennhacc.Text.Trim();

            if (string.IsNullOrEmpty(maNhaCungCap) || string.IsNullOrEmpty(tenNhaCungCap))
            {
                MessageBox.Show("Nhập đầy đủ mã và tên!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var dbContext = new LoHoiDbContext())
                {
                    if (await dbContext.Suppliers.AnyAsync(s => s.Manhacungcap == maNhaCungCap))
                    {
                        MessageBox.Show("Mã đã tồn tại!", "Cảnh báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    dbContext.Suppliers.Add(new Supplier
                    {
                        Manhacungcap = maNhaCungCap,
                        Name = tenNhaCungCap,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                        total = null
                    });

                    await dbContext.SaveChangesAsync();
                    MessageBox.Show("Thêm thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtmanhacc.Clear();
                    texttennhacc.Clear();
                    await LoadSupplierDataAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btndelete_Click(object sender, EventArgs e)
        {
            if (datanhacc.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một nhà cung cấp để xóa!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var selectedSupplier = (Supplier)datanhacc.SelectedRows[0].DataBoundItem;
                string maNhaCungCap = selectedSupplier.Manhacungcap;

                using (var dbContext = new LoHoiDbContext())
                {
                    var supplier = await dbContext.Suppliers
                        .FirstOrDefaultAsync(s => s.Manhacungcap == maNhaCungCap);

                    if (supplier == null)
                    {
                        MessageBox.Show($"Nhà cung cấp với mã {maNhaCungCap} không tồn tại!",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int relatedProductCount = await dbContext.Products
                        .CountAsync(p => p.SupplierId == supplier.Id);

                    if (relatedProductCount > 0)
                    {
                        MessageBox.Show($"Không thể xóa vì nhà cung cấp có {relatedProductCount} sản phẩm liên quan!",
                            "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var confirmResult = MessageBox.Show(
                        $"Bạn có chắc muốn xóa nhà cung cấp {maNhaCungCap}?",
                        "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (confirmResult != DialogResult.Yes)
                        return;

                    dbContext.Suppliers.Remove(supplier);
                    await dbContext.SaveChangesAsync();

                    MessageBox.Show($"Xóa nhà cung cấp {maNhaCungCap} thành công!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadSupplierDataAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa nhà cung cấp: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void bntedit_Click(object sender, EventArgs e)
        {
            string maNhaCungCap = txtmanhacc.Text.Trim();
            string tenNhaCungCapMoi = texttennhacc.Text.Trim();

            if (string.IsNullOrEmpty(maNhaCungCap) || string.IsNullOrEmpty(tenNhaCungCapMoi))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ mã và tên nhà cung cấp!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var dbContext = new LoHoiDbContext())
                {
                    var supplier = await dbContext.Suppliers
                        .FirstOrDefaultAsync(s => s.Manhacungcap == maNhaCungCap);

                    if (supplier == null)
                    {
                        MessageBox.Show($"Nhà cung cấp với mã {maNhaCungCap} không tồn tại!",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (supplier.Name == tenNhaCungCapMoi)
                    {
                        MessageBox.Show("Tên mới trùng với tên hiện tại, không cần chỉnh sửa!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    bool isNameTaken = await dbContext.Suppliers
                        .AnyAsync(s => s.Name == tenNhaCungCapMoi && s.Manhacungcap != maNhaCungCap);

                    if (isNameTaken)
                    {
                        MessageBox.Show($"Tên '{tenNhaCungCapMoi}' đã được sử dụng bởi nhà cung cấp khác!",
                            "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string tenCu = supplier.Name;
                    supplier.Name = tenNhaCungCapMoi;
                    supplier.UpdateTime = DateTime.Now;

                    await dbContext.SaveChangesAsync();

                    MessageBox.Show($"Chỉnh sửa nhà cung cấp {maNhaCungCap} thành công: '{tenCu}' thành '{tenNhaCungCapMoi}'!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtmanhacc.Clear();
                    texttennhacc.Clear();
                    await LoadSupplierDataAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chỉnh sửa nhà cung cấp: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void txtmanhacc_TextChanged(object sender, EventArgs e)
        {
            string maNhaCungCap = txtmanhacc.Text.Trim();
            if (!string.IsNullOrEmpty(maNhaCungCap))
            {
                using (var dbContext = new LoHoiDbContext())
                {
                    bool exists = await dbContext.Suppliers.AnyAsync(s => s.Manhacungcap == maNhaCungCap);
                }
            }
        }

        private void texttennhacc_TextChanged(object sender, EventArgs e)
        {
        }

        private void datanhacc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedSupplier = (Supplier)datanhacc.Rows[e.RowIndex].DataBoundItem;
                txtmanhacc.Text = selectedSupplier.Manhacungcap;
                texttennhacc.Text = selectedSupplier.Name;
            }
        }

        private void textsearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = textsearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                filteredSuppliers = allSuppliers;
            }
            else
            {
                filteredSuppliers = allSuppliers
                    .Where(s => s.Manhacungcap.ToLower().Contains(searchText) ||
                                s.Name.ToLower().Contains(searchText))
                    .ToList();
            }

            currentPage = 1;
            DisplayPage(currentPage);
        }

        // Chức năng đóng form khi nhấn nút btnclose
        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng form hiện tại
        }
    }
}