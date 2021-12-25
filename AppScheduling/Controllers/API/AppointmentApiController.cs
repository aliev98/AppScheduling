using AppScheduling.Models.ViewModels;
using AppScheduling.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AppScheduling.Controllers.API
{
    [Route("api/Appointment")]
    [ApiController]
    public class AppointmentApiController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string loginUserId;
        private readonly string role;

        public AppointmentApiController(IAppointmentService appointmentService, IHttpContextAccessor httpContextAccessor)
        {
            _appointmentService = appointmentService;
            _httpContextAccessor = httpContextAccessor;
            loginUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            role = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }

        [HttpPost]
        [Route("SaveCalendarData")]
        public IActionResult SaveCalendarData(AppointmentVM data)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();

            try
            {
                commonResponse.Status = _appointmentService.AddUpdate(data).Result;

                if (commonResponse.Status == 1)
                {
                    commonResponse.Message = "Appointment updated";
                }
                if (commonResponse.Status == 2)
                {
                    commonResponse.Message = "Appointment added";
                }
            }

            catch (Exception e)
            {
                commonResponse.Message = e.Message;
                commonResponse.Status = 0;
            }

            return Ok(commonResponse);
        }


        [HttpGet]
        [Route("GetCalendarData")]
        public IActionResult GetCalendarData(string doctorId)
        { 
            CommonResponse<List<AppointmentVM>> commonResponse = new CommonResponse<List<AppointmentVM>>();
            try
            {
                if(role == "Patient")
                {
                    commonResponse.dataenum = _appointmentService.PatientsEventsById(loginUserId);
                    commonResponse.Status = 1;
                }
                else if(role == "Doctor")
                {
                    commonResponse.dataenum = _appointmentService.DoctorsEventsById(loginUserId);
                    commonResponse.Status = 1;
                }
                else
                {
                    commonResponse.dataenum = _appointmentService.DoctorsEventsById(doctorId);
                    commonResponse.Status = 1;
                }
            }
            catch (Exception e)
            {
                commonResponse.Message = e.Message;
                commonResponse.Status = 0;
            }

            return Ok(commonResponse);
        }

        [HttpGet]
        [Route("GetCalendarDataById/{id}")]
        public IActionResult GetCalendarDataById(int id)
        {
            CommonResponse<AppointmentVM> commonResponse = new CommonResponse<AppointmentVM>();

            try
            {
                commonResponse.dataenum = _appointmentService.GetById(id);
                commonResponse.Status = 1;
            }
            catch (Exception e)
            {
                commonResponse.Message = e.Message;
                commonResponse.Status = 0;
            }

            return Ok(commonResponse);
        }

        [HttpGet]
        [Route("DeleteAppointment/{id}")]
        public async Task <IActionResult> DeleteAppointment(int id)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();

            try
            {
                commonResponse.Status = await _appointmentService.Delete(id);
                commonResponse.Message = commonResponse.Status == 1 ? "Appointment deleted" : "Something went wrong";
            }
            catch (Exception e)
            {
                commonResponse.Message = e.Message;
                commonResponse.Status = 0;
            }

            return Ok(commonResponse);
        }

        [HttpGet]
        [Route("ConfirmEvent/{id}")]
        public IActionResult ConfirmEvent(int id)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();

            try
            {
                var result =   _appointmentService.ConfirmEvent(id).Result;
                if(result > 0)
                {
                    commonResponse.Status = 1;
                    commonResponse.Message = "Appointment confirmed";
                }

                else
                {
                    commonResponse.Status = 0;
                    commonResponse.Message = "Something went wrong";
                }
            }
            catch (Exception e)
            {
                commonResponse.Message = e.Message;
                commonResponse.Status = 0;
            }

            return Ok(commonResponse);
        }

    }
}
