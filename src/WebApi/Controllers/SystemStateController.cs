using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityLibrary.Infrastructure.State;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SystemStateService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SystemStateController : ControllerBase
    {
        private readonly ILogger<SystemStateController> _logger;

        public SystemStateController(ILogger<SystemStateController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public EasSystemState Get()
        {
            return new EasSystemState()
            {
                Errors = new List<EasStateError>(new[]
                {
                    new EasStateError() {ErrorCode = 1, Description = "TestError1"},
                    new EasStateError() {ErrorCode = 2, Description = "TestError2"},
                }),
                Warnings = new List<EasStateWarning>(new[]
                {
                    new EasStateWarning() {Description = "Warning1"}, new EasStateWarning() {Description = "Warning2"},
                }),
                Notifications = new List<EasStateNotification>(new[]
                {
                    new EasStateNotification() {Notification = "Hello", Email = "test@gmail.com"},
                    new EasStateNotification() {Notification = "World", Email = "test2@gmail.com"},
                })
            };
        }
    }
}
