﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Authorization.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using Tailspin.Surveys.Data.DataModels;
using Tailspin.Surveys.Security;
using Tailspin.Surveys.Security.Policy;
using Xunit;

namespace MultiTentantSurveyAppTests
{
    public class SurveyAuthorizationHandlerTests
    {
      
        [Fact]
        public void Handle_Read_PassesForOwner()
        {
            var survey = new Survey("test survey") { OwnerId = 54321, TenantId = 12345 };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(SurveyClaimTypes.SurveyUserIdClaimType, "54321"),
                new Claim(SurveyClaimTypes.SurveyTenantIdClaimType, "12345"),
                new Claim(ClaimTypes.Role, Roles.SurveyCreator)
            }));
            var authzContext = new AuthorizationContext(new IAuthorizationRequirement[] { }, principal, survey);
            var target = new TestableSurveyAuthorizationHandler();
            target.Handle(authzContext, Operations.Read, survey);
            Assert.True(authzContext.HasSucceeded);
        }

        [Fact]
        public void Handle_Read_PassesForContributor()
        {
            var survey = new Survey("test survey") { Contributors = new List<SurveyContributor> { new SurveyContributor { UserId = 54321 } } };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(SurveyClaimTypes.SurveyUserIdClaimType, "54321"),
                new Claim(SurveyClaimTypes.SurveyTenantIdClaimType, "12345"),
            }));
            var authzContext = new AuthorizationContext(new IAuthorizationRequirement[] { }, principal, survey);
            var target = new TestableSurveyAuthorizationHandler();
            target.Handle(authzContext, Operations.Read, survey);
            Assert.True(authzContext.HasSucceeded);
        }

        [Fact]
        public void Handle_Read_FailsForNonOwner()
        {
            var survey = new Survey("test survey") { OwnerId = 54321, TenantId = 54321};
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(SurveyClaimTypes.SurveyUserIdClaimType, "11111"),
                new Claim(SurveyClaimTypes.SurveyTenantIdClaimType, "12345"),
                new Claim(ClaimTypes.Role, Roles.SurveyCreator)
            }));
            var authzContext = new AuthorizationContext(new IAuthorizationRequirement[] { }, principal, survey);
            var target = new TestableSurveyAuthorizationHandler();
            target.Handle(authzContext, Operations.Read, survey);
            Assert.True(authzContext.HasFailed);
        }

        [Fact]
        public void Handle_Update_PassesForOwner()
        {
            var survey = new Survey("test survey") { OwnerId = 54321, TenantId = 12345 };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(SurveyClaimTypes.SurveyUserIdClaimType, "54321"),
                new Claim(SurveyClaimTypes.SurveyTenantIdClaimType, "12345"),
                new Claim(ClaimTypes.Role, Roles.SurveyCreator)
            }));
            var authzContext = new AuthorizationContext(new IAuthorizationRequirement[] { }, principal, survey);
            var target = new TestableSurveyAuthorizationHandler();
            target.Handle(authzContext, Operations.Update, survey);
            Assert.True(authzContext.HasSucceeded);
        }

        [Fact]
        public void Handle_Update_PassesForContributor()
        {
            var survey = new Survey("test survey") { Contributors = new List<SurveyContributor> { new SurveyContributor { UserId = 54321 } } };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(SurveyClaimTypes.SurveyUserIdClaimType, "54321"),
                new Claim(SurveyClaimTypes.SurveyTenantIdClaimType, "54321")
            }));
            var authzContext = new AuthorizationContext(new IAuthorizationRequirement[] { }, principal, survey);
            var target = new TestableSurveyAuthorizationHandler();
            target.Handle(authzContext, Operations.Update, survey);
            Assert.True(authzContext.HasSucceeded);
        }

        [Fact]
        public void Handle_Update_FailsForNonOwner()
        {
            var survey = new Survey("test survey") { OwnerId = 54321, TenantId = 12345 };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(SurveyClaimTypes.SurveyUserIdClaimType, "11111"),
                new Claim(SurveyClaimTypes.SurveyTenantIdClaimType, "12345"),
                new Claim(ClaimTypes.Role, Roles.SurveyCreator)
            }));
            var authzContext = new AuthorizationContext(new IAuthorizationRequirement[] { }, principal, survey);
            var target = new TestableSurveyAuthorizationHandler();
            target.Handle(authzContext, Operations.Update, survey);
            Assert.True(authzContext.HasFailed);
        }

        [Fact]
        public void Handle_Delete_FailsForNonOwner()
        {
            var survey = new Survey("test survey") { OwnerId = 54321, TenantId = 12345 };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(SurveyClaimTypes.SurveyUserIdClaimType, "11111"),
                new Claim(SurveyClaimTypes.SurveyTenantIdClaimType, "12345"),
                new Claim(ClaimTypes.Role, Roles.SurveyCreator)
            }));
            var authzContext = new AuthorizationContext(new IAuthorizationRequirement[] { }, principal, survey);
            var target = new TestableSurveyAuthorizationHandler();
            target.Handle(authzContext, Operations.Delete, survey);
            Assert.True(authzContext.HasFailed);
        }

        [Fact]
        public void Handle_Delete_PassesForOwner()
        {
            var survey = new Survey("test survey") { OwnerId = 54321, TenantId = 12345 };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(SurveyClaimTypes.SurveyUserIdClaimType, "54321"),
                new Claim(SurveyClaimTypes.SurveyTenantIdClaimType, "12345"),
                new Claim(ClaimTypes.Role, Roles.SurveyCreator)
            }));
            var authzContext = new AuthorizationContext(new IAuthorizationRequirement[] { }, principal, survey);
            var target = new TestableSurveyAuthorizationHandler();
            target.Handle(authzContext, Operations.Delete, survey);
            Assert.True(authzContext.HasSucceeded);
        }

        [Fact]
        public void Handle_Delete_PassesForAdmin()
        {
            var survey = new Survey("test survey") { OwnerId = 54321, TenantId = 12345 };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(SurveyClaimTypes.SurveyUserIdClaimType, "11111"),
                new Claim(SurveyClaimTypes.SurveyTenantIdClaimType, "12345"),
                new Claim(ClaimTypes.Role, Roles.SurveyAdmin)
            }));
            var authzContext = new AuthorizationContext(new IAuthorizationRequirement[] { }, principal, survey);
            var target = new TestableSurveyAuthorizationHandler();
            target.Handle(authzContext, Operations.Delete, survey);
            Assert.True(authzContext.HasSucceeded);
        }

        [Fact]
        public void Handle_Delete_FailsForAdminOfDifferentTenant()
        {
            var survey = new Survey("test survey") { OwnerId = 54321, TenantId = 12345 };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(SurveyClaimTypes.SurveyUserIdClaimType, "11111"),
                new Claim(SurveyClaimTypes.SurveyTenantIdClaimType, "othertenant"),
                new Claim(ClaimTypes.Role, Roles.SurveyAdmin)
            }));
            var authzContext = new AuthorizationContext(new IAuthorizationRequirement[] { }, principal, survey);
            var target = new TestableSurveyAuthorizationHandler();
            target.Handle(authzContext, Operations.Delete, survey);
            Assert.True(authzContext.HasFailed);
        }

        [Fact]
        public void Handle_Delete_PassesForAdminUserWithOtherRoles()
        {
            var survey = new Survey("test survey") { OwnerId = 54321, TenantId = 12345 };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(SurveyClaimTypes.SurveyUserIdClaimType, "11111"),
                new Claim(SurveyClaimTypes.SurveyTenantIdClaimType, "12345"),
                new Claim(ClaimTypes.Role, Roles.SurveyReader),
                new Claim(ClaimTypes.Role, Roles.SurveyAdmin),
                new Claim(ClaimTypes.Role, Roles.SurveyReader)
            }));
            var authzContext = new AuthorizationContext(new IAuthorizationRequirement[] { }, principal, survey);
            var target = new TestableSurveyAuthorizationHandler();
            target.Handle(authzContext, Operations.Delete, survey);
            Assert.True(authzContext.HasSucceeded);
        }

        [Fact]
        public void Handle_Create_PassesForCreatorWithOtherRoles()
        {
            var survey = new Survey("test survey") { OwnerId = 54321, TenantId = 12345 };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(SurveyClaimTypes.SurveyUserIdClaimType, "11111"),
                new Claim(SurveyClaimTypes.SurveyTenantIdClaimType, "12345"),
                new Claim(ClaimTypes.Role, Roles.SurveyReader),
                new Claim(ClaimTypes.Role, Roles.SurveyCreator),
                new Claim(ClaimTypes.Role, Roles.SurveyReader)
            }));
            var authzContext = new AuthorizationContext(new IAuthorizationRequirement[] { }, principal, survey);
            var target = new TestableSurveyAuthorizationHandler();
            target.Handle(authzContext, Operations.Create, survey);
            Assert.True(authzContext.HasSucceeded);
        }

        [Fact]
        public void Handle_Create_FailesForUserWithNoCreatorRoleAssignments()
        {
            var survey = new Survey("test survey") { OwnerId = 54321, TenantId = 12345 };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(SurveyClaimTypes.SurveyUserIdClaimType, "11111"),
                new Claim(SurveyClaimTypes.SurveyTenantIdClaimType, "12345"),
                new Claim(ClaimTypes.Role, Roles.SurveyReader),
                new Claim(ClaimTypes.Role, Roles.SurveyReader)
            }));
            var authzContext = new AuthorizationContext(new IAuthorizationRequirement[] { }, principal, survey);
            var target = new TestableSurveyAuthorizationHandler();
            target.Handle(authzContext, Operations.Create, survey);
            Assert.True(authzContext.HasFailed);
        }
    }

    internal class TestableSurveyAuthorizationHandler : SurveyAuthorizationHandler
    {
        private static Mock<ILogger> _logger = new Mock<ILogger>();
        public TestableSurveyAuthorizationHandler():base(_logger.Object)
        {}
        internal new void Handle(AuthorizationContext context, OperationAuthorizationRequirement operation, Survey resource)
        {
            base.Handle(context, operation, resource);
        }
    }
}
