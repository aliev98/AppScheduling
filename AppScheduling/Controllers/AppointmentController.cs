using AppScheduling.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppScheduling.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public IActionResult Index()
        {
            ViewBag.doctors = _appointmentService.GetDoctorList();
            ViewBag.patients = _appointmentService.GetPatientList();

            return View();
        }

        public IActionResult removeAppointment(int id)
        {
            _appointmentService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
