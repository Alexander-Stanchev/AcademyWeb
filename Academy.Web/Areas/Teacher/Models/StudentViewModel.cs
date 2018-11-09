using Academy.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Academy.Web.Areas.Teacher.Models
{
    public class StudentViewModel
    {
        public StudentViewModel(User student)
        {
            Id = student.Id;
            UserName = student.UserName;
        }

        [Required]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }


    }
}
