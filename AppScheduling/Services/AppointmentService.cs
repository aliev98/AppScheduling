using AppScheduling.Models;
using AppScheduling.Models.ViewModels;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppScheduling.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailSender _emailSender;

        public AppointmentService(ApplicationDbContext db, IEmailSender emailSender)
        {
            _db = db;
            _emailSender = emailSender;
        }

        public async Task<int> AddUpdate(AppointmentVM model)
        {
            var startDate = DateTime.Parse(model.StartDate);
            var endDate = DateTime.Parse(model.StartDate).AddMinutes(Convert.ToDouble(model.Duration));
            var patient = _db.Users.FirstOrDefault(u => u.Id == model.PatientId);
            var doctor = _db.Users.FirstOrDefault(u => u.Id == model.DoctorId);


            if (model != null && model.Id > 0)
            {
                var appointment = _db.Appointments.FirstOrDefault(x => x.Id == model.Id);
                appointment.Title = model.Title;
                appointment.Description = model.Description;
                appointment.StartDate = startDate;
                appointment.EndDate = endDate;
                appointment.Duration = model.Duration;
                appointment.DoctorId = model.DoctorId;
                appointment.IsDoctorApproved = false;
                appointment.AdminId = model.AdminId;
                await _db.SaveChangesAsync();
                return 1;
            }

            else
            {
                Appointment appointment = new Appointment()
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartDate = startDate,
                    EndDate = endDate,
                    Duration = model.Duration,
                    DoctorId = model.DoctorId,
                    PatientId = model.PatientId,
                    IsDoctorApproved = false,
                    AdminId = model.AdminId
                };

                //await _emailSender.SendEmailAsync(doctor.Email, "Appointment Created",
                //    $"Your appointment with {patient.Name} is created and in pending status");

                //await _emailSender.SendEmailAsync(patient.Email, "Appointment Created",
                //   $"Your appointment with {doctor.Name} is created and in pending status");

                _db.Appointments.Add(appointment);
                await _db.SaveChangesAsync();
                return 2;
            }
        }


        public List<DoctorVm> GetDoctorList()
        {
            var doctors = (from user in _db.Users
                           join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                           join roles in _db.Roles.Where(x => x.Name == "Doctor") on userRoles.RoleId equals roles.Id

                           select new DoctorVm
                           {
                               Id = user.Id,
                               Name = user.Name
                           }).ToList();

            return doctors;
        }

        public List<PatientVm> GetPatientList()
        {
            var patients = (from user in _db.Users
                           join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                           join roles in _db.Roles.Where(x => x.Name == Helper.Helper.Patient) on userRoles.RoleId equals roles.Id

                           select new PatientVm
                           {
                               Id = user.Id,
                               Name = user.Name
                           }).ToList();

            return patients;
        }

        public AppointmentVM GetById(int id)
        {
            return _db.Appointments.Where(x => x.Id == id).Select(c => new AppointmentVM()
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Duration = c.Duration,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                IsDoctorApproved = c.IsDoctorApproved,
                PatientId = c.PatientId,
                DoctorId = c.DoctorId,
                PatientName = _db.Users.Where(x => x.Id == c.PatientId).Select(c => c.Name).FirstOrDefault(),
                DoctorName = _db.Users.Where(x => x.Id == c.DoctorId).Select(c => c.Name).FirstOrDefault()
            }).SingleOrDefault();
        }

        public List<AppointmentVM> DoctorsEventsById(string doctorId)
        {
            return _db.Appointments.Where(x => x.DoctorId == doctorId).ToList().Select(c => new AppointmentVM()
            {
                Id = c.Id,
                Description = c.Description,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Title = c.Title,
                Duration = c.Duration,
                IsDoctorApproved = c.IsDoctorApproved
            }).ToList();
        }
        public List<AppointmentVM> PatientsEventsById(string patientId)
        {
            return _db.Appointments.Where(x => x.PatientId == patientId).ToList().Select(c => new AppointmentVM()
            {
                Id = c.Id,
                Description = c.Description,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Title = c.Title,
                Duration = c.Duration,
                IsDoctorApproved = c.IsDoctorApproved
            }).ToList();
        }

        public async Task <int> Delete(int id)
        {
            var appointment = _db.Appointments.FirstOrDefault(x => x.Id == id);
            if (appointment != null)
            {
                _db.Appointments.Remove(appointment);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> ConfirmEvent(int id)
        {
            var appointment = _db.Appointments.FirstOrDefault(x => x.Id == id);
            if (appointment != null)
            {
                appointment.IsDoctorApproved = true;
                return await _db.SaveChangesAsync();
            }
            return 0;
        }
    }
}
