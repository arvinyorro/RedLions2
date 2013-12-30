namespace RedLions.Presentation.Web.Components
{
    using System.Collections.Generic;
    using System.Linq;
    using DTO = RedLions.Application.DTO;

    public static class ModelExtensions
    {
        public static IEnumerable<Models.Member> ToModels(this IEnumerable<DTO.Member> memberDTOs)
        {
            return memberDTOs.Select(x => new Models.Member(x));
        }
    }
}