using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BrowseMessageCA
{
    public partial class Form1 : Form
    {
        // version du logiciel
        private const int majVer = 1;
        private const int minver = 2;

        // terrains sender et receiver
        private string Terrain1 = "LY";
        private string Terrain2 = "BA";
        private List<(string, string)> CouplesTerrains = new List<(string, string)>();
        private string fileContent = string.Empty;
        //List<string> AllTerrains = new List<string>();

        private int LigneNul = -1;
        private const string NulText = "Autre";
        // toujours laisser "type" en premier dans cette enum
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
        // toujours laisser type/TITLE en premier dans cette List<>
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
            if (!autresTypes.Contains(ttype)) autresTypes.Add(ttype);
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
        private bool isLoaded = false;
        //private bool dispNoLAM = false;
        private int dispList = -1;
        List<string> filelines = new List<string>();
        List<string> autresTypes = new List<string>();
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
            label2.Text = "Balises " + Terrain1 + "->" + Terrain2;
            label3.Text = "Balises " + Terrain2 + "->" + Terrain1;
            listMessages.Columns[3].Text = Terrain1 + "->" + Terrain2;
            listMessages.Columns[4].Text = Terrain2 + "->" + Terrain1;
            listMessages.MouseClick += listMessages_OnClick;
            listSubMessages.MouseClick += listSubMessages_OnClick;
            this.Text = "Analyse des messages v" + majVer.ToString() + "." + minver.ToString() + " par David Lafarge";
        }
        private void listMessages_OnClick(object sender, MouseEventArgs e)
        {
            if (!isLoaded) return;
            var info = listMessages.HitTest(e.Location);
            if (info.Item != null && info.SubItem != null)
            {
                int rowIndex = info.Item.Index;
                int columnIndex = info.Item.SubItems.IndexOf(info.SubItem); // Column number

                ClickFunction(columnIndex, rowIndex, info.Item.Text);
            }
        }
        List<MessageCALine> listDispMes;
        private void ClickFunction(int col, int row, string itemtxt)
        {
            listSubMessages.Items.Clear();
            textMessage.Text = "";
            int i = -1;
            for (int j = 0; j < MCAMesType.Count; j++)
            {
                if (MCAMesType[j].text == itemtxt) i = j;
                else if (itemtxt.StartsWith(NulText)) i = j;
            }
            dispList = i;
            if (i == -1) return;
            if (row <= LigneNul)
            {
                if (col == 1)
                {
                    //dispNoLAM = false;
                    foreach (var msg in listMes[i])
                    {
                        var item = new ListViewItem(msg.registration);
                        string ass = "";
                        if (msg.associe == true) ass = "Oui/"; else ass = "Non/";
                        if (msg.notassocie == true) ass += "Oui"; else ass += "Non";
                        item.SubItems.Add(ass);
                        item.SubItems.Add(msg.sender + "->" + msg.receiver);
                        listSubMessages.Items.Add(item);
                        listDispMes = listLAMes[i];
                    }
                }
                else if (col == 2)
                {
                    //dispNoLAM = true;
                    foreach (var msg in listLAMes[i])
                    {
                        var item = new ListViewItem(msg.registration);
                        string ass = "";
                        if (msg.associe == true) ass = "Oui/"; else ass = "Non/";
                        if (msg.notassocie == true) ass += "Oui"; else ass += "Non";
                        item.SubItems.Add(ass);
                        item.SubItems.Add(msg.sender + "->" + msg.receiver);
                        listSubMessages.Items.Add(item);
                        listDispMes = listMes[i];
                    }
                }
                else if (col == 3)
                {
                    //dispNoLAM = true;
                    foreach (var msg in listlybaMes[i])
                    {
                        var item = new ListViewItem(msg.registration);
                        string ass = "";
                        if (msg.associe == true) ass = "Oui/"; else ass = "Non/";
                        if (msg.notassocie == true) ass += "Oui"; else ass += "Non";
                        item.SubItems.Add(ass);
                        item.SubItems.Add(msg.sender + "->" + msg.receiver);
                        listSubMessages.Items.Add(item);
                        listDispMes = listlybaMes[i];
                    }
                }
            }
            else
            {
                if (col == 3)
                {
                    //fsgjfdyyjk
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
            if (row < listDispMes.Count)
                textMessage.Text = listDispMes[row].fullline;
            else textMessage.Text = "";
            //if (dispNoLAM)
            //{
            //    if (row < listLAMes[dispList].Count)
            //        textMessage.Text = listLAMes[dispList][row].fullline;
            //    else textMessage.Text = "";
            //}
            //else
            //{
            //    if (row < listMes[dispList].Count)
            //        textMessage.Text = listMes[dispList][row].fullline;
            //    else textMessage.Text = "";
            //}
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
            string fileline;
            autresTypes = new List<string>();
            foreach (var fleline in filelines)
            {
                fileline = fleline;
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
                bool ignoreline = false;
                // si c'est un LAM on retire la partie -REFDATA qui contient un SEQNUM, un -SENDER FAC et un -RECVR FAC qu'on veut ignorer alors on les retire
                if (fileline.IndexOf("-TITLE LAM") >= 0)
                {
                    // on retire le SEQNUM après REFDATA
                    fileline = Regex.Replace(fleline, @"(?<=-REFDATA(?:.(?!-MSGREF))*?)-SEQNUM\s+[^\s\-]+", "");
                    // on retire le REFDATA jusqu'au receiver du REFDATA (on ne garde que la partie MSGREF de la ligne LAM)
                    fileline = Regex.Replace(fileline, @"-REFDATA.*?-RECVR -FAC [^\s\-]+", "");
                }
                // si c'est un MAC on retire la partie -MSGREF qui contient un SEQNUM, un -SENDER FAC et un -RECVR FAC qu'on veut ignorer alors on les retire
                else if (fileline.IndexOf("-TITLE MAC") >= 0)
                {
                    // on retire le SEQNUM après MSGREF
                    fileline = Regex.Replace(fleline, @"(?<=-MSGREF(?:.(?!-REFDATA))*?)-SEQNUM\s+[^\s\-]+", "");
                    // on retire le MSGREF jusqu'au receiver du MSGREF (on ne garde que la partie REFDATA de la ligne MAC)
                    fileline = Regex.Replace(fileline, @"-MSGREF.*?-RECVR -FAC [^\s\-]+", "");
                }
                // On ajoute une fausse donnée inutile à la fin pour éviter les cas où ce qui nous intéresse tombe en EOL
                fileline += "-FIN MSG";
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
                                            {
                                                ms.mestype = GetMessageTypeFromText(fileline.Substring(start, dashIndex - start).Trim());
                                            }
                                            else if (elt.Id == MCAElementType.departure)
                                                ms.departure = fileline.Substring(start, dashIndex - start).Trim();
                                            else if (elt.Id == MCAElementType.destination)
                                                ms.destination = fileline.Substring(start, dashIndex - start).Trim();
                                            else if (elt.Id == MCAElementType.sender)
                                            {
                                                ms.sender = fileline.Substring(start, dashIndex - start).Trim();
                                                if (ms.sender != Terrain1 && ms.sender != Terrain2)
                                                {
                                                    ignoreline = true;
                                                    index = start;
                                                    break;
                                                }
                                            }
                                            else if (elt.Id == MCAElementType.receiver)
                                            {
                                                ms.receiver = fileline.Substring(start, dashIndex - start).Trim();
                                                if (ms.receiver != Terrain1 && ms.receiver != Terrain2)
                                                {
                                                    ignoreline = true;
                                                    index = start;
                                                    break;
                                                }
                                            }
                                            else if (elt.Id == MCAElementType.ptid)
                                                ms.ptid = fileline.Substring(start, dashIndex - start).Trim();
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
                            case MCAElementType.seqnum:
                                {
                                    int start = index + elt.Text.Length;
                                    int dashIndex = fileline.IndexOf('-', start);
                                    if (dashIndex != -1 && dashIndex > start)
                                    {
                                        // Pour le LAM, on n'a gardé que le SEQNUM qui donne le message auquel il fait référence
                                        string nexelt = fileline.Substring(dashIndex, fileline.IndexOf(" ", dashIndex) - dashIndex).Trim();
                                        if (ms.mestype == MCAMessageType.LAM)
                                            ms.seqnumrel = int.Parse(fileline.Substring(start, dashIndex - start).Trim());
                                        else
                                            ms.seqnum = int.Parse(fileline.Substring(start, dashIndex - start).Trim());
                                    }
                                    index = start;
                                }
                                break;
                        }
                    }
                }
                if (ignoreline) continue;

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
                if (ms.registration == "" && ms.mestype != MCAMessageType.NUL && ms.mestype != MCAMessageType.LAM) continue;
                //MessageBox.Show("The line at " + ms.datetime.ToString() + " have no registration information and doesn't correspond to another line found before, it is ignored");
                msLines.Add(ms);
            }
        }
        List<MessageCALine>[] listMes = new List<MessageCALine>[MCAMesType.Count];
        List<MessageCALine>[] listLAMes = new List<MessageCALine>[MCAMesType.Count];
        List<MessageCALine>[] listlybaMes = new List<MessageCALine>[MCAMesType.Count];
        List<MessageCALine>[] listlybaLAMes = new List<MessageCALine>[MCAMesType.Count];
        List<MessageCALine>[] listbalyMes = new List<MessageCALine>[MCAMesType.Count];
        List<MessageCALine>[] listbalyLAMes = new List<MessageCALine>[MCAMesType.Count];
        List<MessageCALine> listlybaNDMes = new List<MessageCALine>();
        List<MessageCALine> listlybaANAMes = new List<MessageCALine>();
        List<MessageCALine> listlybaNAMes = new List<MessageCALine>();
        List<MessageCALine> listlybaAMes = new List<MessageCALine>();
        List<MessageCALine> listbalyNDMes = new List<MessageCALine>();
        List<MessageCALine> listbalyANAMes = new List<MessageCALine>();
        List<MessageCALine> listbalyNAMes = new List<MessageCALine>();
        List<MessageCALine> listbalyAMes = new List<MessageCALine>();

        // remplit les tableaux en fonction de ce dont on a besoin, une fois que les messages ont été analysés
        private void FillTableau()
        {
            listMessages.Items.Clear();
            listSubMessages.Items.Clear();
            ListViewItem item;
            for (int j = 0; j < MCAMesType.Count; j++)
            {
                var type = MCAMesType[j];
                listMes[j] = new List<MessageCALine>();
                listLAMes[j] = new List<MessageCALine>();
                listlybaMes[j] = new List<MessageCALine>();
                listlybaLAMes[j] = new List<MessageCALine>();
                listbalyMes[j] = new List<MessageCALine>();
                listbalyLAMes[j] = new List<MessageCALine>();
                foreach (var msg in msLines)
                {
                    if (msg.mestype == MCAMesType[j].Type)
                    {
                        listMes[j].Add(msg);
                        if (!msg.islam) listLAMes[j].Add(msg);
                        if (msg.sender == Terrain1 && msg.receiver == Terrain2)
                        {
                            listlybaMes[j].Add(msg);
                            if (!msg.islam) listlybaLAMes[j].Add(msg);
                        }
                        if (msg.sender == Terrain2 && msg.receiver == Terrain1)
                        {
                            listbalyMes[j].Add(msg);
                            if (!msg.islam) listbalyLAMes[j].Add(msg);
                        }
                    }
                }
                if (type.text == "NUL")
                {
                    string autmsg = "";
                    for (int i = 0; i < autresTypes.Count; i++)
                    {
                        autmsg += autresTypes[i];
                        if (i < autresTypes.Count - 1) autmsg += "/";
                    }
                    LigneNul = listMessages.Items.Count;
                    item = new ListViewItem(NulText + " (" + autmsg + ")");
                }
                else item = new ListViewItem(type.text);
                item.SubItems.Add(listMes[j].Count().ToString());
                item.SubItems.Add(listLAMes[j].Count().ToString());
                item.SubItems.Add(listlybaMes[j].Count.ToString());
                item.SubItems.Add(listlybaMes[j].Count.ToString());
                item.SubItems.Add(listbalyLAMes[j].Count.ToString());
                item.SubItems.Add(listbalyLAMes[j].Count.ToString());
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
            listBALY.Items.Clear();
            listLYBA.Items.Clear();
            // on dénombre les messages NON DECODE/ASSOCIABLE NON ASSOCIE/NON ASSOCIABLE/ASSOCIE
            // et on en profite pour lister les balises de coordination
            foreach (var msg in msLines)
            {
                listlybaNDMes = new List<MessageCALine>();
                listlybaANAMes = new List<MessageCALine>();
                listlybaNAMes = new List<MessageCALine>();
                listlybaAMes = new List<MessageCALine>();
                listbalyNDMes = new List<MessageCALine>();
                listbalyANAMes = new List<MessageCALine>();
                listbalyNAMes = new List<MessageCALine>();
                listbalyAMes = new List<MessageCALine>();

                if (msg.sender == Terrain1 && msg.receiver == Terrain2)
                {
                    if (msg.notdecode)
                    {
                        nndeclyba++;
                        listlybaNDMes.Add(msg);
                    }
                    if (msg.notassocie)
                    {
                        nanalyba++;
                        listlybaANAMes.Add(msg);
                    }
                    if (msg.notassociable)
                    {
                        nnalyba++;
                        listlybaNAMes.Add(msg);
                    }
                    if (msg.associe)
                    {
                        nasslyba++;
                        listlybaAMes.Add(msg);
                    }
                    if (msg.ptid != "") listLYBA.Items.Add(msg.ptid);
                }
                if (msg.sender == Terrain2 && msg.receiver == Terrain1)
                {
                    if (msg.notdecode)
                    {
                        nndecbaly++;
                        listbalyNDMes.Add(msg);
                    }
                    if (msg.notassocie)
                    {
                        nanabaly++;
                        listbalyANAMes.Add(msg);
                    }
                    if (msg.notassociable)
                    {
                        nnabaly++;
                        listbalyNAMes.Add(msg);
                    }
                    if (msg.associe)
                    {
                        nassbaly++;
                        listbalyAMes.Add(msg);
                    }
                    if (msg.ptid != "") listBALY.Items.Add(msg.ptid);
                }
            }
            // retirer les doublons dans les listboxes des balises
            var distinctItems = listLYBA.Items.Cast<string>().Distinct().ToList();
            listLYBA.Items.Clear();
            listLYBA.Items.AddRange(distinctItems.ToArray());
            distinctItems = listBALY.Items.Cast<string>().Distinct().ToList();
            listBALY.Items.Clear();
            listBALY.Items.AddRange(distinctItems.ToArray());

            // lister les messages NON DECODE/ASSOCIABLE NON ASSOCIE/NON ASSOCIABLE/ASSOCIE
            item = new ListViewItem("NON DECODE");
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add(nndeclyba.ToString());
            item.SubItems.Add("");
            item.SubItems.Add(nndecbaly.ToString());
            item.SubItems.Add("");
            listMessages.Items.Add(item);
            item = new ListViewItem("ASSOCIABLE NON ASSOCIE");
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add(nanalyba.ToString());
            item.SubItems.Add("");
            item.SubItems.Add(nanabaly.ToString());
            item.SubItems.Add("");
            listMessages.Items.Add(item);
            item = new ListViewItem("NON ASSOCIABLE");
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add(nnalyba.ToString());
            item.SubItems.Add("");
            item.SubItems.Add(nnabaly.ToString());
            item.SubItems.Add("");
            listMessages.Items.Add(item);
            item = new ListViewItem("ASSOCIE");
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add(nasslyba.ToString());
            item.SubItems.Add("");
            item.SubItems.Add(nassbaly.ToString());
            item.SubItems.Add("");
            listMessages.Items.Add(item);
        }
        // on fait un tour rapide de tous les terrains sender/receiver présents dans le fichier et on les met dans une liste
        private List<(string, string)> ListeCouplesTerrains(string fc)
        {
            var result = new List<(string, string)>();

            var lines = fc.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                if (!line.Contains("-REFDATA") || line.Contains("-MSGREF"))
                    continue;

                var senderFac = GetFacAfterKeyword(line, "-SENDER -FAC");
                var recvrFac = GetFacAfterKeyword(line, "-RECVR -FAC");

                if (senderFac != null && recvrFac != null)
                {
                    var tuple = (senderFac, recvrFac);
                    var reversed = (recvrFac, senderFac);

                    if (!result.Contains(tuple) && !result.Contains(reversed))
                    {
                        result.Add(tuple);
                    }
                }
            }
            return result;
        }
        // obtient la valeur après un "-FAC" donné
        private string GetFacAfterKeyword(string line, string keyword)
        {
            int index = line.IndexOf(keyword);
            if (index == -1)
                return null;

            index += keyword.Length;
            var remaining = line.Substring(index).TrimStart();

            var parts = remaining.Split(new[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0)
                return parts[0];

            return null;
        }

        private void GetAllTerrains(string fc)
        {
            // les terrains sont placés après les "-FAC"
            var matches = Regex.Matches(fc, @"-FAC\s+([^\s\-]+)");
            var facSet = new HashSet<string>();
            foreach (Match match in matches) facSet.Add(match.Groups[1].Value);
            CouplesTerrains = ListeCouplesTerrains(fc);
        }
        // action quand on va charger un nouveau fichier
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
                        // on obtient le chemin du fichier sélectionné
                        filePath = openFileDialog.FileName;

                        var fileStream = openFileDialog.OpenFile();
                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            // on lit le fichier dans une string
                            fileContent = reader.ReadToEnd();
                            //dispNoLAM = false;
                            dispList = -1;
                            filelines = new List<string>();
                            // on cherche vite fait les terrains et on les liste dans les 2 combo boxes
                            GetAllTerrains(fileContent);
                            comboBox1.Items.Clear();
                            foreach (var (tr1, tr2) in CouplesTerrains)
                            {
                                comboBox1.Items.Add(tr1 + "<->" + tr2);
                            }
                            comboBox1.SelectedIndex = 0;
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

        private void button2_Click(object sender, EventArgs e)
        {
            listMessages.Items.Clear();
            listSubMessages.Items.Clear();
            listLYBA.Items.Clear();
            listBALY.Items.Clear();
            string ctr = comboBox1.Text;
            string[] parts = ctr.Split(new[] { "<->" }, StringSplitOptions.None);
            Terrain1 = parts[0];
            Terrain2 = parts.Length > 1 ? parts[1] : "";
            if (Terrain1 == Terrain2 || Terrain2 == "")
            {
                MessageBox.Show("Erreur sur les terrains choisi, vérifiez avant de cliquer sur OK", "Action ignorée");
                return;
            }
            label2.Text = "Balises " + Terrain1 + "->" + Terrain2;
            label3.Text = "Balises " + Terrain2 + "->" + Terrain1;
            listMessages.Columns[3].Text = Terrain1 + "->" + Terrain2;
            listMessages.Columns[4].Text = Terrain2 + "->" + Terrain1;
            ParseMessageCA(fileContent);
            FillTableau();

        }
    }
}
