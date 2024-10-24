namespace DataImportApp.Models
{
    public class ImportLog 
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string User { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
