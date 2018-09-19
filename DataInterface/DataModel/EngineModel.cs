using System;

namespace DataInterface.DataModel
{
    
	public class EngineModel
	{
        public string EngineName { get; set; }
        public string IPAddress { get; set; }
        public int Port { get; set; }
        public bool enable { get; set; }
        public bool connected { get; set; }
        public int id { get; set; }
        public System.Net.IPAddress systemIP { get; set; }


    }
}
