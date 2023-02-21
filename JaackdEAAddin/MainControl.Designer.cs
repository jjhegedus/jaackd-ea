namespace JaackdEAAddin {
  partial class MainControl {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.title = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // title
      // 
      this.title.AutoSize = true;
      this.title.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
      this.title.Location = new System.Drawing.Point(156, 25);
      this.title.Name = "title";
      this.title.Size = new System.Drawing.Size(269, 37);
      this.title.TabIndex = 0;
      this.title.Text = "Jaackd Main Window";
      // 
      // MainControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.title);
      this.Name = "MainControl";
      this.Size = new System.Drawing.Size(571, 375);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label title;
  }
}
