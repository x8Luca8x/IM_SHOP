using System.Text.Json;

namespace IM_API
{
    public class IMJsonNaimingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name.ToLower();
        }
    }
}
