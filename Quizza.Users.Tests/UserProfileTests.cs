using Quizza.Users.Domain.Constants;
using Quizza.Users.Domain.Models;
using Quizza.Users.Domain.Models.Entities;

namespace Quizza.Users.Tests
{
    public class UserProfileTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void UserProfile_IfEmailIsNull_ThrowsArgumentException(string? email)
        {
            var request = new SignUpRequest { Email = email };
            var exception = Assert.Throws<ArgumentException>(() => new UserProfile(request));
            Assert.Equal(ModelErrors.EmptyEmail, exception.Message);
        }

        [Fact]
        public void UserProfile_SetsPropertiesFromRequestModel()
        {
            var request = new SignUpRequest
            {
                Email = "email@domain.com",
                FirstName = "first-name",
                DateOfBirth = DateTime.Today,
                Gender = "G",
                LastName = "Last-Name",
                OtherNames = "others",
                Phone = "0122435555",
            };

            var userProfile = new UserProfile(request);
            Assert.NotEqual(Guid.Empty, userProfile.Id);
            Assert.Equal(request.FirstName.ToLower(), userProfile.FirstName);
            Assert.Equal(request.LastName.ToLower(), userProfile.LastName);
            Assert.Equal(request.OtherNames.ToLower(), userProfile.OtherNames);
            Assert.Equal(request.Email.ToLower(), userProfile.Email);
            Assert.Equal(request.DateOfBirth, userProfile.DateOfBirth);
            Assert.Equal(request.Gender.ToUpper(), userProfile.Gender);
            Assert.Equal(request.Phone, userProfile.Phone);
            Assert.Equal(DateTime.UtcNow.Date, userProfile.DateCreated.Date);
        }

        [Fact]
        public void LogAccessFailed_UpdatesFailCount()
        {
            // Create a user profile
            var userProfile = new UserProfile(OkSignupRequest);

            // Simulate failed login
            userProfile.LogAccessFailure(true, 1, 5);

            Assert.Equal(1, userProfile.AccessFailedCount);
        }

        [Fact]
        public void LogAccessFailed_IfMaxAttempts_LocksAccount()
        {
            const int maxAttempts = 3;
            const int lockoutDuration = 5;
            // Create a user profile
            var userProfile = new UserProfile(OkSignupRequest);

            // Simulate failed login up to max attempts
            for (int i = 0; i < maxAttempts; i++)
                userProfile.LogAccessFailure(true, maxAttempts, lockoutDuration);

            Assert.Equal(maxAttempts, userProfile.AccessFailedCount);
            Assert.True(userProfile.IsAccountLocked);
            var userLockoutDuration = (userProfile.LockoutExpiry!.Value - DateTime.UtcNow).TotalMinutes;
            Assert.Equal(lockoutDuration, Math.Ceiling(userLockoutDuration));
        }

        [Fact]
        public void LogAccessFailed_IfNotLockoutEnable_DoesNotLockAccount()
        {
            const int maxAttempts = 3;
            const int lockoutDuration = 5;
            // Create a user profile
            var userProfile = new UserProfile(OkSignupRequest);

            // Simulate failed login up to max attempts
            for (int i = 0; i < maxAttempts; i++)
                userProfile.LogAccessFailure(false, maxAttempts, lockoutDuration);

            Assert.Equal(maxAttempts, userProfile.AccessFailedCount);
            Assert.False(userProfile.IsAccountLocked);
            Assert.Null(userProfile.LockoutExpiry);
        }

        [Fact]
        public void LogAccessSuccess_ClearsFailures()
        {
            // Create a user profile
            var userProfile = new UserProfile(OkSignupRequest);

            // Simulate failed login
            userProfile.LogAccessFailure(true, 1, 5);

            // Then simulate successful login
            userProfile.LogAccessSuccess();

            Assert.False(userProfile.IsAccountLocked);
            Assert.Null(userProfile.LockoutExpiry);
            Assert.Equal(0, userProfile.AccessFailedCount);
        }

        [Fact]
        public void UserProfile_IgnoresPassword()
        {
            var userProfile = new UserProfile(OkSignupRequest);
            Assert.Null(userProfile.PasswordHash);
        }

        [Fact]
        public void SetPassword_UpdatesPasswordDetails()
        {
            const string passwordHash = "password-hash-demo";
            var userProfile = new UserProfile(OkSignupRequest);
            userProfile.SetPassword(passwordHash);
            var timeLapse = (DateTime.UtcNow - userProfile.LastPasswordChange!.Value).TotalSeconds;
            Assert.True(timeLapse < 1);
            Assert.Equal(passwordHash, userProfile.PasswordHash);
        }

        [Fact]
        public void AddRole_AddsRoleToUser()
        {
            const string role1 = "1st-role";
            const string role2 = "2nd-role";
            var userProfile = new UserProfile(OkSignupRequest);
            userProfile.AddRole(role1);
            userProfile.AddRole(role2);

            Assert.NotEmpty(userProfile.Roles);
            Assert.Collection(userProfile.Roles, 
                 r => Assert.Equal(role1, r.Role),
                 r => Assert.Equal(role2, r.Role));
            Assert.All(userProfile.Roles, r => Assert.Equal(userProfile.Id, r.UserProfileId));
        }

        static SignUpRequest OkSignupRequest => new()
        {
            Email = "email@domain.com",
            FirstName = "first-name",
            DateOfBirth = DateTime.Today,
            Gender = "G",
            LastName = "Last-Name",
            OtherNames = "others",
            Phone = "0122435555",
        };
    }
}