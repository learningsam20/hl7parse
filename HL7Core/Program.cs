// See https://aka.ms/new-console-template for more information

using HL7.Dotnetcore;


static void ProcessCSV()
{
    string entireMessage = @"MSH|^~\&|QUANTERIX|SIMOA1|||20210909042127||ORU^R01|53256|P|2.3 
PID|1||123456^^^GATECH||TEST^PATIENT||19951220|F 
ORC|RE||003109701234A||CM||||20210909052539 
OBR|1||003109701234A$1$202109081033$COVID19|COVID19||||||||||||||^SIMOA1||||202109091631175682507|||F||^^^^^Routine 
OBX|1||DRE||COVID19POSITIVE||||||F";
    Message msg = new(entireMessage);
    msg.ParseMessage();

    List<string> lstFiles = Directory.GetFiles(@"E:\GT\Healthcare Informatics\Assignments\lab6\src\data", "*.csv").ToList();
    List<string> lstMessages = new List<string>();
    foreach (string csvFilePath in lstFiles)
    {

        List<string> lstText = File.ReadAllLines(csvFilePath).Where(obj => !obj.StartsWith("POS") && !obj.StartsWith("NEG") && !obj.StartsWith("Sample")).ToList();
        foreach (string strMessage in lstText)
        {
            Message msgCsv = msg;
            //Console.WriteLine("csv: " + strMessage.Split(new String[] {"," }, StringSplitOptions.RemoveEmptyEntries)[0]);
            //Console.WriteLine("seg: " + msgCsv.Segments("ORC")[0].Fields(3).Value);
            string pid = strMessage.Split(new String[] { "," }, StringSplitOptions.None)[0];
            string result = strMessage.Split(new String[] { "," }, StringSplitOptions.None)[19];
            //Console.WriteLine("replacing: " + pid + ":" + result);
            msgCsv.Segments("ORC")[0].Fields(3).Value = pid;
            msgCsv.Segments("OBR")[0].Fields(3).Value = pid;
            msgCsv.Segments("OBX")[0].Fields(5).Value = result;
            //Console.WriteLine(msgCsv.SerializeMessage(true));
            lstMessages.Add(msgCsv.SerializeMessage(true));
        }
    }
    Console.WriteLine("completed processing of hl7. wrote " + lstMessages.Count + " messages.");
    File.WriteAllLines(@"E:\GT\Healthcare Informatics\Assignments\lab6\src\out\hl7messages.txt", lstMessages);
}
static Message ProcessMessage(string entireMessage, Dictionary<string, Dictionary<int, string>> dictLabels)

{
    Message msg = new(entireMessage);
    msg.ParseMessage();
    Console.WriteLine(new String('*', 60));
    Console.WriteLine("message version: " + msg.Version);
    Console.WriteLine("message structure: " + msg.MessageStructure);
    Console.WriteLine("number of segments: " + msg.SegmentCount);
    foreach (Segment s in msg.Segments())
    {
        Console.WriteLine(new String('*', 60));
        Console.WriteLine("segment name: " + s.Name + " has " + s.GetAllFields().Count + " fields.");
        int fi = 0;
        foreach (Field f in s.GetAllFields())
        {
            fi += 1;
            Console.WriteLine("field " + fi.ToString().PadLeft(2, '0') + " -> label: " + dictLabels[s.Name][fi] + ", value: " + f.Value);
        }
    }
    Console.WriteLine(new String('*', 60));
    return msg;
}

Dictionary<string, Dictionary<int, string>> dictLabels = new Dictionary<string, Dictionary<int, string>>();
Dictionary<int, string> segLabels = new Dictionary<int, string>();
segLabels.Add(1, "Field Separator");
segLabels.Add(10, "Message Control ID");
segLabels.Add(11, "Processing ID");
segLabels.Add(12, "Version ID");
segLabels.Add(13, "Sequence Number");
segLabels.Add(14, "Continuation Pointer");
segLabels.Add(15, "Accept Acknowledgement Type");
segLabels.Add(16, "Application Acknowledgement Type");
segLabels.Add(17, "Country Code");
segLabels.Add(18, "Character Set");
segLabels.Add(19, "Principal Language of Message");
segLabels.Add(2, "Encoding Characters");
segLabels.Add(3, "Sending Application");
segLabels.Add(4, "Sending Facility");
segLabels.Add(5, "Receiving Application");
segLabels.Add(6, "Receiving Facility");
segLabels.Add(7, "Date / Time of Message");
segLabels.Add(8, "Security");
segLabels.Add(9, "Message Type");
dictLabels.Add("MSH", segLabels);
segLabels = new Dictionary<int, string>();
segLabels.Add(1, "Set ID - Observation Request");
segLabels.Add(10, "Collector Identifier");
segLabels.Add(11, "Specimen Action Code");
segLabels.Add(12, "Danger Code");
segLabels.Add(13, "Relevant Clinical Information");
segLabels.Add(14, "Specimen Received Date/Time");
segLabels.Add(15, "Specimen Source");
segLabels.Add(16, "Ordering Provider");
segLabels.Add(17, "Order Callback Phone Number");
segLabels.Add(18, "Placer Field 1");
segLabels.Add(19, "Placer Field 2");
segLabels.Add(2, "Placer Order Number");
segLabels.Add(20, "Filler Field 1");
segLabels.Add(21, "Filler Field 2");
segLabels.Add(22, "Results Rpt/Status Chng - Date/Time");
segLabels.Add(23, "Charge To Practice");
segLabels.Add(24, "Diagnostic Service Section ID");
segLabels.Add(25, "Result Status");
segLabels.Add(26, "Parent Result");
segLabels.Add(27, "Quantity/Timing");
segLabels.Add(28, "Result Copies To");
segLabels.Add(29, "Parent Number");
segLabels.Add(3, "Filler Order Number");
segLabels.Add(30, "Transportation Mode");
segLabels.Add(31, "Reason For Study");
segLabels.Add(32, "Principal Result Interpreter");
segLabels.Add(33, "Assistant Result Interpreter");
segLabels.Add(34, "Technician");
segLabels.Add(35, "Transcriptionist");
segLabels.Add(36, "Scheduled Date/Time");
segLabels.Add(37, "Number Of Sample Containers");
segLabels.Add(38, "Transport Logistics Of Collected Sample");
segLabels.Add(39, "Collector s Comment");
segLabels.Add(4, "Universal Service Identifier");
segLabels.Add(40, "Transport Arrangement Responsibility");
segLabels.Add(41, "Transport Arranged");
segLabels.Add(42, "Escort Required");
segLabels.Add(43, "Planned Patient Transport Comment");
segLabels.Add(5, "Priority");
segLabels.Add(6, "Requested Date/Time");
segLabels.Add(7, "Observation Date/Time");
segLabels.Add(8, "Observation End Date/Time");
segLabels.Add(9, "Collection Volume");
dictLabels.Add("OBR", segLabels);
segLabels = new Dictionary<int, string>();
segLabels.Add(1, "Set ID - Patient ID");
segLabels.Add(10, "Race");
segLabels.Add(11, "Patient Address");
segLabels.Add(12, "County Code");
segLabels.Add(13, "Phone Number - Home");
segLabels.Add(14, "Phone Number - Business");
segLabels.Add(15, "Primary Language");
segLabels.Add(16, "Marital Status");
segLabels.Add(17, "Religion");
segLabels.Add(18, "Patient Account Number");
segLabels.Add(19, "SSN Number - Patient");
segLabels.Add(2, "Patient ID (External ID)");
segLabels.Add(20, "Driver's License Number");
segLabels.Add(21, "Mother's Identifier");
segLabels.Add(22, "Ethnic Group");
segLabels.Add(23, "Birth Place");
segLabels.Add(24, "Multiple Birth Indicator");
segLabels.Add(25, "Birth Order");
segLabels.Add(26, "Citizenship");
segLabels.Add(27, "Veterans Military Status");
segLabels.Add(28, "Nationality Code");
segLabels.Add(29, "Patient Death Date and Time");
segLabels.Add(3, "Patient ID (Internal ID)");
segLabels.Add(30, "Patient Death Indicator");
segLabels.Add(4, "Alternate Patient ID");
segLabels.Add(5, "Patient Name");
segLabels.Add(6, "Mother's Maiden Name");
segLabels.Add(7, "Date of Birth");
segLabels.Add(8, "Sex");
segLabels.Add(9, "Patient Alias");
dictLabels.Add("PID", segLabels);
segLabels = new Dictionary<int, string>();
segLabels.Add(1, "Order Control");
segLabels.Add(10, "Entered By");
segLabels.Add(11, "Verified By");
segLabels.Add(12, "Ordering Provider");
segLabels.Add(13, "Enterer's Location");
segLabels.Add(14, "Call Back Phone Number");
segLabels.Add(15, "Order Effective Date/Time");
segLabels.Add(16, "Order Control Code Reason");
segLabels.Add(17, "Entering Organization");
segLabels.Add(18, "Entering Device");
segLabels.Add(19, "Action By");
segLabels.Add(2, "Placer Order Number");
segLabels.Add(3, "Filler Order Number");
segLabels.Add(4, "Placer Group Number");
segLabels.Add(5, "Order Status");
segLabels.Add(6, "Response Flag");
segLabels.Add(7, "Quantity/Timing");
segLabels.Add(8, "Parent Order");
segLabels.Add(9, "Date/Time of Transaction");
dictLabels.Add("ORC", segLabels);
segLabels = new Dictionary<int, string>();
segLabels.Add(1, "Set ID - OBX");
segLabels.Add(10, "Nature of Abnormal Test");
segLabels.Add(11, "Observ Result Status");
segLabels.Add(12, "Date Last Obs Normal Values");
segLabels.Add(13, "User Defined Access Checks");
segLabels.Add(14, "Date/Time of the Observation");
segLabels.Add(15, "Producer's ID");
segLabels.Add(16, "Responsible Observer");
segLabels.Add(17, "Observation Method");
segLabels.Add(2, "Value Type");
segLabels.Add(3, "Observation Identifier");
segLabels.Add(4, "Observation Sub-ID");
segLabels.Add(5, "Observation Value");
segLabels.Add(6, "Units");
segLabels.Add(7, "References Range");
segLabels.Add(8, "Abnormal Flags");
segLabels.Add(9, "Probability");
dictLabels.Add("OBX", segLabels);

try
{
    Console.WriteLine("Start of processing");
    //Message((new List<string>() { }).ToArray());
    string entireMessage = @"MSH|^~\&|QUANTERIX|SIMOA1|||20210909042127||ORU^R01|53256|P|2.3 
PID|1||123456^^^GATECH||TEST^PATIENT||19951220|F 
ORC|RE||003109701234A||CM||||20210909052539 
OBR|1||003109701234A$1$202109081033$COVID19|COVID19||||||||||||||^SIMOA1||||202109091631175682507|||F||^^^^^Routine 
OBX|1||DRE||COVID19POSITIVE||||||F";
    Message msg = ProcessMessage(entireMessage, dictLabels);
    ProcessCSV();
}
catch (Exception ex)
{
    Console.WriteLine("Error in processing: " + ex.Message);
}
Console.WriteLine("End of processing");