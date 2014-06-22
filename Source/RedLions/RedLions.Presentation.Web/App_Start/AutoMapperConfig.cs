namespace RedLions.Presentation.Web
{
    using RedLions.Presentation.Web;
    using DTO = RedLions.Application.DTO;
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
            Mapper.CreateMap<DTO.InquiryChatMessage, Models.InquiryChatMessage>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SenderUsername))
                .ReverseMap()
                .ForMember(dest => dest.SenderUsername, opt => opt.MapFrom(src => src.Name));

            Mapper.CreateMap<DTO.InquiryChatSession, Models.InquiryChatSession>()
                .ForMember(dest => dest.ThumbMessage, opt => opt.MapFrom(src => src.LastMessage))
                .ReverseMap();

            Mapper.CreateMap<DTO.Subscription, Models.Subscription>()
                .ReverseMap();
        }
    }
}