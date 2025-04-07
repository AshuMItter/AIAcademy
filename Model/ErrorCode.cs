namespace AIAcademy.Model
{
  public  class CodeError
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public int LineNumber { get; set; }
        public string ErrorType { get; set; }
        public string AffectedCode { get; set; }

        public string Kind { get; set; }
        public string SuggestionsByTrainer { get; set; }
        public string WeakAreasOfCSharp { get; set; }


         }
}
