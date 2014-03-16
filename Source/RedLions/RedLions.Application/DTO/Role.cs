namespace RedLions.Application.DTO
{
    using System;
    using System.Collections.Generic;
    using RedLions.Business;

    public class Role
    {
        public string Title { get; set; }
    }

    internal static class RoleAssembler
    {
        internal static IEnumerable<DTO.Role> ToDTOList()
        {
            var roleDTOs = new List<DTO.Role>();
            foreach(var name in Enum.GetNames(typeof(Business.Role)))
            {
                roleDTOs.Add(new DTO.Role() { Title = name } );
            }

            return roleDTOs;
        }

        internal static IEnumerable<DTO.Role> ToDTOList(IEnumerable<Business.Role> roles)
        {
            var roleDTOs = new List<DTO.Role>();
            foreach (var role in roles)
            {
                roleDTOs.Add(new DTO.Role() { Title = Enum.GetName(typeof(Business.Role), role) });
            }

            return roleDTOs;
        }
    }
}
