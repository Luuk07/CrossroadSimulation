namespace AmpelSimulation
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelCounter = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.labelTimer = new System.Windows.Forms.Label();
            this.trackBarOfSimSpeed = new System.Windows.Forms.TrackBar();
            this.speedOfSim = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOfSimSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // labelCounter
            // 
            this.labelCounter.AutoSize = true;
            this.labelCounter.Location = new System.Drawing.Point(342, 32);
            this.labelCounter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelCounter.Name = "labelCounter";
            this.labelCounter.Size = new System.Drawing.Size(0, 13);
            this.labelCounter.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 56);
            this.button1.TabIndex = 1;
            this.button1.Text = "Modi 1";
            this.button1.UseVisualStyleBackColor = true;
          
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(40, 20);
            this.button2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(5, 5);
            this.button2.TabIndex = 2;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(0, 55);
            this.button3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(86, 63);
            this.button3.TabIndex = 3;
            this.button3.Text = "Modi 2";
            this.button3.UseVisualStyleBackColor = true;
          
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(0, 116);
            this.button4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(86, 62);
            this.button4.TabIndex = 4;
            this.button4.Text = "Modi 3";
            this.button4.UseVisualStyleBackColor = true;
           
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(0, 174);
            this.button5.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(86, 57);
            this.button5.TabIndex = 5;
            this.button5.Text = "Modi 4";
            this.button5.UseVisualStyleBackColor = true;
          
            // 
            // labelTimer
            // 
            this.labelTimer.AutoSize = true;
            this.labelTimer.Location = new System.Drawing.Point(511, 5);
            this.labelTimer.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTimer.Name = "labelTimer";
            this.labelTimer.Size = new System.Drawing.Size(35, 13);
            this.labelTimer.TabIndex = 7;
            this.labelTimer.Text = "label2";
            // 
            // trackBarOfSimSpeed
            // 
            this.trackBarOfSimSpeed.Location = new System.Drawing.Point(524, 116);
            this.trackBarOfSimSpeed.Maximum = 25;
            this.trackBarOfSimSpeed.Minimum = 1;
            this.trackBarOfSimSpeed.Name = "trackBarOfSimSpeed";
            this.trackBarOfSimSpeed.Size = new System.Drawing.Size(227, 45);
            this.trackBarOfSimSpeed.TabIndex = 8;
            this.trackBarOfSimSpeed.Value = 1;
            // 
            // speedOfSim
            // 
            this.speedOfSim.AutoSize = true;
            this.speedOfSim.Location = new System.Drawing.Point(531, 80);
            this.speedOfSim.Name = "speedOfSim";
            this.speedOfSim.Size = new System.Drawing.Size(109, 13);
            this.speedOfSim.TabIndex = 9;
            this.speedOfSim.Text = "Tempo der Simulation";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 649);
            this.Controls.Add(this.speedOfSim);
            this.Controls.Add(this.trackBarOfSimSpeed);
            this.Controls.Add(this.labelTimer);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelCounter);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOfSimSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelCounter;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label labelTimer;
        private System.Windows.Forms.TrackBar trackBarOfSimSpeed;
        private System.Windows.Forms.Label speedOfSim;
    }
}

