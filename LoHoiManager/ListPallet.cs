using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LoHoiManager.Data;
using LoHoiManager.model;
using Microsoft.EntityFrameworkCore;

namespace LoHoiManager
{
    public partial class ListPallet : Form
    {
        private int currentPage = 1;
        private readonly int pageSize = 20;
        private List<Pallet> allPallets;
        private List<Pallet> filteredPallets;
        private Label lblPageInfo;
        private Button btnPrevious, btnNext;
        private Pallet selectedPallet;

        public ListPallet()
        {
            InitializeComponent();
            InitializeFormProperties();
            InitializeControls();
            InitializeEvents();
            LoadPalletData();
        }

        private void InitializeFormProperties()
        {
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MinimumSize = new Size(800, 600);
        }

        private void InitializeControls()
        {
            // Thiết lập DataGridView
            dataPallet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataPallet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataPallet.ReadOnly = true;
            dataPallet.RowHeadersVisible = true;
            dataPallet.RowHeadersWidth = 60;

            // Khởi tạo controls phân trang
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
            dataPallet.DataBindingComplete += (s, e) => dataPallet.ClearSelection();
            dataPallet.SelectionChanged += DataPallet_SelectionChanged;
        }

        private void LoadPalletData()
        {
            try
            {
                using (var context = new LoHoiDbContext())
                {
                    allPallets = context.Pallets.Include("Products").ToList();
                    filteredPallets = allPallets;
                    currentPage = 1;
                    DisplayPage(currentPage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu Pallet: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayPage(int page)
        {
            if (filteredPallets == null || !filteredPallets.Any())
            {
                dataPallet.DataSource = null;
                lblPageInfo.Text = "Không tìm thấy kết quả";
                currentPage = 1;
                return;
            }

            int totalPages = (int)Math.Ceiling((double)filteredPallets.Count / pageSize);
            currentPage = Math.Clamp(page, 1, totalPages);
            dataPallet.CurrentCell = null;

            var pageData = filteredPallets
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new
                {
                    p.Name,
                    p.DefaultWeight,
                    p.CreateTime,
                    p.UpdateTime
                })
                .ToList();

            dataPallet.DataSource = pageData;

            // Thêm số thứ tự (STT) vào tiêu đề hàng
            for (int i = 0; i < pageData.Count; i++)
            {
                int stt = (currentPage - 1) * pageSize + i + 1;
                dataPallet.Rows[i].HeaderCell.Value = stt.ToString();
            }

            // Tùy chỉnh cột hiển thị
            dataPallet.Columns["Name"].HeaderText = "Tên Pallet";
            dataPallet.Columns["DefaultWeight"].HeaderText = "Trọng lượng Pallet";
            dataPallet.Columns["CreateTime"].HeaderText = "Thời gian tạo";
            dataPallet.Columns["UpdateTime"].HeaderText = "Thời gian cập nhật";

            // Định dạng tiêu đề cột
            var headerFont = new Font(dataPallet.Font.FontFamily, 12, FontStyle.Bold);
            foreach (DataGridViewColumn column in dataPallet.Columns)
            {
                column.HeaderCell.Style.Font = headerFont;
                column.HeaderCell.Style.BackColor = SystemColors.Control;
            }

            dataPallet.ClearSelection();
            UpdatePaginationInfo();
        }

        private void CenterDataGridView()
        {
            const int verticalSpace = 45;
            dataPallet.Location = new Point(
                (ClientSize.Width - dataPallet.Width) / 2,
                ClientSize.Height - dataPallet.Height - verticalSpace
            );

            int controlY = dataPallet.Bottom + 10;
            btnPrevious.Location = new Point(dataPallet.Left, controlY);
            btnNext.Location = new Point(dataPallet.Right - btnNext.Width, controlY);
            lblPageInfo.Location = new Point(
                (dataPallet.Left + dataPallet.Right - lblPageInfo.Width) / 2,
                controlY
            );
        }

        private void ChangePage(int direction)
        {
            int totalPages = (int)Math.Ceiling((double)filteredPallets.Count / pageSize);
            currentPage = Math.Clamp(currentPage + direction, 1, totalPages > 0 ? totalPages : 1);
            DisplayPage(currentPage);
        }

        private void UpdatePaginationInfo()
        {
            int totalPages = (int)Math.Ceiling((double)filteredPallets.Count / pageSize);
            lblPageInfo.Text = filteredPallets.Any() ? $"Trang {currentPage}/{totalPages}" : "Không tìm thấy kết quả";
        }

        private void DataPallet_SelectionChanged(object sender, EventArgs e)
        {
            if (dataPallet.SelectedRows.Count > 0)
            {
                var selectedRow = dataPallet.SelectedRows[0];
                string palletName = selectedRow.Cells["Name"].Value.ToString();
                double defaultWeight = Convert.ToDouble(selectedRow.Cells["DefaultWeight"].Value);

                namePallet.Text = palletName;
                trongluongpallet.Text = defaultWeight.ToString();

                int rowIndex = selectedRow.Index;
                int palletIndex = (currentPage - 1) * pageSize + rowIndex;
                selectedPallet = filteredPallets[palletIndex];
            }
            else
            {
                namePallet.Clear();
                trongluongpallet.Clear();
                selectedPallet = null;
            }
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(namePallet.Text))
            {
                MessageBox.Show("Vui lòng nhập tên Pallet!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!double.TryParse(trongluongpallet.Text, out double defaultWeight) || defaultWeight <= 0)
            {
                MessageBox.Show("Trọng lượng phải là số dương!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var context = new LoHoiDbContext())
                {
                    string inputName = namePallet.Text.Trim();
                    if (context.Pallets.Any(p => p.Name.ToLower() == inputName.ToLower()))
                    {
                        MessageBox.Show($"Tên Pallet '{inputName}' đã tồn tại! Vui lòng chọn tên khác.",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var newPallet = new Pallet
                    {
                        Name = inputName,
                        DefaultWeight = defaultWeight,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now
                    };

                    context.Pallets.Add(newPallet);
                    context.SaveChanges();
                    LoadPalletData();

                    namePallet.Clear();
                    trongluongpallet.Clear();

                    MessageBox.Show("Thêm Pallet thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm Pallet: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            if (selectedPallet == null)
            {
                MessageBox.Show("Vui lòng chọn một Pallet để chỉnh sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(namePallet.Text))
            {
                MessageBox.Show("Vui lòng nhập tên Pallet!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!double.TryParse(trongluongpallet.Text, out double inputWeight))
            {
                MessageBox.Show("Trọng lượng phải là số hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra nếu DefaultWeight bị thay đổi
            if (inputWeight != selectedPallet.DefaultWeight)
            {
                MessageBox.Show("Không được thay đổi trọng lượng Pallet!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var context = new LoHoiDbContext())
                {
                    string inputName = namePallet.Text.Trim();
                    if (context.Pallets.Any(p => p.Name.ToLower() == inputName.ToLower() && p.Id != selectedPallet.Id))
                    {
                        MessageBox.Show($"Tên Pallet '{inputName}' đã tồn tại! Vui lòng chọn tên khác.",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var palletToUpdate = context.Pallets.Find(selectedPallet.Id);
                    if (palletToUpdate == null)
                    {
                        MessageBox.Show("Pallet không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    palletToUpdate.Name = inputName;
                    palletToUpdate.UpdateTime = DateTime.Now;
                    // Không cập nhật DefaultWeight để giữ nguyên giá trị ban đầu

                    context.SaveChanges();
                    LoadPalletData();

                    namePallet.Clear();
                    trongluongpallet.Clear();
                    selectedPallet = null;

                    MessageBox.Show("Chỉnh sửa Pallet thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chỉnh sửa Pallet: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            if (selectedPallet == null)
            {
                MessageBox.Show("Vui lòng chọn một Pallet để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var context = new LoHoiDbContext())
                {
                    var palletToDelete = context.Pallets.Include("Products").FirstOrDefault(p => p.Id == selectedPallet.Id);
                    if (palletToDelete == null)
                    {
                        MessageBox.Show("Pallet không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Kiểm tra nếu pallet có sản phẩm
                    if (palletToDelete.Products.Any())
                    {
                        MessageBox.Show($"Pallet '{palletToDelete.Name}'không thể xóa Pallet!",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var confirmResult = MessageBox.Show(
                        $"Bạn có chắc chắn muốn xóa Pallet '{selectedPallet.Name}' không?",
                        "Xác nhận xóa",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (confirmResult != DialogResult.Yes)
                        return;

                    context.Pallets.Remove(palletToDelete);
                    context.SaveChanges();
                    LoadPalletData();

                    namePallet.Clear();
                    trongluongpallet.Clear();
                    selectedPallet = null;

                    MessageBox.Show("Xóa Pallet thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa Pallet: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void inputsearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Lấy chuỗi tìm kiếm và chuẩn hóa
                string searchText = inputsearch.Text.Trim().ToLower();

                // Lọc danh sách pallet dựa trên Name
                if (string.IsNullOrEmpty(searchText))
                {
                    // Nếu ô tìm kiếm rỗng, hiển thị toàn bộ danh sách
                    filteredPallets = allPallets;
                }
                else
                {
                    // Lọc các pallet có Name chứa chuỗi tìm kiếm
                    filteredPallets = allPallets
                        .Where(p => p.Name.ToLower().Contains(searchText))
                        .ToList();
                }

                // Đặt lại trang hiện tại về 1
                currentPage = 1;

                // Hiển thị trang đầu tiên của kết quả lọc
                DisplayPage(currentPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm Pallet: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}