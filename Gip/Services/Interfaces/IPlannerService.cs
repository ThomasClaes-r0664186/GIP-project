using Gip.Models;
using Gip.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gip.Services.Interfaces
{
    public interface IPlannerService
    {
        List<Planner> GetPlanningStudent(int weekToUse, ApplicationUser user);
        List<Planner> GetPlanningLectAdmin(int weekToUse);
        //add planning throw error wanneer het dubbele boeking is etc.
        void AddPlanning(ApplicationUser user, DateTime datum, DateTime tijd, double _duratie, int lokaalId, int vakid, string? lessenlijst, bool? checkbox, int lokaal2Id);
        List<Planner> GetLokalen();
        void DeletePlanning(int cmId);
        void EditPlanning(int cmId, int newVakcode, string newDatum, string newStartMoment, double newDuratie, int newLokaalid, string newLessenlijst, ApplicationUser user);
        Planner GetTopic(int cmId, bool lector, ApplicationUser user);
        List<Planner> GetCourseMoments(int vakcode);
        List<EditStudInCmViewModel> GetStudsInCm(int cmId);
        void EditStudsInCm(List<EditStudInCmViewModel> model, int cmId);
        CourseUsersViewModel GetCourseUsers(int vakcode);
        //gebruik maken van EditStudsInCm?
        List<EditStudInCmViewModel> GetStudsInEachCm(int vakcode);
        Task<string> AddStudsToEachCm(List<EditStudInCmViewModel> model, int vakcode);
    }
}
