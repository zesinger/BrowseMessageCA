namespace BrowseMessageCA
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("ACT");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("ABI");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("MAC");
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("REV");
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("PNT");
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("LAM");
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listMessages = new System.Windows.Forms.ListView();
            this.cTypes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cLAM = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cDir1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cDir2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listSubMessages = new System.Windows.Forms.ListView();
            this.columnReg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnAssocie = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textMessage = new System.Windows.Forms.TextBox();
            this.listLYBA = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.listBALY = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(123, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(605, 28);
            this.button1.TabIndex = 0;
            this.button1.Text = "Parcourir...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Fichier texte à ouvrir:";
            // 
            // listMessages
            // 
            this.listMessages.AutoArrange = false;
            this.listMessages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cTypes,
            this.cNumber,
            this.cLAM,
            this.cDir1,
            this.cDir2});
            this.listMessages.FullRowSelect = true;
            this.listMessages.GridLines = true;
            this.listMessages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listMessages.HideSelection = false;
            this.listMessages.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6});
            this.listMessages.Location = new System.Drawing.Point(15, 48);
            this.listMessages.MultiSelect = false;
            this.listMessages.Name = "listMessages";
            this.listMessages.Size = new System.Drawing.Size(353, 273);
            this.listMessages.TabIndex = 2;
            this.listMessages.UseCompatibleStateImageBehavior = false;
            this.listMessages.View = System.Windows.Forms.View.Details;
            // 
            // cTypes
            // 
            this.cTypes.Text = "Types";
            this.cTypes.Width = 86;
            // 
            // cNumber
            // 
            this.cNumber.Text = "Total";
            // 
            // cLAM
            // 
            this.cLAM.Text = "Non LAMés";
            this.cLAM.Width = 80;
            // 
            // cDir1
            // 
            this.cDir1.Text = "LY->BA";
            // 
            // cDir2
            // 
            this.cDir2.Text = "BA->LY";
            // 
            // listSubMessages
            // 
            this.listSubMessages.AutoArrange = false;
            this.listSubMessages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnReg,
            this.columnAssocie,
            this.columnPath});
            this.listSubMessages.FullRowSelect = true;
            this.listSubMessages.GridLines = true;
            this.listSubMessages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listSubMessages.HideSelection = false;
            this.listSubMessages.Location = new System.Drawing.Point(470, 327);
            this.listSubMessages.MultiSelect = false;
            this.listSubMessages.Name = "listSubMessages";
            this.listSubMessages.Size = new System.Drawing.Size(258, 214);
            this.listSubMessages.TabIndex = 3;
            this.listSubMessages.UseCompatibleStateImageBehavior = false;
            this.listSubMessages.View = System.Windows.Forms.View.Details;
            // 
            // columnReg
            // 
            this.columnReg.Text = "Immatriculation";
            this.columnReg.Width = 90;
            // 
            // columnAssocie
            // 
            this.columnAssocie.Text = "Associé/Non";
            this.columnAssocie.Width = 80;
            // 
            // columnPath
            // 
            this.columnPath.Text = "Direction";
            this.columnPath.Width = 80;
            // 
            // textMessage
            // 
            this.textMessage.Location = new System.Drawing.Point(15, 327);
            this.textMessage.Multiline = true;
            this.textMessage.Name = "textMessage";
            this.textMessage.ReadOnly = true;
            this.textMessage.Size = new System.Drawing.Size(449, 214);
            this.textMessage.TabIndex = 4;
            // 
            // listLYBA
            // 
            this.listLYBA.FormattingEnabled = true;
            this.listLYBA.Location = new System.Drawing.Point(390, 63);
            this.listLYBA.Name = "listLYBA";
            this.listLYBA.Size = new System.Drawing.Size(111, 251);
            this.listLYBA.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(387, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Balises LY->BA";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(506, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Balises BA->LY";
            // 
            // listBALY
            // 
            this.listBALY.FormattingEnabled = true;
            this.listBALY.Location = new System.Drawing.Point(509, 63);
            this.listBALY.Name = "listBALY";
            this.listBALY.Size = new System.Drawing.Size(111, 251);
            this.listBALY.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 553);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBALY);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listLYBA);
            this.Controls.Add(this.textMessage);
            this.Controls.Add(this.listSubMessages);
            this.Controls.Add(this.listMessages);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Analyse des messages:";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader cLAM;
        public System.Windows.Forms.ListView listMessages;
        public System.Windows.Forms.ListView listSubMessages;
        public System.Windows.Forms.ColumnHeader columnReg;
        private System.Windows.Forms.ColumnHeader columnAssocie;
        private System.Windows.Forms.TextBox textMessage;
        private System.Windows.Forms.ColumnHeader columnPath;
        private System.Windows.Forms.ColumnHeader cTypes;
        private System.Windows.Forms.ColumnHeader cNumber;
        public System.Windows.Forms.ColumnHeader cDir1;
        public System.Windows.Forms.ColumnHeader cDir2;
        public System.Windows.Forms.ListBox listLYBA;
        public System.Windows.Forms.ListBox listBALY;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label3;
    }
}

