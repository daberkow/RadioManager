using System;
using System.Collections.Generic;
using System.Text;

namespace RadioManager.Structs
{
    struct Drive
    {
        private string i_drive_letter;

        public string drive_letter
        {
            get
            {
                return i_drive_letter;
            }
            set
            {
                i_drive_letter = value;
            }
        }

        private string i_drive_type;

        public string drive_type
        {
            get
            {
                return i_drive_type;
            }
            set
            {
                i_drive_type = value;
            }
        }

        private string i_drive_label;

        public string drive_label
        {
            get
            {
                return i_drive_label;
            }
            set
            {
                i_drive_label = value;
            }
        }

        private bool i_has_music_folder;

        public bool has_music_folder
        {
            get
            {
                return i_has_music_folder;
            }
            set
            {
                i_has_music_folder = value;
            }
        }
        public Drive(string passed_letter, string passed_type, string passed_label)
        {
            i_drive_letter = passed_letter;
            i_drive_type = passed_type;
            i_drive_label = passed_label;
            i_has_music_folder = false;
        }

        public Drive(string passed_letter, string passed_type, string passed_label, bool passed_music_folder)
        {
            i_drive_letter = passed_letter;
            i_drive_type = passed_type;
            i_drive_label = passed_label;
            i_has_music_folder = passed_music_folder;
        }

        public static bool operator ==(Drive left, Drive right)
        {
            return (left == right);
        }

        /*
            return (this.drive_label == passed.drive_label && this.drive_letter == passed.drive_letter && this.drive_type == passed.drive_type && this.has_music_folder == passed.has_music_folder);
        }*/

        public static bool operator !=(Drive left, Drive right)
        {
            return !(left == right);
        }
    }
}
