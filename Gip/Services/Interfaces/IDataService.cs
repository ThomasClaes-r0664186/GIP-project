using Gip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gip.Models.ViewModels;

namespace Gip.Services.Interfaces
{
    public interface IDataService
    {
        Tuple<IQueryable<Course>,int,int> GetVakken(int start,int length,string searchValue, string sortColumnName, string sortDirection);
        List<VakViewModel> GetVakkenStudent(ApplicationUser user);
        List<VakViewModel> GetVakkenAllLectAdm();
        Tuple<IQueryable<LokaalViewModel>,int,int> GetLokalen(int start,int length,string searchValue, string sortColumnName, string sortDirection);
        IQueryable<LokaalViewModel> GetLokalenAll();
        Tuple<IQueryable<FieldOfStudy>, int, int> GetFieldOfStudies(int start, int length, string searchValue,
            string sortColumnName, string sortDirection);
        IQueryable<FieldOfStudy> GetFieldOfStudies();
    }
}