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
        Tuple<IQueryable<FieldOfStudyViewModel>, int, int> GetFieldOfStudies(int start, int length, string searchValue, string sortColumnName, string sortDirection);
        IQueryable<FieldOfStudyViewModel> GetFieldOfStudies();
        Tuple<IQueryable<StudentRequestsViewModel>, int, int> GetAanvragen(int start, int length, string searchValue, string sortColumnName, string sortDirection);
        IQueryable<StudentRequestsViewModel> GetAanvragen();
        Tuple<IQueryable<ApplicationUser>, int, int> GetUsers(int start, int length, string searchValue, string sortColumnName, string sortDirection);
        IQueryable<ApplicationUser> GetUsers();
        List<StudentRequestsViewModel> GetStudentRequestsWithSearch(string searchValue);
    }
}