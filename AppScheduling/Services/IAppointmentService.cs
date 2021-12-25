using AppScheduling.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppScheduling.Services
{
    public interface IAppointmentService
    {
        public List<DoctorVm> GetDoctorList();
        public List<PatientVm> GetPatientList();
        public Task<int> AddUpdate(AppointmentVM model);
        public List<AppointmentVM> DoctorsEventsById(string doctorId);
        public List<AppointmentVM> PatientsEventsById(string patientId);
        public AppointmentVM GetById(int id);
        public Task <int> Delete(int id);
        public Task<int> ConfirmEvent(int id);

    }
}
