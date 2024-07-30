namespace LogisticsSystem.API.Service
{
    public class ApiResponse
    {
        public ApiResponse(string message, bool status = false)
        {
            this.Message = message;
            this.Status = status;
        }

        public ApiResponse(bool status, object result)
        {
            this.Status = status;
            this.Result = result;
        }
        public ApiResponse(bool status, object result,int count)
        {
            this.Status = status;
            this.Result = result;
            this.MaxPageCount = count;
        }

        public string Message { get; set; }
        public bool Status { get; set; }
        public object Result { get; set; }
        public int? MaxPageCount {  get; set; } 
    }
}
