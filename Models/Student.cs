// File: Models/Student.cs

using System.ComponentModel.DataAnnotations;

namespace DemoMvc551.Models
{
    public class Student
    {
       [Key]
        public string ? StudentCode { get; set; }
        public string ? FullName { get; set; } 
       
    }
}
