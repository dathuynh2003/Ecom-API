namespace Ecom.Shared.Common
{
    public class ApiResponse<T> where T : class
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public static ApiResponse<T> Fail(string message) =>
            new()
            {
                Data = null,
                Success = false,
                Message = message
            };

        public static ApiResponse<T> Ok(T? data, string message) =>
            new()
            {
                Success = true,
                Message = message,
                Data = data
            };
    }
}
