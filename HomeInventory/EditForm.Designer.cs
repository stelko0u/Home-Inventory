namespace HomeInventory
{
    partial class EditForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SaveButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.CategoryBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.DateBox = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.PriceBox = new System.Windows.Forms.TextBox();
            this.QuantityBox = new System.Windows.Forms.TextBox();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SaveButton
            // 
            this.SaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveButton.Location = new System.Drawing.Point(45, 343);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(200, 42);
            this.SaveButton.TabIndex = 21;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(42, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 16);
            this.label5.TabIndex = 20;
            this.label5.Text = "Category";
            // 
            // CategoryBox
            // 
            this.CategoryBox.FormattingEnabled = true;
            this.CategoryBox.Location = new System.Drawing.Point(45, 106);
            this.CategoryBox.Name = "CategoryBox";
            this.CategoryBox.Size = new System.Drawing.Size(200, 24);
            this.CategoryBox.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(42, 267);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 16);
            this.label4.TabIndex = 18;
            this.label4.Text = "Date";
            // 
            // DateBox
            // 
            this.DateBox.Location = new System.Drawing.Point(45, 286);
            this.DateBox.Name = "DateBox";
            this.DateBox.Size = new System.Drawing.Size(200, 22);
            this.DateBox.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 207);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 16);
            this.label3.TabIndex = 16;
            this.label3.Text = "Price";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 16);
            this.label2.TabIndex = 15;
            this.label2.Text = "Quantity";
            // 
            // PriceBox
            // 
            this.PriceBox.Location = new System.Drawing.Point(45, 226);
            this.PriceBox.Name = "PriceBox";
            this.PriceBox.Size = new System.Drawing.Size(200, 22);
            this.PriceBox.TabIndex = 14;
            this.PriceBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PriceBox_KeyPress);
            // 
            // QuantityBox
            // 
            this.QuantityBox.Location = new System.Drawing.Point(45, 165);
            this.QuantityBox.Name = "QuantityBox";
            this.QuantityBox.Size = new System.Drawing.Size(200, 22);
            this.QuantityBox.TabIndex = 13;
            this.QuantityBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.QuantityBox_KeyPress);
            // 
            // NameBox
            // 
            this.NameBox.Location = new System.Drawing.Point(45, 51);
            this.NameBox.Name = "NameBox";
            this.NameBox.Size = new System.Drawing.Size(200, 22);
            this.NameBox.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(42, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 16);
            this.label6.TabIndex = 11;
            this.label6.Text = "Name";
            // 
            // EditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 450);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.CategoryBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DateBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PriceBox);
            this.Controls.Add(this.QuantityBox);
            this.Controls.Add(this.NameBox);
            this.Controls.Add(this.label6);
            this.Name = "EditForm";
            this.Text = "EditForm";
            this.Load += new System.EventHandler(this.EditForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox CategoryBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker DateBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox PriceBox;
        private System.Windows.Forms.TextBox QuantityBox;
        private System.Windows.Forms.TextBox NameBox;
        private System.Windows.Forms.Label label6;
    }
}