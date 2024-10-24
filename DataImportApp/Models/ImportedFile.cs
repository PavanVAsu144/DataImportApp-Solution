namespace DataImportApp.Models
{
    public class ImportedFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public DateTime ImportedOn { get; set; }
        public string UploadedBy { get; set; }

        // Adding the IntegerColumn as per your requirement
        public int? IntegerColumn { get; set; } // Nullable to handle cases where it's missing or invalid
    }
}
