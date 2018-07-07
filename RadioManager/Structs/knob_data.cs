using System;
using System.Collections.Generic;
using System.Text;

namespace RadioManager.Structs
{
    public struct knob_data
    {
        private byte i_station_type;

        public byte Station_Type
        {
            get
            {
                return i_station_type;
            }
            set
            {
                i_station_type = value;
            }
        }

        private short i_station;

        public short Station
        {
            get
            {
                return i_station;
            }
            set
            {
                i_station = value;
            }
        }

        private short i_volume;

        public short Volume
        {
            get
            {
                return i_volume;
            }
            set
            {
                i_volume = value;
            }
        }
                

        public knob_data(byte station_type, short station, short volume)
        {
            i_station_type = station_type;
            i_station = station;
            i_volume = volume;
        }

        public static bool operator ==(knob_data left, knob_data right)
        {
            if (0 <= left.Volume && left.Volume <= 100 && 1 <= left.Station && left.Station <= 10 && 1 <= left.Station_Type && left.Station_Type <= 3)
                return false;
            if (0 <= right.Volume && right.Volume <= 100 && 1 <= right.Station && right.Station <= 10 && 1 <= right.Station_Type && right.Station_Type <= 3)
                return false;
            return (left == right);
        }
        /*public override bool Equals(knob_data passed)
        {
            if (this.Volume != null && this.Station != null && this.Station_Type != null)
                return false;

            if (passed.Volume != null && passed.Station != null && passed.Station_Type != null)
                return false;
            return (this == passed);
        }*/

        public static bool operator !=(knob_data left, knob_data right)
        {
            return !(left == right);
        }

    }
}
