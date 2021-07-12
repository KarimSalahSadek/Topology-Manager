using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;



namespace Topology_Manager
{
    //API Definitions
    public class Topology_API
    {
        public bool ReadJson(string filename, ref List<Topology_Manager.Topology> topologies)
        {

            List<Topology_Manager.Topology> new_topologies = new List<Topology_Manager.Topology>();
            try
            {
                string JSON = System.IO.File.ReadAllText(filename);
                if (JSON.Substring(JSON.Length - 5).ToLower() != ".json") { Console.WriteLine("File type must be json"); return false; }
                if (JSON[0] != '[') JSON = '[' + JSON;
                if (JSON[JSON.Length - 1] != ']') JSON = JSON + ']';
                new_topologies = JsonConvert.DeserializeObject<List<Topology_Manager.Topology>>(JSON);
                topologies.AddRange(new_topologies);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return false;
            }
            finally
            {
                foreach (Topology_Manager.Topology t in new_topologies)
                {
                    Console.WriteLine("Topology " + t.id + " added to memory");
                }

            }

            return true;
        }
        public bool WriteJson(string TopologyID, ref List<Topology_Manager.Topology> topologies)
        {
            foreach (Topology_Manager.Topology t in topologies)
            {
                if (t.id == TopologyID)
                {
                    try
                    {
                        string JSON = JsonConvert.SerializeObject(t, Formatting.Indented);
                        StreamWriter sw = new StreamWriter("LastSavedJsonTopology.json");
                        sw.Write(JSON);
                        sw.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception: " + e.Message);
                        return false;
                    }
                    finally
                    {
                        Console.WriteLine("Topology " + t.id + " saved successfully!");
                    }
                    return true;
                }

            }
            Console.WriteLine("Topology " + TopologyID + " not found!");
            return false;
        }
        public List<Topology_Manager.Topology> QueryTopologies(ref List<Topology_Manager.Topology> topologies)
        {
            foreach (Topology_Manager.Topology t in topologies)
            {
                try
                {
                    string JSON = JsonConvert.SerializeObject(t, Formatting.Indented);
                    Console.Write(JSON);
                    Console.Write('\n');
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }
            }

            return topologies;
        }
        public bool DeleteTopology(string TopologyID, ref List<Topology_Manager.Topology> topologies)
        {
            foreach (Topology_Manager.Topology t in topologies)
            {
                if (t.id == TopologyID)
                {
                    try
                    {
                        topologies.Remove(t);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception: " + e.Message);
                        return false;
                    }
                    finally
                    {
                        Console.WriteLine("Topology " + t.id + " deleted successfully!");

                    }
                    return true;
                }
            }
            Console.WriteLine("Topology " + TopologyID + " not found!");
            return false;
        }
        public List<Topology_Manager.Component> QueryDevices(string TopologyID, ref List<Topology_Manager.Topology> topologies)
        {
            foreach (Topology_Manager.Topology t in topologies)
            {
                if (t.id == TopologyID)
                {
                    Console.WriteLine("Topology " + t.id + " devices:");
                    Console.WriteLine("Device ID\t::\tDevice Type");
                    try
                    {
                        foreach (Topology_Manager.Component c in t.components)
                        {
                            Console.WriteLine(c.id + "\t::\t" + c.type);
                        }
                        return t.components;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception: " + e.Message);
                    }
                    break;
                }
            }
            return new List<Topology_Manager.Component>();
        }
        public List<Topology_Manager.Component> QueryDevicesWithNetlistNode(string TopologyID, string netlistnode, ref List<Topology_Manager.Topology> topologies)
        {
            List<Topology_Manager.Component> returnedList = new List<Topology_Manager.Component>();

            foreach (Topology_Manager.Topology t in topologies)
            {
                if (t.id == TopologyID)
                {
                    Console.WriteLine("Topology " + t.id + " devices connected to node " + netlistnode + ":");
                    Console.WriteLine("Device ID\t::\tDevice Type");
                    try
                    {
                        foreach (Topology_Manager.Component c in t.components)
                        {
                            if (c.netlist.ContainsValue(netlistnode))
                            {
                                Console.WriteLine(c.id + "\t::\t" + c.type);
                                returnedList.Add(c);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception: " + e.Message);
                    }
                    break;
                }
            }
            return returnedList;
        }

    }

    public class Topology
    {
        //public string JSONstr;
        public string id { get; set; }
        public List<Component> components { get; set; }
        
    }
    public class Component
    {
        public string type { get; set; }
        public string id { get; set; }
        public Dictionary<string,string> netlist { get; set; }
        [JsonExtensionData]
        public IDictionary<string,dynamic> other { get; set; }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            List<Topology> topologies = new List<Topology>();
            Console.WriteLine("Topology Manager by Karim Salah\nEnter 'help' to view available commands");
            string input;
            while (true) {

                try
                {
                    input = Console.ReadLine();
                    if (input == "help")
                    {
                        Console.WriteLine("ReadJson file_path");
                        Console.WriteLine("WriteJson Topology_ID");
                        Console.WriteLine("QueryTopologies");
                        Console.WriteLine("DeleteTopology Topology_ID");
                        Console.WriteLine("QueryDevices Topology_ID");
                        Console.WriteLine("QueryDevicesWithNetlistnode Topology_ID netlistnode");
                        continue;
                    }
                    if (input.Length < 8) { Console.WriteLine("Unknown command!"); continue; }
                    if (input.Substring(0, Math.Min("readJson".Length, input.Length)) == "readJson")
                    {
                        
                        List<Topology_Manager.Topology> new_topologies = new List<Topology_Manager.Topology>();
                        try
                        {
                            string filename = input.Substring("readJson".Length + 1);
                            string JSON = System.IO.File.ReadAllText(filename.Trim('"'));
                            if (JSON[0] != '[') JSON = '[' + JSON;
                            if (JSON[JSON.Length - 1] != ']') JSON = JSON + ']';
                            new_topologies = JsonConvert.DeserializeObject<List<Topology_Manager.Topology>>(JSON);
                            topologies.AddRange(new_topologies);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Exception: " + e.Message);
                        }
                        finally
                        {
                            foreach (Topology_Manager.Topology t in new_topologies)
                            {
                                Console.WriteLine("Topology " + t.id + " added to memory");
                            }

                        }
                        
                    }
                    else if (input.Substring(0, Math.Min("writeJson".Length, input.Length)) == "writeJson")
                    {
                        string id;
                        try
                        {
                            id = input.Substring("writeJson".Length + 1);
                        }
                        catch
                        {
                            Console.WriteLine("Invalid id!");
                            continue;
                        }
                        foreach (Topology t in topologies)
                        {
                            if (t.id == id)
                            {
                                try
                                {
                                    string JSON = JsonConvert.SerializeObject(t, Formatting.Indented);
                                    StreamWriter sw = new StreamWriter("LastSavedJsonTopology.json");
                                    sw.Write(JSON);
                                    sw.Close();
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Exception: " + e.Message);
                                }
                                finally
                                {
                                    Console.WriteLine("Topology " + t.id + " saved successfully!");
                                }
                                break;
                            }
                        }
                    }

                    else if (input.Substring(0, Math.Min("queryDevicesWithNetlistNode".Length, input.Length)) == "queryDevicesWithNetlistNode")
                    {
                        string id;
                        string netlistnode;
                        try
                        {
                            input = input.TrimEnd(' ');
                            int secondSpace = input.LastIndexOf(' ');
                            id = input.Substring("queryDevicesWithNetlistNode".Length + 1, secondSpace - "queryDevicesWithNetlistNode".Length - 1);
                            netlistnode = input.Substring(secondSpace + 1);
                        }
                        catch
                        {
                            Console.WriteLine("Invalid id!");
                            continue;
                        }
                        foreach (Topology t in topologies)
                        {
                            if (t.id == id)
                            {
                                Console.WriteLine("Topology " + t.id + " devices connected to node " + netlistnode + ":");
                                Console.WriteLine("Device ID\t::\tDevice Type");
                                try
                                {
                                    foreach (Component c in t.components)
                                    {
                                        if (c.netlist.ContainsValue(netlistnode))
                                        {
                                            Console.WriteLine(c.id + "\t::\t" + c.type);
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Exception: " + e.Message);
                                }
                                break;
                            }
                        }
                    }
                    else if (input.Substring(0,  Math.Min("queryDevices".Length,input.Length)) == "queryDevices")
                    {
                        string id;
                        try
                        {
                            id = input.Substring("queryDevices".Length + 1);
                        }
                        catch
                        {
                            Console.WriteLine("Invalid id!");
                            continue;
                        }
                        foreach (Topology t in topologies)
                        {
                            if (t.id == id)
                            {
                                Console.WriteLine("Topology " + t.id + " devices:");
                                Console.WriteLine("Device ID\t::\tDevice Type");
                                try
                                {
                                    foreach (Component c in t.components)
                                    {
                                        Console.WriteLine(c.id + "\t::\t" + c.type);
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Exception: " + e.Message);
                                }
                                break;
                            }
                        }
                    }
                    else if (input.Substring(0, Math.Min("queryTopologies".Length, input.Length)) == "queryTopologies")
                    {
                        foreach (Topology t in topologies)
                        {
                            try
                            {
                                string JSON = JsonConvert.SerializeObject(t, Formatting.Indented);
                                Console.Write(JSON);
                                Console.Write('\n');
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Exception: " + e.Message);
                            }
                        }
                    }
                    else if (input.Substring(0, Math.Min("deleteTopology".Length, input.Length)) == "deleteTopology")
                    {
                        string id;
                        try
                        {
                            id = input.Substring("deleteTopology".Length + 1);
                        }
                        catch
                        {
                            Console.WriteLine("Invalid id!");
                            continue;
                        }
                        foreach (Topology t in topologies)
                        {
                            if (t.id == id)
                            {
                                try
                                {
                                    topologies.Remove(t);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Exception: " + e.Message);
                                }
                                finally
                                {
                                    Console.WriteLine("Topology " + t.id + " deleted successfully!");
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Unknown command!");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Unknown command!");
                    continue;
                }
            }
            

        }
    }
}
