namespace Core.Repositories
open System
open Core.Records
open System.Threading.Tasks
type IUserRepository =
  abstract member CreateUser: User -> Task
  abstract member GetUserById: Guid -> Task<Option<User>>
  abstract member UpdateUser: User -> Task
  abstract member DeleteUserById: Guid -> Task
