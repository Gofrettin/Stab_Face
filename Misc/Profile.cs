using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;

namespace Stab_Face.Misc
{
    [DataContractAttribute()]
    public class Profile
    {
        [DataMember()]
        public String name;

        [DataMember()]
        public List<Waypoint> waypoints;

        [DataMember()]
        public List<UInt16> Factions;

        public Profile()
        {
            name = "NewProfile";
            waypoints = new List<Waypoint>();
            Factions = new List<UInt16>();
        }

        public void setName(String name)
        {
            this.name = name;
        }

        public void AddWaypoint(Waypoint wp)
        {
            this.waypoints.Add(wp);
        }

        public void addFaction(UInt16 fac)
        {
            this.Factions.Add(fac);
        }

        public List<Waypoint> getWaypoints()
        {
            return this.waypoints;
        }

        public List<UInt16> getFactions()
        {
            return this.Factions;
        }

        public void Save()
        {
            using (FileStream writer = new FileStream("Profiles\\" + this.name + ".xml",
            FileMode.Create, FileAccess.Write))
            {
                DataContractSerializer ser;
                ser = new DataContractSerializer(this.GetType());
                ser.WriteObject(writer, this);
            }
        }

        public static Profile Load(String path)
        {
            Profile p;
            using (FileStream reader = new FileStream(path,
            FileMode.Open, FileAccess.Read))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(Profile));
                p = (Profile)ser.ReadObject(reader);
            }
            return p;
        }
    }

}
