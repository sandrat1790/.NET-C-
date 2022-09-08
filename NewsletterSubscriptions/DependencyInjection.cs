using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sabio.Data;
using Sabio.Models.Domain;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Api.Controllers;
using Sabio.Web.Api.StartUp.DependencyInjection;
using Sabio.Web.Core.Services;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sabio.Web.StartUp
{
    public class DependencyInjection
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is IConfigurationRoot)
            {
                services.AddSingleton<IConfigurationRoot>(configuration as IConfigurationRoot);   // IConfigurationRoot
            }

            services.AddSingleton<IConfiguration>(configuration);   // IConfiguration explicitly

            string connString = configuration.GetConnectionString("Default");
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.2
            // The are a number of differe Add* methods you can use. Please verify which one you
            // should be using services.AddScoped<IMyDependency, MyDependency>();

            // services.AddTransient<IOperationTransient, Operation>();

            // services.AddScoped<IOperationScoped, Operation>();

            // services.AddSingleton<IOperationSingleton, Operation>();
            services.AddSingleton<IAuthenticationService<int>, WebAuthenticationService>();

            services.AddSingleton<Sabio.Data.Providers.IDataProvider, SqlDataProvider>(delegate (IServiceProvider provider)
            {
                return new SqlDataProvider(connString);
            }
            );
            services.AddSingleton<IAppFileService, AppFileService>();
            services.AddSingleton<IBlogsService, BlogsService>();
            services.AddSingleton<ICommentService, CommentService>();
            services.AddSingleton<IContactsService, ContactsService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IEmployeeService, EmployeeService>();
            services.AddSingleton<IFAQCategoryService, FAQCategoryService>();
            services.AddSingleton<IFAQsService, FAQsService>();
            services.AddSingleton<IFilesService, FilesService>();
            services.AddSingleton<IFlowStripeService, FlowStripeService>();
            services.AddSingleton<IForgotPasswordService, ForgotPasswordService>();
            services.AddSingleton<IGoogleAnalyticsService, GoogleAnalyticsService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IIdentityProvider<int>, WebAuthenticationService>();
            services.AddSingleton<IJobAttendanceService, JobAttendanceService>();
            services.AddSingleton<IJobProposalService, JobProposalService>();
            services.AddSingleton<IJobService, JobService>();
            services.AddSingleton<ILicensesRelatedService, LicensesRelatedService>();
            services.AddSingleton<ILicensesService, LicensesService>();
            services.AddSingleton<ILicenseTypesService, LicenseTypesService>();
            services.AddSingleton<ILocationService, LocationService>();
            services.AddSingleton<ILookUpService, LookUpService>();
            services.AddSingleton<IMessageService, MessageService>();
            services.AddSingleton<INewsletterSubscriptionService, NewsletterSubscriptionService>();
            services.AddSingleton<INewsletterTemplatesService, NewsletterTemplatesService>();
            services.AddSingleton<IOrganizationService, OrganizationService>();
            services.AddSingleton<IRatingService, RatingService>();
            services.AddHostedService<RepeatingService>();
            services.AddSingleton<IShareStoryService, ShareStoryService>();
            services.AddSingleton<ISiteReferencesService, SiteReferencesService>();
            services.AddSingleton<ISubcontractorsService, SubcontractorsService>();
            services.AddSingleton<ISurveyAnswerService, SurveyAnswerService>();
            services.AddSingleton<ISurveyInstanceService, SurveyInstanceService>();        
            services.AddSingleton<ISurveyQuestionAnswerOptionsService, SurveyQuestionAnswerOptionsService>();
            services.AddSingleton<ISurveyQuestionService, SurveyQuestionService>();
            services.AddSingleton<ISurveyService, SurveyService>();
            services.AddSingleton<ITrainingService, TrainingService>();
            services.AddSingleton<IUserMapperService, UserMapperService>();
            services.AddSingleton<IUserProfilesService, UserProfileService>();
            services.AddSingleton<IUserService, UserService>();

            services.AddSingleton(typeof(IUserTrackerService<>), typeof(UserTrackerService<>));

            GetAllEntities().ForEach(tt =>
            {
                IConfigureDependencyInjection idi = Activator.CreateInstance(tt) as IConfigureDependencyInjection;

                //This will not error by way of being null. BUT if the code within the method does
                // then we would rather have the error loadly on startup then worry about debuging the issues as it runs
                idi.ConfigureServices(services, configuration);
            });
        }

        public static List<Type> GetAllEntities()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                 .Where(x => typeof(IConfigureDependencyInjection).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                 .ToList();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }
    }
}