using AutoMapper;
using Smart_City.Dtos;
using Smart_City.Models;

namespace Smart_City.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Users
            CreateMap<User, UserDto>();
            CreateMap<User, UserBriefDto>();
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "Citizen"));

            // Complaints
            CreateMap<Complaint, ComplaintDto>();
            CreateMap<ComplaintCreateDto, Complaint>();

            // Suggestions
            CreateMap<Suggestion, SuggestionDto>();
            CreateMap<SuggestionCreateDto, Suggestion>();
            CreateMap<SuggestionUpdateDto, Suggestion>();

            // Bills
            CreateMap<Bill, BillDto>();
            CreateMap<BillCreateDto, Bill>();

            // Utility Issues
            CreateMap<UtilityIssue, UtilityIssueDto>();
            CreateMap<UtilityIssueCreateDto, UtilityIssue>();
            CreateMap<UtilityIssueUpdateDto, UtilityIssue>();

            // Notifications
            CreateMap<Notification, NotificationDto>();
            CreateMap<NotificationCreateDto, Notification>();
        }
    }
}