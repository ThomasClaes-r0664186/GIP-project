using Gip.Models;
using Gip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Gip.Models.ViewModels;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace Gip.Services
{
    public class DataService : IDataService
    {
        private gipDatabaseContext db;
        private IVakService service;
        private ILectorService lectorService;
        private IAdministrationService adminService;
        public DataService(gipDatabaseContext db,IVakService service,ILectorService lectorService,IAdministrationService adminService)
        {
            this.adminService = adminService;
            this.lectorService = lectorService;
            this.service = service;
            this.db = db;
        }

        public Tuple<IQueryable<Course>, int,int> GetVakken(int start, int length, string searchValue, string sortColumnName, string sortDirection)
        {
            var qry = from d in db.Course select d;
            int recordsTotal = qry.Count();
            int filtered = recordsTotal;
            if (!string.IsNullOrEmpty(searchValue))
            {

                qry = from d in db.Course
                    where d.Titel.ToLower().Contains(searchValue.ToLower()) ||
                          d.Vakcode.Trim().ToLower().Contains(searchValue.ToLower())
                    select d;
                filtered = qry.Count();
            }
            qry = qry.OrderBy(sortColumnName + " "+ sortDirection);
            qry = qry.Skip(start).Take(length);
            return Tuple.Create(qry,recordsTotal,filtered);
        }

        public List<VakViewModel> GetVakkenStudent(ApplicationUser user)
        {
            return service.GetVakkenStudent(user).ToList();
        }
        
        public List<VakViewModel> GetVakkenAllLectAdm()
        {
            return service.GetVakkenLectAdm().ToList();
        }

        public Tuple<IQueryable<LokaalViewModel>,int,int> GetLokalen(int start, int length,string searchValue,string sortColumnName, string sortDirection)
        {
            IQueryable<LokaalViewModel> qry;
            qry = from d in db.Room
                select new LokaalViewModel()
                {
                    Capaciteit = d.Capaciteit,
                    Gebouw = d.Gebouw,
                    Id = d.Id,
                    Middelen = d.Middelen,
                    Nummer = d.Nummer,
                    Type = d.Type,
                    Verdiep = d.Verdiep,
                    Lokaal = d.Gebouw.Trim() + d.Verdiep + d.Nummer.Trim()
                };
            int recordsTotal = qry.Count();
            int filtered = recordsTotal;
            if (!string.IsNullOrEmpty(searchValue))
            {
                qry = from d in db.Room
                    where d.Middelen.ToLower().Contains(searchValue.ToLower()) ||
                          d.Type.ToLower().Contains(searchValue.ToLower()) ||
                          (d.Gebouw.Trim().ToLower() + d.Verdiep + d.Nummer.Trim().ToLower()).Contains(searchValue.ToLower())
                    select new LokaalViewModel()
                    {
                        Capaciteit = d.Capaciteit,
                        Gebouw = d.Gebouw,
                        Id = d.Id,
                        Middelen = d.Middelen,
                        Nummer = d.Nummer,
                        Type = d.Type,
                        Verdiep = d.Verdiep,
                        Lokaal = d.Gebouw.Trim() + d.Verdiep + d.Nummer.Trim()
                    };
                filtered = qry.Count();
            }

            if (!string.IsNullOrEmpty(sortColumnName))
            {
                qry = qry.OrderBy(sortColumnName + " "+ sortDirection);    
            }
            
            qry = qry.Skip(start).Take(length);
            return Tuple.Create(qry,recordsTotal,filtered);
        }
        public IQueryable<LokaalViewModel> GetLokalenAll()
        {
            IQueryable<LokaalViewModel> qry;
                qry = from d in db.Room
                    select new LokaalViewModel()
                    {
                        Capaciteit = d.Capaciteit,
                        Gebouw = d.Gebouw,
                        Id = d.Id,
                        Middelen = d.Middelen,
                        Nummer = d.Nummer,
                        Type = d.Type,
                        Verdiep = d.Verdiep,
                        Lokaal = d.Gebouw.Trim() + d.Verdiep + d.Nummer.Trim()
                    };
                return qry;
        }
        public Tuple<IQueryable<FieldOfStudyViewModel>,int,int> GetFieldOfStudies(int start, int length,string searchValue,string sortColumnName, string sortDirection)
        {
            var qry = from d in db.FieldOfStudy select new FieldOfStudyViewModel()
            {
                Id = d.Id,
                RichtingCode = d.RichtingCode,
                RichtingTitel = d.RichtingTitel,
                Type = d.Type,
                RichtingStudiepunten = d.RichtingStudiepunten
            };
            int recordsTotal = qry.Count();
            int filtered = recordsTotal;
            if (!string.IsNullOrEmpty(searchValue))
            {
                qry = from d in db.FieldOfStudy
                    where d.RichtingCode.ToLower().Contains(searchValue.ToLower()) ||
                          d.Type.Trim().ToLower().Contains(searchValue.ToLower()) ||
                          d.RichtingTitel.ToLower().Trim().Contains(searchValue.ToLower())
                    select new FieldOfStudyViewModel()
                    {
                        Id = d.Id,
                        RichtingCode = d.RichtingCode,
                        RichtingTitel = d.RichtingTitel,
                        Type = d.Type,
                        RichtingStudiepunten = d.RichtingStudiepunten
                    };
                filtered = qry.Count();
            }
            if (!string.IsNullOrEmpty(sortColumnName))
            {
                qry = qry.OrderBy(sortColumnName + " "+ sortDirection);    
            }
            qry = qry.Skip(start).Take(length);
            return Tuple.Create(qry,recordsTotal,filtered);
        }

        public IQueryable<FieldOfStudyViewModel> GetFieldOfStudies()
        {
            return from d in db.FieldOfStudy
                select new FieldOfStudyViewModel()
                {
                    Id = d.Id,
                    RichtingCode = d.RichtingCode,
                    RichtingTitel = d.RichtingTitel,
                    Type = d.Type,
                    RichtingStudiepunten = d.RichtingStudiepunten
                };
        }
        public Tuple<IQueryable<ApplicationUser>,int,int> GetUsers(int start, int length,string searchValue,string sortColumnName, string sortDirection)
        {
            var qry = adminService.GetUsers().AsQueryable();
            int recordsTotal = qry.Count();
            int filtered = recordsTotal;
            if (!string.IsNullOrEmpty(searchValue))
            {
                qry = adminService.GetUsers()
                    .Where(u => u.UserName.Contains(searchValue) || u.VoorNaam.Contains(searchValue) || u.Naam.Contains(searchValue));
                filtered = qry.Count();
            }
            if (!string.IsNullOrEmpty(sortColumnName))
            {
                qry = qry.OrderBy(sortColumnName + " "+ sortDirection);
            }
            qry = qry.Skip(start).Take(length);
            return Tuple.Create(qry,recordsTotal,filtered);
        }

        public IQueryable<ApplicationUser> GetUsers()
        {
            return adminService.GetUsers();
        }
        public Tuple<IQueryable<StudentRequestsViewModel>,int,int> GetAanvragen(int start, int length,string searchValue,string sortColumnName, string sortDirection)
        {
            var qry = lectorService.GetStudentRequests().AsQueryable();
            int recordsTotal = qry.Count();
            int filtered = recordsTotal;
            if (!string.IsNullOrEmpty(searchValue))
            {
                qry = GetStudentRequestsWithSearch(searchValue).AsQueryable();
                filtered = qry.Count();
            }
            if (!string.IsNullOrEmpty(sortColumnName))
            {
                qry = qry.OrderBy(sortColumnName + " "+ sortDirection);    
            }
            qry = qry.Skip(start).Take(length);
            return Tuple.Create(qry,recordsTotal,filtered);
        }

        public IQueryable<StudentRequestsViewModel> GetAanvragen()
        {
            return lectorService.GetStudentRequests().AsQueryable();
        }
        public List<StudentRequestsViewModel> GetStudentRequestsWithSearch(string searchValue)
        {
            List<StudentRequestsViewModel> studentRequests = new List<StudentRequestsViewModel>();

            var vakL = from course in db.Course
                orderby course.Vakcode
                select course;

            var cuL = from cu in db.CourseUser
                join user in db.Users on cu.ApplicationUserId equals user.Id
                join vak in db.Course on cu.CourseId equals vak.Id
                orderby user.UserName
                where cu.GoedGekeurd == false
                && (
                    cu.Courses.Titel.ToLower().Contains(searchValue.ToLower()) ||
                    cu.Courses.Vakcode.ToLower().Contains(searchValue.ToLower()) ||
                    cu.ApplicationUsers.Naam.ToLower().Contains(searchValue.ToLower()) ||
                    cu.ApplicationUsers.VoorNaam.ToLower().Contains(searchValue.ToLower())
                )
                select new { cuId = cu.Id, cId = vak.Id, titel = vak.Titel, vakCode = vak.Vakcode, RNum = user.UserName, naam = user.Naam, voorNaam = user.VoorNaam };

            foreach (var vakI in vakL)
            {
                //StudentRequestsViewModel studReq = new StudentRequestsViewModel { cuId = -1, Titel = vakI.Titel, VakCode = vakI.Vakcode };
                //studentRequests.Add(studReq);

                var cuL2 = cuL.Where(c => c.cId.Equals(vakI.Id));

                foreach (var res in cuL2)
                {
                    StudentRequestsViewModel studReq = new StudentRequestsViewModel
                    {
                        RNum = res.RNum, cuId = res.cuId, VakCode = res.vakCode, Titel = res.titel, Naam = res.naam,
                        VoorNaam = res.voorNaam
                    };
                    studentRequests.Add(studReq);
                }
            }

            return studentRequests;
        }
    }
}