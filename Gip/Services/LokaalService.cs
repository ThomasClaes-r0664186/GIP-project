using Gip.Models;
using Gip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using Newtonsoft.Json;

namespace Gip.Services
{
    public class LokaalService : ILokaalService
    {
        private gipDatabaseContext db;

        public LokaalService(gipDatabaseContext db) 
        {
            this.db = db;
        }

        public void AddLokaal(string gebouw, int verdiep, string nummer, string type, int capaciteit, string middelen)
        {
            var rInUse = from d in db.Room
                         where d.Gebouw == gebouw
                         where d.Verdiep == verdiep
                         where d.Nummer == nummer
                         select d;

            if (rInUse.Any())
            {
                throw new ArgumentException("Dit lokaal bestaat reeds.");
            }

            Room room = new Room { Gebouw = gebouw.ToUpper(), Verdiep = verdiep, Nummer = nummer, Type = type, Capaciteit = capaciteit, Middelen = middelen };
            db.Room.Add(room);
            db.SaveChanges();
        }

        public void DeleteLokaal(int lokaalId)
        {
            Room room = db.Room.Find(lokaalId);

            if (room == null)
            {
                throw new ArgumentException("Dit lokaal werd niet in de databank gevonden.");
            }

            var qryDelR = from cm in db.CourseMoment
                          where cm.RoomId == room.Id
                          select cm;

            if (qryDelR.Any())
            {
                foreach (var CoUs in qryDelR)
                {
                    db.CourseMoment.Remove(CoUs);
                }
                db.SaveChanges();
            }

            db.Room.Remove(room);
            db.SaveChanges();
            
        }

        public void EditLokaal(int lokaalId, string gebouw, int verdiep, string nummer, string type, int capaciteit, string middelen)
        {
            var rInUse = from d in db.Room
                         where d.Gebouw == gebouw
                         where d.Verdiep == verdiep
                         where d.Nummer == nummer
                         select d;

            if (rInUse.Any())
            {
                throw new ArgumentException("Dit lokaal bestaat reeds.");
            }    

            Room room = db.Room.Find(lokaalId);
            Room newRoom = new Room { Gebouw = gebouw.Trim(), Verdiep = verdiep, Nummer = nummer, Type = type, Capaciteit = capaciteit, Middelen = middelen };

            room.Gebouw = newRoom.Gebouw;
            room.Verdiep = newRoom.Verdiep;
            room.Nummer = newRoom.Nummer;
            room.Type = newRoom.Type;
            room.Capaciteit = newRoom.Capaciteit;
            room.Middelen = newRoom.Middelen;

            db.SaveChanges();
        }
    }
}
