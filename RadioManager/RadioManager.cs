/*
 * Dan Berkowitz - Radio Manager software
 * buildingtents.com
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Management;
using System.IO;
using System.Windows.Forms;
using System.Net;
using RadioManager.Structs;
using System.IO.Ports;
using System.Runtime.InteropServices;


namespace RadioManager
{
    public partial class RadioManager : Form
    {
        //This is used to help shutdown after IDLE
        [DllImport("user32.dll")]
        public static extern int ExitWindowsEx(int uFlags, int dwReason);

        //Used in multithreading invocation
        delegate void StringParameterDelegate(string value);
        delegate void INTValue(int value1);
        
        public static bool Closing_app = false; // So all threads know if closing
        public static string FolderLocation = @"C:\Music\";// a default location
        private static List<Drive> Drives = new List<Drive>(); //stores drive information
        private static int stations_reserver = 0; //1 for each station reserved for a memory card
        private static knob_data GLOBAL_KNOB = new Structs.knob_data(); //stores all knob data accross threads
        public static bool change_station = false;
        public static string current_set_folder = "";
        private static string arduino_port = "COM3"; //default com number
        private static string added_songs = "";
        private static int shutdown_time = 0; // these two handle shutdown information
        private static bool shutdown_armed = false;

        /// <summary>
        /// This is all the code for the RadioManager for the Radio Project, buildingtents.com
        /// </summary>
        public RadioManager()
        {
            InitializeComponent();
            // Read the settings file in same directory and import if needed
            read_settings("settings.txt");
            //Scan for cards in the PC
            bool test = read_for_cards();
            //sets first volume to 0
            Change_Volume(0);
            //Spawns threads to handle knobs, and station stuff
            ThreadStart Read_Knob = new ThreadStart(this.Station_knob_wrapper);
            Thread Read_Knob_thread = new Thread(Read_Knob);
            Read_Knob_thread.Name = "RadioManager - Read Knob Thread";
            Read_Knob_thread.Start();
            ThreadStart Station_changer = new ThreadStart(this.change_station_wrapper);
            Thread Station_changer_thread = new Thread(Station_changer);
            Station_changer_thread.Name = "RadioManager - Station Changer Thread";
            Station_changer_thread.Start();
            Thread.Sleep(500); //let the read knob start
            set_folder("NA"); // first playlist build
            //MessageBox.Show("test");
        }

        #region initialize
        /// <summary>
        /// Scan for removable cards using WMI query
        /// </summary>
        /// <returns> if unique cards were found</returns>
        private bool read_for_cards()
        {
            EnumerationOptions options = new EnumerationOptions();
            options.Rewindable = false;
            options.ReturnImmediately = true;

            string query = "select * from Win32_LogicalDisk";

            ManagementObjectSearcher WMI_Searcher = new ManagementObjectSearcher(@"root\cimv2", query, options);

            List<Drive> temp_list = new List<Drive>();

            try
            {
                foreach (ManagementObject device in WMI_Searcher.Get())
                {
                    if (device["Description"].ToString() == "Removable Disk")
                    {
                        if (check_card_structor(device["Caption"].ToString()[0]))
                            temp_list.Add(new Drive(device["Caption"].ToString().Substring(0,2), device["Description"].ToString(), device["VolumeName"].ToString(), true));
                        else{
                            temp_list.Add(new Drive(device["Caption"].ToString().Substring(0,2), device["Description"].ToString(), "", false));
                            write_error_log("Drive " + device["Caption"].ToString() + " missing music structor\r\n");
                        }
                    }
                }
            }
            catch { } //in case of error

            if (temp_list.Count != 0)
            {
                SD_Select.Items.Clear();
                foreach(Drive disk in temp_list)
                {
                    if(disk.has_music_folder)// check if the folder structor exists, then adds drive if it does
                        SD_Select.Items.Add(disk.drive_letter);
                }
            }

            if (temp_list != Drives)// if list differs from historical data
            {
                Drives = temp_list;
                stations_reserver = 0;
                foreach (Drive Temp_drive in temp_list)
                {
                    if (Temp_drive.has_music_folder)
                        stations_reserver++;
                }
                return true;
            }
            else
                return false;
        }

        #endregion

        /// <summary>
        /// If it seems that the station has changed, this thread is signaled to change the playlist
        /// It's a seperate thread so that the reading knob thread doesnt get bogged down by this and cant change volume
        /// </summary>
        private void change_station_wrapper()
        {
            while (!Closing_app)
            {
                if (change_station)
                {
                    set_folder(current_set_folder);
                    change_station = false;
                }
                Thread.Sleep(250);//this is important to not eat up pc, too long makes it feel slow
            }
        }

        #region Read_knob_thread
        /**************************************************
         ********* Reading Knob Thread
         **************************************************/
        private void Station_knob_wrapper()
        {
            while (!Closing_app)
            {
                knob_data Recorded_Data = GLOBAL_KNOB;
                //watch this it may set it to reference not actual data
                read_knobs(arduino_port);
                //if the old data and new data doesnt match, check each knobs data then run where needed
                if (Recorded_Data != GLOBAL_KNOB)
                {
                    if (Recorded_Data.Station_Type != GLOBAL_KNOB.Station_Type)
                    {
                        Recorded_Data.Station_Type = GLOBAL_KNOB.Station_Type;
                        write_stationtype(GLOBAL_KNOB.Station_Type.ToString());
                        //station type changed
                    }
                    if (Recorded_Data.Station != GLOBAL_KNOB.Station)
                    {
                        Recorded_Data.Station = GLOBAL_KNOB.Station;
                        change_station = true;
                        write_station(GLOBAL_KNOB.Station.ToString());
                        //Station changed
                    }
                    if (Recorded_Data.Volume != GLOBAL_KNOB.Volume)
                    {
                        write_volume(GLOBAL_KNOB.Volume.ToString());
                        Change_Volume(int.Parse(GLOBAL_KNOB.Volume.ToString()));
                    }
                }
                if (shutdown_armed && (shutdown_time < DateTime.Now.Second))
                {//final shutdown code
                    ExitWindowsEx(1, 0);
                }
                Thread.Sleep(250);
            }
        }

        private void Change_Volume(int value)
        {
            if (Player != null && !Closing_app)
            {
                if (Player.InvokeRequired)
                {
                    Player.BeginInvoke(new INTValue(Change_Volume), new object[] { value });
                    return;
                }
                try
                {
                    Player.settings.volume = value;
                }
                catch { }//janky fix
            }
        }

        private void read_knobs(string passed_port) // True for bool means add to static
        {
            byte[] buffer = new byte[256];
            string knob_data = "------------";
            switch (passed_port)
            {
                case "preset"://if I cant find a arduino, default to this code
                    knob_data = "|50|3|----\r";
                    
                    write_arduino("No");
                    break;

                case "NA":
                    write_arduino("COM1");//start at com1
                    try
                    {
                        using (SerialPort sp = new SerialPort("COM1", 9600))
                        {
                            while (knob_data.Length != 10)
                            {
                                sp.Open();
                                knob_data = sp.ReadLine();
                                //read directly
                                //sp.Read(buffer, 0, (int)buffer.Length);
                                //read using a Stream
                                //sp.BaseStream.Read(buffer, 0, (int)buffer.Length);
                                sp.Close();
                            }
                        }
                    }
                    catch//com1 didnt work, do default
                    {
                        write_error_log("Cant connect to arduino on COM1\r\n");
                        knob_data = "|50|3|----\r";

                        write_arduino("No");
                    }
                   break;
                default:
                   try//try port in memory
                   {
                       if (check_com_port(passed_port))
                       {
                           using (SerialPort sp = new SerialPort(passed_port, 9600))
                           {
                               knob_data = "";
                               //string[] ports = SerialPort.GetPortNames();
                               sp.Open();
                               for (int i = 0; i < 3; i++)
                               {
                                   //char temp = sp.ReadChar().ToString()[0];
                                   knob_data += sp.ReadLine();
                               }

                               sp.Close();
                               knob_data = knob_data.Substring(knob_data.LastIndexOf('\r') - 11, 12);
                               write_arduino(passed_port);
                               while (knob_data[0] != '|' || knob_data[9] != '-' || knob_data[11] != '\r')
                               {
                                   knob_data = "";
                                   sp.Open();
                                   for (int i = 0; i < 3; i++)
                                   {
                                       //char temp = sp.ReadChar().ToString()[0];
                                       knob_data += sp.ReadLine();
                                   }

                                   sp.Close();
                                   knob_data = knob_data.Substring(knob_data.LastIndexOf('\r') - 11, 12);
                               }
                           }
                       }
                       else
                       {
                           write_error_log("Error in COM Port\r\n");
                           write_error_log("Cant connect to arduino on " + passed_port + "\r\n");
                           knob_data = "|50|3|----\r";

                           write_arduino("No");
                       }
                   }
                   catch {//something fialed, do default
                       write_error_log("Cant connect to arduino on " + passed_port + "\r\n");
                       knob_data = "|50|3|----\r";

                       write_arduino("No");
                   }

                   string[] knobs = knob_data.Split('|');

                    GLOBAL_KNOB.Station = short.Parse(knobs[2]);
                    GLOBAL_KNOB.Station_Type = 2; // my am/fm/amc knob doesnt do anything
                    GLOBAL_KNOB.Volume = short.Parse(knobs[1]);
                   break;
            }
            

            

        }
        /// <summary>
        /// All of these are for multithreading performance
        /// </summary>
        /// <param name="writedata"></param>
        private void write_station(string writedata)
        {
            if (setable_station.InvokeRequired)
            {
                setable_station.BeginInvoke(new StringParameterDelegate(write_station), new object[] { writedata });
                return;
            }
            setable_station.Text = writedata;
        }
        private void write_stationtype(string writedata)
        {
            if (setable_station_style.InvokeRequired)
            {
                setable_station_style.BeginInvoke(new StringParameterDelegate(write_stationtype), new object[] { writedata });
                return;
            }
            setable_station_style.Text = writedata;
        }
        private void write_volume(string writedata)
        {
            if (setable_volume.InvokeRequired)
            {
                setable_volume.BeginInvoke(new StringParameterDelegate(write_volume), new object[] { writedata });
                return;
            }
            setable_volume.Text = writedata;
        }
        private void write_arduino(string writedata)
        {
            if (setable_volume.InvokeRequired)
            {
                setable_volume.BeginInvoke(new StringParameterDelegate(write_arduino), new object[] { writedata });
                return;
            }
            setable_arduino_stat.Text = writedata;
        }
        /**************************************************
        ********* END Reading Knob Thread
        **************************************************/
        #endregion

        #region Change Station
        /**************************************************
        ********* Set Station Knob Thread
        **************************************************/

        /// <summary>
        /// This is at the heart of everything, it builds the playlists from a passed folder
        /// </summary>
        /// <param name="passed_host_holder"></param>
        private void compile_station(string passed_host_holder) // 88-108
        {
            clear_playlist("");
            shutdown_armed = false;
            switch (passed_host_holder)
            {
                case "static":
                    break;
                case "":
                    List<string> not_shuffled_ads_list = new List<string>();
                    List<string> not_shuffled_song_list = new List<string>();
                    foreach (Drive Single_Drive in Drives)
                    {
                        if (Single_Drive.has_music_folder)
                        {
                            for (int k = 1; k < 11; k++)
                            {
                                DirectoryInfo Dir_search = new DirectoryInfo(Single_Drive.drive_letter + ":\\music\\" + k.ToString() + "\\music\\"); //sub folders spots and songs
                                List<FileInfo> files_music = new List<FileInfo>(Dir_search.GetFiles());
                                foreach (FileInfo File in files_music)
                                {
                                    not_shuffled_song_list.Add(File.FullName.ToString());                                
                                }
                                files_music = null;

                                Dir_search = new DirectoryInfo(passed_host_holder + "\\spots\\"); //sub folders spots and songs
                                List<FileInfo> files_ads = new List<FileInfo>(Dir_search.GetFiles());
                                foreach (FileInfo File in files_ads)
                                {
                                    not_shuffled_ads_list.Add(File.FullName.ToString());
                                }
                                files_ads = null;
                            }
                            //Now I have not shuffled data, but cant shuffle till out of for-each
                        }
                        else
                        {
                            //Scan drive for media and add
                            if(Directory.Exists(Single_Drive.drive_letter.ToString() + "\\"))
                                recursive_music_hunt(Single_Drive.drive_letter.ToString() + "\\" , not_shuffled_song_list);
                        }
                    }
                    //now we need to shuffle both and add to player
                    // stopped here
                    break;
                default:
                    string last_file_name = "";
                    switch (GLOBAL_KNOB.Station_Type)
                    { // 1 just ads, 2 mixed, 3 just music, this really isnt used, but here if one day wants to be
                        case 1:
                            DirectoryInfo Dir_search_ads_only = new DirectoryInfo(passed_host_holder + "\\spots\\"); //sub folders spots and songs
                            List<FileInfo> files_ads_only = new List<FileInfo>(Dir_search_ads_only.GetFiles());
                            while (files_ads_only.Count != 0)
                            {
                                Random random_num = new Random(Convert.ToInt32(DateTime.Now.Second.ToString()));
                                int ran = random_num.Next(0, files_ads_only.Count);
                                write_TreeView_current(files_ads_only[ran].FullName.ToString());
                                add_song_to_current_playlist(files_ads_only[ran].FullName.ToString());
                                files_ads_only.RemoveAt(ran);
                            }
                            files_ads_only = null;
                            //now i have a list, that is shuffled

                            Player.Ctlcontrols.play();
                            break;
                        case 2://add checks here
                            try
                            {
                                DirectoryInfo Dir_search_music = new DirectoryInfo(passed_host_holder + "\\music\\"); //sub folders spots and songs
                                List<FileInfo> files_music = new List<FileInfo>(Dir_search_music.GetFiles());
                                DirectoryInfo Dir_search_ads = new DirectoryInfo(passed_host_holder + "\\spots\\"); //sub folders spots and songs
                                List<FileInfo> files_ads = new List<FileInfo>(Dir_search_ads.GetFiles());
                                List<string> files_shuffled_ads = new List<string>();
                                while (files_ads.Count != 0)
                                {
                                    Random random_num = new Random(Convert.ToInt32(DateTime.Now.Second.ToString()));
                                    int ran = random_num.Next(0, files_ads.Count);
                                    files_shuffled_ads.Add(files_ads[ran].FullName.ToString());
                                    files_ads.RemoveAt(ran);
                                }
                                files_ads = null;
                                int i = 0;
                                while (files_music.Count != 0)
                                {
                                    Random random_num = new Random(Convert.ToInt32(DateTime.Now.Second.ToString()));
                                    int ran = random_num.Next(0, files_music.Count);
                                    write_TreeView_current(files_music[ran].FullName.ToString());
                                    add_song_to_current_playlist(files_music[ran].FullName.ToString());
                                    files_music.RemoveAt(ran);
                                    if (i % 4 == 0 && files_shuffled_ads.Count != 0)
                                    {//Add ads every 4
                                        write_TreeView_current(files_shuffled_ads[0]);
                                        add_song_to_current_playlist(files_shuffled_ads[0]);
                                        files_shuffled_ads.RemoveAt(0);
                                    }
                                }
                                files_music = null;
                                while (files_shuffled_ads.Count != 0)
                                {
                                    write_TreeView_current(files_shuffled_ads[0]);
                                    add_song_to_current_playlist(files_shuffled_ads[0]);
                                    files_shuffled_ads.RemoveAt(0);
                                }
                                //now i have a list, that is shuffled
                                last_file_name = passed_host_holder + "\\closing.mp3";
                                add_song_to_current_playlist(last_file_name);

                                Player.Ctlcontrols.play();
                                break;
                            }
                            catch
                            {//plays static
                                break;
                            }
                        case 3:
                            DirectoryInfo Dir_search_music_only = new DirectoryInfo(passed_host_holder + "\\music\\"); //sub folders spots and songs
                            List<FileInfo> files_music_only = new List<FileInfo>(Dir_search_music_only.GetFiles());
                            while (files_music_only.Count != 0)
                            {
                                Random random_num = new Random(Convert.ToInt32(DateTime.Now.Second.ToString()));
                                int ran = random_num.Next(0, files_music_only.Count);
                                write_TreeView_current(files_music_only[ran].FullName.ToString());
                                add_song_to_current_playlist(files_music_only[ran].FullName.ToString());
                                files_music_only.RemoveAt(ran);
                            }
                            files_ads_only = null;
                            //now i have a list, that is shuffled
                            last_file_name = passed_host_holder + "\\closing.mp3";
                            add_song_to_current_playlist(last_file_name);

                            Player.Ctlcontrols.play();
                            break;
                    }
                    break;
            }
           
            DirectoryInfo dirser = new DirectoryInfo(passed_host_holder); // so like a F:music\3\
            try
            {
                dirser = new DirectoryInfo(dirser.Parent.FullName + "\\Sounds\\");
            }
            catch { }
            for (int i = 0; i < 300; i++)
            {
                write_TreeView_current(dirser.FullName + "\\static.mp3");
                add_song_to_current_playlist(dirser.FullName + "\\static.mp3");
            }
            write_TreeView_current(dirser.FullName + "\\shutdown.mp3");
            add_song_to_current_playlist(dirser.FullName + "\\shutdown.mp3");
            DateTime dt = DateTime.Now;
            shutdown_time = dt.Second + 1804;//this is actuallywrong cause it doesnt count the song times, but I didnt fix in time for final project
            shutdown_armed = true;
        }

        /// <summary>
        /// handles folder being given and checked for music
        /// </summary>
        /// <param name="passed_letter"></param>
        private void set_folder(string passed_letter)
        {
            current_set_folder = passed_letter;
            
            if (passed_letter == "NA")
            {
                if (stations_reserver > 0)
                {
                    foreach (Drive disk in Drives)
                    {
                        if (disk.has_music_folder)
                        {
                            sd_card_setable.Text = disk.drive_letter;
                            if (Directory.Exists(disk.drive_letter + "\\Music\\" + GLOBAL_KNOB.Station + "\\"))
                            {
                                compile_station(disk.drive_letter + "\\Music\\" + GLOBAL_KNOB.Station + "\\");
                                break;
                            }
                        }
                    }
                }else{
                    //read out error
                }
            }
            else
            {
                try
                {
                    sd_card_setable.Text = passed_letter;
                    if (Directory.Exists(passed_letter + "\\Music\\" + GLOBAL_KNOB.Station + "\\"))
                    {
                        compile_station(passed_letter + "\\Music\\" + GLOBAL_KNOB.Station + "\\");
                    }
                    
                }
                catch
                {
                    set_folder("NA");
                }
            }
        }

        /// <summary>
        /// Can hunt recursively for music through a directory
        /// </summary>
        /// <param name="location"></param>
        /// <param name="file_list"></param>
        private void recursive_music_hunt(string location, List<string> file_list)
        {

            foreach (string Dir in Directory.GetDirectories(location))
            {
                recursive_music_hunt(Dir, file_list);
            }

            foreach (string Fil in Directory.GetFiles(location))
            {
                string debug = Fil.Substring(Fil.LastIndexOf("."), Fil.Length - Fil.LastIndexOf("."));
                switch (Fil.Substring(Fil.LastIndexOf("."), Fil.Length - Fil.LastIndexOf(".")))
                {//types of files scanned for
                    case ".m4a":
                    case ".wav":
                    case ".mp3":
                    case ".aac":
                    case ".wma":
                    case ".midi":
                        file_list.Add(Fil);
                        break;
                }
            }
        }

        /// <summary>
        /// More multithreading goodness
        /// </summary>
        /// <param name="writedata"></param>
        private void write_TreeView_current(string writedata)
        {
            if (treeView1.InvokeRequired)
            {
                treeView1.BeginInvoke(new StringParameterDelegate(write_TreeView_current), new object[] { writedata });
                return;
            }
            if (writedata == "CLEAR_LIST")
                treeView1.Nodes[0].Nodes.Clear();
            else
            {
                if (!added_songs.Contains(writedata))
                {
                    treeView1.Nodes[0].Nodes.Add(writedata);
                    added_songs += "|" + writedata;// weird double data in list
                }
            }
            treeView1.ExpandAll();
        }
        private void write_error_log(string writedata)
        {
            if (textBox1.InvokeRequired)
            {
                textBox1.BeginInvoke(new StringParameterDelegate(write_error_log), new object[] { writedata });
                return;
            }
            if (writedata == "CLEAR_LIST")
                textBox1.Text = "";
            else
            {
                if (!textBox1.Text.EndsWith(writedata))
                    textBox1.Text += writedata;
            }
           
        }
        private void add_song_to_playlist(string passed_song)
        { // not in use
            if (Player.InvokeRequired)
            {
                Player.BeginInvoke(new StringParameterDelegate(add_song_to_playlist), new object[] { passed_song });
                return;
            }

            //http://social.msdn.microsoft.com/Forums/en/csharpgeneral/thread/87d0b59b-0fe4-4201-9b6d-07eca7bae0cd
            string myPlaylist = "Current";

            WMPLib.IWMPPlaylist pl;

            WMPLib.IWMPPlaylistArray plItems;

            plItems = Player.playlistCollection.getByName(myPlaylist);

            if (plItems.count == 0)

                pl = Player.playlistCollection.newPlaylist(myPlaylist);

            else

                pl = plItems.Item(0); // may need to remove this

            if (System.IO.File.Exists(passed_song))
            {

                WMPLib.IWMPMedia m1 = Player.newMedia(passed_song);

                pl.appendItem(m1);

            }

        }
        private void add_song_to_current_playlist(string passed_song)
        {
            if (Player.InvokeRequired)
            {
                Player.BeginInvoke(new StringParameterDelegate(add_song_to_current_playlist), new object[] { passed_song });
                return;
            }
            write_TreeView_current(passed_song);
            if (System.IO.File.Exists(passed_song))
            {

                WMPLib.IWMPMedia m1 = Player.newMedia(passed_song);

                Player.currentPlaylist.appendItem(m1);

            }
        }
        private void clear_playlist(string writedata)
        {
            if (Player.InvokeRequired)
                Player.Invoke(new StringParameterDelegate(clear_playlist), new object[] { writedata }); // Ill be honest, this is mostly lazy coding
            Player.currentPlaylist.clear();
            write_TreeView_current("CLEAR_LIST");
            added_songs = "";

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(textBox2.Text + "\\Music\\" + GLOBAL_KNOB.Station + "\\"))
                compile_station(textBox2.Text + "\\Music\\" + GLOBAL_KNOB.Station + "\\");
            else
            {
                MessageBox.Show("Cant Find Folder");
                write_error_log("Cant find folder\r\n");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            read_for_cards();
            set_folder("NA");
        }

        private void SD_Select_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_folder(SD_Select.SelectedItem.ToString() + ":");
        }

        private void RadioManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            Closing_app = true;
            Player.Ctlcontrols.stop();
            clear_playlist("blah");
            Thread.Sleep(600);//enough time for working threads to see finish
            Application.Exit();
        }

        /**************************************************
        ********* END Set Station Knob Thread
        **************************************************/
        #endregion

        //This was a theory of a web playlist that for lack of time I didnt implement fully
        /*#region Web Playlist
        private void play_youtube_playlist(string url)
        {
            Int16 length = Convert.ToInt16(GetSongLength(url));
            webBrowser.Navigate(url);
        }
        public string GetSongLength(string Url)
        {
            // Open a connection
            HttpWebRequest WebRequestObject = (HttpWebRequest)HttpWebRequest.Create(Url);

            // You can also specify additional header values like 
            // the user agent or the referer:
            WebRequestObject.UserAgent = ".NET Framework/2.0";
            //WebRequestObject.Referer = "http://www.example.com/";

            // Request response:
            WebResponse Response = WebRequestObject.GetResponse();

            // Open data stream:
            Stream WebStream = Response.GetResponseStream();

            // Create reader object:
            StreamReader Reader = new StreamReader(WebStream);

            // Read the entire stream content:
            string PageContent = Reader.ReadToEnd();

            // Cleanup
            Reader.Close();
            WebStream.Close();
            Response.Close();
            string sub = "";
            if (PageContent.Contains("length_"))
            {
                int pos1 = PageContent.IndexOf("\"length_seconds\": ");
                int pos2 = PageContent.IndexOf(",", pos1);
                sub = PageContent.Substring(pos1 + 18, pos2 - pos1 - 18);
            }

            return sub;
        }
        #endregion*/

        /// <summary>
        /// Code below scans folder structors and makes sure that the music card is in a usable state
        /// </summary>
        /// <param name="passed_letter"></param>
        /// <returns></returns>
        #region Card Checking
        private bool check_card_structor(char passed_letter)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(passed_letter.ToString(), "^[a-zA-Z]+$"))
            {
                if (Directory.Exists(passed_letter.ToString() + ":\\Music"))
                {
                    if (Directory.Exists(passed_letter.ToString() + ":\\Music\\1") &&
                            Directory.Exists(passed_letter.ToString() + ":\\Music\\2") &&
                            Directory.Exists(passed_letter.ToString() + ":\\Music\\3") &&
                            Directory.Exists(passed_letter.ToString() + ":\\Music\\4") &&
                            Directory.Exists(passed_letter.ToString() + ":\\Music\\5") &&
                            Directory.Exists(passed_letter.ToString() + ":\\Music\\6") &&
                            Directory.Exists(passed_letter.ToString() + ":\\Music\\7") &&
                            Directory.Exists(passed_letter.ToString() + ":\\Music\\8") &&
                            Directory.Exists(passed_letter.ToString() + ":\\Music\\9") &&
                            Directory.Exists(passed_letter.ToString() + ":\\Music\\10"))
                    {
                        for (int i = 1; i < 11; i++)
                        {
                            if (Directory.Exists(passed_letter.ToString() + ":\\Music\\" + i + "\\music") ||
                                Directory.Exists(passed_letter.ToString() + ":\\Music\\" + i + "\\spots"))
                            {
                                if (!Directory.Exists(passed_letter.ToString() + ":\\Music\\" + "\\sounds"))
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        #endregion

        /// <summary>
        /// creates flder structor in given location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(textBox2.Text))
            {
                try
                {
                    DirectoryInfo location = new DirectoryInfo(textBox2.Text);
                    Directory.CreateDirectory(location.ToString() + "Music");
                    location = new DirectoryInfo(textBox2.Text + "\\music");
                    for (int i = 1; i < 11; i++)
                    {
                        Directory.CreateDirectory(location.ToString() + "\\" + i);
                        location = new DirectoryInfo(textBox2.Text + "\\music\\" + i);
                        Directory.CreateDirectory(location.ToString() + "\\spots");
                        Directory.CreateDirectory(location.ToString() + "\\music");
                        location = new DirectoryInfo(textBox2.Text + "\\music");
                    }
                    Directory.CreateDirectory(location.ToString() + "\\Sounds");
                    MessageBox.Show("Folder structure created successfully");
                }
                catch
                {
                    MessageBox.Show("Error writing folder structor");
                }
            }
            else
            {
                write_error_log("Location given to create folders not found\r\n");
            }
        }

        private void COM_Select_SelectedValueChanged(object sender, EventArgs e)
        {
            arduino_port = COM_Select.SelectedItem.ToString();
        }

        private void read_settings(string settings_file)
        {

            FileInfo Running = new FileInfo(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            DirectoryInfo Containing = Running.Directory;
            if (File.Exists(Containing + "\\settings.txt"))
            {
                string[] settings = File.ReadAllLines(Containing + "\\settings.txt", Encoding.Default);
                foreach (string line in settings)
                {
                    if (line.StartsWith("com ports: "))
                        arduino_port = line.Substring(10, line.Length - 10);
                }
            }
        }

        /// <summary>
        /// Writes settings file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            FileInfo Running = new FileInfo(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            DirectoryInfo Containing = Running.Directory;
            if (!File.Exists(Containing + "\\settings.txt"))
            {
                List<string> settings = new List<string>();
                settings.Add("Settings for RadioManager, v" + label8.Text);
                settings.Add("com port: " + arduino_port);
                settings.Add("working folder: " + current_set_folder);
                File.WriteAllLines(Containing + "\\settings.txt", settings.ToArray());
            }
            else
            {
                DialogResult result = MessageBox.Show("A settings file already exists, overwrite?", "Error settings file", MessageBoxButtons.OKCancel);
                if (result == System.Windows.Forms.DialogResult.OK)
                    File.Delete(Containing + "\\settings.txt");
                List<string> settings = new List<string>();
                settings.Add("Settings for RadioManager, v" + label8.Text);
                settings.Add("com ports: " + arduino_port);
                settings.Add("working folder: " + current_set_folder);
                File.WriteAllLines(Containing + "\\settings.txt", settings.ToArray());
            }

        }

        /// <summary>
        /// sees what ports are avalible
        /// </summary>
        /// <param name="passed_Port"></param>
        /// <returns></returns>
        private bool check_com_port(string passed_Port)
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                if (port == passed_Port)
                    return true;
            }
            return false;
        }


    }
}
