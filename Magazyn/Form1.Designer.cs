namespace Magazyn
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
            listViewProdukty = new ListView();
            buttonIssueProduct = new Button();
            buttonReceiveProduct = new Button();
            buttonSearchProduct = new Button();
            buttonEditProduct = new Button();
            SuspendLayout();
            // 
            // listViewProdukty
            // 
            listViewProdukty.Location = new Point(12, 12);
            listViewProdukty.Name = "listViewProdukty";
            listViewProdukty.Size = new Size(604, 298);
            listViewProdukty.TabIndex = 3;
            listViewProdukty.UseCompatibleStateImageBehavior = false;
            listViewProdukty.View = View.Details;
            // 
            // buttonIssueProduct
            // 
            buttonIssueProduct.Location = new Point(639, 230);
            buttonIssueProduct.Name = "buttonIssueProduct";
            buttonIssueProduct.Size = new Size(134, 58);
            buttonIssueProduct.TabIndex = 4;
            buttonIssueProduct.Text = "Wydaj produkt";
            buttonIssueProduct.UseVisualStyleBackColor = true;
            buttonIssueProduct.Click += buttonIssueProduct_Click;
            // 
            // buttonReceiveProduct
            // 
            buttonReceiveProduct.Location = new Point(639, 166);
            buttonReceiveProduct.Name = "buttonReceiveProduct";
            buttonReceiveProduct.Size = new Size(134, 58);
            buttonReceiveProduct.TabIndex = 5;
            buttonReceiveProduct.Text = "Przyjmij produkt";
            buttonReceiveProduct.UseVisualStyleBackColor = true;
            buttonReceiveProduct.Click += buttonReceiveProduct_Click;
            // 
            // buttonSearchProduct
            // 
            buttonSearchProduct.Location = new Point(639, 38);
            buttonSearchProduct.Name = "buttonSearchProduct";
            buttonSearchProduct.Size = new Size(134, 58);
            buttonSearchProduct.TabIndex = 6;
            buttonSearchProduct.Text = "Wyszukaj produkt";
            buttonSearchProduct.UseVisualStyleBackColor = true;
            buttonSearchProduct.Click += buttonSearchProduct_Click;
            // 
            // buttonEditProduct
            // 
            buttonEditProduct.Location = new Point(639, 102);
            buttonEditProduct.Name = "buttonEditProduct";
            buttonEditProduct.Size = new Size(134, 58);
            buttonEditProduct.TabIndex = 7;
            buttonEditProduct.Text = "Edytuj produkt";
            buttonEditProduct.UseVisualStyleBackColor = true;
            buttonEditProduct.Click += buttonEditProduct_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(798, 331);
            Controls.Add(buttonEditProduct);
            Controls.Add(buttonSearchProduct);
            Controls.Add(buttonReceiveProduct);
            Controls.Add(buttonIssueProduct);
            Controls.Add(listViewProdukty);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion
        private ListView listViewProdukty;
        private Button buttonIssueProduct;
        private Button buttonReceiveProduct;
        private Button buttonSearchProduct;
        private Button buttonEditProduct;
    }
}
