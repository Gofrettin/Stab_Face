using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;

namespace Stab_Face.Misc
{
    [DataContractAttribute()]
    public class Profile
    {
        [DataMember()]
        private String name;

        [DataMember()]
        private List<Waypoint> waypoints;

        [DataMember()]
        private List<Waypoint> ghostWaypoints;

        [DataMember()]
        private List<UInt16> factions;

        public Profile()
        {
            name = "NewProfile";
            waypoints = new List<Waypoint>();
            ghostWaypoints = new List<Waypoint>();
            factions = new List<UInt16>();
        }

        public void setName(String name)
        {
            this.name = name;
        }

        public void AddWaypoint(Waypoint wp)
        {
            this.waypoints.Add(wp);
            Debug.Print("Profile " + this.name + " Added Waypoint: " + wp.ToString());
        }

        public void AddGhostWaypoint(Waypoint wp)
        {
            this.ghostWaypoints.Add(wp);
            Debug.Print("Profile " + this.name + " Added Ghost Waypoint: " + wp.ToString());
        }

        public void addFaction(UInt16 fac)
        {
            this.factions.Add(fac);
            Debug.Print("Profile " + this.name + " Added Faction: " + fac.ToString());
        }

        public String getName()
        {
            return this.name;
        }

        public List<Waypoint> getWaypoints()
        {
            return this.waypoints;
        }

        public List<Waypoint> getGhostWaypoints()
        {
            return this.ghostWaypoints;
        }

        public List<UInt16> getFactions()
        {
            return this.factions;
        }

        public void Save()
        {
            using (FileStream writer = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\Profiles\\" + this.name + ".xml",
            FileMode.Create, FileAccess.Write))
            {
                DataContractSerializer ser;
                ser = new DataContractSerializer(this.GetType());
                ser.WriteObject(writer, this);
            }
        }

        /// <summary>
        /// Loads a Stab Face Profile
        /// </summary>
        /// <param name="path">Absolute Path to Profile.</param>
        /// <returns>The loaded Profile Object.</returns>
        public static Profile Load(String path)
        {
            Profile p = null;
            using (FileStream reader = new FileStream(path,
            FileMode.Open, FileAccess.Read))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(Profile));
                p = (Profile)ser.ReadObject(reader);
            }
            return p;
        }

        /// <summary>
        /// Attempts to load a Glider Profile.
        /// </summary>
        /// <param name="path">Absolute Path to Glider Profile.</param>
        /// <returns>A Stab Face Profile.</returns>
        public static Profile LoadGliderProfile(String path)
        {
            Profile p = new Profile();

            using (FileStream reader = new FileStream(path,
            FileMode.Open, FileAccess.Read))
            {
                XmlReader xreader = XmlReader.Create(reader);
                String currentElement = "";
                while (xreader.Read())
                {
                    switch (xreader.NodeType)
                    {
                        case XmlNodeType.Element:
                            currentElement = xreader.Name;
                            break;
                        case XmlNodeType.Text:
                            switch (currentElement.ToUpper())
                            {
                                case "MINLEVEL":
                                    //TODO
                                    Debug.WriteLine("MinLevel: " + xreader.Value);
                                    break;
                                case "MAXLEVEL":
                                    //TODO
                                    Debug.WriteLine("MaxLevel: " + xreader.Value);
                                    break;
                                case "FACTIONS":
                                    String[] facs = xreader.Value.Split(' ');
                                    foreach (string fac in facs)
                                    {
                                        p.addFaction(Convert.ToUInt16(fac));
                                    }
                                    break;
                                case "LUREMINUTES":
                                    //TODO
                                    Debug.WriteLine("LureMinutes: " + xreader.Value);
                                    break;
                                case "SKIPWAYPOINTS":
                                    //TODO
                                    Debug.WriteLine("SkipWaypoints: " + xreader.Value);
                                    break;
                                case "NATURALRUN":
                                    //TODO
                                    Debug.WriteLine("NaturalRun: " + xreader.Value);
                                    break;
                                case "WAYPOINT":
                                    String[] wps = xreader.Value.Split(' ');
                                    p.AddWaypoint(new Waypoint(
                                        Convert.ToSingle(wps[0]),
                                        Convert.ToSingle(wps[1]),
                                        0.0f));
                                    break;
                                case "GHOSTWAYPOINT":
                                    String[] gwps = xreader.Value.Split(' ');
                                    p.AddGhostWaypoint(new Waypoint(
                                        Convert.ToSingle(gwps[0]),
                                        Convert.ToSingle(gwps[1]),
                                        0.0f));
                                    break;
                                default:
                                    break;
                            }
                            break;
                    }
                }
            }

            return p;
        }
    }

}
