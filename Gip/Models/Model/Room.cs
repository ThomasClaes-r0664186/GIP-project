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
        public int Number 
        { 
            get =>_number;
            set {
                if (value < 0)
                {
                    throw new ArgumentException("Lokaal nummer mag niet negatief zijn.");
                }
                else {
                    _number = value;
                }
            } 
        }

        public int Capacity 
        { 
            get => _capacity;
            set 
            {
                if (value < 0) {
                    throw new ArgumentException("Lokaal capaciteit kan niet negtief zijn.");
                }
                else
                {
                    _capacity = value;
                }
            }
        }

        public int Floor 
        { 
            get => _floor;
            set 
            {
                if (value < 0) {
                    throw new ArgumentException("Het verdiep mag niet negatief zijn.");
                }
                else
                {
                    _floor = value;
                }
            } 
        }

        public bool Whiteboard { get; set; }

        public bool Projector { get; set; }
        
        public bool Wifi { get; set; }
        
        public string Type { get; set; }
        
        public string Build { get; set; }
    }
}