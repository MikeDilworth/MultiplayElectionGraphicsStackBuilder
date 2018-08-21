using System;

namespace DataInterface.DataModel
{
    
	public class EngineModel
	{
        public string IPAddress { get; set; }
        public int Port { get; set; }
        public bool enable { get; set; }
        public bool connected { get; set; }
        public int id { get; set; }

	}
}
