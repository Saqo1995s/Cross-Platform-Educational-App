using System;
using System.Threading.Tasks;
using IUniversity.Common.Constants;
using IUniversity.Common.Models;
using IUniversity.Common.Models.Base;
using IUniversity.Common.Models.Requests;
using IUniversity.Common.Models.Responses;
using LearningPlatform.Utilities;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LearningPlatform.Wrappers
{
    public sealed class LearningPlatformRestApiClient
    {
        private static LearningPlatformRestApiClient _instance = null;
        private readonly RestClient _client = null;

        public static LearningPlatformRestApiClient Instance => _instance ??= new LearningPlatformRestApiClient();

        public LearningPlatformRestApiClient()
        {
            //var options = new RestClientOptions(ApiConstants.ApiUrl)
            //{
            //    ThrowOnAnyError = true,
            //    Timeout = 1000,
            //    Proxy = WebRequest.DefaultWebProxy
            //};

            _client = new RestClient(ApiConstants.ApiUrl);
        }

        #region Helpers

        public void AddBearerAuthorization(string token)
        {
            _client.AddDefaultHeader(KnownHeaders.Authorization, "Bearer " + token); ;
        }

        private RestRequest CreateRequest(string endpoint)
        {
            var request = new RestRequest(endpoint)
            {
                Timeout = 5000 //5 sec
            };

            //request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Accept", "application/json");

            return request;
        }

        #endregion

        #region Requests

        public async Task<AuthResult> LoginUser(string username, string password)
        {
            var request = CreateRequest(ApiConstants.LoginEndpoint);

            var loginRequest = new LoginRequest
            {
                Username = username,
                Password = password
            };

            request.AddBody(loginRequest);

            var result = await _client.PostAsync<AuthResult>(request);

            return result;
        }

        public async Task<User> RegisterNewUserAsync(RegisterRequest registrationRequest, UserTypes userType)
        {
            var accountRequest = CreateRequest(ApiConstants.RegisterEndpoint);

            accountRequest.AddBody(registrationRequest);

            var accountResult = await _client.PostAsync<AuthResult>(accountRequest);

            User finalResult = null;

            if (accountResult.Success)
            {
                switch (userType)
                {
                    case UserTypes.Student:
                    {
                        var student = new Student
                        {
                            Email = registrationRequest.Email,
                            AccountId = accountResult.UserId,
                            FirstName = registrationRequest.FirstName,
                            LastName = registrationRequest.LastName,
                            Group = registrationRequest.Group
                        };

                        var request = CreateRequest(ApiConstants.StudentsEndpoint);
                        request.AddBody(student);

                        finalResult = await _client.PostAsync<Student>(request);

                        break;
                    }
                    case UserTypes.Teacher:
                    {
                        var teacher = new Teacher
                        {
                            Email = registrationRequest.Email,
                            AccountId = accountResult.UserId,
                            FirstName = registrationRequest.FirstName,
                            LastName = registrationRequest.LastName,
                        };

                        var request = CreateRequest(ApiConstants.TeachersEndpoint);
                        request.AddBody(teacher);

                        finalResult = await _client.PostAsync<Teacher>(request);

                        break;
                    }
                }
            }

            return finalResult;
        }

        public async Task<Admin> GetAdminByAccountId(Guid id)
        {
            var request = CreateRequest($"{ApiConstants.AdminsEndpoint}/GetAdminByAccountId");

            request.AddQueryParameter("id", id);

            var result = await _client.GetAsync<Admin>(request);

            return result;
        }

        public async Task<Student> GetStudentByAccountId(Guid id)
        {
            var request = CreateRequest($"{ApiConstants.StudentsEndpoint}/GetStudentByAccountId");

            request.AddQueryParameter("id", id);

            var result = await _client.GetAsync<Student>(request);

            return result;
        }

        public async Task<Teacher> GetTeacherByAccountId(Guid id)
        {
            var request = CreateRequest($"{ApiConstants.TeachersEndpoint}/GetTeacherByAccountId");

            request.AddQueryParameter("id", id);

            var result = await _client.GetAsync<Teacher>(request);

            return result;
        }

        public async Task LogoutUserAsync()
        {
            var request = CreateRequest(ApiConstants.LogoutEndpoint);

            await _client.DeleteAsync(request);
            _client.DefaultParameters.RemoveParameter(KnownHeaders.Authorization);
        }

        public async Task<bool> RefreshTokenAsync()
        {
            var request = CreateRequest(ApiConstants.RefreshEndpoint);

            var refreshRequest = new RefreshRequest
            {
                RefreshToken = Settings.RefreshToken,
                Token = Settings.BearerToken
            };

            request.AddBody(refreshRequest);

            var result = await _client.PostAsync<AuthResult>(request);

            return result.Success;
        }

        public async Task<Teacher[]> GetTeachersAsync()
        {
            var request = CreateRequest(ApiConstants.TeachersEndpoint);

            var result = await _client.GetAsync<Teacher[]>(request);

            return result;
        }

        public async Task<bool> AddNewCourseAsync(Course course)
        {
            var request = CreateRequest(ApiConstants.CoursesEndpoint);

            request.AddBody(course);

            var result = await _client.PostAsync<Course>(request);

            return result != null;
        }

        public async Task<Course[]> GetCoursesAsync()
        {
            var request = CreateRequest(ApiConstants.CoursesEndpoint);

            var result = await _client.GetAsync<Course[]>(request);

            return result;
        }

        public async Task<Course[]> GetCoursesForGroup(string group)
        {
            var request = CreateRequest($"{ApiConstants.CoursesEndpoint}/GetCoursesByGroup");
            request.AddQueryParameter("group", group);

            var result = await _client.GetAsync<Course[]>(request);

            return result;
        }

        public async Task<bool> AssignCourseToGroupAsync(CourseAssignment courseAssignment)
        {
            var request = CreateRequest(ApiConstants.CoursesAssignmentEndpoint);

            request.AddBody(courseAssignment);

            var result = await _client.PostAsync<CourseAssignment>(request);

            return result != null;
        }

        public async Task<bool> CheckApiServiceAvailable() //ping
        {
            var request = CreateRequest(ApiConstants.PingEndpoint);

            RestResponse result = null;
            try
            {
                result = await _client.HeadAsync(request);
            }
            catch (Exception)
            {
                //ignore
            }
            

            return result != null && result.IsSuccessful;
        }

        public async Task<bool> UpdateCourseAsync(Course course)
        {
            var request = CreateRequest($"{ApiConstants.CoursesEndpoint}/{course.Id}");
            //request.AddQueryParameter("id", course.Id);
            request.AddBody(course);

            RestResponse result = null;
            try
            {
                result = await _client.PutAsync(request);
            }
            catch (Exception ex)
            {
                //ignore - there is a most common issue ith IIS local server when POST method is not allowed
            }
            
            return result is {IsSuccessful: true};
        }

        public async Task<Teacher> GetTeacherByIdAsync(int id)
        {
            var request = CreateRequest(ApiConstants.TeachersEndpoint);
            request.AddQueryParameter("id", id);

            Teacher teacher = null; 
            try
            {
                var response = await _client.GetAsync(request);

                var jsonToken = JToken.Parse(response.Content);

                //TODO: after deploy on azure check is thi s bug or no, if it is a bug remove JArray case!
                if (jsonToken is JArray jArray) //possibly IIS bug
                {
                    teacher = JsonConvert.DeserializeObject<Teacher>(jArray.First.ToString());
                }
                else //this case added because deployed api may be work fine!
                {
                    teacher = JsonConvert.DeserializeObject<Teacher>(response.Content);
                }
            }
            catch (Exception e)
            {
               //ignore
            }
            
            return teacher;
        }

        public async Task<bool> AddTeacherAssignment(TeacherAssignment teacherAssignment)
        {
            var request = CreateRequest(ApiConstants.TeacherAssignmentEndpoint);
            request.AddBody(teacherAssignment);

            var response = await _client.PostAsync(request);

            return response.IsSuccessful;
        }

        public async Task<TeacherAssignment[]> GetTeacherAssignmentsByTeacherId(int teacherId)
        {
            var request = CreateRequest($"{ApiConstants.TeacherAssignmentEndpoint}/GetTeacherAssignmentsByTeacherId");
            request.AddQueryParameter("id", teacherId);

            var response = await _client.GetAsync<TeacherAssignment[]>(request);

            return response;
        }

        #endregion
    }
}
