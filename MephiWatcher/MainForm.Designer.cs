namespace MephiWatcher
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
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
            SplitContainer _ioSplitContainer;
            Button _configButton;
            _chooseVuzComboBox = new ComboBox();
            _cancelButton = new Button();
            _configInfoTextBox = new TextBox();
            _findButton = new Button();
            _programsFlowLayoutPanel = new FlowLayoutPanel();
            _ioSplitContainer = new SplitContainer();
            _configButton = new Button();
            ((System.ComponentModel.ISupportInitialize)_ioSplitContainer).BeginInit();
            _ioSplitContainer.Panel1.SuspendLayout();
            _ioSplitContainer.Panel2.SuspendLayout();
            _ioSplitContainer.SuspendLayout();
            SuspendLayout();
            // 
            // _ioSplitContainer
            // 
            _ioSplitContainer.Dock = DockStyle.Fill;
            _ioSplitContainer.FixedPanel = FixedPanel.Panel1;
            _ioSplitContainer.Location = new Point(0, 0);
            _ioSplitContainer.Name = "_ioSplitContainer";
            _ioSplitContainer.Orientation = Orientation.Horizontal;
            // 
            // _ioSplitContainer.Panel1
            // 
            _ioSplitContainer.Panel1.Controls.Add(_chooseVuzComboBox);
            _ioSplitContainer.Panel1.Controls.Add(_cancelButton);
            _ioSplitContainer.Panel1.Controls.Add(_configInfoTextBox);
            _ioSplitContainer.Panel1.Controls.Add(_configButton);
            _ioSplitContainer.Panel1.Controls.Add(_findButton);
            _ioSplitContainer.Panel1MinSize = 120;
            // 
            // _ioSplitContainer.Panel2
            // 
            _ioSplitContainer.Panel2.Controls.Add(_programsFlowLayoutPanel);
            _ioSplitContainer.Size = new Size(1193, 790);
            _ioSplitContainer.SplitterDistance = 274;
            _ioSplitContainer.SplitterWidth = 10;
            _ioSplitContainer.TabIndex = 0;
            // 
            // _chooseVuzComboBox
            // 
            _chooseVuzComboBox.DisplayMember = "UserFriendlyName";
            _chooseVuzComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _chooseVuzComboBox.FormattingEnabled = true;
            _chooseVuzComboBox.Location = new Point(149, 123);
            _chooseVuzComboBox.Name = "_chooseVuzComboBox";
            _chooseVuzComboBox.Size = new Size(131, 38);
            _chooseVuzComboBox.TabIndex = 4;
            _chooseVuzComboBox.SelectedIndexChanged += _chooseVuzComboBox_SelectedIndexChanged;
            // 
            // _cancelButton
            // 
            _cancelButton.Location = new Point(12, 123);
            _cancelButton.Name = "_cancelButton";
            _cancelButton.Size = new Size(131, 40);
            _cancelButton.TabIndex = 3;
            _cancelButton.Text = "Отмена";
            _cancelButton.UseVisualStyleBackColor = true;
            _cancelButton.Click += _cancelButton_Click;
            // 
            // _configInfoTextBox
            // 
            _configInfoTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _configInfoTextBox.Location = new Point(286, 12);
            _configInfoTextBox.Multiline = true;
            _configInfoTextBox.Name = "_configInfoTextBox";
            _configInfoTextBox.ReadOnly = true;
            _configInfoTextBox.Size = new Size(895, 259);
            _configInfoTextBox.TabIndex = 2;
            // 
            // _configButton
            // 
            _configButton.Location = new Point(149, 12);
            _configButton.Name = "_configButton";
            _configButton.Size = new Size(131, 105);
            _configButton.TabIndex = 1;
            _configButton.Text = "Настройка";
            _configButton.UseVisualStyleBackColor = true;
            _configButton.Click += _configButton_Click;
            // 
            // _findButton
            // 
            _findButton.Location = new Point(12, 12);
            _findButton.Name = "_findButton";
            _findButton.Size = new Size(131, 105);
            _findButton.TabIndex = 0;
            _findButton.Text = "Найти";
            _findButton.UseVisualStyleBackColor = true;
            _findButton.Click += _findButton_Click;
            // 
            // _programsFlowLayoutPanel
            // 
            _programsFlowLayoutPanel.AutoScroll = true;
            _programsFlowLayoutPanel.Dock = DockStyle.Fill;
            _programsFlowLayoutPanel.FlowDirection = FlowDirection.TopDown;
            _programsFlowLayoutPanel.Location = new Point(0, 0);
            _programsFlowLayoutPanel.Name = "_programsFlowLayoutPanel";
            _programsFlowLayoutPanel.Padding = new Padding(6);
            _programsFlowLayoutPanel.Size = new Size(1193, 506);
            _programsFlowLayoutPanel.TabIndex = 0;
            _programsFlowLayoutPanel.WrapContents = false;
            _programsFlowLayoutPanel.SizeChanged += _programsFlowLayoutPanel_SizeChanged;
            _programsFlowLayoutPanel.ControlAdded += _programsFlowLayoutPanel_ControlAdded;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1193, 790);
            Controls.Add(_ioSplitContainer);
            Name = "MainForm";
            Text = "Отслеживатель МИФИ";
            _ioSplitContainer.Panel1.ResumeLayout(false);
            _ioSplitContainer.Panel1.PerformLayout();
            _ioSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_ioSplitContainer).EndInit();
            _ioSplitContainer.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel _programsFlowLayoutPanel;
        private TextBox _configInfoTextBox;
        private Button _findButton;
        private Button _cancelButton;
        private ComboBox _chooseVuzComboBox;
    }
}
