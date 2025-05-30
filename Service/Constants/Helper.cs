namespace Service.Constants
{
    public static class Helper
    {
        public static string Interpolate(string constant, object[] placeHolders)
        {
            for (int i = 0; i < placeHolders.Length; i++)
            {
                constant = constant.Replace(i.ToString(), placeHolders[i].ToString());
            }
            return constant;
        }
    }
}