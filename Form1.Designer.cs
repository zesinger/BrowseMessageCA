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
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem("ACT");
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem("ABI");
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem("MAC");
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem("REV");
            System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem("PNT");
            System.Windows.Forms.ListViewItem listViewItem13 = new System.Windows.Forms.ListViewItem("LAM");
            System.Windows.Forms.ListViewItem listViewItem14 = new System.Windows.Forms.ListViewItem("Autres");
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
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(123, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(212, 28);
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
            listViewItem8,
            listViewItem9,
            listViewItem10,
            listViewItem11,
            listViewItem12,
            listViewItem13,
            listViewItem14});
            this.listMessages.Location = new System.Drawing.Point(15, 108);
            this.listMessages.MultiSelect = false;
            this.listMessages.Name = "listMessages";
            this.listMessages.Size = new System.Drawing.Size(447, 273);
            this.listMessages.TabIndex = 2;
            this.listMessages.UseCompatibleStateImageBehavior = false;
            this.listMessages.View = System.Windows.Forms.View.Details;
            // 
            // cTypes
            // 
            this.cTypes.Text = "Types";
            this.cTypes.Width = 180;
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
            this.listSubMessages.Location = new System.Drawing.Point(15, 387);
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
            this.textMessage.Location = new System.Drawing.Point(279, 387);
            this.textMessage.Multiline = true;
            this.textMessage.Name = "textMessage";
            this.textMessage.ReadOnly = true;
            this.textMessage.Size = new System.Drawing.Size(449, 214);
            this.textMessage.TabIndex = 4;
            // 
            // listLYBA
            // 
            this.listLYBA.FormattingEnabled = true;
            this.listLYBA.Location = new System.Drawing.Point(498, 123);
            this.listLYBA.Name = "listLYBA";
            this.listLYBA.Size = new System.Drawing.Size(111, 251);
            this.listLYBA.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(495, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Balises LY->BA";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(614, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Balises BA->LY";
            // 
            // listBALY
            // 
            this.listBALY.FormattingEnabled = true;
            this.listBALY.Location = new System.Drawing.Point(617, 123);
            this.listBALY.Name = "listBALY";
            this.listBALY.Size = new System.Drawing.Size(111, 251);
            this.listBALY.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(129, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(161, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Couples sender/receiver trouvés";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(132, 72);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(153, 21);
            this.comboBox1.TabIndex = 12;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(291, 55);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(44, 37);
            this.button2.TabIndex = 15;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 613);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label5);
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
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.ComboBox comboBox1;
        public System.Windows.Forms.Button button2;
    }
}

