﻿namespace FileImportExportAPI.Models
{
    public class User:User_csv
    {
        public int Id { get; set; }
        
    }
    public class User_csv
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
