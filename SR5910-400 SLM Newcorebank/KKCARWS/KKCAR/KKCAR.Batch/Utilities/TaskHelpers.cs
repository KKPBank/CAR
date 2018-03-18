using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

///<summary>
/// Class Name : TaskHelpers
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.Batch.Utilities
{
    public class TaskHelpers
    {
        public static async Task HttpPostAysnc(string requestUri, dynamic jsonObject)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
                var resp = await client.PostAsync(requestUri, content);
            }
        }
    }
}
