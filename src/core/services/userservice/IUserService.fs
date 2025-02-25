namespace core.Services.UserService
open core.Records
open System
open System.Threading.Tasks
type IUserService =
  abstract member CreateUser: User -> Task
  abstract member GetUserById: Guid -> Task<Option<User>>
  abstract member UpdateUser: User -> Task
  abstract member DeleteUserById: Guid -> Task 
