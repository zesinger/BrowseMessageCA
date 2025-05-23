using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BrowseMessageCA
{
    public partial class Form1 : Form
    {
        const int majVer = 1;
        const int minver = 0;
        public enum MCAElementType
        {
            type,
            sender,
            receiver,
            registration,
            seqnum,
            ssr,
            departure,
            destination,
            associe,
            notassocie,
            notassociable,
            notdecode,
            ptid,
        }
        readonly public List<(MCAElementType Id, string Text)> MessageCAElement = new List<(MCAElementType Id, string Text)>()
        {
            (MCAElementType.type,"-TITLE"),
            (MCAElementType.sender,"-SENDER -FAC"),
            (MCAElementType.receiver,"-RECVR -FAC"),
            (MCAElementType.registration,"-ARCID"), // must be before SEQNUM
            (MCAElementType.seqnum,"-SEQNUM"),
            (MCAElementType.ssr,"-SSRCODE"),
            (MCAElementType.departure,"-ADEP"),
            (MCAElementType.destination,"-ADES"),
            (MCAElementType.associe,"-ASSOCIE"),
            (MCAElementType.notassocie,"-ASSOCIABLE NON ASSOCIE"),
            (MCAElementType.notassociable,"-NON ASSOCIABLE"),
            (MCAElementType.notdecode,"-NON DECODE"),
            (MCAElementType.ptid,"-PTID"),
        };
        public enum MCAMessageType
        {
            ACT,
            ABI,
            MAC,
            REV,
            PNT,
            LAM,
            NUL,
        }
        static public List<(MCAMessageType Type, string text)> MCAMesType = new List<(MCAMessageType Type, string text)>()
        {
            (MCAMessageType.ACT,"ACT"),
            (MCAMessageType.ABI,"ABI"),
            (MCAMessageType.MAC,"MAC"),
            (MCAMessageType.REV,"REV"),
            (MCAMessageType.PNT,"PNT"),
            (MCAMessageType.LAM,"LAM"),
            (MCAMessageType.NUL,"NUL"),
        };
        private MCAMessageType GetMessageTypeFromText(string ttype)
        {
            foreach (var type in MCAMesType)
            {
                if (ttype == type.text) return type.Type;
            }
            return MCAMessageType.NUL;
        }
        private string GetMessageTypeFromType(MCAMessageType type)
        {
            foreach (var ttype in MCAMesType)
            {
                if (type == ttype.Type) return ttype.text;
            }
            return "NUL";
        }
        private int dispList = -1;
        private bool dispNoLAM = false;
        private bool isLoaded = false;
        List<string> filelines = new List<string>();
        public struct MessageCALine
        {
            public string fullline;
            public DateTime datetime;
            public List<(MCAElementType type, string value)> elements;
            public MCAMessageType mestype;
            public string registration;
            public string departure;
            public string destination;
            public string sender;
            public string receiver;
            public int seqnum;
            public int seqnumrel;
            public bool islam;
            public bool associe;
            public bool notassocie;
            public bool notassociable;
            public bool notdecode;
            public string ptid;
        }
        private List<MessageCALine> msLines;

        public Form1()
        {
            InitializeComponent();
            listMessages.MouseClick += listMessages_OnClick;
            listSubMessages.MouseClick += listSubMessages_OnClick;
            this.Text = "Analyse des messages v" + majVer.ToString() + "." + minver.ToString()+" par David Lafarge";
        }
        private void listMessages_OnClick(object sender, MouseEventArgs e)
        {
            if (!isLoaded) return;
            var info = listMessages.HitTest(e.Location);
            if (info.Item != null && info.SubItem != null)
            {
                //int rowIndex = info.Item.Index; // Row number
                int columnIndex = info.Item.SubItems.IndexOf(info.SubItem); // Column number

                ClickFunction(columnIndex, info.Item.Text);
            }
        }
        private void ClickFunction(int col, string itemtxt)
        {
            listSubMessages.Items.Clear();
            textMessage.Text = "";
            int i = -1;
            for (int j = 0; j < MCAMesType.Count; j++)
            {
                if (MCAMesType[j].text == itemtxt) i = j;
            }
            dispList = i;
            if (i == -1) return;
            if (col == 0 || col == 1)
            {
                dispNoLAM = false;
                foreach (var msg in listMes[i])
                {
                    var item = new ListViewItem(msg.registration);
                    string ass = "";
                    if (msg.associe == true) ass = "Oui/"; else ass = "Non/";
                    if (msg.notassocie == true) ass += "Oui"; else ass += "Non";
                    item.SubItems.Add(ass);
                    item.SubItems.Add(msg.sender + "->" + msg.receiver);
                    listSubMessages.Items.Add(item);
                }
            }
            else if (col == 2)
            {
                dispNoLAM = true;
                foreach (var msg in listLAMes[i])
                {
                    var item = new ListViewItem(msg.registration);
                    string ass = "";
                    if (msg.associe == true) ass = "Oui/"; else ass = "Non/";
                    if (msg.notassocie == true) ass += "Oui"; else ass += "Non";
                    item.SubItems.Add(ass);
                    item.SubItems.Add(msg.sender + "->" + msg.receiver);
                    listSubMessages.Items.Add(item);
                }
            }
        }
        private void listSubMessages_OnClick(object sender, MouseEventArgs e)
        {
            if (!isLoaded) return;
            var info = listSubMessages.HitTest(e.Location);
            if (info.Item != null && info.SubItem != null)
            {
                int rowIndex = info.Item.Index; // Row number

                ClickSubFunction(rowIndex);
            }
        }
        private void ClickSubFunction(int row)
        {
            if (dispNoLAM)
            {
                if (row < listLAMes[dispList].Count)
                    textMessage.Text = listLAMes[dispList][row].fullline;
                else textMessage.Text = "";
            }
            else
            {
                if (row < listMes[dispList].Count)
                    textMessage.Text = listMes[dispList][row].fullline;
                else textMessage.Text = "";
            }
        }
        private void ParseMessageCA(string fileContent)
        {
            filelines = fileContent
                .Replace("\r\n", "\n")    // Normalize Windows line endings
                .Replace("\r", "\n")      // Normalize old Mac line endings
                .Split('\n')              // Split into lines
                .Where(line => !string.IsNullOrWhiteSpace(line))  // Filter out empty or whitespace-only lines
                .ToList();
            msLines = new List<MessageCALine>();
            foreach (var fileline in filelines)
            {
                MessageCALine ms = new MessageCALine();
                ms.fullline = fileline;
                ms.registration = "";
                ms.seqnumrel = -1;
                ms.islam = false;
                ms.elements = new List<(MCAElementType type, string value)>();
                ms.associe = false;
                ms.notassocie = false;
                ms.notdecode = false;
                ms.notassociable = false;
                ms.sender = "";
                ms.receiver = "";
                ms.ptid = "";
                // extract date/time
                string dateTimePart = fileline.Substring(0, 20);
                string normalized = dateTimePart.Replace("H", ":").Replace("'", ":").Replace("\"", "");
                if (char.IsDigit(normalized[0]) && normalized[1] == '/')
                {
                    normalized = ("0" + normalized).Trim(); // Add leading 0 and removing " "
                }
                // une ligne qui ne commence pas par la date et l'heure est ignorée:
                if (!DateTime.TryParseExact(normalized, "dd/MM/yyyy HH:mm:ss",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out ms.datetime)) continue;
                foreach (var elt in MessageCAElement)
                {
                    int index = 0;
                    while ((index = fileline.IndexOf(elt.Text, index)) != -1)
                    {
                        switch (elt.Id)
                        {
                            // messages that have an extra information to get
                            case MCAElementType.type:
                            case MCAElementType.sender:
                            case MCAElementType.receiver:
                            case MCAElementType.registration: // registration
                            case MCAElementType.ssr:
                            case MCAElementType.departure: // departure
                            case MCAElementType.destination: // destination
                            case MCAElementType.ptid:
                                {
                                    int start = index + elt.Text.Length;
                                    int dashIndex = fileline.IndexOf('-', start);
                                    if (dashIndex != -1 && dashIndex > start)
                                    {
                                        (MCAElementType type, string value) nelt = (elt.Id, fileline.Substring(start, dashIndex - start).Trim());
                                        if (elt.Id != MCAElementType.type && elt.Id != MCAElementType.registration &&
                                            elt.Id != MCAElementType.departure && elt.Id != MCAElementType.destination &&
                                            elt.Id != MCAElementType.sender && elt.Id != MCAElementType.receiver &&
                                            elt.Id != MCAElementType.ptid)
                                            ms.elements.Add(nelt);
                                        else
                                        {
                                            if (elt.Id == MCAElementType.registration)
                                                ms.registration = fileline.Substring(start, dashIndex - start).Trim();
                                            else if (elt.Id == MCAElementType.type)
                                                ms.mestype = GetMessageTypeFromText(fileline.Substring(start, dashIndex - start).Trim());
                                            else if (elt.Id == MCAElementType.departure)
                                                ms.departure = fileline.Substring(start, dashIndex - start).Trim();
                                            else if (elt.Id == MCAElementType.destination)
                                                ms.destination = fileline.Substring(start, dashIndex - start).Trim();
                                            else if (elt.Id == MCAElementType.sender && ms.sender == "")
                                                ms.sender = fileline.Substring(start, dashIndex - start).Trim();
                                            else if (elt.Id == MCAElementType.receiver && ms.receiver == "") 
                                                ms.receiver = fileline.Substring(start, dashIndex - start).Trim();
                                            else if (elt.Id == MCAElementType.ptid)
                                                ms.ptid= fileline.Substring(start, dashIndex - start).Trim();
                                        }
                                    }
                                    index = start;
                                }
                                // messages to save in their specific variable
                                break;
                            // messages that have no extra information
                            case MCAElementType.associe:
                                {
                                    ms.associe = true;
                                    index += elt.Text.Length;
                                }
                                break;
                            case MCAElementType.notassocie:
                                {
                                    ms.notassocie = true;
                                    index += elt.Text.Length;
                                }
                                break;
                            case MCAElementType.notdecode:
                                {
                                    ms.notdecode = true;
                                    index += elt.Text.Length;
                                }
                                break;
                            case MCAElementType.notassociable:
                                {
                                    ms.notassociable = true;
                                    index += elt.Text.Length;
                                }
                                break;
                            // SEQNUM must be handled separately
                            case MCAElementType.seqnum:
                                {
                                    int start = index + elt.Text.Length;
                                    int dashIndex = fileline.IndexOf('-', start);
                                    if (dashIndex != -1 && dashIndex > start)
                                    {
                                        // we check the text behind the the number if this is the real seqnum of the message or the number of the main message for this flight
                                        string nexelt = fileline.Substring(dashIndex, fileline.IndexOf(" ", dashIndex) - dashIndex).Trim();
                                        if (nexelt == "-REFDATA")
                                            ms.seqnumrel = int.Parse(fileline.Substring(start, dashIndex - start).Trim());
                                        else if (nexelt == "-ARCID")
                                            ms.seqnum = int.Parse(fileline.Substring(start, dashIndex - start).Trim());
                                    }
                                    index = start;
                                }
                                break;

                        }
                    }
                }
                if (ms.registration == "" && ms.seqnumrel != -1)
                {
                    // this is a line with no registration, it must be linked via SEQNUM to a previous line
                    for (int i = 0; i < msLines.Count; i++)
                    {
                        var updated = msLines[i];
                        if (ms.seqnumrel == updated.seqnum && (ms.datetime - updated.datetime).TotalSeconds < 10 &&
                            ms.sender == updated.sender && ms.receiver == updated.receiver)
                        {
                            ms.registration = updated.registration;
                            ms.departure = msLines[i].departure;
                            ms.destination = msLines[i].destination;
                            // declare the initial message as LAMed
                            if (ms.mestype == MCAMessageType.LAM)
                            {
                                updated.islam = true;
                                msLines[i] = updated;
                            }
                        }
                    }
                }
                if (ms.registration == "" && ms.mestype != MCAMessageType.LAM)
                    MessageBox.Show("The line at " + ms.datetime.ToString() + " have no registration information and doesn't correspond to another line found before, it is ignored");
                msLines.Add(ms);
            }
        }
        List<MessageCALine>[] listMes = new List<MessageCALine>[MCAMesType.Count];
        List<MessageCALine>[] listLAMes = new List<MessageCALine>[MCAMesType.Count];
        private void FillTableau()
        {
            listMessages.Items.Clear();
            listSubMessages.Items.Clear();
            ListViewItem item;
            for (int j = 0; j < MCAMesType.Count; j++)
            {
                var type = MCAMesType[j];
                if (type.text == "NUL") continue;// || type.text == "LAM") continue;
                listMes[j] = new List<MessageCALine>();
                listLAMes[j] = new List<MessageCALine>();
                int nlyba = 0;
                int nbaly = 0;
                foreach (var msg in msLines)
                {

                    if (msg.mestype == MCAMesType[j].Type)
                    {
                        listMes[j].Add(msg);
                        if (!msg.islam) listLAMes[j].Add(msg);
                        if (msg.sender == "LY" && msg.receiver == "BA")
                        {
                            nlyba++;
                        }
                        if (msg.sender == "BA" && msg.receiver == "LY")
                        {
                            nbaly++;
                        }
                    }
                }
                item = new ListViewItem(type.text);
                item.SubItems.Add(listMes[j].Count().ToString());
                item.SubItems.Add(listLAMes[j].Count().ToString());
                item.SubItems.Add(nlyba.ToString());
                item.SubItems.Add(nbaly.ToString());
                listMessages.Items.Add(item);
            }
            int nndeclyba = 0;
            int nanalyba = 0;
            int nnalyba = 0;
            int nasslyba = 0;
            int nndecbaly = 0;
            int nanabaly = 0;
            int nnabaly = 0;
            int nassbaly = 0;
            //List<string> baliselyba = new List<string>();
            //List<string> balisebaly = new List<string>();
            listBALY.Items.Clear();
            listLYBA.Items.Clear();
            foreach (var msg in msLines)
            {
                if (msg.sender == "LY" && msg.receiver == "BA")
                {
                    if (msg.notdecode) nndeclyba++;
                    if (msg.notassocie) nanalyba++;
                    if (msg.notassociable) nnalyba++;
                    if (msg.associe) nasslyba++;
                    if (msg.ptid!="") listLYBA.Items.Add(msg.ptid);
                }
                if (msg.sender == "BA" && msg.receiver == "LY")
                {
                    if (msg.notdecode) nndecbaly++;
                    if (msg.notassocie) nanabaly++;
                    if (msg.notassociable) nnabaly++;
                    if (msg.associe) nassbaly++;
                    if (msg.ptid != "") listBALY.Items.Add(msg.ptid);
                }
            }
            item = new ListViewItem("NON DECODE");
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add(nndeclyba.ToString());
            item.SubItems.Add(nndecbaly.ToString());
            listMessages.Items.Add(item);
            item = new ListViewItem("ASSOCIABLE NON ASSOCIE");
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add(nanalyba.ToString());
            item.SubItems.Add(nanabaly.ToString());
            listMessages.Items.Add(item);
            item = new ListViewItem("NON ASSOCIABLE");
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add(nnalyba.ToString());
            item.SubItems.Add(nnabaly.ToString());
            listMessages.Items.Add(item);
            item = new ListViewItem("ASSOCIE");
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add(nasslyba.ToString());
            item.SubItems.Add(nassbaly.ToString());
            listMessages.Items.Add(item);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var filePath = string.Empty;

                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = "c:\\";
                    openFileDialog.Filter = "txt files (*.txt)|*.txt";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //Get the path of specified file
                        filePath = openFileDialog.FileName;

                        //Read the contents of the file into a stream
                        var fileStream = openFileDialog.OpenFile();
                        string fileContent = string.Empty;
                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            fileContent = reader.ReadToEnd();
                            ParseMessageCA(fileContent);
                            //foreach (var msg in msLines)
                            //{
                            //    foreach (var type in MCAMesType)
                            //    {
                            //        if (msg.departure == "LY" && msg.destination == "BA" && msg.mestype == type.Type)
                            //        {

                            //        }
                            //}
                            FillTableau();
                            isLoaded = true;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
