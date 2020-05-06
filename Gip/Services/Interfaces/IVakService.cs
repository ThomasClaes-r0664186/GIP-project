using Gip.Models;
using Gip.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gip.Services.Interfaces
{
    public interface IVakService
    {
        List<VakViewModel> GetVakkenStudent(ApplicationUser user);
        List<VakViewModel> GetVakkenLectAdm();
        void AddVak(string vakcode, string titel, int studiepunten);
        void DeleteVak(int vakcode);
        void EditVak(int vakcodeOld, string vakcodeNew, string titel, int studiepunten);
        void Subscribe(int vakCode, ApplicationUser user);
        void UnSubscribe(int vakcode, ApplicationUser user);
    }
}
