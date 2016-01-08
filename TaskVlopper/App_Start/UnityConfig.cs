using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using TaskVlopper.Models;
using Microsoft.Owin.Security;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TaskVlopper.Identity;
using TaskVlopper.Base.Logic;
using TaskVlopper.Logic;
using TaskVlopper.Base.Repository;
using TaskVlopper.Repository;
using TaskVlopper.Base;
using TaskVlopper.Repository.Base;
using TaskVlopper.Base.Model;
using TaskVlopper.Base.Repository.Serialize;
using TaskVlopper.Repository.Base.Serialize;

namespace TaskVlopper.ServiceLocator
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }


        private static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<ApplicationDbContext>();
            container.RegisterType<ApplicationSignInManager>();
            container.RegisterType<ApplicationUserManager>();
            container.RegisterType<EmailService>();

            container.RegisterType<IAuthenticationManager>(
                new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));

            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(
                new InjectionConstructor(typeof(ApplicationDbContext)));

            //Logic
            container.RegisterType<ITestLogic, TestLogic>();
            container.RegisterType<IProjectLogic, ProjectLogic>();
            container.RegisterType<ITaskLogic, TaskLogic>();
            container.RegisterType<IWorklogLogic, WorklogLogic>();
            container.RegisterType<IMeetingLogic, MeetingLogic>();
            //Repository
            container.RegisterType<ITestRepository, TestRepository>();
            container.RegisterType<IProjectRepository, ProjectRepository>();
            container.RegisterType<ITaskRepository, TaskRepository>();
            container.RegisterType<IWorklogRepository, WorklogRepository>();
            container.RegisterType<IUserTaskAssignmentRepository, UserTaskAssignmentRepository>();
            container.RegisterType<IUserProjectAssignmentRepository, UserProjectAssignmentRepository>();
            container.RegisterType<IMeetingRepository, MeetingRepository>();
            container.RegisterType<IMeetingParticipantsRepository, MeetingParticipantsRepository>();
            //GenericSerializer
            container.RegisterType<IProjectSerialize, ProjectSerialize>();
            container.RegisterType<ITaskSerialize, TaskSerialize>();
            container.RegisterType<IWorklogSerialize, WorklogSerialize>();
            container.RegisterType<IMeetingSerialize, MeetingSerialize>();
            container.RegisterType<IMeetingParticipantsSerialize, MeetingParticipantsSerialize>();

        }
    }
}
