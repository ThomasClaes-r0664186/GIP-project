using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gip.Models.Model
{
    public class Room
    {
        private int _number, _capacity, _floor;
        private bool _whiteboard, _projector, _wifi;
        private string _type, _building;

        public Room(int num, int capacity, int floor, bool white, bool proj, bool wifi, string type, string build) {
            Number = num;
            Capacity = capacity;
            Floor = floor;
            Whiteboard = white;
            Projector = proj;
            Wifi = wifi;
            Type = type;
            Build = build;
        }

        //voorwaarden aan toevoegen.
        public int Number { get; set; }
        public int Capacity { get; set; }
        public int Floor { get; set; }
        public bool Whiteboard { get; set; }
        public bool Projector { get; set; }
        public bool Wifi { get; set; }
        public string Type { get; set; }
        public string Build { get; set; }
    }
}