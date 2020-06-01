namespace NeuralNetworkExample2
{
    partial class TeachNetworkNumber
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TeachNetworkNumber));
            this.picturebx_currentNum = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_ActualNum = new System.Windows.Forms.Label();
            this.lbl_learningRate = new System.Windows.Forms.Label();
            this.lbl_moment = new System.Windows.Forms.Label();
            this.pnl_graphic = new System.Windows.Forms.Panel();
            this.lbl_graphic = new System.Windows.Forms.Label();
            this.btn_start = new System.Windows.Forms.Button();
            this.lbl_iteration = new System.Windows.Forms.Label();
            this.lbl_right = new System.Windows.Forms.Label();
            this.lbl_mistakes = new System.Windows.Forms.Label();
            this.lbl_percent = new System.Windows.Forms.Label();
            this.pnl_draw = new System.Windows.Forms.Panel();
            this.btn_check = new System.Windows.Forms.Button();
            this.btn_clear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picturebx_currentNum)).BeginInit();
            this.SuspendLayout();
            // 
            // picturebx_currentNum
            // 
            this.picturebx_currentNum.Location = new System.Drawing.Point(12, 12);
            this.picturebx_currentNum.Name = "picturebx_currentNum";
            this.picturebx_currentNum.Size = new System.Drawing.Size(100, 100);
            this.picturebx_currentNum.TabIndex = 0;
            this.picturebx_currentNum.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F);
            this.label1.Location = new System.Drawing.Point(121, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 31);
            this.label1.TabIndex = 1;
            this.label1.Text = "=";
            // 
            // lbl_ActualNum
            // 
            this.lbl_ActualNum.AutoSize = true;
            this.lbl_ActualNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 55.65F);
            this.lbl_ActualNum.Location = new System.Drawing.Point(156, 20);
            this.lbl_ActualNum.Name = "lbl_ActualNum";
            this.lbl_ActualNum.Size = new System.Drawing.Size(79, 85);
            this.lbl_ActualNum.TabIndex = 2;
            this.lbl_ActualNum.Text = "0";
            // 
            // lbl_learningRate
            // 
            this.lbl_learningRate.AutoSize = true;
            this.lbl_learningRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.lbl_learningRate.Location = new System.Drawing.Point(9, 134);
            this.lbl_learningRate.Name = "lbl_learningRate";
            this.lbl_learningRate.Size = new System.Drawing.Size(101, 17);
            this.lbl_learningRate.TabIndex = 3;
            this.lbl_learningRate.Text = "Learning rate: ";
            // 
            // lbl_moment
            // 
            this.lbl_moment.AutoSize = true;
            this.lbl_moment.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.lbl_moment.Location = new System.Drawing.Point(9, 161);
            this.lbl_moment.Name = "lbl_moment";
            this.lbl_moment.Size = new System.Drawing.Size(66, 17);
            this.lbl_moment.TabIndex = 4;
            this.lbl_moment.Text = "Moment: ";
            // 
            // pnl_graphic
            // 
            this.pnl_graphic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pnl_graphic.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pnl_graphic.Location = new System.Drawing.Point(181, 202);
            this.pnl_graphic.Name = "pnl_graphic";
            this.pnl_graphic.Size = new System.Drawing.Size(895, 335);
            this.pnl_graphic.TabIndex = 5;
            this.pnl_graphic.Paint += new System.Windows.Forms.PaintEventHandler(this.pnl_graphic_Paint);
            // 
            // lbl_graphic
            // 
            this.lbl_graphic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_graphic.AutoSize = true;
            this.lbl_graphic.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.lbl_graphic.Location = new System.Drawing.Point(179, 175);
            this.lbl_graphic.Name = "lbl_graphic";
            this.lbl_graphic.Size = new System.Drawing.Size(62, 17);
            this.lbl_graphic.TabIndex = 6;
            this.lbl_graphic.Text = "Graphic:";
            // 
            // btn_start
            // 
            this.btn_start.Location = new System.Drawing.Point(12, 223);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(163, 45);
            this.btn_start.TabIndex = 8;
            this.btn_start.Text = "Start";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // lbl_iteration
            // 
            this.lbl_iteration.AutoSize = true;
            this.lbl_iteration.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.lbl_iteration.Location = new System.Drawing.Point(9, 192);
            this.lbl_iteration.Name = "lbl_iteration";
            this.lbl_iteration.Size = new System.Drawing.Size(67, 17);
            this.lbl_iteration.TabIndex = 9;
            this.lbl_iteration.Text = "Iteration: ";
            // 
            // lbl_right
            // 
            this.lbl_right.AutoSize = true;
            this.lbl_right.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.lbl_right.Location = new System.Drawing.Point(265, 110);
            this.lbl_right.Name = "lbl_right";
            this.lbl_right.Size = new System.Drawing.Size(49, 17);
            this.lbl_right.TabIndex = 10;
            this.lbl_right.Text = "Right: ";
            // 
            // lbl_mistakes
            // 
            this.lbl_mistakes.AutoSize = true;
            this.lbl_mistakes.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.lbl_mistakes.Location = new System.Drawing.Point(265, 141);
            this.lbl_mistakes.Name = "lbl_mistakes";
            this.lbl_mistakes.Size = new System.Drawing.Size(71, 17);
            this.lbl_mistakes.TabIndex = 11;
            this.lbl_mistakes.Text = "Mistakes: ";
            // 
            // lbl_percent
            // 
            this.lbl_percent.AutoSize = true;
            this.lbl_percent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.lbl_percent.Location = new System.Drawing.Point(265, 169);
            this.lbl_percent.Name = "lbl_percent";
            this.lbl_percent.Size = new System.Drawing.Size(20, 17);
            this.lbl_percent.TabIndex = 12;
            this.lbl_percent.Text = "%";
            // 
            // pnl_draw
            // 
            this.pnl_draw.BackColor = System.Drawing.Color.White;
            this.pnl_draw.Location = new System.Drawing.Point(487, 37);
            this.pnl_draw.Name = "pnl_draw";
            this.pnl_draw.Size = new System.Drawing.Size(100, 100);
            this.pnl_draw.TabIndex = 13;
            // 
            // btn_check
            // 
            this.btn_check.Location = new System.Drawing.Point(487, 143);
            this.btn_check.Name = "btn_check";
            this.btn_check.Size = new System.Drawing.Size(100, 23);
            this.btn_check.TabIndex = 14;
            this.btn_check.Text = "Check";
            this.btn_check.UseVisualStyleBackColor = true;
            this.btn_check.Click += new System.EventHandler(this.btn_check_Click);
            // 
            // btn_clear
            // 
            this.btn_clear.Location = new System.Drawing.Point(487, 172);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(100, 23);
            this.btn_clear.TabIndex = 15;
            this.btn_clear.Text = "clear";
            this.btn_clear.UseVisualStyleBackColor = true;
            this.btn_clear.Click += new System.EventHandler(this.btn_clear_Click);
            // 
            // TeachNetworkNumber
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(1088, 549);
            this.Controls.Add(this.btn_clear);
            this.Controls.Add(this.btn_check);
            this.Controls.Add(this.pnl_draw);
            this.Controls.Add(this.lbl_percent);
            this.Controls.Add(this.lbl_mistakes);
            this.Controls.Add(this.lbl_right);
            this.Controls.Add(this.lbl_iteration);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.lbl_graphic);
            this.Controls.Add(this.pnl_graphic);
            this.Controls.Add(this.lbl_moment);
            this.Controls.Add(this.lbl_learningRate);
            this.Controls.Add(this.lbl_ActualNum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picturebx_currentNum);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TeachNetworkNumber";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "+";
            ((System.ComponentModel.ISupportInitialize)(this.picturebx_currentNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picturebx_currentNum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_ActualNum;
        private System.Windows.Forms.Label lbl_learningRate;
        private System.Windows.Forms.Label lbl_moment;
        private System.Windows.Forms.Panel pnl_graphic;
        private System.Windows.Forms.Label lbl_graphic;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.Label lbl_iteration;
        private System.Windows.Forms.Label lbl_right;
        private System.Windows.Forms.Label lbl_mistakes;
        private System.Windows.Forms.Label lbl_percent;
        private System.Windows.Forms.Panel pnl_draw;
        private System.Windows.Forms.Button btn_check;
        private System.Windows.Forms.Button btn_clear;
    }
}

