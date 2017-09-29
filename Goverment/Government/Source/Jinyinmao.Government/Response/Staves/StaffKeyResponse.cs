using Jinyinmao.Government.Models;

namespace Jinyinmao.Government.Response.Staves
{
    public class StaffWithKeyResponse : StaffResponse
    {
        public string Key { get; set; }
    }

    internal static partial class StaffEx
    {
        internal static StaffResponse ToStaffWithKeyResponse(this Staff staff)
        {
            return new StaffWithKeyResponse
            {
                Id = staff.Id,
                Cellphone = staff.Cellphone,
                Email = staff.Email,
                Key = staff.Key,
                Name = staff.Name,
                CreatedBy = staff.CreatedBy,
                CreatedTime = staff.CreatedTime,
                LastModifiedBy = staff.LastModifiedBy,
                LastModifiedTime = staff.LastModifiedTime
            };
        }
    }
}