namespace Framework.Model
{
    public static class MessageResponse
    {
        private static Dictionary<string, (bool, string, string)> messageResponses = new Dictionary<string, (bool, string, string)>
        {
            { "Success", (true, "200", "Success" ) },
            { "BadRequest", (false, "400", "Bad Request") },
            { "Unauthorized", (false, "401", "Unauthorized") },
            { "Duplicate", (false, "409", "Duplicate Exist" ) },
            { "Forbidden", (false, "403","Forbidden Request") },
            { "NotFound", (false, "404", "Not Found" ) },
            { "ServerError", (false, "500", "Internal Server Error" ) },
            { "ServiceUnavailable", (false, "503","Service Unavailable") }
        };

        public static Dictionary<string, (bool, string, string)> Responses
        {
            get { return messageResponses; }
        }
    }
}