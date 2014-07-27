namespace RedLions.Application
{
    using System.Linq;
    using RedLions.Business;
    using AutoMapper;

    /// <summary>
    /// This class is responsible for all application layer AutoMapper
    /// configurations. This class must be used everytime any services
    /// are used.
    /// </summary>
    /// <remarks>
    /// 
    /// Automapper is used to automate the mapping between domain entities
    /// and DTO objects.
    /// 
    /// If the services are to be used in an MVC, use this class in the 
    /// Global.asax upon start up.
    /// 
    /// If the services are to be used in an unit testing, use this class
    /// in the setup or initalization.
    /// 
    /// For more info about automapping, http://automapper.org/ and 
    /// https://github.com/AutoMapper/AutoMapper/wiki/_pages
    /// 
    /// </remarks>
    public class AutoMapperConfig
    {
        /// <summary>
        /// Configures the AutoMapper mappings.
        /// </summary>
        /// <remarks>
        /// dest = destination.
        /// opt = option
        /// src = source
        /// </remarks>
        public static void Register()
        {
            Mapper.CreateMap<Business.InquiryChatSession, DTO.InquiryChatSession>();
            Mapper.CreateMap<Business.InquiryChatMessage, DTO.InquiryChatMessage>();
            Mapper.CreateMap<Business.Subscription, DTO.Subscription>();
            Mapper.CreateMap<Business.Announcement, DTO.Announcement>();
            Mapper.CreateMap<Business.User, DTO.User>();
            Mapper.CreateMap<Business.Payment, DTO.Payment>()
                .ForMember(dest => dest.ReferrerName, opt => opt.MapFrom(src => string.Format("{0} {1}", src.Referrer.FirstName, src.Referrer.LastName))); ;
        }
    }
}
