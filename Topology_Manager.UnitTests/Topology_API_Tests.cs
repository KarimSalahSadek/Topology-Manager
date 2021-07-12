using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Topology_Manager.UnitTests
{
    [TestClass]
    public class Topology_API_Tests
    {
        [TestMethod]
        public void ReadJson_FileIsNotJson_ReturnsFalse()
        {
            var API = new Topology_Manager.Topology_API();
            var topologies = new List<Topology>();

            bool res = API.ReadJson("topology.jpg", ref topologies);

            Assert.IsFalse(res);
        }
        [TestMethod]
        public void ReadJson_FileDoesntExist_ReturnsFalse()
        {
            var API = new Topology_Manager.Topology_API();
            var topologies = new List<Topology>();

            bool res = API.ReadJson("topology_file.json", ref topologies);

            Assert.IsFalse(res);
        }
        [TestMethod]
        public void WriteFile_InvalidTopologyID_ReturnsFalse()
        {
            var API = new Topology_Manager.Topology_API();
            var topologies = new List<Topology>();

            bool res = API.WriteJson("TID", ref topologies);

            Assert.IsFalse(res);
        }
        [TestMethod]
        public void QueryTopologies_ValidInput_ReturnsInputTopologyList()
        {
            var API = new Topology_Manager.Topology_API();
            var topologies = new List<Topology>();

            var res = API.QueryTopologies(ref topologies);

            Assert.AreSame(res,topologies);
        }
        [TestMethod]
        public void DeleteTopology_ValidTopologyID_ReturnsTrue()
        {
            var API = new Topology_Manager.Topology_API();
            var topologies = new List<Topology>();
            var topology = new Topology();
            topology.id = "T1";
            topologies.Add(topology);

            var res = API.DeleteTopology("T1", ref topologies);

            Assert.IsTrue(res);
        }
        [TestMethod]
        public void DeleteTopology_InvalidTopologyID_ReturnsFalse()
        {
            var API = new Topology_Manager.Topology_API();
            var topologies = new List<Topology>();
            var topology = new Topology();
            topology.id = "T1";
            topologies.Add(topology);

            var res = API.DeleteTopology("T2", ref topologies);

            Assert.IsFalse(res);
        }
        [TestMethod]
        public void QueryDevices_InvalidTopologyID_ReturnsEmptyComponentList()
        {
            var API = new Topology_Manager.Topology_API();
            var topologies = new List<Topology>();

            List<Component> res = API.QueryDevices("t2", ref topologies);

            Assert.IsTrue(res.Count==0);
        }
        [TestMethod]
        public void QueryDevicesWithNetlistNode_InvalidTopologyID_ReturnsEmptyComponentList()
        {
            var API = new Topology_Manager.Topology_API();
            var topologies = new List<Topology>();
            var topology = new Topology();
            topology.id = "T1";
            topologies.Add(topology);

            var res = API.QueryDevicesWithNetlistNode("invalidID","invalid_node", ref topologies);

            Assert.IsTrue(res.Count == 0);
        }

    }
}
