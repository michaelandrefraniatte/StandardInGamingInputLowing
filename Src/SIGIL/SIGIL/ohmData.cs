/********************************************
 * Created by Dr. J. L. Doty, July 2018
 * http://www.jldoty.com/
 * Downloads for this article are governed by the The Code Project Open License (CPOL):
 * https://www.codeproject.com/info/cpol10.aspx
 * Update July
 *  -incorporated Timer into ohmDataTree
 *  -added UpdateSensorData to update the values without recreating the tree
 *  -added UpdateSensors event so other classes can hook into the update event chain
 * ********************************************/

#define TrapUSBNo
using System;
using System.Windows.Forms;
using System.Linq;
using OpenHardwareMonitor.Hardware;
namespace SIGIL
{
    public class ohmDataTree : IDisposable
    {
        private Computer computerHW = new Computer();
        public ohmHwNode[] hWareList = null;
        bool disposed = false;// Flag: Has Dispose already been called?
        public string[] sensorFormats; //Array of format strings for each SensorType
        public string[] sensorUnits;    //Array of units strings for each SensorType

        public Timer timer = new Timer();
        public event EventHandler UpdateSensors;

#if TrapUSB
        //Got ManagementEventWatcher details from https://stackoverflow.com/questions/620144/detecting-usb-drive-insertion-and-removal-using-windows-service-and-c-sharp
        ManagementEventWatcher watcher = new ManagementEventWatcher();
#endif

        #region Constructor

        public ohmDataTree()
        {
            //Create fast-access arrays for format and unit strings for SensorTypes
            int nMax = (int)Enum.GetValues(typeof(SensorType)).Cast<SensorType>().Last();
            sensorFormats = new string[nMax + 1];
            sensorUnits = new string[nMax + 1];
            foreach (SensorType st in Enum.GetValues(typeof(SensorType)))
            {
                sensorFormats[(int)st] = SensorFormat(st);
                sensorUnits[(int)st] = SensorUnits(st);
            }

            computerHW.MainboardEnabled = true;
            computerHW.FanControllerEnabled = true; //This doesn't seem to do anything
            computerHW.CPUEnabled = true;
            computerHW.GPUEnabled = true;
            computerHW.RAMEnabled = true;
            computerHW.HDDEnabled = true;
            computerHW.Open();
            computerHW.HardwareAdded += ComputerHW_HardwareAdded;
            computerHW.HardwareRemoved += ComputerHW_HardwareRemoved;
            ScanHardware();
            //MessageBox.Show(computerHW.GetReport()); //Gives full report of all hardware and sensors
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Enabled = true;

#if TrapUSB
            //Using this form fires the event several times for a single USB insertion, and only once is the "DriveName" property valid
            //WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2 or EventType = 3");

            //Using this form fires the event only once with the "DriveName" property valid, but this will catch only HDD's
            WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2 or EventType = 3");

            watcher.EventArrived += Watcher_EventArrived;
            watcher.Query = query;
            watcher.Start();
#endif
        }

        #endregion Constructor

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateSensorData();
            OnUpdateSensors(new EventArgs());
        }

        protected virtual void OnUpdateSensors(EventArgs e)
        {
            UpdateSensors?.Invoke(this, e);
        }
#if TrapUSB
        private void Watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            try
            {
                //This block is needed to ensure that the "DriveName" property is valid when trapping Win32_DeviceChangeEvent
                //bool dNameFound = false;

                //foreach (PropertyData property in e.NewEvent.Properties)
                //    if (property.Name == "DriveName")
                //    {
                //        dNameFound = true;
                //        break;
                //    }
                //if (!dNameFound)
                //    return;

                string driveName = e.NewEvent.Properties["DriveName"].Value.ToString();
                int eventType = Convert.ToInt16(e.NewEvent.Properties["EventType"].Value);

                //string eventName = Enum.GetName(typeof(EventType), eventType);

                Console.WriteLine("{0}: {1} {2}", DateTime.Now, driveName, eventType.ToString());

                configChanged = true;

                computerHW.Close();
                computerHW.Open();
            }
            catch (ManagementException me)
            {
                ////Purely for debugging
                ////Throw away the not found exception
                ////Console.WriteLine("Exception: " + me.Message);
                //string Props = "";
                //foreach (PropertyData property in e.NewEvent.Properties)
                //    Props += property.Name + "(" + (property.Value == null ? "null" : property.Value.ToString()) + "), ";
                //Console.WriteLine(Props);
            }

        }
#endif

        #region Properties

        public bool MainboardEnabled
        {
            get { return computerHW.MainboardEnabled; }
            set
            {
                if (value == computerHW.MainboardEnabled)
                    return;
                computerHW.MainboardEnabled = value;
            }
        }

        public bool FanControllerEnabled
        {
            get { return computerHW.FanControllerEnabled; }
            set
            {
                if (value == computerHW.FanControllerEnabled)
                    return;
                computerHW.FanControllerEnabled = value;
            }
        }

        public bool CPUEnabled
        {
            get { return computerHW.CPUEnabled; }
            set
            {
                if (value == computerHW.CPUEnabled)
                    return;
                computerHW.CPUEnabled = value;
            }
        }

        public bool GPUEnabled
        {
            get { return computerHW.GPUEnabled; }
            set
            {
                if (value == computerHW.GPUEnabled)
                    return;
                computerHW.GPUEnabled = value;
            }
        }

        public bool RAMEnabled
        {
            get { return computerHW.RAMEnabled; }
            set
            {
                if (value == computerHW.RAMEnabled)
                    return;
                computerHW.RAMEnabled = value;
            }
        }

        public bool HDDEnabled
        {
            get { return computerHW.HDDEnabled; }
            set
            {
                if (value == computerHW.HDDEnabled)
                    return;
                computerHW.HDDEnabled = value;
            }
        }

        #endregion Properties

        public bool configChanged = true;

        private void ComputerHW_HardwareRemoved(IHardware hardware)
        {
            configChanged = true;
            ScanHardware();
        }

        private void ComputerHW_HardwareAdded(IHardware hardware)
        {
            configChanged = true;
            ScanHardware();
        }

        /// <summary>
        /// Calls recursive UpdateSensorData method to update the values contained in ohmSensor class
        /// </summary>
        public void UpdateSensorData()
        {
            UpdateSensorData(hWareList);
        }

        /// <summary>
        /// Recursive UpdateSensorData method to update the values contained in ohmSensor class
        /// </summary>
        /// <param name="hList"></param>
        private void UpdateSensorData(ohmHwNode[] hList)
        {
            if (hList == null)
                return;
            for (int i = 0; i < hList.Length; i++)
            {
                hList[i].hWare.Update();
                if (hList[i].ohmSensors != null)
                    for (int j = 0; j < hList[i].ohmSensors.Length; j++)
                        UpdateSensorValues(hList[i].ohmSensors[j]);
                if (hList[i].ohmChildren != null)
                    UpdateSensorData(hList[i].ohmChildren);
            }
        }

        /// <summary>
        /// Calls recursive ScanHardware method to create the ohmDataTree
        /// </summary>
        private void ScanHardware()
        {
            nodeCount = SensorCount = 0;
            hWareList = ScanHardware(null, computerHW.Hardware);
        }

        //These are included primarily for testing
        public int nodeCount;
        public int SensorCount;

        /// <summary>
        /// Recursive ScanHardware method that creates the ohmDataTree
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="hardWare"></param>
        /// <returns></returns>
        private ohmHwNode[] ScanHardware(ohmHwNode parent, IHardware[] hardWare)
        {
            if (hardWare.Count() == 0)
                return null;
            ohmHwNode[] hList = new ohmHwNode[hardWare.Count()];
            nodeCount += hardWare.Count();
            for (int i = 0; i < hardWare.Count(); i++)
            {
                hList[i] = new ohmHwNode();
                hList[i].hWare = hardWare[i];
                try
                {
                    hardWare[i].Update();
                }
                catch
                {
                    //throw away any exception
                }
                hList[i].ohmParent = parent;
                hList[i].name = hardWare[i].Name;
                hList[i].type = hardWare[i].HardwareType;
                hList[i].id = hardWare[i].Identifier;
                //need to implement SensorAdded and SensorRemoved
                if (hardWare[i].Sensors.Count() > 0)
                {
                    SensorCount += hardWare[i].Sensors.Count();
                    hList[i].ohmSensors = new ohmSensor[hardWare[i].Sensors.Count()];
                    for (int j = 0; j < hardWare[i].Sensors.Count(); j++)
                    {
                        hList[i].ohmSensors[j] = new ohmSensor();
                        hList[i].ohmSensors[j].sensor = hardWare[i].Sensors[j];
                        hList[i].ohmSensors[j].Name = hardWare[i].Sensors[j].Name;
                        hList[i].ohmSensors[j].Identifier = hardWare[i].Sensors[j].Identifier.ToString();
                        hList[i].ohmSensors[j].sType = hardWare[i].Sensors[j].SensorType;
                        //hList[i].ohmSensors[j].Parent = hardWare[i].Sensors[j].Parent;
                        hList[i].ohmSensors[j].Index = hardWare[i].Sensors[j].Index;

                        UpdateSensorValues(hList[i].ohmSensors[j]);
                    }
                }
                hList[i].ohmChildren = ScanHardware(hList[i], hardWare[i].SubHardware);
            }
            return hList;
        }

        /// <summary>
        /// Updates the values and strings in the ohmSensor class
        /// </summary>
        /// <param name="ohms"></param>
        private void UpdateSensorValues(ohmSensor ohms)
        {
            ohms.Value = ohms.sensor.Value;
            ohms.Min = ohms.sensor.Min;
            ohms.Max = ohms.sensor.Max;

            ohms.stValue = getValueString(ohms.sType, ohms.Value);
            ohms.stMin = getValueString(ohms.sType, ohms.Min);
            ohms.stMax = getValueString(ohms.sType, ohms.Max);

        }

        /// <summary>
        /// Calls the recursive SensorFromID method to find a sensor with a specific id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>null if no match found</returns>
        public ohmSensor SensorFromID(string id)
        {
            return SensorFromID(id, hWareList);
        }

        /// <summary>
        /// Recursive SensorFromID method to find a sensor with a specific id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="hardWare"></param>
        /// <returns>null if no match found</returns>
        private ohmSensor SensorFromID(string id, ohmHwNode[] hardWare)
        {
            if (hardWare == null)
                return null;
            for (int i = 0; i < hardWare.Length; i++)
            {
                ohmHwNode hw = hardWare[i];
                if (hw.ohmSensors != null)
                    for (int j = 0; j < hw.ohmSensors.Length; j++)
                        if (hw.ohmSensors[j].Identifier == id)
                            return hw.ohmSensors[j];
                ohmSensor ohms = SensorFromID(id, hw.ohmChildren);
                if (ohms != null)
                    return ohms;
            }
            return null;
        }

        /// <summary>
        /// Calls recursive SensorFromIndex method to find a sensor by the linear order in which it appears in the tree
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ohmSensor SensorFromIndex(int index)
        {
            int itemIndex = 0;
            return SensorFromIndex(index, hWareList, ref itemIndex);
        }

        /// <summary>
        /// Recursive SensorFromIndex method to find a sensor by the linear order in which it appears in the tree
        /// </summary>
        /// <param name="index"></param>
        /// <param name="hardWare"></param>
        /// <param name="itemIndex"></param>
        /// <returns></returns>
        private ohmSensor SensorFromIndex(int index, ohmHwNode[] hardWare, ref int itemIndex)
        {
            if (hardWare == null)
                return null;
            for (int i = 0; i < hardWare.Length; i++)
            {
                ohmHwNode hw = hardWare[i];
                if (hw.ohmSensors != null)
                    for (int j = 0; j < hw.ohmSensors.Length; j++)
                    {
                        itemIndex++;
                        if (index == itemIndex)
                            return hw.ohmSensors[j];
                    }
                ohmSensor ohms = SensorFromIndex(index, hw.ohmChildren, ref itemIndex);
                if (ohms != null)
                    return ohms;
            }
            return null;
        }

        /// <summary>
        /// Returns a sensor units string
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string SensorUnits(SensorType type)
        {
            switch (type)
            {
                case SensorType.Voltage:
                    return "v";
                case SensorType.Clock:
                    return "MHz";
                case SensorType.Temperature:
                    return "°C";
                case SensorType.Level:
                case SensorType.Control:
                case SensorType.Load:
                    return "%";
                case SensorType.Fan:
                    return "rpm";
                case SensorType.Flow:
                    return "L/h";

                //No documentation so had to guess on these:
                case SensorType.SmallData:
                    return "MB";
                case SensorType.Data:
                    return "GB";
                case SensorType.Power:
                    return "W";

                    //No documentation and no guess on these:
                    //case SensorType.Factor: 
            }
            return "";
        }

        /// <summary>
        /// Returns a sensor data format string
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string SensorFormat(SensorType type)
        {
            switch (type)
            {
                case SensorType.Voltage:
                    return "N3";
                case SensorType.Clock:
                    return "N0";
                case SensorType.Temperature:
                    return "N1";
                case SensorType.Level:
                case SensorType.Control:
                case SensorType.Load:
                    return "N1";
                case SensorType.Fan:
                    return "N0";
                case SensorType.Flow:
                    return "N1";

                //No documentation so had to guess on these:
                case SensorType.SmallData:
                    return "N1";
                case SensorType.Data:
                    return "N1";
                case SensorType.Power:
                    return "N1";

                    //No documentation and no guess on these:
                    //case SensorType.Factor: 
            }
            return "";
        }

        /// <summary>
        /// Returns a complete formatted value string with units appended.  
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string getValueString(SensorType type, float? value)
        {
            if (value == null)
                return "--- " + sensorUnits[(int)type];
            else
                return value.Value.ToString(sensorFormats[(int)type]) + " " + sensorUnits[(int)type];
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
#if TrapUSB
                watcher.Stop();
#endif
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            computerHW.Close();

            disposed = true;
        }

        ~ohmDataTree()
        {
            Dispose(false);
        }
    }

    /// <summary>
    /// Container class for a hardware node in the ohmDataTree
    /// </summary>
    public class ohmHwNode
    {
        public IHardware hWare = null; //The OpenHardwareMonitor interface for a hardware node
        public ohmHwNode ohmParent = null; //pointer to the parent node
        public ohmHwNode[] ohmChildren = null; //array of pointers to the child nodes

        public string name = ""; //Hardware name
        public HardwareType type; //Hardware type
        public Identifier id;

        public ohmSensor[] ohmSensors = null; //array of ohmSensors in this hardware node
    }

    /// <summary>
    /// Container class for a sensor contained in a hardware node in the ohmDataTree
    /// </summary>
    public class ohmSensor
    {
        public ISensor sensor = null; //The OpenHardwareMonitor interface for a sensor
        public string Name = ""; //Sensor Name
        public string Identifier = ""; //Sensor Identifier
        public SensorType sType; //Sensor Type
        public float? Value; //nullable native Sensor Value
        public float? Min;  //nullable native Sensor Min
        public float? Max;  //nullable native Sensor Max
        public string stValue = ""; //formatted Value String
        public string stMin = ""; //formatted Min String
        public string stMax = ""; //formatted Max String
        public int Index = 0;
    }

}
