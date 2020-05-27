using Gip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gip.Services.Interfaces
{
    public interface IFieldOfStudyService
    {
        IQueryable<FieldOfStudy> GetAllFieldOfStudy();
        void AddRichting(string code, string titel, string type, int studiepunten);
        void DeleteRichting(int richtindId);
        void EditRichting(int richtindId, string richtingCode, string richtingTitel, string type, int richtingStudiepunten);
    }
}
