using Remake.Models;

namespace Remake.Security.Authorization
{
    public class Roles  /// sonradan rol eklemek istersek bu class yazılıp implemente edilecek // Listeye Db'den gelen veri ile doldurulacak
    {
        kesifdbContext DB = new kesifdbContext();
        public List<Roller> roles= new List<Roller>();
        public List<Roller> GetRoles()
        {
            var Roles = DB.Rollers.ToList();
            foreach (var item in Roles)
            {
                roles.Add(item);
            }
            return roles;
        }
    }
}
