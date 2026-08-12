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
            comboBoxCostumerType = new ComboBox();
            comboBoxOrderSize = new ComboBox();
            comboBoxProduct = new ComboBox();
            labelCustomerType = new Label();
            labelOrderSize = new Label();
            labelProduct = new Label();
            SuspendLayout();
            // 
            // buttonSend
            // 
            buttonSend.Location = new Point(369, 201);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(90, 31);
            buttonSend.TabIndex = 3;
            buttonSend.Text = "Send";
            buttonSend.UseVisualStyleBackColor = true;
            buttonSend.Click += buttonSend_Click;
            // 
            // comboBoxCostumerType
            // 
            comboBoxCostumerType.FormattingEnabled = true;
            comboBoxCostumerType.Location = new Point(171, 54);
            comboBoxCostumerType.Name = "comboBoxCostumerType";
            comboBoxCostumerType.Items.AddRange(new object[] {
            "corporate",
            "personal"});
            comboBoxCostumerType.Size = new Size(288, 23);
            comboBoxCostumerType.TabIndex = 4;
            // 
            // comboBoxOrderSize
            // 
            comboBoxOrderSize.FormattingEnabled = true;
            comboBoxOrderSize.Location = new Point(171, 99);
            comboBoxOrderSize.Name = "comboBoxOrderSize";
            comboBoxOrderSize.Items.AddRange(new object[] {
            "high",
            "medium",
            "low"});
            comboBoxOrderSize.Size = new Size(288, 23);
            comboBoxOrderSize.TabIndex = 5;
            // 
            // comboBoxProduct
            // 
            comboBoxProduct.FormattingEnabled = true;
            comboBoxProduct.Location = new Point(171, 141);
            comboBoxProduct.Name = "comboBoxProduct";
            comboBoxProduct.Items.AddRange(new object[] {
            "table",
            "cupboard"});
            comboBoxProduct.Size = new Size(288, 23);
            comboBoxProduct.TabIndex = 6;
            // 
            // labelCustomerType
            // 
            labelCustomerType.AutoSize = true;
            labelCustomerType.Location = new Point(56, 57);
            labelCustomerType.Name = "labelCustomerType";
            labelCustomerType.Size = new Size(90, 15);
            labelCustomerType.TabIndex = 7;
            labelCustomerType.Text = "Customer Type:";
            // 
            // labelOrderSize
            // 
            labelOrderSize.AutoSize = true;
            labelOrderSize.Location = new Point(56, 102);
            labelOrderSize.Name = "labelOrderSize";
            labelOrderSize.Size = new Size(63, 15);
            labelOrderSize.TabIndex = 8;
            labelOrderSize.Text = "Order Size:";
            // 
            // labelProduct
            // 
            labelProduct.AutoSize = true;
            labelProduct.Location = new Point(56, 144);
            labelProduct.Name = "labelProduct";
            labelProduct.Size = new Size(52, 15);
            labelProduct.TabIndex = 9;
            labelProduct.Text = "Product:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(542, 267);
            Controls.Add(labelProduct);
            Controls.Add(labelOrderSize);
            Controls.Add(labelCustomerType);
            Controls.Add(comboBoxProduct);
            Controls.Add(comboBoxOrderSize);
            Controls.Add(comboBoxCostumerType);
            Controls.Add(buttonSend);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonSend;
        private ComboBox comboBoxCostumerType;
        private ComboBox comboBoxOrderSize;
        private ComboBox comboBoxProduct;
        private Label labelCustomerType;
        private Label labelOrderSize;
        private Label labelProduct;
    }
}
