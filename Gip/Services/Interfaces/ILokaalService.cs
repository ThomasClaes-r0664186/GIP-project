using Gip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gip.Services.Interfaces
{
    public interface ILokaalService
    {
        void AddLokaal(string gebouw, int verdiep, string nummer, string type, int capaciteit, string middelen);
        void DeleteLokaal(int lokaalId);
        void EditLokaal(int lokaalId, string gebouw, int verdiep, string nummer, string type, int capaciteit, string middelen);
    }
}
