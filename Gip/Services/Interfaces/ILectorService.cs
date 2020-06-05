using Gip.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gip.Services.Interfaces
{
    public interface ILectorService
    {
        List<StudentRequestsViewModel> GetStudentRequests();
        void ApproveStudent(int cuId);
        void DenyStudent(int cuId, string beschrijving);
    }
}
