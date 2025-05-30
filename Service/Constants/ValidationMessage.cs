namespace Service.Constants
{
    public static class ValidationMessage
    {
        #region Token

        public const string InvalidToken = "Invalid token";
        public const string InvalidRefreshToken = "Invalid refresh token";
        public const string TokenRequired = "Token is required";
        public const string ExpiredToken = "Token has expired";

        #endregion

        #region User

        public const string IncorrectPassword = "Incorrect password";
        public const string SamePasswordNotAllowed = "Using the same password is not allowed";

        #endregion

    }
}
