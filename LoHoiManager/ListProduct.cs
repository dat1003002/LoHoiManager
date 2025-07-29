using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LoHoiManager.Data;
using LoHoiManager.model;
using Microsoft.EntityFrameworkCore;

namespace LoHoiManager
{
    public partial class ListProduct : Form
    {
        private int currentPage = 1;
        private readonly int pageSize = 20;
        private List<ProductType> allProductTypes;
        private Label lblPageInfo;
        private Button btnPrevious, btnNext;

        public ListProduct()
        {
            InitializeComponent();

            // Form settings
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(800, 600);

            // DataGridView settings
            dataProductype.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataProductype.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataProductype.ReadOnly = true;
            dataProductype.RowHeadersVisible = true;
            dataProductype.RowHeadersWidth = 60;

            // Initialize controls and events
            InitializeControls();
            InitializeEvents();
            LoadProductTypeData();
        }

        private void InitializeControls()
        {
            // Initialize pagination controls
            btnPrevious = new Button { Text = "Previous", Size = new Size(100, 30) };
            btnNext = new Button { Text = "Next", Size = new Size(100, 30) };
            lblPageInfo = new Label { Text = "Trang 1", AutoSize = true };

            Controls.Add(btnPrevious);
            Controls.Add(btnNext);
            Controls.Add(lblPageInfo);
        }

        private void InitializeEvents()
        {
            btnPrevious.Click += (s, e) => ChangePage(-1);
            btnNext.Click += (s, e) => ChangePage(1);
            Load += (s, e) => CenterDataGridView();
            Resize += (s, e) => CenterDataGridView();
            dataProductype.DataBindingComplete += (s, e) => dataProductype.ClearSelection();
            dataProductype.SelectionChanged += DataProductype_SelectionChanged;
        }

        private void LoadProductTypeData()
        {
            try
            {
                using (var context = new LoHoiDbContext())
                {
                    allProductTypes = context.ProductTypes.ToList();
                    currentPage = 1;
                    DisplayPage(currentPage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayPage(int page)
        {
            if (allProductTypes == null || !allProductTypes.Any())
            {
                dataProductype.DataSource = null;
                lblPageInfo.Text = "Không tìm thấy kết quả";
                currentPage = 1;
                textname.Clear();
                return;
            }

            int totalPages = (int)Math.Ceiling((double)allProductTypes.Count / pageSize);
            currentPage = Math.Clamp(page, 1, totalPages);
            dataProductype.CurrentCell = null;

            var pageData = allProductTypes
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(pt => new
                {
                    pt.Name,
                    pt.CreateTime,
                    pt.UpdateTime
                })
                .ToList();

            dataProductype.DataSource = pageData;

            // Add row number (STT) to row headers
            for (int i = 0; i < pageData.Count; i++)
            {
                int stt = (currentPage - 1) * pageSize + i + 1;
                dataProductype.Rows[i].HeaderCell.Value = stt.ToString();
            }

            // Customize column headers
            dataProductype.Columns["Name"].HeaderText = "Loại Củi";
            dataProductype.Columns["CreateTime"].HeaderText = "Thời Gian Tạo";
            dataProductype.Columns["UpdateTime"].HeaderText = "Thời Gian Cập Nhật";

            // Format column headers
            var headerFont = new Font(dataProductype.Font.FontFamily, 12, FontStyle.Bold);
            foreach (DataGridViewColumn column in dataProductype.Columns)
            {
                column.HeaderCell.Style.Font = headerFont;
                column.HeaderCell.Style.BackColor = SystemColors.Control;
            }

            // Clear any selection to prevent automatic selection of the first row
            dataProductype.ClearSelection();
            textname.Clear(); // Ensure textbox is empty unless a row is explicitly selected
            UpdatePaginationInfo();
        }

        private void CenterDataGridView()
        {
            const int verticalSpace = 45;
            dataProductype.Location = new Point(
                (ClientSize.Width - dataProductype.Width) / 2,
                ClientSize.Height - dataProductype.Height - verticalSpace
            );

            int controlY = dataProductype.Bottom + 10;
            btnPrevious.Location = new Point(dataProductype.Left, controlY);
            btnNext.Location = new Point(dataProductype.Right - btnNext.Width, controlY);
            lblPageInfo.Location = new Point(
                (dataProductype.Left + dataProductype.Right - lblPageInfo.Width) / 2,
                controlY
            );
        }

        private void ChangePage(int direction)
        {
            int totalPages = (int)Math.Ceiling((double)allProductTypes.Count / pageSize);
            currentPage = Math.Clamp(currentPage + direction, 1, totalPages > 0 ? totalPages : 1);
            DisplayPage(currentPage);
        }

        private void DataProductype_SelectionChanged(object sender, EventArgs e)
        {
            // Only update textname if a row is explicitly selected
            if (dataProductype.SelectedRows.Count > 0)
            {
                var selectedRow = dataProductype.SelectedRows[0];
                string productTypeName = selectedRow.Cells["Name"].Value?.ToString() ?? string.Empty;
                textname.Text = productTypeName;
            }
            else
            {
                textname.Clear(); // Clear textbox if no row is selected
            }
        }

        private void UpdatePaginationInfo()
        {
            int totalPages = (int)Math.Ceiling((double)allProductTypes.Count / pageSize);
            lblPageInfo.Text = allProductTypes.Any() ? $"Trang {currentPage}/{totalPages}" : "Không tìm thấy kết quả";
        }

        private void dataProductype_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Optional: Refresh data on cell click (can be removed if not needed)
            DisplayPage(currentPage);
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textname.Text))
            {
                MessageBox.Show("Vui lòng nhập tên loại sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var context = new LoHoiDbContext())
                {
                    string inputName = textname.Text.Trim();
                    if (context.ProductTypes.Any(pt => pt.Name.ToLower() == inputName.ToLower()))
                    {
                        MessageBox.Show($"Tên loại sản phẩm '{inputName}' đã tồn tại! Vui lòng chọn tên khác.",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var newProductType = new ProductType
                    {
                        Name = inputName,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now
                    };

                    context.ProductTypes.Add(newProductType);
                    context.SaveChanges();
                    LoadProductTypeData();

                    textname.Clear();

                    MessageBox.Show("Thêm loại sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm loại sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textname_TextChanged(object sender, EventArgs e)
        {
            // No search functionality, used only for input
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textname.Text))
            {
                MessageBox.Show("Vui lòng chọn một loại sản phẩm để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var context = new LoHoiDbContext())
                {
                    string productTypeName = textname.Text.Trim();
                    var productType = context.ProductTypes.FirstOrDefault(pt => pt.Name == productTypeName);

                    if (productType == null)
                    {
                        MessageBox.Show($"Loại sản phẩm '{productTypeName}' không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Xác nhận trước khi xóa
                    var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa loại sản phẩm '{productTypeName}' không?",
                        "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        context.ProductTypes.Remove(productType);
                        context.SaveChanges();
                        LoadProductTypeData(); // Tải lại danh sách
                        textname.Clear(); // Xóa nội dung textbox
                        MessageBox.Show("Xóa loại sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa loại sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnedit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textname.Text))
            {
                MessageBox.Show("Vui lòng chọn loại sản phẩm để chỉnh sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var context = new LoHoiDbContext())
                {
                    string originalName = dataProductype.SelectedRows[0].Cells["Name"].Value.ToString();
                    string newName = textname.Text.Trim();

                    var productType = context.ProductTypes.FirstOrDefault(pt => pt.Name == originalName);
                    if (productType == null)
                    {
                        MessageBox.Show($"Loại sản phẩm '{originalName}' không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (context.ProductTypes.Any(pt => pt.Name.ToLower() == newName.ToLower() && pt.Name != originalName))
                    {
                        MessageBox.Show($"Tên '{newName}' đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    productType.Name = newName;
                    productType.UpdateTime = DateTime.Now;
                    context.SaveChanges();
                    LoadProductTypeData();
                    textname.Clear();
                    MessageBox.Show("Chỉnh sửa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chỉnh sửa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textsearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (var context = new LoHoiDbContext())
                {
                    string searchText = textsearch.Text.Trim().ToLower();
                    allProductTypes = context.ProductTypes
                        .Where(pt => string.IsNullOrEmpty(searchText) || pt.Name.ToLower().Contains(searchText))
                        .ToList();
                    currentPage = 1;
                    DisplayPage(currentPage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}