namespace WinClient
{
    partial class Form1
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
            buttonSend = new Button();
            comboBoxMaterial = new ComboBox();
            comboBoxCostumerType = new ComboBox();
            labelMaterial = new Label();
            labelCostumerType = new Label();
            SuspendLayout();
            // 
            // buttonSend
            // 
            buttonSend.Location = new Point(369, 147);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(90, 31);
            buttonSend.TabIndex = 3;
            buttonSend.Text = "Send";
            buttonSend.UseVisualStyleBackColor = true;
            buttonSend.Click += buttonSend_Click;
            // 
            // comboBoxMaterial
            // 
            comboBoxMaterial.FormattingEnabled = true;            
            comboBoxMaterial.Location = new Point(171, 54);
            comboBoxMaterial.Name = "comboBoxMaterial";
            comboBoxMaterial.Items.AddRange(new object[] {
            "wood",
            "metal"});
            comboBoxMaterial.Size = new Size(288, 23);
            comboBoxMaterial.TabIndex = 4;
            // 
            // comboBoxCostumerType
            // 
            comboBoxCostumerType.FormattingEnabled = true;
            comboBoxCostumerType.Items.AddRange(new object[] { "high", "medium", "low" });
            comboBoxCostumerType.Location = new Point(171, 99);
            comboBoxCostumerType.Name = "comboBoxCostumerType";
            comboBoxCostumerType.Items.AddRange(new object[] {
            "b2b",
            "b2c"});
            comboBoxCostumerType.Size = new Size(288, 23);
            comboBoxCostumerType.TabIndex = 5;
            // 
            // labelMaterial
            // 
            labelMaterial.AutoSize = true;
            labelMaterial.Location = new Point(56, 57);
            labelMaterial.Name = "labelMaterial";
            labelMaterial.Size = new Size(53, 15);
            labelMaterial.TabIndex = 7;
            labelMaterial.Text = "Material:";
            // 
            // labelCostumerType
            // 
            labelCostumerType.AutoSize = true;
            labelCostumerType.Location = new Point(56, 102);
            labelCostumerType.Name = "labelCostumerType";
            labelCostumerType.Size = new Size(90, 15);
            labelCostumerType.TabIndex = 8;
            labelCostumerType.Text = "Costumer Type:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(542, 203);
            Controls.Add(labelCostumerType);
            Controls.Add(labelMaterial);
            Controls.Add(comboBoxCostumerType);
            Controls.Add(comboBoxMaterial);
            Controls.Add(buttonSend);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonSend;
        private ComboBox comboBoxMaterial;
        private ComboBox comboBoxCostumerType;
        private Label labelMaterial;
        private Label labelCostumerType;
    }
}
