using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppScheduling.Helper
{
    public static class Helper
    {
        public static string Admin = "Admin";
        public static string Patient = "Patient";
        public static string Doctor = "Doctor";

        public static List<SelectListItem> GetRolesForDropDown(bool isAdmin)
        {
            if (isAdmin)
            {
                return new List<SelectListItem>
                {
                    new SelectListItem{Value="Admin", Text="Admin"}
                };
            }

            return new List<SelectListItem>
            {
                new SelectListItem{ Value = Helper.Patient, Text = Helper.Patient },
                new SelectListItem{ Value = Helper.Doctor, Text = Helper.Doctor }
            };
        }

        public static List<SelectListItem> GetTimeDropDown()
        {
            int minute = 60;
            List<SelectListItem> duration = new List<SelectListItem>();
            for (int i = 1; i <= 12; i++)
            {
                duration.Add(new SelectListItem { Value = minute.ToString(), Text = i + " h" });
                minute += 30;
                duration.Add(new SelectListItem { Value = minute.ToString(), Text = i + " h 30 min" });
                minute += 30;
            }
            return duration;
        }
    }
}
