using Quartz;
using Shared.Quartz.Models;

namespace Shared.Quartz
{
    public static class UseServiceQuartz
    {
        public static IServiceCollection AddServiceQuartz(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddQuartz(q =>
            {
                var tasks = configuration
                    .GetSection("ScheduledTasks")
                    .Get<List<ScheduledTaskConfig>>()!;

                foreach (var task in tasks)
                {
                    var jobKey = new JobKey($"job-{task.Name}");

                    q.AddJob<ScheduledJob>(opts => opts
                        .WithIdentity(jobKey)
                        .UsingJobData("task", task.Name));

                    q.AddTrigger(opts => opts
                        .ForJob(jobKey)
                        .WithIdentity($"trigger-{task.Name}")
                        .WithCronSchedule(task.Cron));
                }
            });

            services.AddQuartzHostedService(opt =>
            {
                opt.WaitForJobsToComplete = true;
            });

            return services;
        }
    }
}
