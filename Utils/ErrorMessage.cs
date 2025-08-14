namespace Utils
{
    public class ErrorMessage
    {
        public ErrorMessage(string message)
        {
            Message = message;
        }

        public ErrorMessage()
        {
        }

        public string Message { get; set; } = "";
    }
}
