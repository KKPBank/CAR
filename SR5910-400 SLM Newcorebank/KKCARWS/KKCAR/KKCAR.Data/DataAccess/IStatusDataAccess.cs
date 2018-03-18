using KKCAR.Entity;

///<summary>
/// Class Name : IStatusDataAccess
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.Data.DataAccess
{
    public interface IStatusDataAccess
    {
        CarStatusEntity GetCarID(CarStatusEntity status);
        bool SaveCarStatus(CarStatusEntity status);
    }
}
