using Smart_City.Managers;
using Smart_City.Repositories;

namespace Smart_City.Extensions;

public static class ApplicationServiceExtensions
{
	public static IServiceCollection AddApplicationRepositories(this IServiceCollection services)
	{
		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<IComplaintRepositry, ComplaintsRepositry>();
		services.AddScoped<ISuggestionsRepositories, SuggestionsRepositories>();
		services.AddScoped<IBillRepository, BillRepository>();
		services.AddScoped<INotificationsRepository, NotificationRepository>();
		services.AddScoped<IUtilityIssueRepository, UtilityIssueRepository>();
		return services;
	}

	public static IServiceCollection AddApplicationManagers(this IServiceCollection services)
	{
		services.AddScoped<IAuthManager, AuthManager>();
		services.AddScoped<IUserManager, UserManager>();
		services.AddScoped<IComplaintManager, ComplaintManager>();
		services.AddScoped<ISuggestionManager, SuggestionManager>();
		services.AddScoped<INotificationManager, NotificationManager>();
		services.AddScoped<IUtilityIssueManager, UtilityIssueManager>();
		return services;
	}

}
