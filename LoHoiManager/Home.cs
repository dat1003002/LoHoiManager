using OfficeOpenXml;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO.Ports;
using Spire.Xls;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;
using LoHoiManager.Data;
using Microsoft.EntityFrameworkCore;
using LoHoiManager.model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace LoHoiManager
{
    public partial class Home : Form
    {
        #region Fields
        private System.Windows.Forms.Timer timer;
        private SerialPort serialPort;
        private string weight = "0.00 kg";
        private string stableWeight = "0.00 kg";
        private string previousWeight = "0.00 kg";
        private bool isStable = false;
        private bool previousIsStable = false;
        private DateTime lastUpdate = DateTime.MinValue;
        private const int UPDATE_INTERVAL_MS = 200;
        private const float WEIGHT_THRESHOLD = 0.5f;
        private int stableCount = 0;
        private string currentTime;
        private const int STABLE_COUNT_THRESHOLD = 3;
        private const float MAX_WEIGHT_JUMP = 20.0f;
        private string displayWeight = "0.00 kg";
        private string displayStatus = "Unstable";
        private LoHoiDbContext _dbContext;
        private string? selectedSupplierName;
        private string? displayProductName;
        private string? selectedPalletName;
        private List<ProductViewModel> _productViewModels;
        private bool isProcessing = false;
        private string? selectedFactoryName;
        #endregion

        #region Constructor
        public Home()
        {
            OfficeOpenXml.ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            InitializeComponent();
            this.DoubleBuffered = true;
            this.FormClosing += new FormClosingEventHandler(Home_FormClosing);
            this.Resize += Home_Resize;

            timer = new System.Windows.Forms.Timer { Interval = 1000 };
            timer.Tick += Timer_Tick;
            timer.Start();

            _dbContext = new LoHoiDbContext();
            LoadProductTypesIntoComboBox();
            LoadSuppliersIntoComboBox();
            cbnhacc.SelectedIndexChanged += cbnhacc_SelectedIndexChanged;
            cbnhacc.MouseDown += cbnhacc_MouseDown;
            cbnhacc.Click += cbnhacc_Click;
            cbnhacc.DropDownStyle = ComboBoxStyle.DropDownList;
            cbproduct.SelectedIndexChanged += cbproduct_SelectedIndexChanged;
            cbproduct.DropDownStyle = ComboBoxStyle.DropDownList;
            cbproduct.MouseDown += cbproduct_MouseDown;
            cbproduct.Click += cbproduct_Click;
            InitializeSerialPort();
            LoadPalletsIntoComboBox();
            comboxpallet.SelectedIndexChanged += comboxpallet_SelectedIndexChanged;
            comboxpallet.MouseDown += comboxpallet_MouseDown;
            comboxpallet.Click += comboxpallet_Click;
            comboxpallet.DropDownStyle = ComboBoxStyle.DropDownList;

            dataProduct.ScrollBars = ScrollBars.Vertical;
            dataProduct.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataProduct.RowHeadersVisible = false;
            dataProduct.ReadOnly = true;
            dataProduct.AllowUserToAddRows = false;
            dataProduct.AllowUserToDeleteRows = false;
            dataProduct.CellContentClick += dataProduct_CellContentClick;

            LoadProductDataAsync();
        }
        #endregion

        #region DataGridView CellContentClick
        private void dataProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        #endregion

        #region ComboBox MouseDown and Click Events
        private void cbproduct_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                cbproduct.DroppedDown = true;
            }
        }

        private void cbproduct_Click(object sender, EventArgs e)
        {
            cbproduct.DroppedDown = true;
        }

        private void cbnhacc_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                cbnhacc.DroppedDown = true;
            }
        }

        private void cbnhacc_Click(object sender, EventArgs e)
        {
            cbnhacc.DroppedDown = true;
        }

        private void comboxpallet_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                comboxpallet.DroppedDown = true;
            }
        }

        private void comboxpallet_Click(object sender, EventArgs e)
        {
            comboxpallet.DroppedDown = true;
        }
        #endregion

        #region ProductType Handling
        private void LoadProductTypesIntoComboBox()
        {
            try
            {
                var productTypes = _dbContext.ProductTypes
                    .Select(pt => new { pt.Id, pt.Name })
                    .ToList();

                cbproduct.DataSource = null;
                cbproduct.Items.Clear();
                cbproduct.DataSource = productTypes;
                cbproduct.DisplayMember = "Name";
                cbproduct.ValueMember = "Id";
                cbproduct.SelectedIndex = -1;

                Console.WriteLine("Đã tải danh sách ProductType vào ComboBox.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách loại sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Lỗi khi tải danh sách ProductType: {ex.Message}");
            }
        }
        #endregion

        #region Serial Port Handling
        private void InitializeSerialPort()
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                if (ports.Length == 0)
                {
                    MessageBox.Show("Không tìm thấy cổng COM nào!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Console.WriteLine("Danh sách cổng COM khả dụng: " + string.Join(", ", ports));

                serialPort = new SerialPort
                {
                    PortName = "COM5",
                    BaudRate = 9600,
                    Parity = Parity.None,
                    DataBits = 8,
                    StopBits = StopBits.One
                };

                serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
                serialPort.Open();
                Console.WriteLine("Đã mở cổng COM: " + serialPort.PortName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở cổng COM: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Lỗi khi mở cổng COM: {ex.Message}");
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = serialPort.ReadExisting().Trim();
                Console.WriteLine("Dữ liệu thô từ cân: " + data);

                if (string.IsNullOrWhiteSpace(data)) return;

                bool newIsStable = data.Contains("ST") && !data.Contains("US");
                string weightPart = data;
                int plusIndex = data.IndexOf("+");
                if (plusIndex != -1) weightPart = data.Substring(plusIndex + 1).Trim();
                weightPart = weightPart.Replace("kg", "").Trim();

                if (float.TryParse(weightPart, out float weightValue))
                {
                    string newWeight = weightValue.ToString("F1") + " kg";

                    if ((DateTime.Now - lastUpdate).TotalMilliseconds >= UPDATE_INTERVAL_MS)
                    {
                        weight = newWeight;
                        isStable = newIsStable;

                        if (!isStable)
                        {
                            stableWeight = newWeight;
                            Console.WriteLine($"Cập nhật stableWeight khi Unstable: {stableWeight}");
                        }
                        else
                        {
                            Console.WriteLine($"Trọng lượng Stable, giữ nguyên: {stableWeight}");
                        }

                        displayWeight = stableWeight;
                        displayStatus = isStable ? "Stable" : "Unstable";

                        lastUpdate = DateTime.Now;
                        previousWeight = weight;
                        previousIsStable = isStable;

                        Console.WriteLine($"Trọng lượng hiện tại: {weight}, Trạng thái: {(isStable ? "Stable" : "Unstable")}, StableWeight: {stableWeight}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi đọc dữ liệu từ cổng COM: {ex.Message}");
            }
        }
        #endregion

        #region Pallet Handling
        private void LoadPalletsIntoComboBox()
        {
            try
            {
                var palletNames = _dbContext.Pallets
                    .Select(p => p.Name)
                    .ToList();

                comboxpallet.DataSource = null;
                comboxpallet.Items.Clear();
                comboxpallet.Items.AddRange(palletNames.ToArray());
                comboxpallet.SelectedIndex = -1;

                Console.WriteLine("Đã tải danh sách pallet vào ComboBox.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách pallet: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Lỗi khi tải danh sách pallet: {ex.Message}");
            }
        }

        private void comboxpallet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboxpallet.SelectedItem != null)
            {
                selectedPalletName = comboxpallet.SelectedItem.ToString();
                Console.WriteLine($"Đã chọn pallet: {selectedPalletName}");
            }
            else
            {
                selectedPalletName = null;
                Console.WriteLine("Không có pallet nào được chọn.");
            }
        }
        #endregion

        #region Timer and Form Events
        private void Timer_Tick(object sender, EventArgs e)
        {
            string newTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            if (newTime != currentTime)
            {
                currentTime = newTime;
                panelhienthithoigian.Invalidate();
            }

            inputtrongluong.Invalidate();
            nhantrangthai.Invalidate();

            if (serialPort != null && !serialPort.IsOpen)
            {
                try
                {
                    serialPort.Open();
                    Console.WriteLine("Đã kết nối lại cổng COM.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Không thể kết nối lại cổng COM: {ex.Message}");
                }
            }
        }

        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (serialPort != null && serialPort.IsOpen)
                {
                    serialPort.Close();
                    Console.WriteLine("Đã đóng cổng COM.");
                }
                _dbContext?.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi đóng form: {ex.Message}");
            }
        }

        private void Home_Load(object sender, EventArgs e)
        {
            try
            {
                lbtieude.Font = new Font(lbtieude.Font.FontFamily, lbtieude.Font.Size, FontStyle.Bold);
                logo.Font = new Font(lbtieude.Font.FontFamily, lbtieude.Font.Size, FontStyle.Bold);

                if (panelcat == null)
                {
                    panelcat = new Panel
                    {
                        Location = new Point(0, 0),
                        Width = this.ClientSize.Width,
                        Height = 50
                    };
                    this.Controls.Add(panelcat);
                }

                ConfigurePanelNhapDuLieu();
                ConfigurePanelXuatKho();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Lỗi Home_Load: {ex.Message}");
            }
        }

        private void Home_Resize(object sender, EventArgs e)
        {
            try
            {
                if (panelcat != null)
                {
                    panelcat.Width = this.ClientSize.Width;
                }

                ConfigurePanelNhapDuLieu();
                ConfigurePanelXuatKho();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thay đổi kích thước form: {ex.Message}");
            }
        }
        #endregion

        #region DataGridView Handling
        private async Task LoadProductDataAsync()
        {
            try
            {
                using (var dbContext = new LoHoiDbContext())
                {
                    _productViewModels = await dbContext.Products
                        .AsNoTracking()
                        .Include(p => p.Pallet)
                        .Include(p => p.Supplier)
                        .OrderByDescending(p => p.CreateTime)
                        .Take(15)
                        .Select(p => new ProductViewModel
                        {
                            Id = p.Id,
                            ProductName = p.Name,
                            Weight = p.Weight,
                            CreateTime = p.CreateTime,
                            PalletName = p.Pallet != null ? p.Pallet.Name : "N/A",
                            SupplierName = p.Supplier != null ? p.Supplier.Name : "N/A",
                            DefaultWeight = p.Pallet != null ? p.Pallet.DefaultWeight : 0
                        })
                        .ToListAsync();

                    DisplayProductData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Lỗi LoadProductDataAsync: {ex.Message}");
            }
        }

        private void DisplayProductData()
        {
            try
            {
                if (_productViewModels == null || !_productViewModels.Any())
                {
                    dataProduct.DataSource = null;
                    return;
                }

                var displayData = _productViewModels
                    .Select((pvm, index) => new
                    {
                        STT = index + 1,
                        pvm.ProductName,
                        pvm.Weight,
                        pvm.PalletName,
                        pvm.SupplierName
                    })
                    .ToList();

                dataProduct.DataSource = displayData;

                foreach (DataGridViewColumn column in dataProduct.Columns)
                {
                    column.Visible = column.Name == "STT" ||
                                     column.Name == "ProductName" ||
                                     column.Name == "Weight" ||
                                     column.Name == "PalletName" ||
                                     column.Name == "SupplierName";
                }

                dataProduct.Columns["STT"].HeaderText = "STT";
                dataProduct.Columns["STT"].DisplayIndex = 0;
                dataProduct.Columns["STT"].Width = 50;
                dataProduct.Columns["STT"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                dataProduct.Columns["ProductName"].HeaderText = "Loại Củi";
                dataProduct.Columns["ProductName"].DisplayIndex = 1;

                dataProduct.Columns["Weight"].HeaderText = "Trọng lượng";
                dataProduct.Columns["Weight"].DisplayIndex = 2;

                dataProduct.Columns["PalletName"].HeaderText = "Tên Pallet";
                dataProduct.Columns["PalletName"].DisplayIndex = 3;

                dataProduct.Columns["SupplierName"].HeaderText = "Công Ty";
                dataProduct.Columns["SupplierName"].DisplayIndex = 4;

                Font headerFont = new Font(dataProduct.Font.FontFamily, 12, FontStyle.Bold);
                foreach (DataGridViewColumn column in dataProduct.Columns)
                {
                    if (column.Visible)
                    {
                        column.HeaderCell.Style.Font = headerFont;
                        column.HeaderCell.Style.ForeColor = Color.Black;
                    }
                }

                dataProduct.DefaultCellStyle.ForeColor = Color.Black;
                dataProduct.DefaultCellStyle.BackColor = Color.White;
                dataProduct.DefaultCellStyle.SelectionForeColor = Color.White;
                dataProduct.DefaultCellStyle.SelectionBackColor = Color.Blue;
                dataProduct.DefaultCellStyle.Font = new Font(dataProduct.Font.FontFamily, 10, FontStyle.Regular);

                Console.WriteLine("Đã hiển thị dữ liệu sản phẩm trong DataGridView.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị dữ liệu sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Lỗi DisplayProductData: {ex.Message}");
            }
        }
        #endregion

        #region Panel Configuration
        private void ConfigurePanelNhapDuLieu()
        {
            try
            {
                panelnhapdulieu.Width = (int)(this.ClientSize.Width * 0.4);
                panelnhapdulieu.Height = this.ClientSize.Height - (panelcat.Bottom + 2) - 10;

                groupBox.Location = new Point(20, 10);
                groupBox.Width = panelnhapdulieu.Width - 40;
                groupBox.Height = 200;

                TableLayoutPanel tableLayout = new TableLayoutPanel
                {
                    Location = new Point(10, 10),
                    Width = groupBox.Width - 20,
                    AutoSize = true,
                    ColumnCount = 2,
                    RowCount = 3,
                    Dock = DockStyle.Fill
                };
                tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
                tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
                tableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                tableLayout.Controls.Add(lbcui, 0, 0);
                tableLayout.Controls.Add(cbproduct, 1, 0);
                tableLayout.Controls.Add(lbnhacc, 0, 1);
                tableLayout.Controls.Add(cbnhacc, 1, 1);
                tableLayout.Controls.Add(lbpallet, 0, 2);
                tableLayout.Controls.Add(comboxpallet, 1, 2);

                groupBox.Controls.Clear();
                groupBox.Controls.Add(tableLayout);

                paneltrangthai.Location = new Point(20, groupBox.Bottom + 20);
                paneltrangthai.Width = (int)(panelnhapdulieu.Width * 0.3);
                paneltrangthai.Height = (int)(panelnhapdulieu.Height * 0.2);

                nhantrangthai.Location = new Point(paneltrangthai.Right + 10, groupBox.Bottom + 20);
                nhantrangthai.Width = panelnhapdulieu.Width - paneltrangthai.Width - 50;
                nhantrangthai.Height = paneltrangthai.Height;

                groupBoxData.Location = new Point(20, nhantrangthai.Bottom + 10);
                groupBoxData.Width = panelnhapdulieu.Width - 40;
                groupBoxData.Height = panelnhapdulieu.Height - (nhantrangthai.Bottom + 10) - 20;

                dataProduct.Location = new Point(10, 50);
                dataProduct.Width = groupBoxData.Width - 20;
                dataProduct.Height = groupBoxData.Height - 69;
                groupBoxData.Controls.Clear();
                groupBoxData.Controls.Add(dataProduct);

                panelnhapdulieu.Controls.Clear();
                panelnhapdulieu.Controls.Add(groupBox);
                panelnhapdulieu.Controls.Add(paneltrangthai);
                panelnhapdulieu.Controls.Add(nhantrangthai);
                panelnhapdulieu.Controls.Add(groupBoxData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi ConfigurePanelNhapDuLieu: {ex.Message}");
            }
        }

        private void ConfigurePanelXuatKho()
        {
            try
            {
                panelxuakho.Width = (int)(this.ClientSize.Width * 0.6);
                panelxuakho.Height = this.ClientSize.Height - (panelcat.Bottom + 2) - 10;
                panelxuakho.Location = new Point(panelnhapdulieu.Right, panelcat.Bottom + 2);

                panelphieunhapkho.Location = new Point(20, 20);
                panelphieunhapkho.Width = panelxuakho.Width - 40;
                panelphieunhapkho.Height = (int)(panelxuakho.Height * 0.1);

                int totalAvailableWidth = panelphieunhapkho.Width - 20;
                int panelmahangWidth = (int)(totalAvailableWidth * 0.225 / 0.9);
                int paneltrongluongWidth = (int)(totalAvailableWidth * 0.45 / 0.9);
                int panelxuatxuWidth = (int)(totalAvailableWidth * 0.225 / 0.9);

                panelmahang.Location = new Point(20, panelphieunhapkho.Bottom + 10);
                panelmahang.Width = panelmahangWidth;
                panelmahang.Height = (int)(panelxuakho.Height * 0.1);

                paneltrongluong.Location = new Point(panelmahang.Right + 10, panelphieunhapkho.Bottom + 10);
                paneltrongluong.Width = paneltrongluongWidth;
                paneltrongluong.Height = (int)(panelxuakho.Height * 0.1);

                panelxuatxu.Location = new Point(paneltrongluong.Right + 10, panelphieunhapkho.Bottom + 10);
                panelxuatxu.Width = panelxuatxuWidth;
                panelxuatxu.Height = (int)(panelxuakho.Height * 0.1);

                int inputmahangWidth = (int)(totalAvailableWidth * 0.225 / 0.9);
                int inputtrongluongWidth = (int)(totalAvailableWidth * 0.45 / 0.9);
                int inputnhacungcapWidth = (int)(totalAvailableWidth * 0.225 / 0.9);

                inputmahang.Location = new Point(20, panelxuatxu.Bottom + 10);
                inputmahang.Width = inputmahangWidth;
                inputmahang.Height = (int)(panelxuakho.Height * 0.4);

                inputtrongluong.Location = new Point(inputmahang.Right + 10, panelxuatxu.Bottom + 10);
                inputtrongluong.Width = inputtrongluongWidth;
                inputtrongluong.Height = (int)(panelxuakho.Height * 0.4);

                inputnhacungcap.Location = new Point(inputtrongluong.Right + 10, panelxuatxu.Bottom + 10);
                inputnhacungcap.Width = inputnhacungcapWidth;
                inputnhacungcap.Height = (int)(panelxuakho.Height * 0.4);

                panelthoigian.Location = new Point(20, inputnhacungcap.Bottom + 10);
                panelthoigian.Width = inputmahangWidth;
                panelthoigian.Height = (int)(panelxuakho.Height * 0.1);

                panelhienthithoigian.Location = new Point(panelthoigian.Right + 10, inputnhacungcap.Bottom + 10);
                panelhienthithoigian.Width = panelxuakho.Width - panelthoigian.Width - 50;
                panelhienthithoigian.Height = (int)(panelxuakho.Height * 0.1);

                panelleftbutton.Location = new Point(20, panelhienthithoigian.Bottom + 10);
                panelleftbutton.Width = panelxuakho.Width - 40;
                panelleftbutton.Height = panelxuakho.Height - panelleftbutton.Top - 20;

                int buttonCount = 5;
                int margin = 10;
                int buttonWidth = (panelleftbutton.Width - (buttonCount + 1) * margin) / buttonCount;
                int buttonHeight = panelleftbutton.Height - 2 * margin;

                btnIn.Location = new Point(margin, margin);
                btnIn.Size = new Size(buttonWidth, buttonHeight);

                btnnhacc.Location = new Point(btnIn.Right + margin, margin);
                btnnhacc.Size = new Size(buttonWidth, buttonHeight);

                bntproduct.Location = new Point(btnnhacc.Right + margin, margin);
                bntproduct.Size = new Size(buttonWidth, buttonHeight);

                button1.Location = new Point(bntproduct.Right + margin, margin);
                button1.Size = new Size(buttonWidth, buttonHeight);

                btnaddcui.Location = new Point(button1.Right + margin, margin);
                btnaddcui.Size = new Size(buttonWidth, buttonHeight);

                panelleftbutton.Controls.AddRange(new Control[] { btnIn, btnnhacc, bntproduct, button1, btnaddcui });

                AddControlsToPanel(panelxuakho, panelphieunhapkho, panelmahang, paneltrongluong, panelxuatxu,
                    inputmahang, inputtrongluong, inputnhacungcap, panelthoigian, panelhienthithoigian,
                    panelleftbutton);

                if (!this.Controls.Contains(panelxuakho))
                    this.Controls.Add(panelxuakho);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi ConfigurePanelXuatKho: {ex.Message}");
            }
        }

        private void AddControlsToPanel(Panel parent, params Control[] controls)
        {
            foreach (var control in controls)
            {
                if (!parent.Controls.Contains(control))
                    parent.Controls.Add(control);
            }
        }

        private void SetDoubleBuffered(Control control)
        {
            control.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(control, true);
        }
        #endregion

        #region Paint Events
        private void panelphieunhapkho_Paint(object sender, PaintEventArgs e)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                int width = panelphieunhapkho.Width - 1;
                int height = panelphieunhapkho.Height - 1;
                int cornerRadius = 4;

                path.AddArc(0, 0, cornerRadius, cornerRadius, 180, 90);
                path.AddArc(width - cornerRadius, 0, cornerRadius, cornerRadius, 270, 90);
                path.AddLine(width, cornerRadius / 2, width, height);
                path.AddLine(width, height, 0, height);
                path.AddLine(0, height, 0, cornerRadius / 2);
                path.CloseFigure();

                using (Pen penTopLeftRight = new Pen(Color.Black, 3))
                {
                    e.Graphics.DrawPath(penTopLeftRight, path);
                }
                using (Pen penBottom = new Pen(Color.Black, 2))
                {
                    e.Graphics.DrawLine(penBottom, 0, height, width, height);
                }
            }

            string text = "PHIẾU NHẬP KHO";
            using (Font font = new Font("Arial", 16, FontStyle.Bold))
            {
                SizeF textSize = e.Graphics.MeasureString(text, font);
                float x = (panelphieunhapkho.Width - textSize.Width) / 2;
                float y = (panelphieunhapkho.Height - textSize.Height) / 2;
                e.Graphics.DrawString(text, font, Brushes.Black, x, y);
            }
        }

        private void panelmahang_Paint(object sender, PaintEventArgs e)
        {
            DrawRectangleAndText(e.Graphics, panelmahang, "Loại Củi");
        }

        private void paneltrongluong_Paint(object sender, PaintEventArgs e)
        {
            DrawRectangleAndText(e.Graphics, paneltrongluong, "Trọng Lượng");
        }

        private void panelxuatxu_Paint(object sender, PaintEventArgs e)
        {
            DrawRectangleAndText(e.Graphics, panelxuatxu, "Xuất Xứ");
        }

        private void inputmahang_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.Black, 2))
            {
                e.Graphics.DrawRectangle(pen, 1, 1, inputmahang.Width - 3, inputmahang.Height - 3);
            }

            using (Font font = new Font("Arial", 18, FontStyle.Bold))
            {
                string displayText = string.IsNullOrEmpty(displayProductName) ? "sản phẩm" : displayProductName;
                SizeF textSize = e.Graphics.MeasureString(displayText, font);
                float x = (inputmahang.Width - textSize.Width) / 2;
                float y = (inputmahang.Height - textSize.Height) / 2;
                e.Graphics.DrawString(displayText, font, Brushes.Black, x, y);
            }
        }

        private void inputtrongluong_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.Black, 2))
            {
                e.Graphics.DrawRectangle(pen, 1, 1, inputtrongluong.Width - 3, inputtrongluong.Height - 3);
            }

            string displayText = displayWeight;
            using (Font font = new Font("Arial", 18, FontStyle.Bold))
            {
                SizeF textSize = e.Graphics.MeasureString(displayText, font);
                float x = (inputtrongluong.Width - textSize.Width) / 2;
                float y = (inputtrongluong.Height - textSize.Height) / 2;
                e.Graphics.DrawString(displayText, font, Brushes.Black, x, y);
            }
        }

        private void inputnhacungcap_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.Black, 2))
            {
                e.Graphics.DrawRectangle(pen, 1, 1, inputnhacungcap.Width - 3, inputnhacungcap.Height - 3);
            }

            using (Font font = new Font("Arial", 16, FontStyle.Bold))
            {
                string displayText = string.IsNullOrEmpty(selectedSupplierName) ? "nhà cung cấp" : selectedSupplierName;
                float maxWidth = inputnhacungcap.Width - 20;
                List<string> lines = WrapText(e.Graphics, displayText, font, maxWidth);

                float lineHeight = e.Graphics.MeasureString("A", font).Height;
                float totalHeight = lines.Count * lineHeight;
                float startY = (inputnhacungcap.Height - totalHeight) / 2;

                for (int i = 0; i < lines.Count; i++)
                {
                    float y = startY + i * lineHeight;
                    SizeF textSize = e.Graphics.MeasureString(lines[i], font);
                    float x = (inputnhacungcap.Width - textSize.Width) / 2;
                    e.Graphics.DrawString(lines[i], font, Brushes.Black, x, y);
                }
            }
        }

        private List<string> WrapText(Graphics g, string text, Font font, float maxWidth)
        {
            List<string> lines = new List<string>();
            if (string.IsNullOrEmpty(text)) return lines;

            string[] words = text.Split(' ');
            string currentLine = "";
            foreach (string word in words)
            {
                string testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
                SizeF textSize = g.MeasureString(testLine, font);

                if (textSize.Width <= maxWidth)
                {
                    currentLine = testLine;
                }
                else
                {
                    if (!string.IsNullOrEmpty(currentLine))
                    {
                        lines.Add(currentLine);
                    }
                    currentLine = word;
                }
            }

            if (!string.IsNullOrEmpty(currentLine))
            {
                lines.Add(currentLine);
            }

            return lines;
        }

        private void panelthoigian_Paint(object sender, PaintEventArgs e)
        {
            DrawRectangleAndText(e.Graphics, panelthoigian, "Thời Gian");
        }

        private void panelhienthithoigian_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.Black, 2))
            {
                e.Graphics.DrawRectangle(pen, 1, 1, panelhienthithoigian.Width - 3, panelhienthithoigian.Height - 3);
            }

            using (Font font = new Font("Arial", 18, FontStyle.Bold))
            {
                SizeF textSize = e.Graphics.MeasureString(currentTime, font);
                float x = (panelhienthithoigian.Width - textSize.Width) / 2;
                float y = (panelhienthithoigian.Height - textSize.Height) / 2;
                e.Graphics.DrawString(currentTime, font, Brushes.Black, x, y);
            }
        }

        private void paneltrangthai_Paint(object sender, PaintEventArgs e)
        {
            DrawRoundedRectangle(e.Graphics, paneltrangthai, "Trạng thái", 14);
        }

        private void nhantrangthai_Paint(object sender, PaintEventArgs e)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                int width = nhantrangthai.Width - 1;
                int height = nhantrangthai.Height - 1;
                int cornerRadius = 10;

                path.AddArc(0, 0, cornerRadius * 2, cornerRadius * 2, 180, 90);
                path.AddArc(width - cornerRadius * 2, 0, cornerRadius * 2, cornerRadius * 2, 270, 90);
                path.AddArc(width - cornerRadius * 2, height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
                path.AddArc(0, height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
                path.CloseFigure();

                nhantrangthai.Region = new Region(path);

                using (Pen pen = new Pen(Color.White, 2))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }

            string text = isStable ? "Stable" : "Unstable";
            using (Font font = new Font("Arial", 14, FontStyle.Bold))
            {
                Brush textBrush = isStable ? Brushes.Black : Brushes.Red;
                SizeF textSize = e.Graphics.MeasureString(text, font);
                float x = (nhantrangthai.Width - textSize.Width) / 2;
                float y = (nhantrangthai.Height - textSize.Height) / 2;
                e.Graphics.DrawString(text, font, textBrush, x, y);
            }
        }

        private void panelleftbutton_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.White, 1))
            {
                e.Graphics.DrawRectangle(pen, 1, 1, panelleftbutton.Width - 3, panelleftbutton.Height - 3);
            }
        }

        private void DrawRectangleAndText(Graphics g, Control control, string text)
        {
            using (Pen pen = new Pen(Color.Black, 2))
            {
                g.DrawRectangle(pen, 1, 1, control.Width - 3, control.Height - 3);
            }

            using (Font font = new Font("Arial", 18, FontStyle.Bold))
            {
                SizeF textSize = g.MeasureString(text, font);
                float x = (control.Width - textSize.Width) / 2;
                float y = (control.Height - textSize.Height) / 2;
                g.DrawString(text, font, Brushes.Black, x, y);
            }
        }

        private void DrawRoundedRectangle(Graphics g, Control control, string text, int fontSize)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                int width = control.Width - 1;
                int height = control.Height - 1;
                int cornerRadius = 10;

                path.AddArc(0, 0, cornerRadius * 2, cornerRadius * 2, 180, 90);
                path.AddArc(width - cornerRadius * 2, 0, cornerRadius * 2, cornerRadius * 2, 270, 90);
                path.AddArc(width - cornerRadius * 2, height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
                path.AddArc(0, height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
                path.CloseFigure();

                control.Region = new Region(path);

                using (Pen pen = new Pen(Color.White, 2))
                {
                    g.DrawPath(pen, path);
                }
            }

            using (Font font = new Font("Arial", fontSize, FontStyle.Bold))
            {
                SizeF textSize = g.MeasureString(text, font);
                float x = (control.Width - textSize.Width) / 2;
                float y = (control.Height - textSize.Height) / 2;
                g.DrawString(text, font, Brushes.Black, x, y);
            }
        }
        #endregion

        #region Button Events
        private async void btnIn_Click(object sender, EventArgs e)
        {
            if (isProcessing)
            {
                MessageBox.Show("Đang xử lý, vui lòng chờ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            isProcessing = true;
            Button btn = sender as Button;
            btn.Enabled = false;

            try
            {
                if (cbnhacc.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn nhà cung cấp trước khi in!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrEmpty(selectedPalletName))
                {
                    MessageBox.Show("Vui lòng chọn pallet trước khi in!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (cbproduct.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn loại sản phẩm trước khi in!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
              

                var selectedSupplier = (dynamic)cbnhacc.SelectedItem;
                int supplierId = selectedSupplier.Id;
                selectedSupplierName = selectedSupplier.Name;

                var selectedPallet = _dbContext.Pallets
                    .FirstOrDefault(p => p.Name == selectedPalletName);
                if (selectedPallet == null)
                {
                    MessageBox.Show($"Không tìm thấy pallet '{selectedPalletName}' trong cơ sở dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int palletId = selectedPallet.Id;
                string palletName = selectedPallet.Name;
                double defaultWeight = selectedPallet.DefaultWeight;

                var selectedProductType = (dynamic)cbproduct.SelectedItem;
                string productName = selectedProductType.Name;

                string weightString = displayWeight.Replace(" kg", "").Trim();
                if (!double.TryParse(weightString, out double weightValue))
                {
                    MessageBox.Show("Trọng lượng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                double netWeight = weightValue - defaultWeight;
                if (netWeight < 0)
                {
                    MessageBox.Show($"Trọng lượng thực tế (NetWeight = {netWeight}) âm, vui lòng kiểm tra lại dữ liệu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var newProduct = new Product
                {
                    Name = productName,
                    Weight = weightString,
                    NetWeight = netWeight,
                    SupplierId = supplierId,
                    PalletId = palletId,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };

                _dbContext.Products.Add(newProduct);
                await _dbContext.SaveChangesAsync();
                Console.WriteLine($"Đã lưu sản phẩm: {productName}, Weight: {weightString}, NetWeight: {netWeight}, Nhà cung cấp ID: {supplierId}, Pallet ID: {palletId}");

                string resourceName = "LoHoiManager.Templates.PhieuNhapKhoTemplate.xlsx";
                using (Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        MessageBox.Show("Không tìm thấy file template trong tài nguyên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string tempFilePath = Path.Combine(Path.GetTempPath(), "PhieuNhapKhoTemp.xlsx");
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

                        worksheet.Cells["B3"].Value = netWeight.ToString("F1") + " kg";
                        worksheet.Cells["B3"].Style.Font.Name = "Times New Roman";
                        worksheet.Cells["B3"].Style.Font.Size = 16;
                        worksheet.Cells["B3"].Style.Font.Bold = true;

                        worksheet.Cells["C3"].Value = selectedSupplierName ?? "Chưa chọn nhà cung cấp";
                        worksheet.Cells["C3"].Style.Font.Name = "Times New Roman";
                        worksheet.Cells["C3"].Style.Font.Size = 14;
                        worksheet.Cells["C3"].Style.Font.Bold = true;

                        worksheet.Cells["B4"].Value = palletName;
                        worksheet.Cells["B4"].Style.Font.Name = "Times New Roman";
                        worksheet.Cells["B4"].Style.Font.Size = 16;
                        worksheet.Cells["B4"].Style.Font.Bold = true;

                        worksheet.Cells["C4"].Value = defaultWeight.ToString("F1") + " kg";
                        worksheet.Cells["C4"].Style.Font.Name = "Times New Roman";
                        worksheet.Cells["C4"].Style.Font.Size = 16;
                        worksheet.Cells["C4"].Style.Font.Bold = true;

                        worksheet.Cells["C5"].Value = defaultWeight.ToString("F1") + " kg";
                        worksheet.Cells["C5"].Style.Font.Name = "Times New Roman";
                        worksheet.Cells["C5"].Style.Font.Size = 16;
                        worksheet.Cells["C5"].Style.Font.Bold = true;

                        worksheet.Cells["B5"].Value = currentTime;
                        worksheet.Cells["B5"].Style.Font.Name = "Times New Roman";
                        worksheet.Cells["B5"].Style.Font.Size = 16;
                        worksheet.Cells["B5"].Style.Font.Bold = true;

                        await package.SaveAsync();
                    }

                    if (!File.Exists(tempFilePath))
                    {
                        MessageBox.Show("File tạm không tồn tại, không thể in!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        File.Delete(tempFilePath);
                        return;
                    }

                    await Task.Delay(500);
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

                    if (File.Exists(tempFilePath))
                        File.Delete(tempFilePath);
                }

                MessageBox.Show("Đã in Phiếu Nhập Kho và lưu dữ liệu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xử lý phiếu nhập kho: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Lỗi btnIn_Click: {ex.Message}");
            }
            finally
            {
                isProcessing = false;
                btn.Enabled = true;
            }
        }

        private void btnnhacc_Click(object sender, EventArgs e)
        {
            try
            {
                Nhacungcap nhacungcapForm = new Nhacungcap();
                nhacungcapForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form Nhà cung cấp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Lỗi btnnhacc_Click: {ex.Message}");
            }
        }

        private void LoadSuppliersIntoComboBox()
        {
            try
            {
                var suppliers = _dbContext.Suppliers
                    .Select(s => new { s.Id, s.Name })
                    .ToList();

                var previousSelectedId = cbnhacc.SelectedValue;

                cbnhacc.DataSource = null;
                cbnhacc.DataSource = suppliers;
                cbnhacc.DisplayMember = "Name";
                cbnhacc.ValueMember = "Id";

                if (previousSelectedId != null && suppliers.Any(s => s.Id.Equals(previousSelectedId)))
                {
                    cbnhacc.SelectedValue = previousSelectedId;
                }
                else
                {
                    cbnhacc.SelectedIndex = -1;
                    selectedSupplierName = null;
                    inputnhacungcap.Invalidate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách nhà cung cấp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Lỗi LoadSuppliersIntoComboBox: {ex.Message}");
            }
        }

        private void cbnhacc_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbnhacc.SelectedItem != null)
                {
                    var selectedSupplier = (dynamic)cbnhacc.SelectedItem;
                    selectedSupplierName = selectedSupplier.Name;
                    inputnhacungcap.Invalidate();
                    inputnhacungcap.Update();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi cbnhacc_SelectedIndexChanged: {ex.Message}");
            }
        }

        private void cbproduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbproduct.SelectedItem != null)
                {
                    var selectedProductType = (dynamic)cbproduct.SelectedItem;
                    displayProductName = selectedProductType.Name;
                    inputmahang.Invalidate();
                    Console.WriteLine($"Đã chọn loại sản phẩm: {displayProductName}");
                }
                else
                {
                    displayProductName = null;
                    inputmahang.Invalidate();
                    Console.WriteLine("Không có loại sản phẩm nào được chọn.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi cbproduct_SelectedIndexChanged: {ex.Message}");
            }
        }

        private void bntproduct_Click(object sender, EventArgs e)
        {
            try
            {
                Listcui sanphamForm = new Listcui();
                sanphamForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form Listcui: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Lỗi bntproduct_Click: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ListPallet palletForm = new ListPallet();
                palletForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form ListPallet: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Lỗi button1_Click: {ex.Message}");
            }
        }

        private void btnaddcui_Click(object sender, EventArgs e)
        {
            try
            {
                ListProduct productcui = new ListProduct();
                productcui.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form ListProduct: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Lỗi btnaddcui_Click: {ex.Message}");
            }
        }
        #endregion
    }
}