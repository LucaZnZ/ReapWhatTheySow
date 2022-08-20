namespace ZnZUtil
{
    public static class FloatUtil
    {
        public static float SmallParse(string parse)
        {
            float one = float.Parse(parse.Replace('.', ','));
            float two = float.Parse(parse.Replace(',', '.'));
            return one > two ? two : one;
        }

        public static bool TrySmallParse(string parse, out float result)
        {
            try
            {
                result = SmallParse(parse);
                return true;
            }
            catch
            {
                result = 0;
                return false;
            }
        }
    }
}