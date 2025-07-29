namespace LoHoiManager
{
    partial class Home
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Home));
            header = new Panel();
            logo = new Label();
            lbtieude = new Label();
            panelcat = new Panel();
            panelnhapdulieu = new Panel();
            groupBoxData = new GroupBox();
            dataProduct = new DataGridView();
            groupBox = new GroupBox();
            lbpallet = new Label();
            lbnhacc = new Label();
            lbcui = new Label();
            cbproduct = new ComboBox();
            comboxpallet = new ComboBox();
            cbnhacc = new ComboBox();
            nhantrangthai = new Panel();
            paneltrangthai = new Panel();
            panelxuakho = new Panel();
            panelthoigian = new Panel();
            panelleftbutton = new Panel();
            btnaddcui = new Button();
            button1 = new Button();
            bntproduct = new Button();
            btnnhacc = new Button();
            btnIn = new Button();
            panelhienthithoigian = new Panel();
            inputnhacungcap = new Panel();
            inputtrongluong = new Panel();
            inputmahang = new Panel();
            panelxuatxu = new Panel();
            paneltrongluong = new Panel();
            panelmahang = new Panel();
            panelphieunhapkho = new Panel();
            header.SuspendLayout();
            panelnhapdulieu.SuspendLayout();
            groupBoxData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataProduct).BeginInit();
            groupBox.SuspendLayout();
            panelxuakho.SuspendLayout();
            panelleftbutton.SuspendLayout();
            SuspendLayout();
            // 
            // header
            // 
            header.BackColor = Color.Crimson;
            header.Controls.Add(logo);
            header.Controls.Add(lbtieude);
            header.Dock = DockStyle.Top;
            header.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            header.Location = new Point(0, 0);
            header.Name = "header";
            header.Size = new Size(1772, 91);
            header.TabIndex = 0;
            // 
            // logo
            // 
            logo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            logo.AutoSize = true;
            logo.Font = new Font("Segoe UI Black", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            logo.ForeColor = Color.Snow;
            logo.Location = new Point(28, 33);
            logo.Name = "logo";
            logo.Size = new Size(164, 31);
            logo.TabIndex = 1;
            logo.Text = "DRB Vietnam";
            // 
            // lbtieude
            // 
            lbtieude.Anchor = AnchorStyles.None;
            lbtieude.AutoSize = true;
            lbtieude.Font = new Font("Segoe UI Black", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbtieude.ForeColor = SystemColors.ButtonHighlight;
            lbtieude.Location = new Point(527, 21);
            lbtieude.Name = "lbtieude";
            lbtieude.Size = new Size(693, 50);
            lbtieude.TabIndex = 0;
            lbtieude.Text = "QUẢN LÝ TRỌNG LƯỢNG CỦI LÒ HƠI";
            lbtieude.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelcat
            // 
            panelcat.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelcat.BackColor = Color.Crimson;
            panelcat.Location = new Point(0, 97);
            panelcat.Name = "panelcat";
            panelcat.Size = new Size(1772, 10);
            panelcat.TabIndex = 1;
            // 
            // panelnhapdulieu
            // 
            panelnhapdulieu.BackColor = Color.FromArgb(44, 61, 77);
            panelnhapdulieu.Controls.Add(groupBoxData);
            panelnhapdulieu.Controls.Add(groupBox);
            panelnhapdulieu.Controls.Add(nhantrangthai);
            panelnhapdulieu.Controls.Add(paneltrangthai);
            panelnhapdulieu.ForeColor = SystemColors.ButtonHighlight;
            panelnhapdulieu.Location = new Point(0, 113);
            panelnhapdulieu.Name = "panelnhapdulieu";
            panelnhapdulieu.Size = new Size(712, 920);
            panelnhapdulieu.TabIndex = 0;
            // 
            // groupBoxData
            // 
            groupBoxData.Controls.Add(dataProduct);
            groupBoxData.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBoxData.ForeColor = SystemColors.ButtonHighlight;
            groupBoxData.Location = new Point(12, 451);
            groupBoxData.Name = "groupBoxData";
            groupBoxData.Size = new Size(690, 466);
            groupBoxData.TabIndex = 12;
            groupBoxData.TabStop = false;
            groupBoxData.Text = "Danh Sách Nhập Kho";
            // 
            // dataProduct
            // 
            dataProduct.BackgroundColor = SystemColors.ButtonHighlight;
            dataProduct.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataProduct.Location = new Point(6, 50);
            dataProduct.Name = "dataProduct";
            dataProduct.RowHeadersWidth = 51;
            dataProduct.Size = new Size(667, 407);
            dataProduct.TabIndex = 12;
            dataProduct.CellContentClick += dataProduct_CellContentClick;
            // 
            // groupBox
            // 
            groupBox.Controls.Add(lbpallet);
            groupBox.Controls.Add(lbnhacc);
            groupBox.Controls.Add(lbcui);
            groupBox.Controls.Add(cbproduct);
            groupBox.Controls.Add(comboxpallet);
            groupBox.Controls.Add(cbnhacc);
            groupBox.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox.ForeColor = SystemColors.ButtonHighlight;
            groupBox.Location = new Point(12, 18);
            groupBox.Name = "groupBox";
            groupBox.Size = new Size(690, 251);
            groupBox.TabIndex = 11;
            groupBox.TabStop = false;
            groupBox.Text = "Chọn Sản Phẩm và Nhà Cung Cấp";
            // 
            // lbpallet
            // 
            lbpallet.AutoSize = true;
            lbpallet.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbpallet.Location = new Point(49, 193);
            lbpallet.Name = "lbpallet";
            lbpallet.Size = new Size(131, 28);
            lbpallet.TabIndex = 13;
            lbpallet.Text = "Chọn Pallet :";
            // 
            // lbnhacc
            // 
            lbnhacc.AutoSize = true;
            lbnhacc.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbnhacc.Location = new Point(27, 125);
            lbnhacc.Name = "lbnhacc";
            lbnhacc.Size = new Size(163, 28);
            lbnhacc.TabIndex = 12;
            lbnhacc.Text = "Nhà Cung Cấp  :";
            // 
            // lbcui
            // 
            lbcui.AutoSize = true;
            lbcui.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbcui.Location = new Point(38, 62);
            lbcui.Name = "lbcui";
            lbcui.Size = new Size(152, 28);
            lbcui.TabIndex = 11;
            lbcui.Text = "Chọn Loại Củi :";
            // 
            // cbproduct
            // 
            cbproduct.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cbproduct.FormattingEnabled = true;
            cbproduct.Location = new Point(207, 52);
            cbproduct.Name = "cbproduct";
            cbproduct.Size = new Size(466, 45);
            cbproduct.TabIndex = 8;
            // 
            // comboxpallet
            // 
            comboxpallet.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            comboxpallet.FormattingEnabled = true;
            comboxpallet.Location = new Point(207, 183);
            comboxpallet.Name = "comboxpallet";
            comboxpallet.Size = new Size(466, 45);
            comboxpallet.TabIndex = 10;
            // 
            // cbnhacc
            // 
            cbnhacc.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cbnhacc.FormattingEnabled = true;
            cbnhacc.Location = new Point(207, 115);
            cbnhacc.Name = "cbnhacc";
            cbnhacc.Size = new Size(466, 45);
            cbnhacc.TabIndex = 7;
            // 
            // nhantrangthai
            // 
            nhantrangthai.BackColor = Color.PaleGoldenrod;
            nhantrangthai.Location = new Point(294, 286);
            nhantrangthai.Name = "nhantrangthai";
            nhantrangthai.Size = new Size(408, 145);
            nhantrangthai.TabIndex = 3;
            nhantrangthai.Paint += nhantrangthai_Paint;
            // 
            // paneltrangthai
            // 
            paneltrangthai.BackColor = Color.FromArgb(39, 185, 154);
            paneltrangthai.Location = new Point(12, 286);
            paneltrangthai.Name = "paneltrangthai";
            paneltrangthai.Size = new Size(276, 145);
            paneltrangthai.TabIndex = 2;
            paneltrangthai.Paint += paneltrangthai_Paint;
            // 
            // panelxuakho
            // 
            panelxuakho.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panelxuakho.BackColor = SystemColors.ButtonFace;
            panelxuakho.Controls.Add(panelthoigian);
            panelxuakho.Controls.Add(panelleftbutton);
            panelxuakho.Controls.Add(panelhienthithoigian);
            panelxuakho.Controls.Add(inputnhacungcap);
            panelxuakho.Controls.Add(inputtrongluong);
            panelxuakho.Controls.Add(inputmahang);
            panelxuakho.Controls.Add(panelxuatxu);
            panelxuakho.Controls.Add(paneltrongluong);
            panelxuakho.Controls.Add(panelmahang);
            panelxuakho.Controls.Add(panelphieunhapkho);
            panelxuakho.Location = new Point(718, 113);
            panelxuakho.Name = "panelxuakho";
            panelxuakho.Size = new Size(1060, 920);
            panelxuakho.TabIndex = 1;
            // 
            // panelthoigian
            // 
            panelthoigian.BackColor = SystemColors.ButtonHighlight;
            panelthoigian.Location = new Point(19, 558);
            panelthoigian.Name = "panelthoigian";
            panelthoigian.Size = new Size(246, 91);
            panelthoigian.TabIndex = 7;
            panelthoigian.Paint += panelthoigian_Paint;
            // 
            // panelleftbutton
            // 
            panelleftbutton.BackColor = Color.FromArgb(52, 74, 95);
            panelleftbutton.Controls.Add(btnaddcui);
            panelleftbutton.Controls.Add(button1);
            panelleftbutton.Controls.Add(bntproduct);
            panelleftbutton.Controls.Add(btnnhacc);
            panelleftbutton.Controls.Add(btnIn);
            panelleftbutton.Location = new Point(19, 673);
            panelleftbutton.Name = "panelleftbutton";
            panelleftbutton.Size = new Size(1017, 235);
            panelleftbutton.TabIndex = 4;
            panelleftbutton.Paint += panelleftbutton_Paint;
            // 
            // btnaddcui
            // 
            btnaddcui.BackColor = Color.FromArgb(39, 185, 154);
            btnaddcui.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            btnaddcui.ForeColor = SystemColors.ButtonHighlight;
            btnaddcui.Location = new Point(859, 86);
            btnaddcui.Name = "btnaddcui";
            btnaddcui.Size = new Size(132, 55);
            btnaddcui.TabIndex = 4;
            btnaddcui.Text = "Tạo Loại Củi";
            btnaddcui.UseVisualStyleBackColor = false;
            btnaddcui.Click += btnaddcui_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(39, 185, 154);
            button1.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            button1.ForeColor = SystemColors.ButtonHighlight;
            button1.Location = new Point(636, 84);
            button1.Name = "button1";
            button1.Size = new Size(194, 55);
            button1.TabIndex = 3;
            button1.Text = "Danh Sách Pallet";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // bntproduct
            // 
            bntproduct.BackColor = Color.FromArgb(39, 185, 154);
            bntproduct.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            bntproduct.ForeColor = SystemColors.ButtonHighlight;
            bntproduct.Location = new Point(431, 84);
            bntproduct.Name = "bntproduct";
            bntproduct.Size = new Size(184, 55);
            bntproduct.TabIndex = 2;
            bntproduct.Text = "Danh Sách Củi";
            bntproduct.UseVisualStyleBackColor = false;
            bntproduct.Click += bntproduct_Click;
            // 
            // btnnhacc
            // 
            btnnhacc.BackColor = Color.FromArgb(39, 185, 154);
            btnnhacc.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            btnnhacc.ForeColor = SystemColors.ButtonHighlight;
            btnnhacc.Location = new Point(207, 84);
            btnnhacc.Name = "btnnhacc";
            btnnhacc.Size = new Size(192, 55);
            btnnhacc.TabIndex = 1;
            btnnhacc.Text = "Nhà Cung Cấp";
            btnnhacc.UseVisualStyleBackColor = false;
            btnnhacc.Click += btnnhacc_Click;
            // 
            // btnIn
            // 
            btnIn.BackColor = Color.FromArgb(39, 185, 154);
            btnIn.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            btnIn.ForeColor = SystemColors.ButtonHighlight;
            btnIn.Location = new Point(27, 86);
            btnIn.Name = "btnIn";
            btnIn.Size = new Size(148, 53);
            btnIn.TabIndex = 0;
            btnIn.Text = "Lưu Và In";
            btnIn.UseVisualStyleBackColor = false;
            btnIn.Click += btnIn_Click;
            // 
            // panelhienthithoigian
            // 
            panelhienthithoigian.BackColor = SystemColors.ControlLightLight;
            panelhienthithoigian.Location = new Point(271, 558);
            panelhienthithoigian.Name = "panelhienthithoigian";
            panelhienthithoigian.Size = new Size(765, 91);
            panelhienthithoigian.TabIndex = 8;
            panelhienthithoigian.Paint += panelhienthithoigian_Paint;
            // 
            // inputnhacungcap
            // 
            inputnhacungcap.BackColor = SystemColors.ButtonHighlight;
            inputnhacungcap.Location = new Point(790, 240);
            inputnhacungcap.Name = "inputnhacungcap";
            inputnhacungcap.Size = new Size(246, 312);
            inputnhacungcap.TabIndex = 6;
            inputnhacungcap.Paint += inputnhacungcap_Paint;
            // 
            // inputtrongluong
            // 
            inputtrongluong.BackColor = SystemColors.ButtonHighlight;
            inputtrongluong.Location = new Point(271, 240);
            inputtrongluong.Name = "inputtrongluong";
            inputtrongluong.Size = new Size(513, 312);
            inputtrongluong.TabIndex = 5;
            inputtrongluong.Paint += inputtrongluong_Paint;
            // 
            // inputmahang
            // 
            inputmahang.BackColor = SystemColors.ButtonHighlight;
            inputmahang.Location = new Point(19, 240);
            inputmahang.Name = "inputmahang";
            inputmahang.Size = new Size(246, 312);
            inputmahang.TabIndex = 4;
            inputmahang.Paint += inputmahang_Paint;
            // 
            // panelxuatxu
            // 
            panelxuatxu.BackColor = SystemColors.ButtonHighlight;
            panelxuatxu.Location = new Point(790, 143);
            panelxuatxu.Name = "panelxuatxu";
            panelxuatxu.Size = new Size(246, 91);
            panelxuatxu.TabIndex = 3;
            panelxuatxu.Paint += panelxuatxu_Paint;
            // 
            // paneltrongluong
            // 
            paneltrongluong.BackColor = SystemColors.ButtonHighlight;
            paneltrongluong.Location = new Point(271, 143);
            paneltrongluong.Name = "paneltrongluong";
            paneltrongluong.Size = new Size(513, 91);
            paneltrongluong.TabIndex = 2;
            paneltrongluong.Paint += paneltrongluong_Paint;
            // 
            // panelmahang
            // 
            panelmahang.BackColor = SystemColors.ButtonHighlight;
            panelmahang.Location = new Point(19, 143);
            panelmahang.Name = "panelmahang";
            panelmahang.Size = new Size(246, 91);
            panelmahang.TabIndex = 1;
            panelmahang.Paint += panelmahang_Paint;
            // 
            // panelphieunhapkho
            // 
            panelphieunhapkho.BackColor = SystemColors.ButtonHighlight;
            panelphieunhapkho.Location = new Point(19, 35);
            panelphieunhapkho.Name = "panelphieunhapkho";
            panelphieunhapkho.Size = new Size(1017, 102);
            panelphieunhapkho.TabIndex = 0;
            panelphieunhapkho.Paint += panelphieunhapkho_Paint;
            // 
            // Home
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonHighlight;
            ClientSize = new Size(1772, 1033);
            Controls.Add(panelxuakho);
            Controls.Add(panelnhapdulieu);
            Controls.Add(panelcat);
            Controls.Add(header);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Home";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Home";
            WindowState = FormWindowState.Maximized;
            header.ResumeLayout(false);
            header.PerformLayout();
            panelnhapdulieu.ResumeLayout(false);
            groupBoxData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataProduct).EndInit();
            groupBox.ResumeLayout(false);
            groupBox.PerformLayout();
            panelxuakho.ResumeLayout(false);
            panelleftbutton.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel header;
        private Label logo;
        private Label lbtieude;
        private Panel panelcat;
        private Panel panelnhapdulieu;
        private GroupBox groupBox1;
        private ComboBox cbnhacc;
        private Panel nhantrangthai;
        private Panel paneltrangthai;
        private Panel panelxuakho;
        private Panel panelthoigian;
        private Panel panelleftbutton;
        private Button btnaddcui;
        private Button button1;
        private Button bntproduct;
        private Button btnnhacc;
        private Button btnIn;
        private Panel panelhienthithoigian;
        private Panel inputnhacungcap;
        private Panel inputtrongluong;
        private Panel inputmahang;
        private Panel panelxuatxu;
        private Panel paneltrongluong;
        private Panel panelmahang;
        private Panel panelphieunhapkho;
        private ComboBox comboxpallet;
        private ComboBox cbproduct;
        private GroupBox groupBox;
        private Label lbpallet;
        private Label lbnhacc;
        private Label lbcui;
        private DataGridView dataProduct;
        private GroupBox groupBoxData;
    }
}
