﻿using System;
using System.Collections.Generic;
using System.Text;
using Easy.Modules.User.Models;
using Easy.Notification;
using Microsoft.AspNetCore.Http;
using ZKEACMS.Notification.ViewModels;
using Microsoft.AspNetCore.DataProtection;

namespace ZKEACMS.Notification
{
    public class NotifyService : INotifyService
    {
        private readonly INotificationManager _notificationManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _dataProtector;
        public NotifyService(INotificationManager notificationManager, IHttpContextAccessor httpContextAccessor, IDataProtectionProvider dataProtectionProvider)
        {
            _notificationManager = notificationManager;
            _httpContextAccessor = httpContextAccessor;
            _dataProtector = dataProtectionProvider.CreateProtector("ResetPassword");
        }
        public void ResetPassword(UserEntity user)
        {
            _notificationManager.Send(new RazorEmailNotice
            {
                Subject = "重置密码",
                To = new string[] { user.Email },
                Model = new ResetPasswordViewModel
                {
                    Link = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/Account/Reset?token={user.ResetToken}&pt={_dataProtector.Protect(user.ResetToken)}"
                },
                TemplatePath = "~/EmailTemplates/ResetPassword.cshtml"
            });
        }
    }
}
